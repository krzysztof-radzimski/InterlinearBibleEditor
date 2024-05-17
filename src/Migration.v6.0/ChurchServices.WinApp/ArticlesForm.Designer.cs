
namespace ChurchServices.WinApp {
    partial class ArticlesForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ArticlesForm));
            ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            btnAddArticle = new DevExpress.XtraBars.BarButtonItem();
            btnDeleteArticle = new DevExpress.XtraBars.BarButtonItem();
            rpHome = new DevExpress.XtraBars.Ribbon.RibbonPage();
            ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            grid = new DevExpress.XtraGrid.GridControl();
            gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)ribbonControl1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridView).BeginInit();
            SuspendLayout();
            // 
            // ribbonControl1
            // 
            ribbonControl1.ExpandCollapseItem.Id = 0;
            ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] { ribbonControl1.ExpandCollapseItem, btnAddArticle, btnDeleteArticle });
            ribbonControl1.Location = new Point(0, 0);
            ribbonControl1.MaxItemId = 3;
            ribbonControl1.MdiMergeStyle = DevExpress.XtraBars.Ribbon.RibbonMdiMergeStyle.Always;
            ribbonControl1.Name = "ribbonControl1";
            ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] { rpHome });
            ribbonControl1.Size = new Size(800, 158);
            // 
            // btnAddArticle
            // 
            btnAddArticle.Caption = "Add new article";
            btnAddArticle.Id = 1;
            btnAddArticle.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("btnAddArticle.ImageOptions.SvgImage");
            btnAddArticle.ItemShortcut = new DevExpress.XtraBars.BarShortcut(Keys.Control | Keys.N);
            btnAddArticle.Name = "btnAddArticle";
            btnAddArticle.ItemClick += btnAddArticle_ItemClick;
            // 
            // btnDeleteArticle
            // 
            btnDeleteArticle.Caption = "Delete";
            btnDeleteArticle.Id = 2;
            btnDeleteArticle.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("btnDeleteArticle.ImageOptions.SvgImage");
            btnDeleteArticle.ItemShortcut = new DevExpress.XtraBars.BarShortcut(Keys.Control | Keys.Delete);
            btnDeleteArticle.Name = "btnDeleteArticle";
            btnDeleteArticle.ItemClick += btnDeleteArticle_ItemClick;
            // 
            // rpHome
            // 
            rpHome.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] { ribbonPageGroup1 });
            rpHome.Name = "rpHome";
            rpHome.Text = "Home";
            // 
            // ribbonPageGroup1
            // 
            ribbonPageGroup1.ItemLinks.Add(btnAddArticle);
            ribbonPageGroup1.ItemLinks.Add(btnDeleteArticle);
            ribbonPageGroup1.Name = "ribbonPageGroup1";
            ribbonPageGroup1.Text = "ribbonPageGroup1";
            // 
            // grid
            // 
            grid.Dock = DockStyle.Fill;
            grid.Location = new Point(0, 158);
            grid.MainView = gridView;
            grid.MenuManager = ribbonControl1;
            grid.Name = "grid";
            grid.Size = new Size(800, 292);
            grid.TabIndex = 2;
            grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { gridView });
            // 
            // gridView
            // 
            gridView.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            gridView.GridControl = grid;
            gridView.Name = "gridView";
            gridView.OptionsBehavior.Editable = false;
            gridView.OptionsBehavior.ReadOnly = true;
            gridView.OptionsEditForm.PopupEditFormWidth = 533;
            gridView.OptionsFind.AlwaysVisible = true;
            gridView.OptionsFind.FindMode = DevExpress.XtraEditors.FindMode.FindClick;
            gridView.OptionsView.ColumnAutoWidth = false;
            gridView.OptionsView.ShowGroupPanel = false;
            gridView.OptionsView.ShowIndicator = false;
            gridView.DoubleClick += gridView_DoubleClick;
            // 
            // ArticlesForm
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(grid);
            Controls.Add(ribbonControl1);
            Name = "ArticlesForm";
            Ribbon = ribbonControl1;
            Text = "ArticlesForm";
            ((System.ComponentModel.ISupportInitialize)ribbonControl1).EndInit();
            ((System.ComponentModel.ISupportInitialize)grid).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraGrid.GridControl grid;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraBars.BarButtonItem btnAddArticle;
        private DevExpress.XtraBars.Ribbon.RibbonPage rpHome;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.BarButtonItem btnDeleteArticle;
    }
}