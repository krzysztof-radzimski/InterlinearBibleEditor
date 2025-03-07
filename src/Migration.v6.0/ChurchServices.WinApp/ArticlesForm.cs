using ChurchServices.Data.Model;
using ChurchServices.Extensions;
using DevExpress.Xpo;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;

namespace ChurchServices.WinApp {
    public partial class ArticlesForm : RibbonForm {
        XPView view;
        UnitOfWork Uow = null;
        public ArticlesForm() {
            InitializeComponent();
            Text = "Articles";
            Uow = new UnitOfWork();
            LoadData();
        }

        internal void LoadData(Article selected = null) {
            view = new XPView(Uow, typeof(Article));
            view.Properties.Add(new ViewProperty("Id", SortDirection.None, "[Oid]", false, true));
            view.Properties.Add(new ViewProperty("Passage", SortDirection.None, "[Passage]", false, true));
            view.Properties.Add(new ViewProperty("Subject", SortDirection.None, "[Subject]", false, true));
            view.Properties.Add(new ViewProperty("AuthorName", SortDirection.None, "[AuthorName]", false, true));
            view.Properties.Add(new ViewProperty("Date", SortDirection.Descending, "[Date]", false, true));
            view.Properties.Add(new ViewProperty("Lead", SortDirection.None, "[Lead]", false, true));
            view.Properties.Add(new ViewProperty("Type", SortDirection.None, "[Type]", false, true));
            view.Properties.Add(new ViewProperty("Hidden", SortDirection.None, "[Hidden]", false, true));
            grid.DataSource = view;
            gridView.BestFitColumns();

            if (selected != null) {
                var idx = gridView.LocateByValue("Id", selected.Oid);
                gridView.FocusedRowHandle = idx;
                gridView.SelectRow(idx);
            }
        }

        private void btnAddArticle_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var frm = new ArticleEditorForm(new Article(Uow) {
                AuthorName = String.Empty,
                Date = DateTime.Now,
                Lead = "",
                Subject = "",
                Passage = ""
            });
            frm.Icon = null;
            frm.IconOptions.SvgImage = btnAddArticle.ImageOptions.SvgImage;
            frm.MdiParent = this.MdiParent;
            frm.Show();
        }

        private void gridView_DoubleClick(object sender, EventArgs e) {
            var record = gridView.GetFocusedRow() as ViewRecord;
            if (record.IsNotNull()) {
                var hitInfo = gridView.CalcHitInfo(grid.PointToClient(Control.MousePosition));
                if (hitInfo.Column != null) {
                    var columnName = hitInfo.Column.FieldName;
                    if (columnName == "Hidden") {
                        var article = new XPQuery<Article>(Uow).Where(x => x.Oid == record["Id"].ToInt()).FirstOrDefault();
                        if (article.IsNotNull()) {
                            article.Hidden = !article.Hidden;
                            article.Save();
                            Uow.CommitChanges();
                            LoadData(article);
                        }
                    }
                    else {
                        var article = new XPQuery<Article>(Uow).Where(x => x.Oid == record["Id"].ToInt()).FirstOrDefault();
                        if (article.IsNotNull()) {
                            gridView.ShowLoadingPanel();

                            var frm = new ArticleEditorForm(article);
                            frm.IconOptions.SvgImage = btnAddArticle.ImageOptions.SvgImage;
                            frm.MdiParent = this.MdiParent;
                            frm.Show();

                            gridView.HideLoadingPanel();
                        }
                    }
                }
            }
        }

        private void btnDeleteArticle_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var record = gridView.GetFocusedRow() as ViewRecord;
            if (record.IsNotNull()) {
                var article = new XPQuery<Article>(Uow).Where(x => x.Oid == record["Id"].ToInt()).FirstOrDefault();
                if (article.IsNotNull()) {
                    if (XtraMessageBox.Show("Are you sure delete selected article?", "Delete", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes) {
                        article.Delete();
                        LoadData();
                    }
                }
            }
        }
    }
}
