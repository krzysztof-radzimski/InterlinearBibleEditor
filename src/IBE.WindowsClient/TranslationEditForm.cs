using DevExpress.Xpo;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraRichEdit.Services;
using IBE.Common.Extensions;
using IBE.Data.Model;
using IBE.WindowsClient.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace IBE.WindowsClient {
    public partial class TranslationEditForm : RibbonForm {
        //private int Index = 1;
        private List<IbeBaseBookItem> BaseBooks;

        public event EventHandler ObjectSaved;
        public Translation Object { get; private set; }
        public List<IbeTreeItem> TreeItems { get; private set; }

        public TranslationEditForm() {
            InitializeComponent();

            editor.ReplaceService<ISyntaxHighlightService>(new HTMLSyntaxHighlightService(editor));
            editor.Document.DefaultCharacterProperties.FontName = "Consolas";
            editor.Document.DefaultCharacterProperties.FontSize = 12;

            txtIntroduction.ReplaceService<ISyntaxHighlightService>(new HTMLSyntaxHighlightService(txtIntroduction));
            txtIntroduction.Document.DefaultCharacterProperties.FontName = "Consolas";
            txtIntroduction.Document.DefaultCharacterProperties.FontSize = 12;

            txtDetailedInfo.ReplaceService<ISyntaxHighlightService>(new HTMLSyntaxHighlightService(txtDetailedInfo));
            txtDetailedInfo.Document.DefaultCharacterProperties.FontName = "Consolas";
            txtDetailedInfo.Document.DefaultCharacterProperties.FontSize = 12;

            txtPreface.ReplaceService<ISyntaxHighlightService>(new HTMLSyntaxHighlightService(txtPreface));
            txtPreface.Document.DefaultCharacterProperties.FontName = "Consolas";
            txtPreface.Document.DefaultCharacterProperties.FontSize = 12;

            this.Text = "Translation editor";
            txtBookType.Properties.DataSource = typeof(TheBookType).GetEnumValues().OfType<TheBookType>();
            foreach (var lang in Enum.GetValues(typeof(Language))) {
                txtLanguage.Properties.Items.Add(lang);
            }
            foreach (var type in Enum.GetValues(typeof(TranslationType))) {
                txtType.Properties.Items.Add(type);
            }

            btnAddBook.Enabled = false;
            btnAddChapter.Enabled = false;
            btnAddVerse.Enabled = false;
            btnAddVerses.Enabled = false;
            btnRecognizeChapterContent.Enabled = false;

            tabs.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
        }

        public TranslationEditForm(int id, UnitOfWork session) : this() {
            if (id != 0 && session.IsNotNull()) {
                Object = new XPQuery<Translation>(session).Where(x => x.Oid == id).FirstOrDefault();
                BindObject();
            }
        }
        public TranslationEditForm(Translation trans) : this() {
            if (trans.IsNotNull()) {
                Object = trans;
                BindObject();
            }
        }

        private void BindObject() {
            if (Object.IsNotNull()) {
                this.Text = $"Translation editor :: {Object.Name}";

                txtName.DataBindings.Add("EditValue", Object, "Name");
                txtDescription.DataBindings.Add("EditValue", Object, "Description");
                txtChapterPsalmString.DataBindings.Add("EditValue", Object, "ChapterPsalmString");
                txtChapterString.DataBindings.Add("EditValue", Object, "ChapterString");
                txtDetailedInfo.DataBindings.Add("Text", Object, "DetailedInfo");
                txtIntroduction.DataBindings.Add("Text", Object, "Introduction");
                txtLanguage.DataBindings.Add("EditValue", Object, "Language");
                txtType.DataBindings.Add("EditValue", Object, "Type");
                cbIsCatholic.DataBindings.Add("EditValue", Object, "Catolic");
                cbIsRecommended.DataBindings.Add("EditValue", Object, "Recommended");
                cbOpenAccess.DataBindings.Add("EditValue", Object, "OpenAccess");
                txtBookType.DataBindings.Add("EditValue", Object, "BookType");
                cbWithGrammarCodes.DataBindings.Add("EditValue", Object, "WithGrammarCodes");
                cbWithStrongs.DataBindings.Add("EditValue", Object, "WithStrongs");
                cbChapterRomanNumbering.DataBindings.Add("EditValue", Object, "ChapterRomanNumbering");
                cbHidden.DataBindings.Add("EditValue", Object, "Hidden");

                LoadTree();
            }
        }

        private void LoadTree() {
            if (Object.IsNotNull()) {
                BaseBooks = new List<IbeBaseBookItem>();
                foreach (var bb in new XPQuery<BookBase>(Object.Session).OrderBy(x => x.NumberOfBook)) {
                    BaseBooks.Add(new IbeBaseBookItem() {
                        Id = bb.Oid,
                        Text = bb.BookTitle
                    });
                }
                txtBaseBook.Properties.DataSource = BaseBooks;

                TreeItems = new List<IbeTreeItem>();
                var root = new IbeRootTreeItem() {
                    ID = $"Translation_{Object.Oid}",
                    ParentID = null,
                    Text = Object.Name.IsNotNullOrEmpty() ? Object.Name : "Root",
                    Tag = Object.Oid
                };
                TreeItems.Add(root);

                foreach (var item in Object.Books.OrderBy(x => x.NumberOfBook)) {
                    var _bb = BaseBooks.Where(x => x.Id == item.BaseBook.Oid).FirstOrDefault();
                    var book = new IbeBookTreeItem() {
                        ID = $"Book_{item.Oid}",
                        ParentID = root.ID,
                        Number = item.NumberOfBook,
                        Text = item.BookName,
                        Tag = item.Oid,
                        AuthorName = item.AuthorName,
                        BaseBook = _bb,
                        BookName = item.BookName,
                        BookShortcut = item.BookShortcut,
                        BookTitle = _bb.Text,
                        Color = item.Color,
                        IsNew = false,
                        IsTranslated = item.IsTranslated,
                        Changed = false,
                        PlaceWhereBookWasWritten = item.PlaceWhereBookWasWritten,
                        Preface = item.Preface,
                        Purpose = item.Purpose,
                        Subject = item.Subject,
                        TimeOfWriting = item.TimeOfWriting,
                        NumberOfChapters = item.NumberOfChapters
                    };

                    foreach (var _item in item.Chapters.OrderBy(x => x.NumberOfChapter)) {
                        var chapter = new IbeChapterTreeItem() {
                            ID = $"Chapter{ _item.Oid}",
                            ParentID = book.ID,
                            Number = _item.NumberOfChapter,
                            Text = _item.NumberOfChapter.ToString(),
                            Tag = _item.Oid,
                            IsNew = false,
                            Changed = false,
                            IsTranslated = _item.IsTranslated,
                            NumberOfVerses = _item.NumberOfVerses
                        };

                        foreach (var __item in _item.Verses.OrderBy(x => x.NumberOfVerse)) {
#if DEBUG
                            //if (item.NumberOfBook == 470 && _item.NumberOfChapter == 1 && __item.NumberOfVerse == 18) 
                            //    { 

                            //}
#endif

                            var subtitles = _item.Subtitles.ToList();
                            var subtitle1 = subtitles.Where(x => x.BeforeVerseNumber == __item.NumberOfVerse && x.Level == 1).FirstOrDefault();
                            var subtitle2 = subtitles.Where(x => x.BeforeVerseNumber == __item.NumberOfVerse && x.Level == 2).FirstOrDefault();

                            var len = __item.Text.IsNotNullOrEmpty() ? __item.Text.Length : 0;
                            var __text = __item.Text;
                            if (len > 40) { __text = __text.Substring(0, 40) + "..."; }
                            __text = $"{__item.NumberOfVerse}. {__text}";
                            var verse = new IbeVerseTreeItem() {
                                ID = $"Verse_{__item.Oid}",
                                ParentID = chapter.ID,
                                Text = __text,
                                Tag = __item.Oid,
                                Value = __item.Text,
                                Number = __item.NumberOfVerse,
                                StartFromNewLine = __item.StartFromNewLine,
                                SubtitleLevel1 = subtitle1.IsNotNull() ? subtitle1.Text : String.Empty,
                                SubtitleLevel2 = subtitle2.IsNotNull() ? subtitle2.Text : String.Empty,
                                IsNew = false,
                                Changed = false
                            };

                            TreeItems.Add(verse);
                        }

                        TreeItems.Add(chapter);
                    }

                    TreeItems.Add(book);
                }

                treeList.DataSource = TreeItems;

                treeList.ExpandToLevel(0);
            }
        }

        public void Save() {
            this.Text = $"Translation editor :: {Object.Name}";
            Object.Save();
            var uow = Object.Session as UnitOfWork;
            uow.CommitChanges();

            foreach (var item in TreeItems.Where(x => x.Type == IbeTreeItemType.Book)) {
                var bookItem = item as IbeBookTreeItem;
                if (item.IsNew) {
                    var book = new Book(uow) {
                        AuthorName = bookItem.AuthorName,
                        BaseBook = new XPQuery<BookBase>(uow).Where(x => x.Oid == bookItem.BaseBook.Id).FirstOrDefault(),
                        BookName = bookItem.BookName,
                        BookShortcut = bookItem.BookShortcut,
                        Color = bookItem.Color,
                        IsTranslated = bookItem.IsTranslated,
                        NumberOfBook = bookItem.Number,
                        ParentTranslation = Object,
                        PlaceWhereBookWasWritten = bookItem.PlaceWhereBookWasWritten,
                        Preface = bookItem.Preface,
                        Purpose = bookItem.Purpose,
                        Subject = bookItem.Subject,
                        TimeOfWriting = bookItem.TimeOfWriting,
                        NumberOfChapters = bookItem.NumberOfChapters
                    };
                    book.Save();
                    uow.CommitChanges();
                    uow.ReloadChangedObjects();

                    bookItem.IsNew = false;
                    bookItem.Changed = false;
                    bookItem.Tag = book.Oid;
                    // bookItem.ID = $"Book_{book.Oid}";
                }
                else if (item.Changed) {
                    var book = new XPQuery<Book>(uow).Where(x => x.Oid == bookItem.Tag).FirstOrDefault();
                    book.AuthorName = bookItem.AuthorName;
                    book.BaseBook = new XPQuery<BookBase>(uow).Where(x => x.Oid == bookItem.BaseBook.Id).FirstOrDefault();
                    book.BookName = bookItem.BookName;
                    book.BookShortcut = bookItem.BookShortcut;
                    book.Color = bookItem.Color;
                    book.IsTranslated = bookItem.IsTranslated;
                    book.NumberOfBook = bookItem.Number;
                    book.PlaceWhereBookWasWritten = bookItem.PlaceWhereBookWasWritten;
                    book.Preface = bookItem.Preface;
                    book.Purpose = bookItem.Purpose;
                    book.Subject = bookItem.Subject;
                    book.TimeOfWriting = bookItem.TimeOfWriting;
                    book.NumberOfChapters = bookItem.NumberOfChapters;

                    book.Save();
                    uow.CommitChanges();

                    bookItem.Changed = false;
                }
            }

            foreach (var item in TreeItems.Where(x => x.Type == IbeTreeItemType.Chapter)) {
                var chapterItem = item as IbeChapterTreeItem;
                if (item.IsNew) {

                    var parentBookItem = TreeItems.Where(x => x.ID == chapterItem.ParentID).FirstOrDefault() as IbeBookTreeItem;

                    var chapter = new Chapter(uow) {
                        IsTranslated = chapterItem.IsTranslated,
                        NumberOfChapter = chapterItem.Number,
                        NumberOfVerses = chapterItem.NumberOfVerses,
                        ParentBook = new XPQuery<Book>(uow).Where(x => x.Oid == parentBookItem.Tag).FirstOrDefault()
                    };
                    chapter.Save();
                    uow.CommitChanges();
                    uow.ReloadChangedObjects();

                    chapterItem.IsNew = false;
                    chapterItem.Changed = false;
                    chapterItem.Tag = chapter.Oid;
                    // chapterItem.ID = $"Chapter_{chapter.Oid}";
                }
                else if (item.Changed) {
                    var chapter = new XPQuery<Chapter>(uow).Where(x => x.Oid == chapterItem.Tag).FirstOrDefault();
                    chapter.IsTranslated = chapterItem.IsTranslated;
                    chapter.NumberOfChapter = chapterItem.Number;
                    chapter.NumberOfVerses = chapterItem.NumberOfVerses;

                    chapter.Save();
                    uow.CommitChanges();

                    chapterItem.Changed = false;
                }
            }

            foreach (var item in TreeItems.Where(x => x.Type == IbeTreeItemType.Verse)) {
                var verseItem = item as IbeVerseTreeItem;
                if (item.IsNew) {
                    var parentChapterItem = TreeItems.Where(x => x.ID == verseItem.ParentID).FirstOrDefault() as IbeChapterTreeItem;

                    var verse = new Verse(uow) {
                        NumberOfVerse = verseItem.Number,
                        StartFromNewLine = verseItem.StartFromNewLine,
                        Text = verseItem.Value,
                        ParentChapter = new XPQuery<Chapter>(uow).Where(x => x.Oid == parentChapterItem.Tag).FirstOrDefault()
                    };

                    SetSubtitle(verse, verseItem, uow, 1);
                    SetSubtitle(verse, verseItem, uow, 2);
                    //if (verseItem.SubtitleLevel1.IsNotNullOrEmpty()) {
                    //    var subtitle1 = verse.ParentChapter.Subtitles.Where(x => x.BeforeVerseNumber == verse.NumberOfVerse && x.Level == 1).FirstOrDefault();
                    //    if (subtitle1.IsNotNull()) {
                    //        subtitle1.BeforeVerseNumber = verse.NumberOfVerse;
                    //        subtitle1.ParentChapter = verse.ParentChapter;
                    //        subtitle1.Level = 1;
                    //        subtitle1.Text = verseItem.SubtitleLevel1;
                    //    }
                    //    else {
                    //        subtitle1 = new Subtitle(uow) {
                    //            BeforeVerseNumber = verse.NumberOfVerse,
                    //            ParentChapter = verse.ParentChapter,
                    //            Level = 1,
                    //            Text = verseItem.SubtitleLevel1
                    //        };
                    //    }
                    //    subtitle1.Save();
                    //}
                    //if (verseItem.SubtitleLevel2.IsNotNullOrEmpty()) {
                    //    var subtitle2 = verse.ParentChapter.Subtitles.Where(x => x.BeforeVerseNumber == verse.NumberOfVerse && x.Level == 2).FirstOrDefault();
                    //    if (subtitle2.IsNotNull()) {
                    //        subtitle2.BeforeVerseNumber = verse.NumberOfVerse;
                    //        subtitle2.ParentChapter = verse.ParentChapter;
                    //        subtitle2.Level = 1;
                    //        subtitle2.Text = verseItem.SubtitleLevel2;
                    //    }
                    //    else {
                    //        subtitle2 = new Subtitle(uow) {
                    //            BeforeVerseNumber = verse.NumberOfVerse,
                    //            ParentChapter = verse.ParentChapter,
                    //            Level = 2,
                    //            Text = verseItem.SubtitleLevel2
                    //        };
                    //    }
                    //    subtitle2.Save();
                    //}

                    verse.Save();
                    uow.CommitChanges();
                    uow.ReloadChangedObjects();

                    verseItem.IsNew = false;
                    verseItem.Changed = false;
                    verseItem.Tag = verse.Oid;
                }
                else if (item.Changed) {
                    var verse = new XPQuery<Verse>(uow).Where(x => x.Oid == verseItem.Tag).FirstOrDefault();
                    verse.StartFromNewLine = verseItem.StartFromNewLine;
                    verse.NumberOfVerse = verseItem.Number;
                    verse.Text = verseItem.Value;

                    SetSubtitle(verse, verseItem, uow, 1);
                    SetSubtitle(verse, verseItem, uow, 2);

                    verse.Save();
                    uow.CommitChanges();

                    verseItem.Changed = false;
                }
            }

            // refresh translations window
            try {
                if (ObjectSaved.IsNotNull()) { ObjectSaved(Object, EventArgs.Empty); }
            }
            catch { }
        }

        private void SetSubtitle(Verse verse, IbeVerseTreeItem verseItem, UnitOfWork uow, int level = 1) {
            var subtitleText = level == 1 ? verseItem.SubtitleLevel1 : verseItem.SubtitleLevel2;
            if (subtitleText.IsNotNullOrEmpty()) {
                var subtitle = verse.ParentChapter.Subtitles.Where(x => x.BeforeVerseNumber == verse.NumberOfVerse && x.Level == level).FirstOrDefault();
                if (subtitle.IsNotNull()) {
                    subtitle.BeforeVerseNumber = verse.NumberOfVerse;
                    subtitle.ParentChapter = verse.ParentChapter;
                    subtitle.Level = level;
                    subtitle.Text = subtitleText;
                }
                else {
                    subtitle = new Subtitle(uow) {
                        BeforeVerseNumber = verse.NumberOfVerse,
                        ParentChapter = verse.ParentChapter,
                        Level = level,
                        Text = subtitleText
                    };
                }
                subtitle.Save();
            }
            else {
                // delete subtitle if exists
                var subtitle = verse.ParentChapter.Subtitles.Where(x => x.BeforeVerseNumber == verse.NumberOfVerse && x.Level == level).FirstOrDefault();
                if (subtitle.IsNotNull()) {
                    subtitle.Delete();
                }
            }
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            Save();
        }

        private void TreeListFocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e) {
            if (e.OldNode.IsNotNull()) {
                IbeTreeItem record = null;
                var oldRecord = treeList.GetDataRecordByNode(e.OldNode) as IbeTreeItem;
                if (oldRecord.IsNotNull()) {
                    switch (oldRecord.Type) {
                        case IbeTreeItemType.Book: { record = tabBook.Tag as IbeTreeItem; break; }
                        case IbeTreeItemType.Chapter: { record = tabChapter.Tag as IbeTreeItem; break; }
                        case IbeTreeItemType.Verse: { record = tabVerse.Tag as IbeTreeItem; break; }
                    }
                }
                if (oldRecord.IsNotNull() && record.IsNotNull()) {

                    if (record.Type == IbeTreeItemType.Verse && tabVerse.Tag == oldRecord && Object.Type != TranslationType.Interlinear) {
                        var verse = oldRecord as IbeVerseTreeItem;
                        if (editor.Text != verse.Value) {
                            verse.Value = editor.Text;
                            verse.Changed = true;
                        }
                        if (verse.StartFromNewLine != cbStartFromNewLine.Checked) {
                            verse.StartFromNewLine = cbStartFromNewLine.Checked;
                            verse.Changed = true;
                        }
                        if (verse.Number != txtNumberOfVerse.Text.ToInt()) {
                            verse.Number = txtNumberOfVerse.Text.ToInt();
                            verse.Changed = true;
                        }
                        if (verse.SubtitleLevel1 != txtSubtitleLevel1.Text) {
                            verse.SubtitleLevel1 = txtSubtitleLevel1.Text;
                            verse.Changed = true;
                        }
                        if (verse.SubtitleLevel2 != txtSubtitleLevel2.Text) {
                            verse.SubtitleLevel2 = txtSubtitleLevel2.Text;
                            verse.Changed = true;
                        }
                    }
                    else if (record.Type == IbeTreeItemType.Chapter && tabChapter.Tag == oldRecord) {
                        var chapter = oldRecord as IbeChapterTreeItem;
                        if (txtNumberOfChapter.Text.ToInt() != chapter.Number) {
                            chapter.Number = txtNumberOfChapter.Text.ToInt();
                            chapter.Changed = true;
                        }
                        if (cbChapterIsTranslated.Checked != chapter.IsTranslated) {
                            chapter.IsTranslated = cbChapterIsTranslated.Checked;
                            chapter.Changed = true;
                        }
                        if (txtChapterNumberOfVerses.Text.ToInt() != chapter.NumberOfVerses) {
                            chapter.NumberOfVerses = txtChapterNumberOfVerses.Text.ToInt();
                            chapter.Changed = true;
                        }
                    }
                    else if (record.Type == IbeTreeItemType.Book && tabBook.Tag == oldRecord) {
                        var book = oldRecord as IbeBookTreeItem;
                        if (txtBookColor.Text != book.Color) {
                            book.Color = txtBookColor.Text;
                            book.Changed = true;
                        }
                        if (txtBookName.Text != book.BookName) {
                            book.BookName = txtBookName.Text;
                            book.Changed = true;
                        }
                        if (txtBookShortcut.Text != book.BookShortcut) {
                            book.BookShortcut = txtBookShortcut.Text;
                            book.Changed = true;
                        }
                        if (txtAuthorName.Text != book.AuthorName) {
                            book.AuthorName = txtAuthorName.Text;
                            book.Changed = true;
                        }
                        if (txtBookTitle.Text != book.BookTitle) {
                            book.BookTitle = txtBookTitle.Text;
                            book.Changed = true;
                        }
                        if (txtNumberOfBook.Text.ToInt() != book.Number) {
                            book.Number = txtNumberOfBook.Text.ToInt();
                            book.Changed = true;
                        }
                        if (txtPlaceWhereBookWasWritten.Text != book.PlaceWhereBookWasWritten) {
                            book.PlaceWhereBookWasWritten = txtPlaceWhereBookWasWritten.Text;
                            book.Changed = true;
                        }
                        if (txtPreface.Text != book.Preface) {
                            book.Preface = txtPreface.Text;
                            book.Changed = true;
                        }
                        if (txtPurpose.Text != book.Purpose) {
                            book.Purpose = txtPurpose.Text;
                            book.Changed = true;
                        }
                        if (txtSubject.Text != book.Subject) {
                            book.Subject = txtSubject.Text;
                            book.Changed = true;
                        }
                        if (txtTimeOfWriting.Text != book.TimeOfWriting) {
                            book.TimeOfWriting = txtTimeOfWriting.Text;
                            book.Changed = true;
                        }
                        if (cbBookIsTranslated.Checked != book.IsTranslated) {
                            book.IsTranslated = cbBookIsTranslated.Checked;
                            book.Changed = true;
                        }
                        if ((txtBaseBook.EditValue as IbeBaseBookItem) != book.BaseBook) {
                            book.BaseBook = txtBaseBook.EditValue as IbeBaseBookItem;
                            book.Changed = true;
                        }
                        if (txtBookNumberOfChapters.Text.ToInt() != book.NumberOfChapters) {
                            book.NumberOfChapters = txtBookNumberOfChapters.Text.ToInt();
                            book.Changed = true;
                        }
                    }
                }
            }

            if (e.Node.IsNotNull()) {
                var _record = treeList.GetDataRecordByNode(e.Node) as IbeTreeItem;
                if (_record.IsNotNull()) {
                    if (_record.Type == IbeTreeItemType.Verse) {
                        LoadVerse(_record as IbeVerseTreeItem);
                    }
                    else if (_record.Type == IbeTreeItemType.Root) {
                        btnAddBook.Enabled = true;
                        btnAddChapter.Enabled = false;
                        btnAddVerse.Enabled = false;
                        btnAddVerses.Enabled = false;
                        btnDeleteBook.Enabled = false;
                        btnRecognizeChapterContent.Enabled = false;
                        tabs.Visible = false;
                    }
                    else if (_record.Type == IbeTreeItemType.Book) {
                        LoadBook(_record as IbeBookTreeItem);
                    }
                    else if (_record.Type == IbeTreeItemType.Chapter) {
                        LoadChapter(_record as IbeChapterTreeItem);
                    }
                }
            }
        }

        private void treeList_NodeChanged(object sender, DevExpress.XtraTreeList.NodeChangedEventArgs e) {
            if (e.Node.IsNotNull()) { }
        }

        private void btnAddBook_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var root = TreeItems.FirstOrDefault();
            if (root.IsNotNull()) {
                var response = XtraInputBox.Show("Type book name:", "Add book", "", MessageBoxButtons.OKCancel);
                if (response.IsNotNullOrEmpty()) {
                    var item = new IbeBookTreeItem() {
                        Text = response,
                        BookName = response,
                        ParentID = root.ID,
                        ID = $"{Guid.NewGuid()}",
                        IsNew = true,
                        Tag = -1,
                        Number = response.ToInt()
                    };
                    TreeItems.Add(item);
                    treeList.RefreshDataSource();
                }
            }
        }

        private void btnAddChapter_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var book = treeList.GetDataRecordByNode(treeList.FocusedNode) as IbeTreeItem;
            if (book.Type == IbeTreeItemType.Book) {
                var response = XtraInputBox.Show("Type chapter number (arabic):", "Add chapter", "", MessageBoxButtons.OKCancel);
                if (response.IsNotNullOrEmpty()) {
                    var item = new IbeChapterTreeItem() {
                        Text = response,
                        ParentID = book.ID,
                        ID = $"{Guid.NewGuid()}",
                        IsNew = true,
                        Tag = -1,
                        Number = response.ToInt(),
                    };
                    TreeItems.Add(item);
                    treeList.RefreshDataSource();

                    treeList.FocusedNode.Expand();
                    treeList.FocusedNode = treeList.FindNodeByKeyID(item.ID);
                }
            }
        }

        private void btnAddVerse_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var chapter = treeList.GetDataRecordByNode(treeList.FocusedNode) as IbeTreeItem;
            if (chapter.Type == IbeTreeItemType.Chapter) {
                var response = XtraInputBox.Show("Type verse number (arabic):", "Add verse", "", MessageBoxButtons.OKCancel);
                if (response.IsNotNullOrEmpty()) {
                    var item = new IbeVerseTreeItem() {
                        Text = response,
                        ParentID = chapter.ID,
                        ID = $"{Guid.NewGuid()}",
                        IsNew = true,
                        Tag = -1,
                        Value = String.Empty,
                        Number = response.ToInt(),
                        StartFromNewLine = false
                    };
                    TreeItems.Add(item);
                    treeList.RefreshDataSource();

                    treeList.FocusedNode.Expand();
                    treeList.FocusedNode = treeList.FindNodeByKeyID(item.ID);
                }
            }
        }

        private void LoadVerse(IbeVerseTreeItem e) {
            if (e.IsNotNull()) {
                btnAddBook.Enabled = false;
                btnAddChapter.Enabled = false;
                btnAddVerse.Enabled = false;
                btnAddVerses.Enabled = false;
                btnDeleteBook.Enabled = false;
                btnRecognizeChapterContent.Enabled = false;

                if (Object.Type == TranslationType.Interlinear && !e.IsNew) {
                    var v = new XPQuery<Verse>(Object.Session).Where(x => x.Oid == e.Tag.ToInt()).FirstOrDefault();
                    //var frm = new InterlinearEditorForm(v);
                    var frm = new VerseGridForm(v) {
                        MdiParent = this.MdiParent
                    };
                    frm.Show();
                }
                else {
                    tabs.SelectedTabPage = tabVerse;
                    tabs.Visible = true;

                    txtNumberOfVerse.Text = e.Number.ToString();
                    cbStartFromNewLine.Checked = e.StartFromNewLine;
                    txtSubtitleLevel1.Text = e.SubtitleLevel1;
                    txtSubtitleLevel2.Text = e.SubtitleLevel2;
                    editor.Text = e.Value;
                }
                tabVerse.Tag = e;
            }
        }

        private void LoadChapter(IbeChapterTreeItem e) {
            if (e.IsNotNull()) {
                btnAddBook.Enabled = false;
                btnAddChapter.Enabled = false;
                btnAddVerse.Enabled = true;
                btnAddVerses.Enabled = true;
                btnRecognizeChapterContent.Enabled = true;
                btnDeleteBook.Enabled = false;

                tabs.SelectedTabPage = tabChapter;
                tabs.Visible = true;

                txtNumberOfChapter.Text = e.Number.ToString();
                txtChapterNumberOfVerses.Text = e.NumberOfVerses.ToString();
                cbChapterIsTranslated.Checked = e.IsTranslated;
                tabChapter.Tag = e;
            }
        }

        private void LoadBook(IbeBookTreeItem e) {
            btnDeleteBook.Enabled = true;
            btnAddBook.Enabled = false;
            btnAddChapter.Enabled = true;
            btnAddVerse.Enabled = false;
            btnAddVerses.Enabled = false;
            btnRecognizeChapterContent.Enabled = false;

            tabs.SelectedTabPage = tabBook;
            tabs.Visible = true;

            txtNumberOfBook.Text = e.Number.ToString();
            txtBookColor.Text = e.Color;
            txtAuthorName.Text = e.AuthorName;
            txtBookName.Text = e.BookName;
            txtBookShortcut.Text = e.BookShortcut;
            txtBookTitle.Text = e.BookTitle;
            txtPreface.Text = e.Preface;
            txtSubject.Text = e.Subject;
            cbBookIsTranslated.Checked = e.IsTranslated;
            txtTimeOfWriting.Text = e.TimeOfWriting;
            txtPlaceWhereBookWasWritten.Text = e.PlaceWhereBookWasWritten;
            txtBookNumberOfChapters.Text = e.NumberOfChapters.ToString();
            if (e.BaseBook.IsNotNull()) {
                txtBaseBook.EditValue = BaseBooks.Where(x => x.Id == e.BaseBook.Id).FirstOrDefault();
            }
            tabBook.Tag = e;
        }

        private void txtBaseBook_EditValueChanged(object sender, EventArgs e) {
            if (txtNumberOfBook.Text.ToInt() == 0) {
                var baseBookItem = txtBaseBook.EditValue as IbeBaseBookItem;
                var uow = Object.Session as UnitOfWork;
                var baseBook = new XPQuery<BookBase>(uow).Where(x => x.Oid == baseBookItem.Id).FirstOrDefault();
                if (baseBook.IsNotNull()) {
                    txtNumberOfBook.Text = baseBook.NumberOfBook.ToString();
                    txtBookColor.Text = baseBook.Color;
                    txtAuthorName.Text = baseBook.AuthorName;
                    txtBookName.Text = baseBook.BookName;
                    txtBookShortcut.Text = baseBook.BookShortcut;
                    txtBookTitle.Text = baseBook.BookTitle;
                    txtPreface.Text = baseBook.Preface;
                    txtSubject.Text = baseBook.Subject;
                    txtTimeOfWriting.Text = baseBook.TimeOfWriting;
                    txtPlaceWhereBookWasWritten.Text = baseBook.PlaceWhereBookWasWritten;
                }
            }
        }

        private void btnAddChapters_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var book = treeList.GetDataRecordByNode(treeList.FocusedNode) as IbeTreeItem;
            if (book.Type == IbeTreeItemType.Book) {
                var response = XtraInputBox.Show("Type chapters count:", "Add chapters", "", MessageBoxButtons.OKCancel);
                if (response.IsNotNullOrEmpty()) {
                    for (int i = 1; i < response.ToInt() + 1; i++) {
                        var item = new IbeChapterTreeItem() {
                            Text = i.ToString(),
                            ParentID = book.ID,
                            ID = $"{Guid.NewGuid()}",
                            IsNew = true,
                            Tag = -1,
                            Number = i,
                        };
                        TreeItems.Add(item);
                    }

                    treeList.RefreshDataSource();
                    treeList.FocusedNode.Expand();
                }
            }
        }

        private void btnAddVerses_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var chapter = treeList.GetDataRecordByNode(treeList.FocusedNode) as IbeTreeItem;
            if (chapter.Type == IbeTreeItemType.Chapter) {
                var response = XtraInputBox.Show("Type verse count:", "Add verses", "1", MessageBoxButtons.OKCancel);
                if (response.IsNotNullOrEmpty()) {
                    for (int i = 1; i < response.ToInt() + 1; i++) {
                        var item = new IbeVerseTreeItem() {
                            Text = i.ToString(),
                            ParentID = chapter.ID,
                            ID = $"{Guid.NewGuid()}",
                            IsNew = true,
                            Tag = -1,
                            Value = String.Empty,
                            Number = i,
                            StartFromNewLine = false
                        };
                        TreeItems.Add(item);
                    }

                    (chapter as IbeChapterTreeItem).NumberOfVerses += response.ToInt();
                    txtChapterNumberOfVerses.Text = (chapter as IbeChapterTreeItem).NumberOfVerses.ToString();
                    chapter.Changed = true;

                    treeList.RefreshDataSource();
                    treeList.FocusedNode.Expand();
                }
            }
        }

        private void btnDeleteBook_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            if (XtraMessageBox.Show("Delete book?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                var book = treeList.GetDataRecordByNode(treeList.FocusedNode) as IbeTreeItem;
                if (book.IsNew) {
                    treeList.DeleteNode(treeList.FocusedNode);
                }
                else {
                    var theBook = Object.Books.Where(x => x.Oid == book.Tag).FirstOrDefault();
                    if (theBook.IsNotNull()) {
                        foreach (var chapter in theBook.Chapters) {
                            foreach (var verse in chapter.Verses) {
                                foreach (var word in verse.VerseWords) {
                                    word.Delete();
                                }
                                verse.Delete();
                            }
                            chapter.Delete();
                        }
                        theBook.Delete();
                        (Object.Session as UnitOfWork).CommitChanges();
                        treeList.ClearNodes();
                        LoadTree();
                    }
                }
            }
        }

        private void btnRecognizeChapterContent_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var chapter = treeList.GetDataRecordByNode(treeList.FocusedNode) as IbeTreeItem;
            if (chapter.Type == IbeTreeItemType.Chapter) {
                using (var dlg = new RecognizeChapterContentForm()) {
                    if (dlg.ShowDialog() == DialogResult.OK) {
                        var result = dlg.GetRecognizedVerses();
                        if (result.Count > 0) {
                            foreach (var recognizedVerse in result) {
                                var __text = recognizedVerse.Content;
                                if (__text.Length > 40) { __text = __text.Substring(0, 40) + "..."; }
                                var item = new IbeVerseTreeItem() {
                                    Text = $"{recognizedVerse.Number}. {__text}",
                                    ParentID = chapter.ID,
                                    ID = $"{Guid.NewGuid()}",
                                    IsNew = true,
                                    Tag = -1,
                                    Value = recognizedVerse.Content,
                                    Number = recognizedVerse.Number,
                                    StartFromNewLine = false
                                };
                                TreeItems.Add(item);
                            }

                            (chapter as IbeChapterTreeItem).NumberOfVerses = result.Count;
                            txtChapterNumberOfVerses.Text = (chapter as IbeChapterTreeItem).NumberOfVerses.ToString();
                            chapter.Changed = true;

                            treeList.RefreshDataSource();
                            treeList.FocusedNode.Expand();
                        }
                    }
                }
            }
        }
    }

    public enum IbeTreeItemType { Root, Book, Chapter, Verse }

    public abstract class IbeTreeItem {
        [Browsable(false)] [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] public int Number { get; set; }

        public string ID { get; set; }
        public string ParentID { get; set; }
        public string Text { get; set; }

        [Browsable(false)] [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] public int Tag { get; set; }

        [Browsable(false)] [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] public IbeTreeItemType Type { get; protected set; }

        [Browsable(false)] [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] public bool Changed { get; set; }

        [Browsable(false)] [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] public bool IsNew { get; set; }

        public IbeTreeItem() { }
    }

    public class IbeRootTreeItem : IbeTreeItem {
        public IbeRootTreeItem() {
            Type = IbeTreeItemType.Root;
        }
    }

    public class IbeBookTreeItem : IbeTreeItem {
        public IbeBaseBookItem BaseBook { get; set; }
        public string BookShortcut { get; set; }
        public string BookName { get; set; }
        public string BookTitle { get; set; }
        public string Color { get; set; }
        public string AuthorName { get; set; }
        public string TimeOfWriting { get; set; }
        public string PlaceWhereBookWasWritten { get; set; }
        public string Purpose { get; set; }
        public string Subject { get; set; }
        public string Preface { get; set; }
        public bool IsTranslated { get; set; }
        public int NumberOfChapters { get; set; }
        public IbeBookTreeItem() {
            Type = IbeTreeItemType.Book;
        }
    }

    public class IbeChapterTreeItem : IbeTreeItem {
        public bool IsTranslated { get; set; }
        public int NumberOfVerses { get; set; }
        public IbeChapterTreeItem() {
            Type = IbeTreeItemType.Chapter;
        }
    }

    public class IbeVerseTreeItem : IbeTreeItem {
        [Browsable(false)] [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] public bool StartFromNewLine { get; set; }

        [Browsable(false)] [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] public string Value { get; set; }

        [Browsable(false)] [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] public string SubtitleLevel1 { get; set; }
        [Browsable(false)] [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] public string SubtitleLevel2 { get; set; }

        public IbeVerseTreeItem() { Type = IbeTreeItemType.Verse; }
    }

    public class IbeBaseBookItem {
        public int Id { get; set; }
        public string Text { get; set; }
        public IbeBaseBookItem() { }
        public override string ToString() => Text;
    }
}
