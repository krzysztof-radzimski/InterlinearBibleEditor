using DevExpress.Xpo;
using DevExpress.XtraEditors;
using EIB.CommentaryEditor.Db.Model;
using System.Data;
using System.Linq;

namespace EIB.CommentaryEditor {
    public partial class CommentaryItemDialog : XtraForm {
        public CommentaryItem Item { get; private set; }

        protected CommentaryItemDialog() {
            InitializeComponent();
            this.IconOptions.SvgImage = Properties.Resources.logo_eib;
        }
        public CommentaryItemDialog(Book book, CommentaryItem item) : this() {
            this.Item = item;

            txtChapterBegin.Properties.MinValue = 1;
            txtChapterBegin.Properties.MaxValue = book.NumberOfChapters;
            txtChapterEnd.Properties.MinValue = 1;
            txtChapterEnd.Properties.MaxValue = book.NumberOfChapters;

            var chapter = new XPQuery<Chapter>(item.Session).Where(x => x.NumberOfBook == book.NumberOfBook && x.NumberOfChapter == item.ChapterBegin).FirstOrDefault();
            txtVerseBegin.Properties.MinValue = 1;
            txtVerseEnd.Properties.MinValue = 1;

            if (chapter != null) {
                txtVerseBegin.Properties.MaxValue = chapter.NumberOfVerses;
                txtVerseEnd.Properties.MaxValue = chapter.NumberOfVerses;
            }

            txtChapterBegin.EditValue = item.ChapterBegin;
            txtChapterEnd.EditValue = item.ChapterEnd;
            txtVerseBegin.EditValue = item.VerseBegin;
            txtVerseEnd.EditValue = item.VerseEnd;
        }

        private void btnApply_Click(object sender, System.EventArgs e) {
            Item.ChapterBegin = (int)txtChapterBegin.Value;
            Item.ChapterEnd = (int)txtChapterEnd.Value;
            Item.VerseBegin = (int)txtVerseBegin.Value;
            Item.VerseEnd = (int)txtVerseEnd.Value;
          
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}