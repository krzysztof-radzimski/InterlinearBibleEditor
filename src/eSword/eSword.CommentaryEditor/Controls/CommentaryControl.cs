using DevExpress.Xpo;
using DevExpress.XtraEditors;
using eSword.CommentaryEditor.Db.Model;
using System;
using System.Linq;

namespace eSword.CommentaryEditor.Controls {
    public partial class CommentaryControl : XtraUserControl {
        public Commentary Commentary { get; private set; }
        public Book Book { get; private set; }
        public int Chapter { get; private set; }

        public bool AllowAddCommentaryRange { get { return true; } }
        public bool AllowRemoveCommentaryRange { get { return false; } }

        public CommentaryControl() {
            InitializeComponent();
        }
        public CommentaryControl(Commentary commentary, Book book, int chapter) : this() {
            this.Commentary = commentary;
            this.Book = book;
            this.Chapter = chapter;
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            lbl.Caption = Chapter == 0 ? $"Introduction to {Book.Title}" : $"{Book.Title} {Chapter}";

            if (Chapter != 0) {
                LoadRanges();
            }
            else {
                if (!Commentary.Items.Where(x => x.Book == Book.NumberOfBook && x.ChapterBegin == Chapter).Any()) {
                    var item = new CommentaryItem(Commentary.Session) {
                        Book = this.Book.NumberOfBook,
                        ChapterBegin = 0,
                        ChapterEnd = 0,
                        VerseBegin = 0,
                        VerseEnd = 0,
                        Comments = String.Empty,
                        ParentCommentary = Commentary
                    };
                    item.Save();
                    Commentary.Save();
                    (Commentary.Session as UnitOfWork).CommitChanges();
                }

                LoadRanges();
            }
        }

        private void LoadRanges() {
            this.grid.DataSource = Commentary.Items.Where(x => x.Book == Book.NumberOfBook && x.ChapterBegin == Chapter);
            this.view.BestFitColumns();

            if (this.view.RowCount > 0) {
                this.view.FocusedRowHandle = 0;
            }
        }

        public void AddCommentaryRange() {
            CommentaryItem item = null;
            var chapter = new XPQuery<Chapter>(Commentary.Session).Where(x => x.NumberOfBook == Book.NumberOfBook && x.NumberOfChapter == Chapter).FirstOrDefault();
            var items = Commentary.Items.Where(x => x.Book == Book.NumberOfBook && x.ChapterBegin == Chapter);
            if (items.Count() == 0) {
                item = new CommentaryItem(Commentary.Session) {
                    Book = Book.NumberOfBook,
                    ChapterBegin = Chapter,
                    ChapterEnd = Chapter,
                    Comments = String.Empty,
                    ParentCommentary = Commentary,
                    VerseBegin = 1,
                    VerseEnd = chapter.NumberOfVerses
                };
            }
            else {
                item = new CommentaryItem(Commentary.Session) {
                    Book = Book.NumberOfBook,
                    ChapterBegin = Chapter,
                    ChapterEnd = Chapter,
                    Comments = String.Empty,
                    ParentCommentary = Commentary,
                    VerseBegin = items.Max(x => x.VerseEnd) + 1,
                    VerseEnd = chapter.NumberOfVerses
                };
            }

            using (var dlg = new CommentaryItemDialog(Book, item)) {
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    item.Save();

                    LoadRanges();
                }
            }
        }
        public void RemoveCommentaryRange() {

        }
        public void Save() {
            var item = editor.Tag as CommentaryItem;
            if (item != null) {
                item.Comments = editor.RtfText;
                item.Save();
            }
        }

        private void view_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e) {
            var item = view.GetRow(e.FocusedRowHandle) as CommentaryItem;
            if (item != null) {
                if (editor.Enabled && editor.Tag != null) {
                    // save prev
                    Save();
                }

                editor.Enabled = true;
                editor.RtfText = item.Comments;
                editor.Tag = item;
            }
        }
    }
}
