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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IBE.WindowsClient {
    public partial class TranslationEditForm : RibbonForm {
        private int Index = 1;
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


                TreeItems = new List<IbeTreeItem>();
                var root = new IbeTreeItem() {
                    ID = $"Translation_{Object.Oid}",
                    ParentID = null,
                    Text = Object.Name.IsNotNullOrEmpty() ? Object.Name : "Root",
                    Tag = Object.Oid
                };
                TreeItems.Add(root);

                foreach (var item in Object.Books) {
                    var book = new IbeTreeItem() {
                        ID = $"Book_{item.Oid}",
                        ParentID = root.ID,
                        Text = item.BookName,
                        Tag = item.Oid
                    };

                    foreach (var _item in item.Chapters) {
                        var chapter = new IbeTreeItem() {
                            ID = $"Chapter{ _item.Oid}",
                            ParentID = book.ID,
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
                                Value = __item.Text
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
                var record = treeList.GetDataRecordByNode(e.OldNode) as IbeVerseTreeItem;
                if (record.IsNotNull()) {
                    if (editor.Text != record.Value) {
                        record.Value = editor.Text;
                        record.Changed = true;
                    }
                }
            }

            if (e.Node.IsNotNull()) {
                var _record = treeList.GetDataRecordByNode(e.Node) as IbeTreeItem;
                var record = treeList.GetDataRecordByNode(e.Node) as IbeVerseTreeItem;
                if (record.IsNotNull()) {
                    btnAddBook.Enabled = false;
                    btnAddChapter.Enabled = false;
                    btnAddVerse.Enabled = false;

                    editor.Text = record.Value;
                    editor.Tag = record;
                }

                if (_record.IsNotNull()) {
                    if (_record.ParentID.IsNull()) {
                        btnAddBook.Enabled = true;
                        btnAddChapter.Enabled = false;
                        btnAddVerse.Enabled = false;
                    }
                    else if (_record.ID.StartsWith("Book")) {
                        btnAddBook.Enabled = false;
                        btnAddChapter.Enabled = true;
                        btnAddVerse.Enabled = false;
                    }
                    else if (_record.ID.StartsWith("Chapter")) {
                        btnAddBook.Enabled = false;
                        btnAddChapter.Enabled = false;
                        btnAddVerse.Enabled = true;
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
                    var item = new IbeTreeItem() {
                        Text = response,
                        ParentID = root.ID,
                        ID = $"Book_New_{Index}",
                        Tag = -1
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
                    var item = new IbeTreeItem() {
                        Text = response,
                        ParentID = book.ID,
                        ID = $"Chapter_New_{Index}",
                        Tag = -1
                    };
                    Index++;
                    TreeItems.Add(item);
                    treeList.RefreshDataSource();

                    treeList.FocusedNode.Expand();

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
                        Value = String.Empty
                    };
                    Index++;
                    TreeItems.Add(item);
                    treeList.RefreshDataSource();

                    treeList.FocusedNode.Expand();
                }
            }
        }
    }

    public class IbeTreeItem {
        public string ID { get; set; }
        public string ParentID { get; set; }
        public string Text { get; set; }
        public IbeTreeItem() { }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Tag { get; set; }
    }

    public class IbeVerseTreeItem : IbeTreeItem {
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Value { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Changed { get; set; }
    }
}
