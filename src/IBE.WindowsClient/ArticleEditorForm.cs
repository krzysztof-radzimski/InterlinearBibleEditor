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
            editor.Document.Delete(editor.Document.Paragraphs[0].Range);

            XHtmlDocumentExporter.Register(editor);
        }

        public ArticleEditorForm(Article article) : this() {
            Article = article;
            if (Article.IsNotNull()) {
                txtAuthor.Text = Article.AuthorName;
                txtLead.Text = Article.Text;
                txtSubject.Text=Article.Subject;
                txtDate.DateTime=Article.Date;

                if (Article.DocumentData.IsNotNull()) {
                    editor.LoadDocument(Article.DocumentData);
                }
            }
        }
        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            //var filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".html");
            //editor.SaveDocument(filePath, DocumentFormat.Html);
            //System.Diagnostics.Process.Start(filePath);
            if (Article.IsNotNull()) {
                Article.DocumentData = editor.SaveDocument(DocumentFormat.OpenXml);
                
                var data = editor.SaveDocument(XHtmlDocumentFormat.Id);
                Article.Text = Encoding.UTF8.GetString(data);

                Article.Date = txtDate.DateTime;
                Article.AuthorName = txtAuthor.Text;
                Article.Lead = txtLead.Text;
                Article.Subject = txtSubject.Text;

                Article.Save();
                var uow = Article.Session as UnitOfWork;
                if (uow.IsNotNull()) {
                    uow.CommitChanges();
                    uow.ReloadChangedObjects();
                }

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
    }
}
