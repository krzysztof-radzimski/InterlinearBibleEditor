using DevExpress.Xpo;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using ChurchServices.Extensions;
using ChurchServices.Data.Export;
using ChurchServices.Data.Export.Controllers;
using ChurchServices.Data.Import.Greek;
using ChurchServices.Data.Model;
using ChurchServices.WinApp.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DevExpress.Xpo.Logger.Transport;
using DevExpress.XtraSplashScreen;

namespace ChurchServices.WinApp {
    public partial class VerseGridForm : RibbonForm {
        private string NAME = "NPI+";
        private UnitOfWork Uow = null;
        private Translation Translation = null;
        private GreekTransliterationController TransliterationController;
        public VerseGridControl VerseControl { get; private set; }
        public IBibleTagController BibleTag { get; }
        public VerseGridForm() {
            InitializeComponent();
            this.Text = "Interlinear Bible Editor";

            Uow = new UnitOfWork();

            Translation = new XPQuery<Translation>(Uow).Where(x => x.Name == NAME).FirstOrDefault();
            txtIndex.EditValue = "NPI.";

            LoadBooks();

            VerseControl = new VerseGridControl() { Dock = DockStyle.Fill };
            pnlContent.Controls.Add(VerseControl);

            TransliterationController = new GreekTransliterationController();
            BibleTag = new BibleTagController();
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

        private void LoadBooks(Translation translation = null) {
            if (translation == null) { translation = Translation; }
            var view = new XPView(Uow, typeof(BookBase)) {
                CriteriaString = $"[Status.BookType] = {(int)translation.BookType}"
            };
            view.Properties.Add(new ViewProperty("NumberOfBook", SortDirection.None, "[NumberOfBook]", false, true));
            view.Properties.Add(new ViewProperty("BookTitle", SortDirection.None, "[BookTitle]", false, true));

            var view2 = new XPView(Uow, typeof(Book)) {
                CriteriaString = $"[ParentTranslation.Oid] = {translation.Oid}"
            };
            view2.Properties.Add(new ViewProperty("NumberOfBook", SortDirection.None, "[NumberOfBook]", false, true));
            var numberOfBooks = new List<int>();
            foreach (ViewRecord item in view2) {
                numberOfBooks.Add(item["NumberOfBook"].ToInt());
            }

            var list = new List<BookBaseInfo>();
            foreach (ViewRecord item in view) {
                var numberOfBook = item["NumberOfBook"].ToInt();
                if (!numberOfBooks.Contains(numberOfBook)) { continue; }
                var bookTitle = item["BookTitle"].ToString();
                list.Add(new BookBaseInfo() {
                    NumberOfBook = numberOfBook,
                    BookTitle = bookTitle
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
                    btnExportChapterToPdf.Enabled =
                        btnExportChapterToPdfTables.Enabled =
                        btnExportChapterToDocx.Enabled =
                        btnExportChapterToDocxTables.Enabled =
                        btnExportBookToDocx.Enabled =
                        btnExportBookToDocxTables.Enabled =
                        btnExportBookToPdf.Enabled =
                        btnExportBookToPdfTables.Enabled =
                        btnNextVerse.Enabled =
                        btnAutoTranslateChapter.Enabled =
                        btnAutoTranslateVerse.Enabled = true;

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
                System.Diagnostics.Process.Start("explorer.exe", currentControl.Verse.GetOblubienicaUrl());
            }
        }

        private void btnExportChapterToPdf_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            Export(ExportSaveFormat.Pdf, ExportMethodType.TextBoxes);
        }
        private void btnExportChapterToDocx_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            Export(ExportSaveFormat.Docx, ExportMethodType.TextBoxes);
        }
        private void btnExportChapterToPdfTables_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            Export(ExportSaveFormat.Pdf, ExportMethodType.Tables);
        }
        private void btnExportChapterToDocxTables_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            Export(ExportSaveFormat.Docx, ExportMethodType.Tables);
        }

        private void Export(ExportSaveFormat format, ExportMethodType method) {
            var currentControl = VerseControl;
            if (currentControl.IsNotNull()) {
                Chapter chapter = currentControl.Verse.ParentChapter;
                if (chapter.IsNotNull()) {
                    mnuExportChapter.Enabled = false;
                    btnProgress.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    Application.DoEvents();

                    var host = System.Configuration.ConfigurationManager.AppSettings["Host"];
                    var licPath = System.Configuration.ConfigurationManager.AppSettings["AsposeLic"];
                    var licInfo = new System.IO.FileInfo(licPath);
                    byte[] licData = null;
                    if (licInfo.Exists) {
                        licData = System.IO.File.ReadAllBytes(licPath);
                    }
                    var outputPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + (format == ExportSaveFormat.Docx ? ".docx" : ".pdf"));
                    var task = ExportInterlinearChapter(licData, host, chapter, format, method, outputPath);
                    task.ContinueWith(t => {
                        this.SafeInvoke(x => {
                            x.btnProgress.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                            x.mnuExportChapter.Enabled = true;
                        });
                        if (t.Exception != null) {
                            this.SafeInvoke(x => {
                                XtraMessageBox.Show(t.Exception.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            });
                        }
                        else if (outputPath != null && File.Exists(outputPath)) {
                            this.SafeInvoke(x => {
                                x.ProcessStart(outputPath);
                            });
                        }
                    });
                }
            }
        }

        private async Task ExportInterlinearChapter(byte[] licData, string host, Chapter chapter, ExportSaveFormat format, ExportMethodType method, string outputPath) {
            if (method == ExportMethodType.TextBoxes) {
                await Task.Run(() => { new InterlinearExporter(licData, host).Export(chapter, format, outputPath); });
            }
            else if (method == ExportMethodType.Tables) {
                await Task.Run(() => { new InterlinearTableExporter(licData, host).ExportChapter(chapter, format, outputPath); });
            }
        }

        private void ExportBook(ExportSaveFormat format, ExportMethodType method) {
            if (VerseControl.IsNotNull()) {
                var book = VerseControl.Verse.ParentChapter.ParentBook;
                if (book.IsNotNull()) {
                    mnuExportBook.Enabled = false;
                    btnProgress.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    Application.DoEvents();

                    var host = System.Configuration.ConfigurationManager.AppSettings["Host"];
                    var licPath = System.Configuration.ConfigurationManager.AppSettings["AsposeLic"];
                    var licInfo = new System.IO.FileInfo(licPath);
                    byte[] licData = null;
                    if (licInfo.Exists) {
                        licData = System.IO.File.ReadAllBytes(licPath);
                    }
                    var outputPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + (format == ExportSaveFormat.Docx ? ".docx" : ".pdf"));
                    var task = ExportInterlinearBook(licData, host, book, format, method, outputPath);
                    task.ContinueWith(t => {
                        this.SafeInvoke(x => {
                            x.mnuExportBook.Enabled = true;
                            x.btnProgress.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                        });
                        if (t.Exception != null) {
                            this.SafeInvoke(x => {
                                XtraMessageBox.Show(t.Exception.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            });
                        }
                        else if (outputPath != null && File.Exists(outputPath)) {
                            this.SafeInvoke(x => {
                                x.ProcessStart(outputPath);
                            });
                        }
                    });
                }
            }
        }

        private async Task ExportInterlinearBook(byte[] licData, string host, Book book, ExportSaveFormat format, ExportMethodType method, string outputPath) {
            if (method == ExportMethodType.TextBoxes) {
                await Task.Run(() => { new InterlinearExporter(licData, host).Export(book, format, outputPath); });
            }
            else if (method == ExportMethodType.Tables) {
                await Task.Run(() => { new InterlinearTableExporter(licData, host).ExportBookTranslation(book, format, outputPath); });
            }
        }

        private void btnLogosSeptuagint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            if (VerseControl.IsNotNull()) {
                System.Diagnostics.Process.Start("explorer.exe", VerseControl.Verse.GetLogosSeptuagintUrl());
            }
        }

        private void btnExportBookToPdf_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            ExportBook(ExportSaveFormat.Pdf, ExportMethodType.TextBoxes);
        }

        private void btnExportBookToDocx_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            ExportBook(ExportSaveFormat.Docx, ExportMethodType.TextBoxes);
        }

        private void btnExportBookToDocxTables_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            ExportBook(ExportSaveFormat.Docx, ExportMethodType.Tables);
        }

        private void btnExportBookToPdfTables_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            ExportBook(ExportSaveFormat.Pdf, ExportMethodType.Tables);
        }

        private void btnAutoTranslateVerse_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            if (XtraMessageBox.Show("Do you want to auto-translate this verse to Polish?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {

                var dic = new XPQuery<AncientDictionaryItem>(Uow).ToList();

                Uow.BeginTransaction();

                var c = new GreekTransliterationController();

                foreach (var verseWord in VerseControl.Verse.VerseWords) {
                    var sourceWord = verseWord.SourceWord.RemoveAny(".", ":", ",", ";", "·", "—", "-", ")", "(", "]", "[", "’", ";", "\"", "?");

                    if (verseWord.Translation.IsNullOrEmpty()) {
                        var exactItem = dic.Where(x => x.Word == sourceWord.ToLower()).FirstOrDefault();
                        if (exactItem.IsNotNull()) {
                            var translation = String.Empty;
                            var _isUpper = System.Char.IsUpper(sourceWord[0]);
                            if (_isUpper && exactItem.Translation.IsNotNullOrEmpty() && exactItem.Translation.Length > 1) {
                                translation = exactItem.Translation.Substring(0, 1).ToUpper() + exactItem.Translation.Substring(1).ToLower();
                            }
                            else {
                                translation = exactItem.Translation.ToLower();
                            }
                            if (translation.IsNotNullOrEmpty()) {
                                translation = translation.RemoveAny(".", ":", ",", ";", "·", "—", "-", ")", "(", "]", "[", "’", ";", "\"", "?");
                                if (verseWord.SourceWord.EndsWith(",")) {
                                    translation += ",";
                                }
                                if (verseWord.SourceWord.EndsWith(";")) {
                                    translation += ";";
                                }
                                if (verseWord.SourceWord.EndsWith("·")) {
                                    translation += ":";
                                }
                                if (verseWord.SourceWord.EndsWith(".")) {
                                    translation += ".";
                                }

                                verseWord.Translation = translation;
                                verseWord.Save();

                                continue;
                            }
                        }



                        var w = c.GetSourceWordWithoutBreathAndAccent(sourceWord, out var isUpper);
                        var item = dic.Where(x => x.Word == w.ToLower()).FirstOrDefault();
                        if (item.IsNotNull()) {
                            var translation = String.Empty;
                            if (isUpper && item.Translation.IsNotNullOrEmpty() && item.Translation.Length > 1) {
                                translation = item.Translation.Substring(0, 1).ToUpper() + item.Translation.Substring(1).ToLower();
                            }
                            else {
                                translation = item.Translation.ToLower();
                            }
                            if (translation.IsNotNullOrEmpty()) {
                                translation = translation.RemoveAny(".", ":", ",", ";", "·", "—", "-", ")", "(", "]", "[", "’", ";", "\"", "?");
                                if (verseWord.SourceWord.EndsWith(",")) {
                                    translation += ",";
                                }
                                if (verseWord.SourceWord.EndsWith(";")) {
                                    translation += ";";
                                }
                                if (verseWord.SourceWord.EndsWith("·")) {
                                    translation += ":";
                                }
                                if (verseWord.SourceWord.EndsWith(".")) {
                                    translation += ".";
                                }

                                verseWord.Translation = translation;
                                verseWord.Save();
                            }
                        }
                    }
                }

                Uow.CommitChanges();

                VerseControl.LoadData(VerseControl.Verse, true);
            }
        }
        private void btnAutoTranslateChapter_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            if (XtraMessageBox.Show("Do you want to auto-translate this chapter to Polish?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {

                var verses = VerseControl.Verse.ParentChapter.Verses.OrderBy(x => x.NumberOfVerse).ToList();
                var dic = new XPQuery<AncientDictionaryItem>(Uow).ToList();

                Uow.BeginTransaction();

                var c = new GreekTransliterationController();

                foreach (var verse in verses.OrderBy(x => x.NumberOfVerse)) {
                    foreach (var verseWord in verse.VerseWords.OrderBy(x => x.NumberOfVerseWord)) {
                        var sourceWord = verseWord.SourceWord.RemoveAny(".", ":", ",", ";", "·", "—", "-", ")", "(", "]", "[", "’", ";", "\"", "?");

                        if (verseWord.Translation.IsNullOrEmpty()) {
                            var exactItem = dic.Where(x => x.Word == sourceWord.ToLower()).FirstOrDefault();
                            if (exactItem.IsNotNull()) {
                                var translation = String.Empty;
                                var _isUpper = sourceWord.IsNotNullOrEmpty() ? Char.IsUpper(sourceWord[0]) : false;
                                if (_isUpper && exactItem.Translation.IsNotNullOrEmpty() && exactItem.Translation.Length > 1) {
                                    translation = exactItem.Translation.Substring(0, 1).ToUpper() + exactItem.Translation.Substring(1).ToLower();
                                }
                                else {
                                    translation = exactItem.Translation.ToLower();
                                }
                                if (translation.IsNotNullOrEmpty()) {
                                    translation = translation.RemoveAny(".", ":", ",", ";", "·", "—", "-", ")", "(", "]", "[", "’", ";", "\"", "?");
                                    if (verseWord.SourceWord.EndsWith(",")) {
                                        translation += ",";
                                    }
                                    if (verseWord.SourceWord.EndsWith(";")) {
                                        translation += ";";
                                    }
                                    if (verseWord.SourceWord.EndsWith("·")) {
                                        translation += ":";
                                    }
                                    if (verseWord.SourceWord.EndsWith(".")) {
                                        translation += ".";
                                    }

                                    verseWord.Translation = translation;
                                    verseWord.Save();

                                    continue;
                                }
                            }

                            var w = c.GetSourceWordWithoutBreathAndAccent(sourceWord, out var isUpper);
                            var item = dic.Where(x => x.Word == w.ToLower()).FirstOrDefault();
                            if (item.IsNotNull()) {
                                var translation = String.Empty;
                                if (isUpper && item.Translation.IsNotNullOrEmpty() && item.Translation.Length > 1) {
                                    translation = item.Translation.Substring(0, 1).ToUpper() + item.Translation.Substring(1).ToLower();
                                }
                                else {
                                    translation = item.Translation.ToLower();
                                }
                                if (translation.IsNotNullOrEmpty()) {
                                    translation = translation.RemoveAny(".", ":", ",", ";", "·", "—", "-", ")", "(", "]", "[", "’", ";", "\"", "?");
                                    if (verseWord.SourceWord.EndsWith(",")) {
                                        translation += ",";
                                    }
                                    if (verseWord.SourceWord.EndsWith(";")) {
                                        translation += ";";
                                    }
                                    if (verseWord.SourceWord.EndsWith("·")) {
                                        translation += ":";
                                    }
                                    if (verseWord.SourceWord.EndsWith(".")) {
                                        translation += ".";
                                    }

                                    verseWord.Translation = translation;
                                    verseWord.Save();
                                }
                            }
                        }
                    }
                }

                VerseControl.Verse.ParentChapter.IsTranslated = true;

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

                if (verse.IsNull()) {
                    if (!text.StartsWith("NPI ")) { text = $"NPI {text}"; }
                    verse = BibleTag.GetRecognizedSiglumVerse(Uow, text);
                }

                if (verse.IsNotNull()) {
                    var verseNumber = verse.NumberOfVerse;

                    if (VerseControl.Verse.IsNotNull() && VerseControl.IsModified()) {
                        if (XtraMessageBox.Show("Do you want to save your changes before opening a new verse?", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                            VerseControl.Save();
                        }
                    }

                    VerseControl.LoadData(verse, Translation.BookType == TheBookType.Bible);
                    btnExportChapterToPdf.Enabled =
                       btnExportChapterToPdfTables.Enabled =
                       btnExportChapterToDocx.Enabled =
                       btnExportChapterToDocxTables.Enabled =
                       btnExportBookToDocx.Enabled =
                       btnExportBookToDocxTables.Enabled =
                       btnExportBookToPdf.Enabled =
                       btnExportBookToPdfTables.Enabled =
                       btnNextVerse.Enabled =
                       btnAutoTranslateChapter.Enabled =
                       btnAutoTranslateVerse.Enabled = true;

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
                    if (bookInfo == null) {
                        this.Translation = verse.ParentTranslation;
                        NAME = Translation.Name;
                        LoadBooks();
                        bookInfo = (txtBooks.Properties.DataSource as List<BookBaseInfo>).Where(x => x.NumberOfBook == book.NumberOfBook).FirstOrDefault();
                    }
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
            if (VerseControl.IsNotNull()) {
                if (XtraMessageBox.Show("Delete words", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                    VerseControl.DeleteWord();
                }
            }
        }

        private void btnMarkWordAsUncentrain_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            if (VerseControl.IsNotNull()) {
                VerseControl.SetSelectedWordAsUncentrain(true);
            }
        }

        private void btnMarkAllWordsAsUncentrain_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            if (VerseControl.IsNotNull()) {
                VerseControl.SetAllWordsAsUncentrain(false);
            }
        }

        private void ProcessStart(string path) {
            var process = new System.Diagnostics.Process();
            process.StartInfo.FileName = path;
            process.StartInfo.UseShellExecute = true;
            process.Start();
        }
    }
}
