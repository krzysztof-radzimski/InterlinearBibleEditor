using DevExpress.Xpo;
using DevExpress.XtraBars.Ribbon;
using ChurchServices.Extensions;
using ChurchServices.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChurchServices.WinApp {
    public partial class TranslationsForm : RibbonForm {
        XPView view;
        public TranslationsForm() {
            InitializeComponent();
            Text = "Translations of Bible";
            LoadData();
        }

        private void LoadData() {
            view = new XPView(new UnitOfWork(), typeof(Translation));
            view.Properties.Add(new ViewProperty("Id", SortDirection.None, "[Oid]", false, true));
            view.Properties.Add(new ViewProperty("Name", SortDirection.None, "[Name]", false, true));
            view.Properties.Add(new ViewProperty("Description", SortDirection.None, "[Description]", false, true));
            view.Properties.Add(new ViewProperty("Type", SortDirection.None, "[Type]", false, true));
            view.Properties.Add(new ViewProperty("Language", SortDirection.None, "[Language]", false, true));
            view.Properties.Add(new ViewProperty("Catolic", SortDirection.None, "[Catolic]", false, true));
            view.Properties.Add(new ViewProperty("OpenAccess", SortDirection.None, "[OpenAccess]", false, true));
            view.Properties.Add(new ViewProperty("Recommended", SortDirection.None, "[Recommended]", false, true));
            view.Properties.Add(new ViewProperty("Hidden", SortDirection.None, "[Hidden]", false, true));
            view.Properties.Add(new ViewProperty("ChapterString", SortDirection.None, "[ChapterString]", false, true));
            view.Properties.Add(new ViewProperty("ChapterPsalmString", SortDirection.None, "[ChapterPsalmString]", false, true));
            view.Properties.Add(new ViewProperty("BookType", SortDirection.None, "[BookType]", false, true));
            grid.DataSource = view;
            gridView.BestFitColumns();
            Application.DoEvents();
            gridView.Columns["BookType"].Group();
            gridView.ExpandAllGroups();
        }

        private void gridView_DoubleClick(object sender, EventArgs e) {
            var selected = gridView.GetFocusedRow() as ViewRecord;
            if (selected.IsNotNull()) {
                var id = selected["Id"].ToInt();
                var frm = new TranslationEditForm(id, view.Session as UnitOfWork) {
                    MdiParent = this.MdiParent                    
                };
                frm.ObjectSaved += new EventHandler(delegate (object _sender, EventArgs args) {

                    LoadData();

                    var idx = gridView.LocateByValue("Id", id);
                    gridView.FocusedRowHandle = idx;
                    gridView.SelectRow(idx);
                });
                frm.Show();
            }
        }

        private void btnCreateTranslation_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var uow = view.Session as UnitOfWork;
            var translation = new Translation(uow) {
                BookType = TheBookType.ChurchFathersLetter,
                Catolic = false,
                ChapterPsalmString = "Psalm",
                ChapterString = "Rozdział",
                Description = String.Empty,
                DetailedInfo = String.Empty,
                Introduction = String.Empty,
                Language = Language.Polish,
                Recommended = false,
                Name = String.Empty,
                Type = TranslationType.Default
            };

            var frm = new TranslationEditForm(translation) {
                MdiParent = this.MdiParent
            };
            frm.ObjectSaved += new EventHandler(delegate (object _sender, EventArgs args) {

                LoadData();

                var idx = gridView.LocateByValue("Id", frm.Object.Oid);
                gridView.FocusedRowHandle = idx;
                gridView.SelectRow(idx);
            });

            frm.Show();
        }
    }
}
