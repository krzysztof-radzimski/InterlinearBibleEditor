using DevExpress.Xpo;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using IBE.Common.Extensions;
using IBE.Data.Export;
using IBE.Data.Import.Greek;
using IBE.Data.Model;
using IBE.WindowsClient.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace IBE.WindowsClient {
    public partial class VerseGridForm : RibbonForm {
        private string NAME = "NPI+";
        private UnitOfWork Uow = null;
        private Translation Translation = null;
        private GreekTransliterationController TransliterationController;
        public VerseGridControl VerseControl { get; private set; }
        public VerseGridForm() {
            InitializeComponent();
            this.Text = "Interlinear Bible Editor";

            Uow = new UnitOfWork();

            Translation = new XPQuery<Translation>(Uow).Where(x => x.Name == NAME).FirstOrDefault();
            txtIndex.EditValue = "NPI.";

            LoadBooks();

            /*
            var view = new XPView(Uow, typeof(BookBase)) {
                CriteriaString = "[Status.Oid] = 1 OR [Status.Oid] = 2" // tylko kanoniczne
            };
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
            */

            VerseControl = new VerseGridControl() { Dock = DockStyle.Fill };
            pnlContent.Controls.Add(VerseControl);

            TransliterationController = new GreekTransliterationController();
        }

        public VerseGridForm(Verse verse) {
            InitializeComponent();
            this.Text = "Interlinear Bible Editor";

            Uow = new UnitOfWork();

            Translation = verse.ParentTranslation;
            NAME = Translation.Name;
           // $"{NAME.Replace("'", "").Replace("+", "")}.";

            LoadBooks();

            //var view = new XPView(Uow, typeof(BookBase)) {
            //    CriteriaString = $"[Status.BookType] = {(int)Translation.BookType}"
            //};
            //view.Properties.Add(new ViewProperty("NumberOfBook", SortDirection.None, "[NumberOfBook]", false, true));
            //view.Properties.Add(new ViewProperty("BookTitle", SortDirection.None, "[BookTitle]", false, true));

            //var list = new List<BookBaseInfo>();
            //foreach (ViewRecord item in view) {
            //    list.Add(new BookBaseInfo() {
            //        NumberOfBook = item["NumberOfBook"].ToInt(),
            //        BookTitle = item["BookTitle"].ToString()
            //    });
            //}
            //editBook.DataSource = list;

            var index = verse.GetVerseIndex();
            btnOblubienicaEu.Visibility = index.NumberOfBook >= 470 ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never;
            btnLogosSeptuagint.Visibility = index.NumberOfBook < 470 ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never;
           

            VerseControl = new VerseGridControl() { Dock = DockStyle.Fill };
            pnlContent.Controls.Add(VerseControl);

            //this.Load += new EventHandler(delegate (object sender, EventArgs e) {
            //    Application.DoEvents();
            //    var bookInfo = list.Where(x => x.NumberOfBook == index.NumberOfBook).FirstOrDefault();
            //    txtBook.EditValue = bookInfo;
            //    editBook_EditValueChanged(this, new DevExpress.XtraEditors.Controls.ChangingEventArgs(null, bookInfo));
            //    Application.DoEvents();
            //    txtChapter.EditValue = index.NumberOfChapter;
            //    editChapter_EditValueChanged(this, new DevExpress.XtraEditors.Controls.ChangingEventArgs(null, index.NumberOfChapter));
            //    Application.DoEvents();
            //    txtVerse.EditValue = index.NumberOfVerse;
            //    editVerse_EditValueChanged(this, new DevExpress.XtraEditors.Controls.ChangingEventArgs(null, index.NumberOfVerse));
            //    Application.DoEvents();
            //});

            TransliterationController = new GreekTransliterationController();

            txtIndex.EditValue = verse.Index;
            txtIndex_KeyUp(txtIndex, new KeyEventArgs(Keys.Enter));
        }

        private void LoadBooks() {
            var view = new XPView(Uow, typeof(BookBase)) {
                CriteriaString = $"[Status.BookType] = {(int)Translation.BookType}"
            };
            view.Properties.Add(new ViewProperty("NumberOfBook", SortDirection.None, "[NumberOfBook]", false, true));
            view.Properties.Add(new ViewProperty("BookTitle", SortDirection.None, "[BookTitle]", false, true));

            var list = new List<BookBaseInfo>();
            foreach (ViewRecord item in view) {
                list.Add(new BookBaseInfo() {
                    NumberOfBook = item["NumberOfBook"].ToInt(),
                    BookTitle = item["BookTitle"].ToString()
                });
            }
            txtBooks.Properties.DataSource = list;
        }

        private void btnSaveVerse_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            VerseControl.Save();
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

                    var numbers = theBook.Chapters.Select(x => x.NumberOfChapter).OrderBy(x => x);
                    foreach (var number in numbers) {
                        list.Add(number);
                    }

                    txtChapter.Properties.DataSource = list;
                    txtChapter.EditValue = null;
                    txtVerse.Properties.DataSource = null;
                    btnNextVerse.Enabled = false;
                    btnPreviousVerse.Enabled = false;

                    txtIndex.EditValue = $"{Translation.Name.Replace("'", "").Replace("+", "")}.{book.NumberOfBook}.";
                }
            }
        }

        private void editChapter_EditValueChanged(object sender, EventArgs e) {
            var arg = e as DevExpress.XtraEditors.Controls.ChangingEventArgs;
            if (arg.IsNotNull()) {
                var chapterNumber = arg.NewValue.ToInt();
                if (chapterNumber == 0) { return; }
                var book = txtBooks.EditValue as BookBaseInfo;
                var theBook = Translation.Books.Where(x => x.NumberOfBook == book.NumberOfBook).FirstOrDefault();
                var theChapter = theBook.Chapters.Where(x => x.NumberOfChapter == chapterNumber).FirstOrDefault();
                var list = new List<int>();

                var numbers = theChapter.Verses.Select(x => x.NumberOfVerse).OrderBy(x => x);
                foreach (var number in numbers) {
                    list.Add(number);
                }

                lblVerseCount.Text = $"of {numbers.Max()}";

                txtVerse.Properties.DataSource = list;
                txtVerse.EditValue = null;
                btnNextVerse.Enabled = false;
                btnPreviousVerse.Enabled = false;

                txtIndex.EditValue = $"{Translation.Name.Replace("'", "").Replace("+", "")}.{book.NumberOfBook}.{chapterNumber}";
            }
        }

        private void editVerse_EditValueChanged(object sender, EventArgs e) {
            var arg = e as DevExpress.XtraEditors.Controls.ChangingEventArgs;
            if (arg.IsNotNull()) {
                var verseNumber = arg.NewValue.ToInt();

                if (VerseControl.Verse.IsNotNull() && VerseControl.IsModified()) {
                    if (XtraMessageBox.Show("Do you want to save your changes before opening a new verse?", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                        VerseControl.Save();
                    }
                }

                Application.DoEvents();

                var book = txtBooks.EditValue as BookBaseInfo;
                var chapterNumber = txtChapter.EditValue.ToInt();
                var verse = new XPQuery<Verse>(Uow).Where(x => x.NumberOfVerse == verseNumber && x.ParentChapter.NumberOfChapter == chapterNumber && x.ParentChapter.ParentBook.NumberOfBook == book.NumberOfBook && x.ParentChapter.ParentBook.ParentTranslation.Name == NAME).FirstOrDefault();
                if (verse.IsNotNull()) {

                    VerseControl.LoadData(verse, Translation.BookType == TheBookType.Bible);

                    btnExportChapterToPDF.Enabled = btnExportChapterToWord.Enabled = btnExportBookToDocx.Enabled = btnExportBookToPdf.Enabled = btnNextVerse.Enabled = btnTranslateChapter.Enabled = true;

                    var allVerses = txtVerse.Properties.DataSource as List<int>;
                    if (allVerses.IsNotNull() && allVerses.Count > 0 && verseNumber == allVerses.Last()) {
                        btnNextVerse.Enabled = false;
                    }
                    btnPreviousVerse.Enabled = verseNumber > 1;

                    this.Text = $"{book.BookTitle} {chapterNumber}:{verseNumber}";

                    btnOblubienicaEu.Visibility = book.NumberOfBook >= 470 ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never;
                    btnLogosSeptuagint.Visibility = book.NumberOfBook < 470 ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never;

                    txtIndex.EditValue = verse.Index; // $"{Translation.Name.Replace("'", "").Replace("+", "")}.{book.NumberOfBook}.{chapterNumber}.{verseNumber}";
                }
            }
        }

        private void btnNextVerse_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var oldValue = txtVerse.EditValue;
            var verse = oldValue.ToInt() + 1;
            var list = txtVerse.Properties.DataSource as List<int>;
            if (list.IsNotNull() && list.Contains(verse)) {
                txtVerse.EditValue = verse;
                editVerse_EditValueChanged(txtVerse, new DevExpress.XtraEditors.Controls.ChangingEventArgs(oldValue, verse));
            }
        }

        private void btnPreviousVerse_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var oldValue = txtVerse.EditValue;
            var verse = oldValue.ToInt() - 1;
            if (verse > 0) {
                txtVerse.EditValue = verse;
                editVerse_EditValueChanged(txtVerse, new DevExpress.XtraEditors.Controls.ChangingEventArgs(oldValue, verse));
            }
        }

        private void btnAddWord_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            if (VerseControl.IsNotNull()) {
                var position = XtraInputBox.Show("Indicate the word index :", "Insert new word at index", "1");
                if (position.IsNotNullOrEmpty()) {
                    VerseControl.Save(); // save changes

                    var index = position.ToInt();
                    foreach (var item in VerseControl.Verse.VerseWords) {
                        if (item.NumberOfVerseWord >= index) {
                            item.NumberOfVerseWord++;
                        }
                    }

                    var word = new VerseWord(Uow) {
                        ParentVerse = VerseControl.Verse,
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
                    VerseControl.Verse.Save();
                    Uow.CommitChanges();

                    VerseControl.LoadData(VerseControl.Verse, false);
                }
            }
        }

        private void btnAddWords_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            if (VerseControl.IsNotNull()) {
                var start = 1;
                if (VerseControl.Verse.VerseWords.IsNotNull() && VerseControl.Verse.VerseWords.Count > 0) {
                    start = VerseControl.Verse.VerseWords.Max(x => x.NumberOfVerseWord) + 1;
                }
                var words = XtraInputBox.Show("Type words:", "Add words", "");
                if (words.IsNotNullOrEmpty()) {
                    var table = words.Split(' ');
                    if (table.Length > 0) {

                        VerseControl.Save(); // save changes before

                        for (int i = 0; i < table.Length; i++) {
                            var _word = table[i];

                            var word = new VerseWord(Uow) {
                                ParentVerse = VerseControl.Verse,
                                Translation = String.Empty,
                                Transliteration = TransliterationController.TransliterateWord(_word),
                                Citation = false,
                                StrongCode = null,
                                FootnoteText = String.Empty,
                                GrammarCode = null,
                                NumberOfVerseWord = start + i,
                                SourceWord = _word,
                                WordOfJesus = false
                            };

                            word.Save();
                        }

                        VerseControl.Verse.Save();
                        Uow.CommitChanges();

                        VerseControl.LoadData(VerseControl.Verse);
                    }
                }
            }
        }

        private void btnDeleteWords_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            if (VerseControl.IsNotNull()) {
                if (XtraMessageBox.Show("Delete all words?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                    VerseControl.DeleteAll();
                }
            }
        }

        private void btnRenumerateWords_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            if (VerseControl.IsNotNull()) {
                VerseControl.RenumerateAll();
            }
        }

        private void btnSetAllAsJesusWords_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            if (VerseControl.IsNotNull()) {
                VerseControl.WordOfGodAll();
            }
        }

        private void btnOblubienicaEu_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var currentControl = VerseControl;//this.Controls.OfType<Control>().Where(x => x is VerseEditorControl).FirstOrDefault() as VerseEditorControl;
            if (currentControl.IsNotNull()) {
                System.Diagnostics.Process.Start(currentControl.Verse.GetOblubienicaUrl());
            }
        }

        private void btnExportChapterToPDF_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            Export(ExportSaveFormat.Pdf);
        }
        private void btnExportChapterToWord_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            Export(ExportSaveFormat.Docx);
        }

        private void Export(ExportSaveFormat format) {
            var currentControl = VerseControl;//this.Controls.OfType<Control>().Where(x => x is VerseEditorControl).FirstOrDefault() as VerseEditorControl;
            if (currentControl.IsNotNull()) {
                Chapter chapter = currentControl.Verse.ParentChapter;
                if (chapter.IsNotNull()) {
                    var licPath = System.Configuration.ConfigurationManager.AppSettings["AsposeLic"];
                    var licInfo = new System.IO.FileInfo(licPath);
                    byte[] licData = null;
                    if (licInfo.Exists) {
                        licData = System.IO.File.ReadAllBytes(licPath);
                    }
                    var outputPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + (format == ExportSaveFormat.Docx ? ".docx" : ".pdf"));
                    new InterlinearExporter(licData).Export(chapter, format, outputPath);
                    if (File.Exists(outputPath)) { System.Diagnostics.Process.Start(outputPath); }
                }
            }
        }

        private void ExportBook(ExportSaveFormat format) {
           if (VerseControl.IsNotNull()) {
                var book = VerseControl.Verse.ParentChapter.ParentBook;
                if (book.IsNotNull()) {
                    var licPath = System.Configuration.ConfigurationManager.AppSettings["AsposeLic"];
                    var licInfo = new System.IO.FileInfo(licPath);
                    byte[] licData = null;
                    if (licInfo.Exists) {
                        licData = System.IO.File.ReadAllBytes(licPath);
                    }
                    var outputPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + (format == ExportSaveFormat.Docx ? ".docx" : ".pdf"));
                    new InterlinearExporter(licData).ExportBookTranslation(book, format, outputPath);
                    if (File.Exists(outputPath)) { System.Diagnostics.Process.Start(outputPath); }
                }
            }
        }

        private void btnLogosSeptuagint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            if (VerseControl.IsNotNull()) {
                System.Diagnostics.Process.Start(VerseControl.Verse.GetLogosSeptuagintUrl());
            }
        }

        private void btnExportBookToPdf_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            ExportBook(ExportSaveFormat.Pdf);
        }

        private void btnExportBookToDocx_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            ExportBook(ExportSaveFormat.Docx);
        }

        private void btnTranslateChapter_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            if (XtraMessageBox.Show("Do you want to translate this chapter?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {

                var verses = VerseControl.Verse.ParentChapter.Verses.OrderBy(x=>x.NumberOfVerse).ToList();
                var dic = new XPQuery<AncientDictionaryItem>(Uow).ToList();

                Uow.BeginTransaction();

                var c = new GreekTransliterationController();

                foreach (var verse in verses) {
                    foreach (var verseWord in verse.VerseWords) {
                        if (verseWord.Translation.IsNullOrEmpty()) {
                            var w = c.GetSourceWordWithoutBreathAndAccent(verseWord.SourceWord.RemoveAny(".", ":", ",", ";", "·", "—", "-", ")", "(", "]", "[", "’", ";", "\""), out var isUpper);
                            var item = dic.Where(x => x.Word == w.ToLower()).FirstOrDefault();
                            if (item.IsNotNull()) {                              
                                if (isUpper && item.Translation.IsNotNullOrEmpty() && item.Translation.Length > 1) {
                                    verseWord.Translation = item.Translation.Substring(0, 1).ToUpper() + item.Translation.Substring(1).ToLower();
                                }
                                else {
                                    verseWord.Translation = item.Translation.ToLower();
                                }
                                verseWord.Save();
                            }
                        }
                    }
                }

                Uow.CommitChanges();

                VerseControl.LoadData(verses.First());
            }
        }

        private void btnUpdateDictionary_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            if (XtraMessageBox.Show("Do you want to update dictionary?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                new DictionaryBuilder().Build(Uow);
            }
        }

        private void txtIndex_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                var text = txtIndex.EditValue.ToString();
                var verse = new XPQuery<Verse>(Uow).Where(x => x.Index == text).FirstOrDefault();
                if (verse.IsNotNull()) {
                    var verseNumber = verse.NumberOfVerse;

                    if (VerseControl.Verse.IsNotNull() && VerseControl.IsModified()) {
                        if (XtraMessageBox.Show("Do you want to save your changes before opening a new verse?", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                            VerseControl.Save();
                        }
                    }

                    VerseControl.LoadData(verse, Translation.BookType == TheBookType.Bible);

                    btnExportChapterToPDF.Enabled = btnExportChapterToWord.Enabled = btnExportBookToDocx.Enabled = btnExportBookToPdf.Enabled = btnNextVerse.Enabled = btnTranslateChapter.Enabled = true;

                    var allVerses = txtVerse.Properties.DataSource as List<int>;
                    if (allVerses.IsNotNull() && allVerses.Count > 0 && verseNumber == allVerses.Last()) {
                        btnNextVerse.Enabled = false;
                    }
                    btnPreviousVerse.Enabled = verseNumber > 1;

                    var book = verse.ParentChapter.ParentBook.BaseBook;
                    this.Text = $"{book.BookTitle} {verse.ParentChapter.NumberOfChapter}:{verseNumber}";

                    btnOblubienicaEu.Visibility = book.NumberOfBook >= 470 ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never;
                    btnLogosSeptuagint.Visibility = book.NumberOfBook < 470 ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never;

                    var bookInfo = (txtBooks.Properties.DataSource as List<BookBaseInfo>).Where(x => x.NumberOfBook == book.NumberOfBook).FirstOrDefault();
                    txtBooks.EditValue = bookInfo;
                    
                    Application.DoEvents();
                    editBook_EditValueChanged(this, new DevExpress.XtraEditors.Controls.ChangingEventArgs(null, bookInfo));

                    Application.DoEvents();
                    txtChapter.EditValue = verse.ParentChapter.NumberOfChapter;

                    Application.DoEvents();
                    editChapter_EditValueChanged(this, new DevExpress.XtraEditors.Controls.ChangingEventArgs(null, verse.ParentChapter.NumberOfChapter));
                    
                    Application.DoEvents();
                    txtVerse.EditValue = verse.NumberOfVerse;
              
                }
            }
        }

        private void btnDeleteWord_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            //
        }
    }
}
