using DevExpress.Xpo;
using DevExpress.XtraBars.Ribbon;
using IBE.Common.Extensions;
using IBE.Data.Model;
using System;
using System.Linq;
using System.Security.Principal;

namespace IBE.WindowsClient {
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
            view = new XPView(new UnitOfWork(), typeof(Article));
            view.Properties.Add(new ViewProperty("Id", SortDirection.None, "[Oid]", false, true));
            view.Properties.Add(new ViewProperty("Subject", SortDirection.None, "[Subject]", false, true));
            view.Properties.Add(new ViewProperty("AuthorName", SortDirection.None, "[AuthorName]", false, true));
            view.Properties.Add(new ViewProperty("Date", SortDirection.None, "[Date]", false, true));
            view.Properties.Add(new ViewProperty("Lead", SortDirection.None, "[Lead]", false, true));
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
    }
}
