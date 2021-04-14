using DevExpress.Xpo;
using DevExpress.XtraEditors;
using eSword.CommentaryEditor.Db.Model;
using System;
using System.Linq;
using System.Windows.Forms;

namespace eSword.CommentaryEditor {
    public partial class ChooseCommentary : XtraForm {
        public Commentary Commentary { get; private set; }
        public ChooseCommentary() {
            InitializeComponent();
            this.IconOptions.SvgImage = Properties.Resources.bible;
            grid.DataSource = new XPQuery<Commentary>(new UnitOfWork()).ToList();
            view.BestFitColumns();
        }

        private void view_DoubleClick(object sender, EventArgs e) {
            Commentary = view.GetFocusedRow() as Commentary;
            DialogResult = DialogResult.OK;
        }

        private void btnApply_Click(object sender, EventArgs e) {
            Commentary = view.GetFocusedRow() as Commentary;
            DialogResult = DialogResult.OK;
        }
    }
}