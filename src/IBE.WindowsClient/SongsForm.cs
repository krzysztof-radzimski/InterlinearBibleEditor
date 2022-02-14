using DevExpress.Xpo;
using DevExpress.XtraBars.Ribbon;
using IBE.Data.Model;
using System;
using System.Threading.Tasks;

namespace IBE.WindowsClient {
    public partial class SongsForm : RibbonForm {
        private UnitOfWork Uow = null;
        public SongsForm() {
            InitializeComponent();
            gridView.ShowLoadingPanel();
            Uow = new UnitOfWork();
            LoadData();
        }

        internal void LoadData() {
            var task = Task.Factory.StartNew(() => {
                var view = new XPView(Uow, typeof(Song));
                view.Properties.Add(new ViewProperty("Id", SortDirection.None, "[Oid]", false, true));
                view.Properties.Add(new ViewProperty("Name", SortDirection.None, "[Name]", false, true));
                view.Properties.Add(new ViewProperty("Signature", SortDirection.None, "[Signature]", false, true));
                view.Properties.Add(new ViewProperty("BPM", SortDirection.None, "[BPM]", false, true));
                view.Properties.Add(new ViewProperty("Number", SortDirection.Ascending, "[Number]", false, true));
                view.Properties.Add(new ViewProperty("Type", SortDirection.None, "[Type]", true, true));
                return view;
            });
            task.ContinueWith(x => {
                this.BeginInvoke(new Action(() => {
                    grid.DataSource = x.Result;
                    gridView.BestFitColumns();
                    gridView.ExpandAllGroups();
                    gridView.HideLoadingPanel();
                }));
            });
        }
    }
}
