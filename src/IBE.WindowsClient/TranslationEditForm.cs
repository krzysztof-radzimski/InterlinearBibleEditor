using DevExpress.Xpo;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraRichEdit.Services;
using IBE.Common.Extensions;
using IBE.Data.Model;
using IBE.WindowsClient.Controllers;
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
    public partial class TranslationEditForm : RibbonForm {
        private int Index = 1;
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
                txtBaseBook.Properties.DataSource = new XPQuery<BookBase>(Object.Session);

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

                LoadTree();
                // tabTranslationContent.PageVisible = false;
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

                foreach (var item in Object.Books) {
                    var book = new IbeBookTreeItem() {
                        ID = $"Book_{item.Oid}",
                        ParentID = root.ID,
                        Number = item.NumberOfBook,
                        Text = item.BookName,
                        Tag = item.Oid
                    };

                    foreach (var _item in item.Chapters) {
                        var chapter = new IbeChapterTreeItem() {
                            ID = $"Chapter{ _item.Oid}",
                            ParentID = book.ID,
                            Number = _item.NumberOfChapter,
                            Text = _item.NumberOfChapter.ToString(),
                            Tag = _item.Oid
                        };

                        foreach (var __item in _item.Verses) {
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
                                StartFromNewLine = __item.StartFromNewLine
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

            // save changes in content
            //foreach (var item in TreeItems) {
            //    if (item.Text.Contains("_New_")) {
            //        if (item.Type == IbeTreeItemType.Book) {
            //            var book = new Book(uow) {
            //                BookName = item.Text,

            //            };
            //        }
            //    }
            //}

            // refresh translations window
            try {
                if (ObjectSaved.IsNotNull()) { ObjectSaved(Object, EventArgs.Empty); }
            }
            catch { }
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            Save();
        }

        private void treeList_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e) {
            if (e.OldNode.IsNotNull()) {
                var oldVerseRecord = treeList.GetDataRecordByNode(e.OldNode) as IbeVerseTreeItem;
                if (oldVerseRecord.IsNotNull()) {
                    if ((tabVerse.Tag as IbeVerseTreeItem) == oldVerseRecord) {
                        if (editor.Text != oldVerseRecord.Value) {
                            oldVerseRecord.Value = editor.Text;
                            oldVerseRecord.Changed = true;
                        }
                        if (oldVerseRecord.StartFromNewLine != cbStartFromNewLine.Checked) {
                            oldVerseRecord.StartFromNewLine = cbStartFromNewLine.Checked;
                            oldVerseRecord.Changed = true;
                        }
                        if (oldVerseRecord.Number != txtNumberOfVerse.Text.ToInt()) {
                            oldVerseRecord.Number = txtNumberOfVerse.Text.ToInt();
                            oldVerseRecord.Changed = true;
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
                        ParentID = root.ID,
                        ID = $"Book_New_{Index}",
                        Tag = -1,
                        Number = response.ToInt()
                    };
                    Index++;
                    TreeItems.Add(item);
                    treeList.RefreshDataSource();
                }
            }
        }

        private void btnAddChapter_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var book = treeList.GetDataRecordByNode(treeList.FocusedNode) as IbeTreeItem;
            if (book.ID.StartsWith("Book")) {
                var response = XtraInputBox.Show("Type chapter number (arabic):", "Add chapter", "", MessageBoxButtons.OKCancel);
                if (response.IsNotNullOrEmpty()) {
                    var item = new IbeChapterTreeItem() {
                        Text = response,
                        ParentID = book.ID,
                        ID = $"Chapter_New_{Index}",
                        Tag = -1,
                        Number = response.ToInt(),
                    };
                    Index++;
                    TreeItems.Add(item);
                    treeList.RefreshDataSource();

                    treeList.FocusedNode.Expand();
                    treeList.FocusedNode = treeList.FindNodeByKeyID(item.ID);
                }
            }
        }

        private void btnAddVerse_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var chapter = treeList.GetDataRecordByNode(treeList.FocusedNode) as IbeTreeItem;
            if (chapter.ID.StartsWith("Chapter")) {
                var response = XtraInputBox.Show("Type verse number (arabic):", "Add verse", "", MessageBoxButtons.OKCancel);
                if (response.IsNotNullOrEmpty()) {
                    var item = new IbeVerseTreeItem() {
                        Text = response,
                        ParentID = chapter.ID,
                        ID = $"Verse_New_{Index}",
                        Tag = -1,
                        Value = String.Empty,
                        Number = response.ToInt(),
                        StartFromNewLine = false
                    };
                    Index++;
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

                if (Object.Type == TranslationType.Interlinear) {
                    tabVerseInterlinear.Controls.Clear();
                    var v = new XPQuery<Verse>(Object.Session).Where(x => x.Oid == e.ID.ToInt()).FirstOrDefault();
                    tabVerseInterlinear.Controls.Add(new VerseEditorControl(v, false) { Dock = DockStyle.Fill });
                    tabs.SelectedTabPage = tabVerseInterlinear;
                    tabs.Visible = true;
                }
                else {
                    tabs.SelectedTabPage = tabVerse;
                    tabs.Visible = true;

                    txtNumberOfVerse.Text = e.Number.ToString();
                    cbStartFromNewLine.Checked = e.StartFromNewLine;
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

                tabs.SelectedTabPage = tabChapter;
                tabs.Visible = true;

                txtNumberOfChapter.Text = e.Number.ToString();
                tabChapter.Tag = e;
            }
        }

        private void LoadBook(IbeBookTreeItem e) {
            btnAddBook.Enabled = false;
            btnAddChapter.Enabled = true;
            btnAddVerse.Enabled = false;

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
            cbIsTranslated.Checked = e.IsTranslated;
            txtTimeOfWriting.Text = e.TimeOfWriting;
            txtPlaceWhereBookWasWritten.Text = e.PlaceWhereBookWasWritten;
            if (e.BaseBook.IsNotNull()) {
                txtBaseBook.EditValue = BaseBooks.Where(x => x.Id == e.BaseBook.Id);
            }
            tabBook.Tag = e;
        }
    }

    public enum IbeTreeItemType { Root, Book, Chapter, Verse }

    public abstract class IbeTreeItem {
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Number { get; set; }

        public string ID { get; set; }
        public string ParentID { get; set; }
        public string Text { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Tag { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IbeTreeItemType Type { get; protected set; }

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
        public IbeBookTreeItem() {
            Type = IbeTreeItemType.Book;
        }
    }

    public class IbeChapterTreeItem : IbeTreeItem {
        public IbeChapterTreeItem() {
            Type = IbeTreeItemType.Chapter;
        }
    }

    public class IbeVerseTreeItem : IbeTreeItem {
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool StartFromNewLine { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Value { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Changed { get; set; }

        public IbeVerseTreeItem() {
            Type = IbeTreeItemType.Verse;
        }
    }

    public class IbeBaseBookItem {
        public int Id { get; set; }
        public string Text { get; set; }
        public IbeBaseBookItem() { }
        public override string ToString() {
            return Text;
        }
    }
}
