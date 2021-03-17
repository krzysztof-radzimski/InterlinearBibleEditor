using DevExpress.Xpo;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using IBE.Common.Extensions;
using IBE.Data.Model;
using IBE.WindowsClient.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IBE.WindowsClient {
    public partial class InterlinearEditorForm : RibbonForm {
        const string NAME = "NPI+";
        UnitOfWork Uow = null;
        Translation Translation = null;
        public InterlinearEditorForm() {
            InitializeComponent();
            this.Text = "Interlinear Bible Editor";

            Uow = new UnitOfWork();

            Translation = new XPQuery<Translation>(Uow).Where(x => x.Name == NAME).FirstOrDefault();

            var view = new XPView(Uow, typeof(BookBase));
            view.CriteriaString = "[Status.Oid] = 1 OR [Status.Oid] = 2"; // tylko kanoniczne
            view.Properties.Add(new ViewProperty("NumberOfBook", SortDirection.None, "[NumberOfBook]", false, true));
            view.Properties.Add(new ViewProperty("BookTitle", SortDirection.None, "[BookTitle]", false, true));

            var list = new List<BookBaseInfo>();
            foreach (ViewRecord item in view) {
                list.Add(new BookBaseInfo() {
                    NumberOfBook = item["NumberOfBook"].ToInt(),
                    BookTitle = item["BookTitle"].ToString()
                });
            }

            editBook.DataSource = list;
        }

        private void btnSaveVerse_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var currentControl = this.Controls.OfType<Control>().Where(x => x is VerseEditorControl).FirstOrDefault() as VerseEditorControl;
            if (currentControl.IsNotNull()) {
                currentControl.Save();
            }
        }


        class BookBaseInfo {
            public int NumberOfBook { get; set; }
            public string BookTitle { get; set; }
            public override string ToString() {
                return BookTitle;
            }
        }

        private void editBook_EditValueChanged(object sender, EventArgs e) {
            var arg = e as DevExpress.XtraEditors.Controls.ChangingEventArgs;
            if (arg.IsNotNull()) {
                var book = arg.NewValue as BookBaseInfo;
                if (book.IsNotNull()) {
                    var theBook = Translation.Books.Where(x => x.NumberOfBook == book.NumberOfBook).FirstOrDefault();
                    var list = new List<int>();
                    for (int i = 1; i <= theBook.NumberOfChapters; i++) {
                        list.Add(i);
                    }
                    editChapter.DataSource = list;
                    txtChapter.EditValue = null;
                    editVerse.DataSource = null;
                    btnNextVerse.Enabled = false;
                    btnPreviousVerse.Enabled = false;
                }
            }
        }

        private void editChapter_EditValueChanged(object sender, EventArgs e) {
            var arg = e as DevExpress.XtraEditors.Controls.ChangingEventArgs;
            if (arg.IsNotNull()) {
                var chapterNumber = arg.NewValue.ToInt();
                var book = txtBook.EditValue as BookBaseInfo;
                var theBook = Translation.Books.Where(x => x.NumberOfBook == book.NumberOfBook).FirstOrDefault();
                var theChapter = theBook.Chapters.Where(x => x.NumberOfChapter == chapterNumber).FirstOrDefault();
                var list = new List<int>();
                for (int i = 1; i <= theChapter.NumberOfVerses; i++) {
                    list.Add(i);
                }
                editVerse.DataSource = list;
                txtVerse.EditValue = null;
                btnNextVerse.Enabled = false;
                btnPreviousVerse.Enabled = false;
            }
        }

        private void editVerse_EditValueChanged(object sender, EventArgs e) {
            var arg = e as DevExpress.XtraEditors.Controls.ChangingEventArgs;
            if (arg.IsNotNull()) {
                var verseNumber = arg.NewValue.ToInt();
                var currentControl = this.Controls.OfType<Control>().Where(x => x is VerseEditorControl).FirstOrDefault() as VerseEditorControl;
                if (currentControl.IsNotNull()) {
                    if (XtraMessageBox.Show("Do you want to save your changes before opening a new verse?", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                        currentControl.Save();
                    }
                    this.Controls.Remove(currentControl);
                    currentControl.Dispose();
                    currentControl = null;
                }

                var book = txtBook.EditValue as BookBaseInfo;
                var chapterNumber = txtChapter.EditValue.ToInt();
                var verse = new XPQuery<Verse>(Uow).Where(x => x.NumberOfVerse == verseNumber && x.ParentChapter.NumberOfChapter == chapterNumber && x.ParentChapter.ParentBook.NumberOfBook == book.NumberOfBook && x.ParentChapter.ParentBook.ParentTranslation.Name == NAME).FirstOrDefault();
                if (verse.IsNotNull()) {
                    var control = new VerseEditorControl(verse);
                    control.Dock = DockStyle.Fill;
                    Application.DoEvents();
                    this.Controls.Add(control);
                    Application.DoEvents();

                    btnNextVerse.Enabled = true;
                    btnPreviousVerse.Enabled = verseNumber > 1;

                    this.Text = $"{book.BookTitle} {chapterNumber}:{verseNumber}";
                }
            }
        }

        private void btnNextVerse_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var oldValue = txtVerse.EditValue;
            var verse = oldValue.ToInt() + 1;
            var list = editVerse.DataSource as List<int>;
            if (list.IsNotNull() && list.Contains(verse)) {
                txtVerse.EditValue = verse;
                editVerse_EditValueChanged(editVerse, new DevExpress.XtraEditors.Controls.ChangingEventArgs(oldValue, verse));
            }
        }

        private void btnPreviousVerse_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var oldValue = txtVerse.EditValue;
            var verse = oldValue.ToInt() - 1;
            if (verse > 0) {
                txtVerse.EditValue = verse;
                editVerse_EditValueChanged(editVerse, new DevExpress.XtraEditors.Controls.ChangingEventArgs(oldValue, verse));
            }
        }
    }
}
