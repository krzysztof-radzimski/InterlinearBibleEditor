using DevExpress.Xpo;
using DevExpress.XtraEditors;
using IBE.Common.Extensions;
using IBE.Data.Model;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace IBE.WindowsClient {
    public partial class StrongsCodesForm : XtraForm {
        public StrongsCodesForm() {
            InitializeComponent();
            view.ShowLoadingPanel();
        }

        private void StrongsCodesForm_Load(object sender, EventArgs e) {
            var task = Task.Factory.StartNew(() => {
                return new XPQuery<StrongCode>(new UnitOfWork()).Where(x => x.Lang == Language.Greek).OrderBy(x => x.Transliteration);
            });
            task.ContinueWith(x => {
                this.SafeInvoke(f => {
                    f.grid.DataSource = x.Result;
                    f.view.BestFitColumns();
                    f.view.HideLoadingPanel();
                });
            });
        }
    }
}
