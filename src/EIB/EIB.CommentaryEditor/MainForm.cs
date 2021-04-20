using DevExpress.Xpo;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors;
using DevExpress.XtraRichEdit;
using EIB.CommentaryEditor.Controls;
using EIB.CommentaryEditor.Db.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.XtraSplashScreen;
using EIB.CommentaryEditor.Db;

namespace EIB.CommentaryEditor {
    public partial class MainForm : DevExpress.XtraBars.FluentDesignSystem.FluentDesignForm, IOverlaySplashScreenHandle {
        private UnitOfWork uow;
        private const string TITLE = "EIB Commentary Editor";
        public Commentary Commentary { get; private set; }
        protected MainForm() {
            InitializeComponent();
            this.IconOptions.SvgImage = Properties.Resources.logo_eib;
            this.Text = TITLE;
            this.uow = new UnitOfWork();
            LoadBooks();
        }
        public MainForm(int commentaryOid) : this() {
            Commentary = new XPQuery<Commentary>(uow).Where(x => x.Oid == commentaryOid).FirstOrDefault();
            this.Text = $"{TITLE} - {Commentary.Title}";
        }

        private void LoadBooks() {
            aceOT.Elements.Clear();
            aceNT.Elements.Clear();

            var books = new XPQuery<Book>(uow).OrderBy(x => x.NumberOfBook);
            foreach (var book in books) {
                var item = new AccordionControlElement(ElementStyle.Group) {
                    Text = book.Title,
                    Tag = book,
                    Name = $"ace{book.Abbreviation}"
                };

                item.Elements.Add(new AccordionControlElement(ElementStyle.Item) {
                    Text = $"Introduction to {book.Title}",
                    Tag = 0,
                    Name = $"ace{book.Abbreviation}Introduction"
                });

                for (int i = 1; i <= book.NumberOfChapters; i++) {
                    var itemChapter = new AccordionControlElement(ElementStyle.Item) {
                        Text = $"{book.Abbreviation} {i}",
                        Tag = i,
                        Name = $"ace{book.Abbreviation}{i}"
                    };

                    item.Elements.Add(itemChapter);
                }

                if (book.NumberOfBook < 40) {
                    aceOT.Elements.Add(item);
                }
                else {
                    aceNT.Elements.Add(item);
                }
            }
            aceOT.Expanded = false;
            aceNT.Expanded = true;
        }

        private void btnClose_ItemClick(object sender, ItemClickEventArgs e) {
            this.Close();
        }

        private void btnAddCommentaryRange_ItemClick(object sender, ItemClickEventArgs e) {
            var control = this.fluentDesignFormContainer.Controls[0] as CommentaryControl;
            if (control != null) {
                control.AddCommentaryRange();
            }
        }

        private void btnRemoveCommentaryRange_ItemClick(object sender, ItemClickEventArgs e) {
            var control = this.fluentDesignFormContainer.Controls[0] as CommentaryControl;
            if (control != null) {
                control.RemoveCommentaryRange();
            }
        }

        private void fluentDesignFormContainer_ControlAdded(object sender, ControlEventArgs e) {
            SetButtonsStatus();
        }

        private void SetButtonsStatus() {
            var activeControl = fluentDesignFormContainer.Controls[0] as CommentaryControl;
            btnAddCommentaryRange.Enabled = activeControl != null && activeControl.AllowAddCommentaryRange && activeControl.Chapter > 0;
            btnRemoveCommentaryRange.Enabled = activeControl != null && activeControl.AllowRemoveCommentaryRange && activeControl.Chapter > 0;
        }

        private void accordionControl_ElementClick(object sender, ElementClickEventArgs e) {
            var selected = e.Element;
            if (selected != null && selected.Style == ElementStyle.Item) {
                var name = $"commentaryControl_" + selected.Name;
                var control = this.fluentDesignFormContainer.Controls.Find(name, true).FirstOrDefault();
                if (control != null) {
                    control.BringToFront();
                }
                else {
                    control = new CommentaryControl(Commentary, selected.OwnerElement.Tag as Book, Convert.ToInt32(selected.Tag)) {
                        Dock = DockStyle.Fill,
                        Name = name
                    };
                    fluentDesignFormContainer.Controls.Add(control);
                    control.BringToFront();
                    Application.DoEvents();
                    SetButtonsStatus();
                }
            }
        }

        private void btnSave_ItemClick(object sender, ItemClickEventArgs e) {
            foreach (CommentaryControl item in fluentDesignFormContainer.Controls) {
                item.Save();
            }
            uow.CommitChanges();
        }

        private void ImportWordFile(string filePath) {
            IOverlaySplashScreenHandle handle = null;
            try {
                handle = SplashScreenManager.ShowOverlayForm(this);
                using (var editor = new RichEditControl()) {
                    editor.LoadDocument(filePath);
                    var fileName = Path.GetFileNameWithoutExtension(filePath);
                    var book = new XPQuery<Book>(uow).Where(x => x.Abbreviation == fileName).FirstOrDefault();
                    if (book == null) {
                        XtraMessageBox.Show("File name not match!");
                        return;
                    }

                    var rangeStartIndex = -1;
                    var rangeEndIndex = -1;

                    var chapter = 0;
                    var chapter2 = 0;

                    var verseRanges = new List<VerseRange>();
                    for (int i = 0; i < 5; i++) {
                        verseRanges.Add(new VerseRange() {
                            VerseBegin = -1,
                            VerseEnd = -1
                        });
                    }

                    Commentary.Items.Where(x => x.Book == book.NumberOfBook).ToList().ForEach(x => { x.Delete(); });
                    uow.CommitChanges();

                    foreach (var par in editor.Document.Paragraphs) {
                        var text = editor.Document.GetText(par.Range).Trim();
                        if (par.Style != null) {
                            if (!String.IsNullOrEmpty(text) && par.Style.Name == "heading 1") {
                                if (text.ToLower().StartsWith("wstęp ")) {
                                    rangeStartIndex = par.Range.Start.ToInt();
                                }
                                else if (rangeStartIndex != -1 && (text.ToLower().StartsWith("list ") || text.ToLower().StartsWith("ewangelia ") || text.ToLower().StartsWith("dzieje "))) {
                                    rangeEndIndex = par.Range.Start.ToInt() - 1;
                                    Commentary.Items.Add(new CommentaryItem(uow) {
                                        Book = book.NumberOfBook,
                                        ChapterBegin = 0,
                                        ChapterEnd = 0,
                                        VerseBegin = 0,
                                        VerseEnd = 0,
                                        Comments = editor.Document.GetRtfText(editor.Document.CreateRange(rangeStartIndex, rangeEndIndex - rangeStartIndex))
                                    });
                                    rangeStartIndex = -1;
                                    rangeEndIndex = -1;
                                }
                            }
                            else if (!String.IsNullOrEmpty(text) && par.Style.Name == "heading 2") {
                                if (rangeStartIndex == -1) { par.Range.Start.ToInt(); }
                            }
                            else if (!String.IsNullOrEmpty(text) && par.Style.Name == "heading 3") {

                                var patternBase = @"\((?<chapter>[0-9]+)(\,)?";
                                var patternRange = @"((?<verseBegin{0}>[0-9]+)([a-z])?(\-(?<verseEnd{0}>[0-9]+)([a-z])?)?(\.)?)";

                                var pattern = patternBase;
                                for (int i = 0; i < 5; i++) {
                                    pattern += String.Format(patternRange, i);
                                    pattern += "?";
                                }
                                pattern += @"\)";

                                if (text.Contains(";")) {
                                    pattern = @"\((?<chapter1>[0-9]+)\,(?<verse1>[0-9]+)\;(?<chapter2>[0-9]+)\,(?<verse2>[0-9]+)\)";
                                    if (Regex.IsMatch(text, pattern)) {
                                        var m = Regex.Match(text, pattern);
                                        chapter = Convert.ToInt32(m.Groups["chapter1"].Value);
                                        chapter2 = Convert.ToInt32(m.Groups["chapter2"].Value);
                                        verseRanges[0].VerseBegin = Convert.ToInt32(m.Groups["verse1"].Value);
                                        verseRanges[0].VerseEnd = Convert.ToInt32(m.Groups["verse2"].Value);
                                        for (int i = 1; i < 5; i++) {
                                            verseRanges[i].VerseBegin = -1;
                                            verseRanges[i].VerseEnd = -1;
                                        }
                                        continue;
                                    }
                                }

                                if (Regex.IsMatch(text, pattern)) {
                                    rangeEndIndex = SavePrevious(editor, book, rangeStartIndex, rangeEndIndex, chapter, chapter2, ref verseRanges, par);

                                    var m = Regex.Match(text, pattern);
                                    chapter = Convert.ToInt32(m.Groups["chapter"].Value);
                                    chapter2 = Convert.ToInt32(m.Groups["chapter"].Value);

                                    for (int i = 0; i < 5; i++) {
                                        if (m.Groups[$"verseEnd{i}"] != null && m.Groups[$"verseEnd{i}"].Success) {
                                            verseRanges[i].VerseBegin = Convert.ToInt32(m.Groups[$"verseBegin{i}"].Value);
                                            verseRanges[i].VerseEnd = Convert.ToInt32(m.Groups[$"verseEnd{i}"].Value);
                                        }
                                        else if (m.Groups[$"verseBegin{i}"] != null && m.Groups[$"verseBegin{i}"].Success) {
                                            verseRanges[i].VerseBegin = Convert.ToInt32(m.Groups[$"verseBegin{i}"].Value);
                                            verseRanges[i].VerseEnd = Convert.ToInt32(m.Groups[$"verseBegin{i}"].Value);
                                        }
                                        else if (chapter != 0 && i == 0) {
                                            verseRanges[i].VerseBegin = 0;
                                            verseRanges[i].VerseEnd = 0;
                                        }
                                        else {
                                            verseRanges[i].VerseBegin = -1;
                                            verseRanges[i].VerseEnd = -1;
                                        }
                                    }

                                    rangeStartIndex = par.Range.Start.ToInt();
                                    rangeEndIndex = -1;
                                }
                                else {
                                    if (rangeStartIndex == -1) { par.Range.Start.ToInt(); }
                                }
                            }
                        }

                        if (par == editor.Document.Paragraphs.Last()) {
                            rangeEndIndex = SavePrevious(editor, book, rangeStartIndex, rangeEndIndex, chapter, chapter2, ref verseRanges, par);
                        }
                    }

                    uow.CommitChanges();
                    Commentary = new XPQuery<Commentary>(uow).Where(x => x.Oid == Commentary.Oid).FirstOrDefault();
                    LoadBooks();
                }
            }
            finally {
                SplashScreenManager.CloseOverlayForm(handle);
            }
        }

        private int SavePrevious(RichEditControl editor, Book book, int rangeStartIndex, int rangeEndIndex,
            int chapter, int chapter2, ref List<VerseRange> verseRanges,
            DevExpress.XtraRichEdit.API.Native.Paragraph par) {
            if (rangeStartIndex != par.Range.Start.ToInt() && chapter != 0) {

                if (par == editor.Document.Paragraphs.Last()) {
                    rangeEndIndex = par.Range.End.ToInt();
                }
                else {
                    rangeEndIndex = par.Range.Start.ToInt() - 1;
                }

                foreach (var verseRange in verseRanges) {
                    if (verseRange.VerseBegin != -1 && verseRange.VerseEnd != -1) {
                        Commentary.Items.Add(new CommentaryItem(uow) {
                            Book = book.NumberOfBook,
                            ChapterBegin = chapter,
                            ChapterEnd = chapter2,
                            VerseBegin = verseRange.VerseBegin,
                            VerseEnd = verseRange.VerseEnd,
                            Comments = editor.Document.GetRtfText(editor.Document.CreateRange(rangeStartIndex, rangeEndIndex - rangeStartIndex))
                        });

                        verseRange.VerseBegin = -1;
                        verseRange.VerseEnd = -1;
                    }
                }
            }

            return rangeEndIndex;
        }

        private void btnImportWordFile_ItemClick(object sender, ItemClickEventArgs e) {
            using (var dlg = new OpenFileDialog() { Filter = "Microsoft Word file (*.docx)|*.docx" }) {
                if (dlg.ShowDialog() == DialogResult.OK) {
                    ImportWordFile(dlg.FileName);
                }
            }
        }

        public void QueueCloseUpAction(Action closeUpAction) {

        }

        public bool QueueFocus(IntPtr controlHandle) {
            return true;
        }

        public bool QueueFocus(Control control) {
            return true;
        }

        private void btnSaveAsCMTX_ItemClick(object sender, ItemClickEventArgs e) {
            using (var dlg = new SaveFileDialog() { Filter = "e-Sword Commentary file (*.cmtx)|*.cmtx" }) {
                if (dlg.ShowDialog() == DialogResult.OK) {
                    IOverlaySplashScreenHandle handle = null;
                    try {
                        handle = SplashScreenManager.ShowOverlayForm(this);
                        new eSwordExportHelper().ExportCmtx(Commentary, dlg.FileName);
                    }
                    finally {
                        SplashScreenManager.CloseOverlayForm(handle);
                    }
                }
            }
        }
    }

    class VerseRange {
        public int VerseBegin { get; set; }
        public int VerseEnd { get; set; }
    }
}
