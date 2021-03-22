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
    public partial class BaseBooksForm : RibbonForm {
        public BaseBooksForm() {
            InitializeComponent();
            Text = "Base Books";
            LoadData();
        }

        private void LoadData() {
            var view = new XPQuery<BookBase>(new UnitOfWork());
            grid.DataSource = view;
            gridView.BestFitColumns();
            //{
            //    var col = gridView.Columns.Add();
            //    col.FieldName = "Status.BiblePart";
            //    col.Caption = "Bible Part";
            //    col.Visible = true;
            //    gridView.BestFitColumns();
            //}
            //{
            //    var col = gridView.Columns.Add();
            //    col.FieldName = "Status.CanonType";
            //    col.Caption = "Canon Type";
            //    col.Visible = true;
            //    gridView.BestFitColumns();
            //}
            //{
            //    var col = gridView.Columns.Add();
            //    col.FieldName = "Status.BookType";
            //    col.Caption = "Book Type";
            //    col.Visible = true;
            //    gridView.BestFitColumns();
            //}
        }

        private void gridView_DoubleClick(object sender, EventArgs e) {
            var obj = gridView.GetFocusedRow() as BookBase;
            if (obj.IsNotNull()) {
                using (var dlg = new BaseBookEditForm(obj)) {

                    if (dlg.ShowDialog() == DialogResult.OK) {
                        dlg.Save();
                        var uow = dlg.Object.Session as UnitOfWork;
                        uow.CommitChanges();
                        uow.ReloadChangedObjects();
                    }
                }
            }
        }
    }
}
