using DevExpress.Xpo;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using ChurchServices.Extensions;
using ChurchServices.Data.Model;
using System;
using System.Linq;

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

        internal void LoadData() {
            view = new XPView(Uow, typeof(Article));
            view.Properties.Add(new ViewProperty("Id", SortDirection.None, "[Oid]", false, true));
            view.Properties.Add(new ViewProperty("Subject", SortDirection.None, "[Subject]", false, true));
            view.Properties.Add(new ViewProperty("AuthorName", SortDirection.None, "[AuthorName]", false, true));
            view.Properties.Add(new ViewProperty("Date", SortDirection.None, "[Date]", false, true));
            view.Properties.Add(new ViewProperty("Lead", SortDirection.None, "[Lead]", false, true));
            view.Properties.Add(new ViewProperty("Type", SortDirection.None, "[Type]", false, true));
            view.Properties.Add(new ViewProperty("Hidden", SortDirection.None, "[Hidden]", false, true));
            grid.DataSource = view;
            gridView.BestFitColumns();
        }

        private void btnAddArticle_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var frm = new ArticleEditorForm(new Article(Uow) {
                AuthorName = Environment.UserName,
                Date = DateTime.Now,
                Lead = "Główna myśl...",
                Subject = "Tytuł ..."
            });
            frm.IconOptions.SvgImage = e.Item.ImageOptions.SvgImage;
            frm.MdiParent = this.MdiParent;
            frm.Show();
        }

        private void gridView_DoubleClick(object sender, EventArgs e) {
            var record = gridView.GetFocusedRow() as ViewRecord;
            if (record.IsNotNull()) {
                var article = new XPQuery<Article>(Uow).Where(x => x.Oid == record["Id"].ToInt()).FirstOrDefault();
                if (article.IsNotNull()) {
                    var frm = new ArticleEditorForm(article);
                    frm.IconOptions.SvgImage = btnAddArticle.ImageOptions.SvgImage;
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
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
