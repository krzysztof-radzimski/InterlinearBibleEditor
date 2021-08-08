using DevExpress.Xpo;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.Commands;
using IBE.Common.Extensions;
using IBE.Data.Model;
using IBE.WindowsClient.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IBE.WindowsClient {
    public partial class ArticleEditorForm : RibbonForm {
        public Article Article { get; private set; }

        public ArticleEditorForm() {
            InitializeComponent();

            editor.Document.Paragraphs.Insert(editor.Document.Range.Start);
            editor.Document.Selection = editor.Document.Paragraphs[0].Range;
            SetParagraphHeading1LevelCommand h1Command = new SetParagraphHeading1LevelCommand(editor);
            h1Command.ForceExecute(h1Command.CreateDefaultCommandUIState());
            SetParagraphHeading2LevelCommand h2Command = new SetParagraphHeading2LevelCommand(editor);
            h2Command.ForceExecute(h1Command.CreateDefaultCommandUIState());
            SetParagraphHeading3LevelCommand h3Command = new SetParagraphHeading3LevelCommand(editor);
            h3Command.ForceExecute(h1Command.CreateDefaultCommandUIState());
            SetParagraphHeading4LevelCommand h4Command = new SetParagraphHeading4LevelCommand(editor);
            h4Command.ForceExecute(h1Command.CreateDefaultCommandUIState());
            SetParagraphHeading5LevelCommand h5Command = new SetParagraphHeading5LevelCommand(editor);
            h5Command.ForceExecute(h1Command.CreateDefaultCommandUIState());

            AddQuoteStyle();

            editor.Document.Delete(editor.Document.Paragraphs[0].Range);

            XHtmlDocumentExporter.Register(editor);
            InitTypes();

            Text = "New article";

            editor.SpellChecker = MainForm.Instance.SpellChecker;
        }

        private void AddQuoteStyle() {
            var qstyle = editor.Document.ParagraphStyles["Quote"];
            if (qstyle == null) {
                qstyle = editor.Document.ParagraphStyles.CreateNew();
                qstyle.Name = "Quote";
                qstyle.LineSpacingType = DevExpress.XtraRichEdit.API.Native.ParagraphLineSpacing.Single;
                qstyle.Alignment = DevExpress.XtraRichEdit.API.Native.ParagraphAlignment.Justify;
                qstyle.FontSize = 10;
                qstyle.LeftIndent = 50;

                editor.Document.ParagraphStyles.Add(qstyle);
            }
        }

        public ArticleEditorForm(Article article) : this() {
            Article = article;
            if (Article.IsNotNull()) {
                Text = Article.Subject;

                txtAuthor.Text = Article.AuthorName;
                txtLead.Text = Article.Lead;
                txtSubject.Text = Article.Subject;
                txtDate.DateTime = Article.Date;

                if (Article.DocumentData.IsNotNull()) {
                    editor.LoadDocument(Article.DocumentData);
                    AddQuoteStyle();
                }

                txtType.EditValue = Article.Type;
                if (Article.AuthorPicture.IsNotNull()) {
                    txtAuthorPicture.Image = Image.FromStream(new MemoryStream(Article.AuthorPicture));
                }
            }
        }
        private void InitTypes() {
            var list = typeof(ArticleType).GetEnumValues().OfType<ArticleType>();
            txtType.Properties.DataSource = list;
        }
        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            //var filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".html");
            //editor.SaveDocument(filePath, DocumentFormat.Html);
            //System.Diagnostics.Process.Start(filePath);
            if (Article.IsNotNull()) {
                var fileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".docx");
                editor.SaveDocument(fileName, DocumentFormat.OpenXml);
                if (File.Exists(fileName)) {
                    Article.DocumentData = File.ReadAllBytes(fileName);
                    try { File.Delete(fileName); } catch { }
                }

                var data = editor.SaveDocument(XHtmlDocumentFormat.Id);
                Article.Text = Encoding.UTF8.GetString(data);

                Article.Date = txtDate.DateTime;
                Article.AuthorName = txtAuthor.Text;
                Article.Lead = txtLead.Text;
                Article.Subject = txtSubject.Text;
                Article.Type = (ArticleType)txtType.EditValue;

                if (txtAuthorPicture.Image.IsNotNull()) {
                    var mem = new MemoryStream();
                    txtAuthorPicture.Image.Save(mem, ImageFormat.Jpeg);
                    Article.AuthorPicture = mem.ToArray();
                }
                else {
                    Article.AuthorPicture = null;
                }

                Article.Save();
                var uow = Article.Session as UnitOfWork;
                if (uow.IsNotNull()) {
                    uow.CommitChanges();
                    uow.ReloadChangedObjects();
                }

                Text = Article.Subject;

                var articles = MdiParent.MdiChildren.Where(x => x is ArticlesForm).FirstOrDefault();
                if (articles.IsNotNull()) {
                    (articles as ArticlesForm).LoadData();
                }
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e) {
            var fileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".html");
            editor.SaveDocument(fileName, XHtmlDocumentFormat.Id);
            System.Diagnostics.Process.Start(fileName);
        }

        private void btnQuote_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
           //var pos = editor.Document.CaretPosition;
           var sel = editor.Document.Selection;
           var pars = editor.Document.Paragraphs.Get(sel);
            //var par = editor.Document.Paragraphs.Get(pos);
            var style = editor.Document.ParagraphStyles["Quote"];
            foreach (var par in pars) {
                if (style.IsNotNull()) {
                    par.Style = style;
                }
            }           
        }
    }
}
