using DevExpress.Xpo;
using DevExpress.XtraBars.Ribbon;
using IBE.Data.Model;
using System;
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

        private void LoadData() {
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
            //UserPrincipal userPrincipal = UserPrincipal.Current;
            //String name = userPrincipal.DisplayName;

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
    }
}
