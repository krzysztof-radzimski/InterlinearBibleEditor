using DevExpress.Xpo;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using ChurchServices.Extensions;
using ChurchServices.Data.Model;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace ChurchServices.WinApp {
    public partial class BaseBooksForm : RibbonForm {
        public BaseBooksForm() {
            InitializeComponent();
            Text = "Base Books";
            LoadData();
        }

        private void LoadData(UnitOfWork uow = null) {
            if (uow == null) { uow = new UnitOfWork(); }
            var view = new XPQuery<BookBase>(uow);
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
                        //uow.ReloadChangedObjects();

                        LoadData(uow);
                    }
                }
            }
        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var uow = (gridView.GetRow(0) as BookBase).Session as UnitOfWork;
            var obj = new BookBase(uow);
            using (var dlg = new BaseBookEditForm(obj)) {
                if (dlg.ShowDialog() == DialogResult.OK) {
                    dlg.Save();
                    uow.CommitChanges();
                    LoadData();
                }
            }
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var uow = (gridView.GetRow(0) as BookBase).Session as UnitOfWork;
            var record = gridView.GetFocusedRow() as ViewRecord;
            if (record.IsNotNull()) {
                var id = record["Id"].ToInt();
                var book = new XPQuery<BookBase>(uow).Where(x => x.Oid == id).FirstOrDefault();
                if (book.TranslationBooks.IsNotNull() && book.TranslationBooks.Count > 0) {
                    if (XtraMessageBox.Show("Base book has related translations. Continue?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) {
                        return;
                    }
                }
                book.Delete();
                uow.CommitChanges();
                LoadData();
            }
        }
    }
}
