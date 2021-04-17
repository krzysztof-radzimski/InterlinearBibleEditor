using DevExpress.Xpo;
using DevExpress.XtraEditors;
using EIB.CommentaryEditor.Db.Model;
using System;
using System.Linq;
using System.Windows.Forms;

namespace EIB.CommentaryEditor {
    public partial class ChooseCommentary : XtraForm {
        private UnitOfWork uow;
        public Commentary Commentary { get; private set; }
        public ChooseCommentary() {
            InitializeComponent();
            this.IconOptions.SvgImage = Properties.Resources.logo_eib;
            uow = new UnitOfWork();
            grid.DataSource = new XPQuery<Commentary>(uow).ToList();
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

        private void btnAddNew_Click(object sender, EventArgs e) {
            var commentary = new Commentary(uow) {
                Abbreviation = String.Empty,
                Information = String.Empty,
                Version = 4,
                Title = String.Empty
            };
            using (var dlg = new CommentaryDialog(commentary)) {
                if (dlg.ShowDialog() == DialogResult.OK) {
                    if (!String.IsNullOrEmpty(commentary.Title)) {
                        commentary.Save();
                        uow.CommitChanges();

                        grid.DataSource = new XPQuery<Commentary>(uow).ToList();
                        view.BestFitColumns();
                    }
                }
            }
        }
    }
}