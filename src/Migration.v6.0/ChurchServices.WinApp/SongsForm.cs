using ChurchServices.Data.Model;
using ChurchServices.Extensions;
using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraGrid;

namespace ChurchServices.WinApp {
    public partial class SongsForm : RibbonForm {
        private UnitOfWork Uow = null;
        private XPView view = null;
        public SongsForm() {
            InitializeComponent();
            gridView.ShowLoadingPanel();
            Uow = new UnitOfWork();
            LoadData();
        }

        internal void LoadData() {
            view = new XPView(Uow, typeof(Song));
            view.Properties.Add(new ViewProperty("Id", SortDirection.None, "[Oid]", false, true));
            view.Properties.Add(new ViewProperty("Name", SortDirection.None, "[Name]", false, true));
            view.Properties.Add(new ViewProperty("Signature", SortDirection.None, "[Signature]", false, true));
            view.Properties.Add(new ViewProperty("BPM", SortDirection.None, "[BPM]", false, true));
            view.Properties.Add(new ViewProperty("Number", SortDirection.Ascending, "[Number]", false, true));
            view.Properties.Add(new ViewProperty("Type", SortDirection.None, "[Type]", false, true));

            string emptyValue = null;
            var criteria = CriteriaOperator.And(
                new BinaryOperator("YouTube", emptyValue, BinaryOperatorType.NotEqual), // Check for not null
                new BinaryOperator("YouTube", String.Empty, BinaryOperatorType.NotEqual)    // Check for not empty
            );

            view.Properties.Add(new ViewProperty("YouTube", SortDirection.None, criteria, false, true));

            grid.DataSource = view;
            gridView.BestFitColumns();
            gridView.Columns["Id"].Visible = false;
            //gridView.Columns["Type"].Group();
            gridView.ExpandAllGroups();
            gridView.HideLoadingPanel();
        }

        private void gridView_DoubleClick(object sender, EventArgs e) {
            var record = gridView.GetFocusedRow() as ViewRecord;
            if (record.IsNotNull()) {
                var id = record["Id"].ToInt();
                var song = new XPQuery<Song>(Uow).Where(x => x.Oid == id).FirstOrDefault();
                using (var dlg = new SongEditorForm(song)) {
                    dlg.IconOptions.SvgImage = this.IconOptions.SvgImage;
                    if (dlg.ShowDialog() == DialogResult.OK) {
                        dlg.Save();
                        LoadData();

                        var rowHandle = gridView.LocateByValue("Id", id);
                        if (rowHandle != GridControl.InvalidRowHandle) {
                            gridView.FocusedRowHandle = rowHandle;
                        }
                    }
                }
            }
        }

        private void btnAddSong_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var song = new Song(Uow) {
                Number = new XPQuery<Song>(Uow).Max(x => x.Number) + 1,
            };
            using (var dlg = new SongEditorForm(song)) {
                dlg.IconOptions.SvgImage = this.IconOptions.SvgImage;
                if (dlg.ShowDialog() == DialogResult.OK) {
                    dlg.Save();
                    LoadData();
                }
                else {
                    song.Delete();
                }
            }
        }

        private void btnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            gridView_DoubleClick(sender, e);
        }

        private void btnDeleteSong_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var record = gridView.GetFocusedRow() as ViewRecord;
            if (record.IsNotNull()) {
                var id = record["Id"].ToInt();
                var song = new XPQuery<Song>(Uow).Where(x => x.Oid == id).FirstOrDefault();
                song.Delete();
                Uow.CommitChanges();
                LoadData();

             
            }
        }
    }
}
