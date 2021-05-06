using DevExpress.Xpo;
using DevExpress.XtraEditors;
using IBE.Common.Extensions;
using IBE.Data.Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace IBE.WindowsClient {
    public partial class GrammarCodeFindDialog : XtraForm {
        public GrammarCode Selected { get; private set; }
        private GrammarCodeFindDialog() {
            InitializeComponent();
        }

        public GrammarCodeFindDialog(GrammarCode grammarCode) : this() {
            LoadData(grammarCode.Session);
            var list = grid.DataSource as List<GrammarCode>;
            if (grammarCode.IsNotNull()) {
                Selected = grammarCode;
                view.FocusedRowHandle = view.FindRow(list.Where(x => x.Oid == grammarCode.Oid).FirstOrDefault());
            }
        }
        public GrammarCodeFindDialog(Session session) : this() {
            LoadData(session);
        }

        private void LoadData(Session session) {
            grid.DataSource = new XPQuery<GrammarCode>(session).OrderBy(x => x.GrammarCodeVariant1).ToList();
            view.BestFitColumns();
        }

        private void view_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e) {
            Selected = view.GetFocusedRow() as GrammarCode;
        }

        private void view_DoubleClick(object sender, System.EventArgs e) {
            Selected = view.GetFocusedRow() as GrammarCode;
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}