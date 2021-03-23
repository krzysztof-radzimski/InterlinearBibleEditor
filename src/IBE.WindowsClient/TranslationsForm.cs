using DevExpress.Xpo;
using DevExpress.XtraBars.Ribbon;
using IBE.Common.Extensions;
using IBE.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IBE.WindowsClient {
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
            view.Properties.Add(new ViewProperty("Recommended", SortDirection.None, "[Recommended]", false, true));
            view.Properties.Add(new ViewProperty("ChapterString", SortDirection.None, "[ChapterString]", false, true));
            view.Properties.Add(new ViewProperty("ChapterPsalmString", SortDirection.None, "[ChapterPsalmString]", false, true));
            grid.DataSource = view;
            gridView.BestFitColumns();
        }

        private void gridView_DoubleClick(object sender, EventArgs e) {
            var selected = gridView.GetFocusedRow() as ViewRecord;
            if (selected.IsNotNull()) {
                var id = selected["Id"].ToInt();
                using (var dlg = new TranslationEditForm(id, view.Session)) {
                    if (dlg.ShowDialog() == DialogResult.OK) {
                        dlg.Save();
                        LoadData();

                        var idx = gridView.LocateByValue("Id", id);
                        gridView.FocusedRowHandle = idx;
                        gridView.SelectRow(idx);
                    }
                }
            }
        }

        private void btnCreateTranslation_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var uow = view.Session as UnitOfWork;
            var translation = new Translation(uow) { 
             
            };
        }
    }
}
