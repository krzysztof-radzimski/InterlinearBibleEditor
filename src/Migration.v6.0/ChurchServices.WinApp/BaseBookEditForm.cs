using DevExpress.Xpo;
using DevExpress.XtraEditors;
using DevExpress.XtraRichEdit;
using ChurchServices.Extensions;
using ChurchServices.Data.Model;
using ChurchServices.WinApp.Controllers;
using System;
using System.IO;
using System.Text;

namespace ChurchServices.WinApp {
    public partial class BaseBookEditForm : XtraForm {
        public BookBase Object { get; }

        private BaseBookEditForm() {
            InitializeComponent();
            XHtmlDocumentExporter.Register(txtPreface);
        }
        public BaseBookEditForm(BookBase bookBase) : this() {
            Object = bookBase;

            txtStatus.Properties.DataSource = new XPQuery<BookStatus>(Object.Session);

            txtBookName.EditValue = Object.BookName;
            txtBookShortcut.EditValue = Object.BookShortcut;
            txtBookTitle.EditValue = Object.BookTitle;
            txtColor.EditValue = Object.Color;
            txtNumberOfBook.EditValue = Object.NumberOfBook;
            txtPlaceWhereBookWasWritten.EditValue = Object.PlaceWhereBookWasWritten;
            if (Object.Preface.IsNotNull()) {
                txtPreface.LoadDocument(Object.PrefaceData, DocumentFormat.OpenXml);
            }
            txtPurpose.EditValue = Object.Purpose;
            txtStatus.EditValue = Object.Status;
            txtSubject.EditValue = Object.Subject;
            txtTimeOfWriting.EditValue = Object.TimeOfWriting;
            txtAuthorName.EditValue = Object.AuthorName;
        }

        internal void Save() {
            var fileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".docx");
            txtPreface.SaveDocument(fileName, DocumentFormat.OpenXml);
            if (File.Exists(fileName)) {
                Object.PrefaceData= File.ReadAllBytes(fileName);
                try { File.Delete(fileName); } catch { }
            }

            var data = txtPreface.SaveDocument(XHtmlDocumentFormat.Id);
            Object.Preface = Encoding.UTF8.GetString(data);
            Object.NumberOfBook = txtNumberOfBook.EditValue.ToInt();
            Object.BookName = txtBookName.Text;
            Object.BookShortcut = txtBookShortcut.Text;
            Object.BookTitle = txtBookTitle.Text;
            Object.Color = txtColor.Text;
            Object.PlaceWhereBookWasWritten = txtPlaceWhereBookWasWritten.Text;
            Object.Purpose = txtPurpose.Text;
            Object.Subject = txtSubject.Text;
            Object.TimeOfWriting = txtTimeOfWriting.Text;
            Object.Status = txtStatus.EditValue as BookStatus;
            Object.AuthorName = txtAuthorName.Text;
            Object.Save();
        }
    }
}
