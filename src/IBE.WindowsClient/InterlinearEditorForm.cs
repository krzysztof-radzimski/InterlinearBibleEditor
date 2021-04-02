using DevExpress.Xpo;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using IBE.Common.Extensions;
using IBE.Data.Import.Greek;
using IBE.Data.Model;
using IBE.WindowsClient.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace IBE.WindowsClient {
    public partial class InterlinearEditorForm : RibbonForm {
        string NAME = "NPI+";
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

        public InterlinearEditorForm(Verse verse) {
            InitializeComponent();
            this.Text = "Interlinear Bible Editor";

            Uow = new UnitOfWork();

            Translation = new XPQuery<Translation>(Uow).Where(x => x.Name == verse.ParentTranslation.Name).FirstOrDefault();
            NAME = Translation.Name;

            var view = new XPView(Uow, typeof(BookBase));
            view.CriteriaString = $"[Status.BookType] = {(int)Translation.BookType}";
            // view.CriteriaString = "[Status.Oid] = 1 OR [Status.Oid] = 2"; // tylko kanoniczne
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

            this.Load += new EventHandler(delegate (object sender, EventArgs e) {
                Application.DoEvents();
                var bookInfo = list.Where(x => x.NumberOfBook == verse.ParentChapter.ParentBook.NumberOfBook).FirstOrDefault();
                txtBook.EditValue = bookInfo;
                editBook_EditValueChanged(this, new DevExpress.XtraEditors.Controls.ChangingEventArgs(null, bookInfo));
                Application.DoEvents();
                txtChapter.EditValue = verse.ParentChapter.NumberOfChapter;
                editChapter_EditValueChanged(this, new DevExpress.XtraEditors.Controls.ChangingEventArgs(null, verse.ParentChapter.NumberOfChapter));
                Application.DoEvents();
                txtVerse.EditValue = verse.NumberOfVerse;
                editVerse_EditValueChanged(this, new DevExpress.XtraEditors.Controls.ChangingEventArgs(null, verse.NumberOfVerse));
                Application.DoEvents();
            });
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
                    //foreach (var chapter in theBook.Chapters.OrderBy(x=>x.NumberOfChapter)) {
                    //    list.Add(chapter.NumberOfChapter);
                    //}
                    var numbers = theBook.Chapters.Select(x => x.NumberOfChapter).OrderBy(x => x);
                    foreach (var number in numbers) {
                        list.Add(number);
                    }
                    //for (int i = 1; i <= theBook.NumberOfChapters; i++) {
                    //    list.Add(i);
                    //}
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

                var numbers = theChapter.Verses.Select(x => x.NumberOfVerse).OrderBy(x => x);
                foreach (var number in numbers) {
                    list.Add(number);
                }

                //for (int i = 1; i <= theChapter.NumberOfVerses; i++) {
                //    list.Add(i);
                //}
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
                    if (currentControl.IsModified()) {
                        if (XtraMessageBox.Show("Do you want to save your changes before opening a new verse?", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                            currentControl.Save();
                        }
                    }
                    this.Controls.Remove(currentControl);
                    currentControl.Dispose();
                    currentControl = null;
                }

                var book = txtBook.EditValue as BookBaseInfo;
                var chapterNumber = txtChapter.EditValue.ToInt();
                var verse = new XPQuery<Verse>(Uow).Where(x => x.NumberOfVerse == verseNumber && x.ParentChapter.NumberOfChapter == chapterNumber && x.ParentChapter.ParentBook.NumberOfBook == book.NumberOfBook && x.ParentChapter.ParentBook.ParentTranslation.Name == NAME).FirstOrDefault();
                if (verse.IsNotNull()) {
                    var control = new VerseEditorControl(verse, Translation.BookType == TheBookType.Bible);
                    control.Dock = DockStyle.Fill;
                    Application.DoEvents();
                    this.Controls.Add(control);
                    Application.DoEvents();

                    btnNextVerse.Enabled = true;
                    var allVerses = editVerse.DataSource as List<int>;
                    if (allVerses.IsNotNull() && allVerses.Count > 0 && verseNumber == allVerses.Last()) {
                        btnNextVerse.Enabled = false;
                    }
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

        private void btnAddWord_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var currentControl = this.Controls.OfType<Control>().Where(x => x is VerseEditorControl).FirstOrDefault() as VerseEditorControl;
            if (currentControl.IsNotNull()) {
                var position = XtraInputBox.Show("Indicate the word index :", "Insert new word at index", "1");
                if (position.IsNotNullOrEmpty()) {
                    var index = position.ToInt();
                    foreach (var item in currentControl.Verse.VerseWords) {
                        if (item.NumberOfVerseWord >= index) {
                            item.NumberOfVerseWord++;
                        }
                    }

                    var word = new VerseWord(Uow) {
                        ParentVerse = currentControl.Verse,
                        Translation = String.Empty,
                        Transliteration = String.Empty,
                        Citation = false,
                        StrongCode = null,
                        FootnoteText = String.Empty,
                        GrammarCode = null,
                        NumberOfVerseWord = index,
                        SourceWord = String.Empty,
                        WordOfJesus = false
                    };
                    word.Save();

                    var control = currentControl.CreateVerseWordControl(word);
                    currentControl.VerseWordsControl.Controls.Add(control);
                    currentControl.VerseWordsControl.Controls.SetChildIndex(control, index - 1);

                    currentControl.Verse.Save();
                    Uow.CommitChanges();
                }
            }
        }

        private void btnAddWords_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var currentControl = this.Controls.OfType<Control>().Where(x => x is VerseEditorControl).FirstOrDefault() as VerseEditorControl;
            if (currentControl.IsNotNull()) {
                var start = 1;
                if (currentControl.Verse.VerseWords.IsNotNull() && currentControl.Verse.VerseWords.Count > 0) {
                    start = currentControl.Verse.VerseWords.Max(x => x.NumberOfVerseWord) + 1;
                }
                var words = XtraInputBox.Show("Type words:", "Add words", "");
                if (words.IsNotNullOrEmpty()) {
                    var table = words.Split(' ');
                    if (table.Length > 0) {
                        for (int i = 0; i < table.Length; i++) {
                            var _word = table[i];

                            var word = new VerseWord(Uow) {
                                ParentVerse = currentControl.Verse,
                                Translation = String.Empty,
                                Transliteration = _word.TransliterateAncientGreek(),
                                Citation = false,
                                StrongCode = null,
                                FootnoteText = String.Empty,
                                GrammarCode = null,
                                NumberOfVerseWord = start + i,
                                SourceWord = _word,
                                WordOfJesus = false
                            };

                            word.Save();


                            var control = currentControl.CreateVerseWordControl(word);
                            currentControl.VerseWordsControl.Controls.Add(control);
                        }

                        currentControl.Verse.Save();
                        Uow.CommitChanges();
                    }
                }
            }
        }

        private void btnDeleteWords_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var currentControl = this.Controls.OfType<Control>().Where(x => x is VerseEditorControl).FirstOrDefault() as VerseEditorControl;
            if (currentControl.IsNotNull()) {
                if (XtraMessageBox.Show("Delete all words?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                    currentControl.DeleteAll();
                }
            }
        }
    }
}
