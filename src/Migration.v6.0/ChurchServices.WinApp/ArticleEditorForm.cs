using DevExpress.Xpo;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.Commands;
using ChurchServices.Extensions;
using ChurchServices.Data.Export.Controllers;
using ChurchServices.Data.Model;
using ChurchServices.WinApp.Controllers;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ChurchServices.WinApp {
    public partial class ArticleEditorForm : RibbonForm {
        public Article Article { get; private set; }
        public IBibleTagController BibleTag { get; }
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

            BibleTag = new BibleTagController();
        }

        private void ReplaceAllSiglum() {
            var ranges = editor.Document.FindAll(new Regex(BibleTagController.SIGLUM_PATTERN_STRICT));
            if (ranges.IsNotNull() && ranges.Length > 0) {
                var dic = new System.Collections.Generic.Dictionary<DevExpress.XtraRichEdit.API.Native.DocumentRange, string>();
                foreach (var range in ranges.OrderBy(x => x.Start).Reverse()) {
                    var rangeText = editor.Document.GetText(range);
                    if (rangeText.IsNotNullOrEmpty()) {
                        var url = BibleTag.GetRecognizedSiglumUrl(Article.Session, rangeText);
                        if (url.IsNotNullOrEmpty()) {
                            dic.Add(range, url);
                        }
                    }
                }

                foreach (var item in dic) {
                    var links = editor.Document.Hyperlinks.Select(x => x.Range);
                    var linkExists = links.Where(x => x.Start <= item.Key.Start && x.End >= item.Key.Start).Any();
                    if (!linkExists) {
                        var link = editor.Document.Hyperlinks.Create(item.Key);
                        link.NavigateUri = $"{item.Value}";
                    }
                }
            }
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
                if (Article.Subject.IsNotNullOrEmpty()) {
                    Text = Article.Subject;
                }
                else {
                    Text = "Nowy";
                }

                txtAuthor.Text = Article.AuthorName;
                txtLead.Text = Article.Lead;
                txtSubject.Text = Article.Subject;
                txtDate.DateTime = Article.Date;
                cbHidden.Checked = Article.Hidden;

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
            if (Article.IsNotNull()) {
                var fileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".docx");
                editor.SaveDocument(fileName, DocumentFormat.OpenXml);
                if (File.Exists(fileName)) {
                    Article.DocumentData = File.ReadAllBytes(fileName);
                    try { File.Delete(fileName); } catch { }
                }

                var data = editor.SaveDocument(XHtmlDocumentFormat.Id);
                var articleHtml = Encoding.UTF8.GetString(data);
                articleHtml = UpdateArticleHtml(articleHtml);
                Article.Text = articleHtml;
                Article.Date = txtDate.DateTime;
                Article.AuthorName = txtAuthor.Text;
                Article.Lead = txtLead.Text;
                Article.Subject = txtSubject.Text;
                Article.Type = (ArticleType)txtType.EditValue;
                Article.Hidden = cbHidden.Checked;

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

        private string UpdateArticleHtml(string articleHtml) {
            var patternFontSize = @"font\-size\:\s?[0-9]+px;";
            articleHtml = Regex.Replace(articleHtml, patternFontSize, string.Empty);

            var patternEmptyStyles = @"\sstyle=""(\s+)?""";
            articleHtml = Regex.Replace(articleHtml, patternEmptyStyles, string.Empty);

            return articleHtml;
        }

        private void btnPreviewHtml_Click(object sender, EventArgs e) {
            var name = Guid.NewGuid().ToString();
            var fileName = Path.Combine(Path.GetTempPath(), $"{name}.html");
#if DEBUG
            //var fileNameDocx = Path.Combine(Path.GetTempPath(), $"{name}.docx");
            //editor.SaveDocument(fileNameDocx, DocumentFormat.OpenXml);
#endif
            var data = editor.SaveDocument(XHtmlDocumentFormat.Id);
            var articleHtml = Encoding.UTF8.GetString(data);
            articleHtml = UpdateArticleHtml(articleHtml);
            File.WriteAllText(fileName, articleHtml, Encoding.UTF8);
            //editor.SaveDocument(fileName, XHtmlDocumentFormat.Id);
            System.Diagnostics.Process.Start("explorer.exe", fileName);
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

        private void btnReplaceAllSiglum_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            ReplaceAllSiglum();
        }

        private void btnRemoveHostFromHyperlinks_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var host = System.Configuration.ConfigurationManager.AppSettings["Host"];
            foreach (var link in editor.Document.Hyperlinks) {
                link.NavigateUri = link.NavigateUri.Replace(host, String.Empty);
            }
        }
    }
}
