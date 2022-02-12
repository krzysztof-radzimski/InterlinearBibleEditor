using DevExpress.Xpo;
using DevExpress.XtraBars.Ribbon;
using IBE.Data.Model;

namespace IBE.WindowsClient {
    public partial class SongsForm : RibbonForm {
        XPView view;
        UnitOfWork Uow = null;
        public SongsForm() {
            InitializeComponent();
            Uow = new UnitOfWork();
            LoadData();
        }

        internal void LoadData() {
            view = new XPView(Uow, typeof(Song));
            view.Properties.Add(new ViewProperty("Id", SortDirection.None, "[Oid]", false, true));
            view.Properties.Add(new ViewProperty("Name", SortDirection.None, "[Name]", false, true));
            view.Properties.Add(new ViewProperty("Signature", SortDirection.None, "[Signature]", false, true));
            view.Properties.Add(new ViewProperty("BPM", SortDirection.None, "[BPM]", false, true));
            view.Properties.Add(new ViewProperty("Number", SortDirection.None, "[Number]", false, true));
            grid.DataSource = view;
            gridView.BestFitColumns();
        }
    }
}
