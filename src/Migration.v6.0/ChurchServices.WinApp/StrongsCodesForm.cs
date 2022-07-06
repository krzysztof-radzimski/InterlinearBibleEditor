using DevExpress.Xpo;
using DevExpress.XtraEditors;
using ChurchServices.Extensions;
using ChurchServices.Data.Model;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ChurchServices.WinApp {
    public partial class StrongsCodesForm : XtraForm {
        public StrongsCodesForm() {
            InitializeComponent();
            view.ShowLoadingPanel();
        }

        private void StrongsCodesForm_Load(object sender, EventArgs e) {
            LoadData();
        }

        private void LoadData(Action action = null) {
            var task = Task.Factory.StartNew(() => {
                return new XPQuery<StrongCode>(new UnitOfWork()).Where(x => x.Lang == Language.Greek).OrderBy(x => x.Transliteration);
            });
            task.ContinueWith(x => {
                this.SafeInvoke(f => {
                    f.grid.DataSource = x.Result;
                    f.view.BestFitColumns();
                    f.view.HideLoadingPanel();
                    if (action != null) { action(); }
                });
            });
        }

        private void view_DoubleClick(object sender, EventArgs e) {
            var strongCode = view.GetFocusedRow() as StrongCode;
            if (strongCode != null) {
                using (var dlg = new StrongsCodeEditForm()) {
                    dlg.LoadData(strongCode);
                    if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                        dlg.SaveData();
                        LoadData(() => {
                            int rowHandle = view.LocateByValue("FullCode", strongCode.FullCode);
                            if (rowHandle != DevExpress.XtraGrid.GridControl.InvalidRowHandle)
                                view.FocusedRowHandle = rowHandle;
                        });
                    }
                }
            }
        }
    }
}
