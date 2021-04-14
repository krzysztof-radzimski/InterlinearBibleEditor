using DevExpress.Xpo;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Navigation;
using eSword.CommentaryEditor.Controls;
using eSword.CommentaryEditor.Db.Model;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace eSword.CommentaryEditor {
    public partial class MainForm : DevExpress.XtraBars.FluentDesignSystem.FluentDesignForm {
        private UnitOfWork uow;
        private const string TITLE = "e-Sword Commentary Editor";
        public Commentary Commentary { get; }
        protected MainForm() {
            InitializeComponent();
            this.IconOptions.SvgImage = Properties.Resources.bible;
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

        private void ItemChapter_Click(object sender, EventArgs e) {

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
    }
}
