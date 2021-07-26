
namespace BooksMaker.WindowsClient {
    partial class MainForm {
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
            this.components = new System.ComponentModel.Container();
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.rpHome = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.dockManager1 = new BooksMaker.WindowsClient.Controls.CustomDockManager(this.components);
            this.dockPanelLeft = new BooksMaker.WindowsClient.Controls.CustomDockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.treeList1 = new DevExpress.XtraTreeList.TreeList();
            this.dockPanelContent = new BooksMaker.WindowsClient.Controls.CustomDockPanel();
            this.dockPanel2_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.tabPane = new DevExpress.XtraBars.Navigation.TabPane();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
            this.dockPanelLeft.SuspendLayout();
            this.dockPanel1_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).BeginInit();
            this.dockPanelContent.SuspendLayout();
            this.dockPanel2_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabPane)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.ribbonControl1.SearchEditItem,
            this.barButtonItem1});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 2;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.rpHome});
            this.ribbonControl1.Size = new System.Drawing.Size(1411, 158);
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "barButtonItem1";
            this.barButtonItem1.Id = 1;
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
            // 
            // rpHome
            // 
            this.rpHome.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1});
            this.rpHome.Name = "rpHome";
            this.rpHome.Text = "Home";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItem1);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "ribbonPageGroup1";
            // 
            // dockManager1
            // 
            this.dockManager1.DockingOptions.FloatOnDblClick = false;
            this.dockManager1.Form = this;
            this.dockManager1.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.dockPanelLeft,
            this.dockPanelContent});
            this.dockManager1.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.MenuStrip",
            "System.Windows.Forms.StatusStrip",
            "System.Windows.Forms.StatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl",
            "DevExpress.XtraBars.Navigation.OfficeNavigationBar",
            "DevExpress.XtraBars.Navigation.TileNavPane",
            "DevExpress.XtraBars.TabFormControl",
            "DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormControl",
            "DevExpress.XtraBars.ToolbarForm.ToolbarFormControl"});
            // 
            // dockPanelLeft
            // 
            this.dockPanelLeft.Controls.Add(this.dockPanel1_Container);
            this.dockPanelLeft.Dock = DevExpress.XtraBars.Docking.DockingStyle.Left;
            this.dockPanelLeft.ID = new System.Guid("c40624af-b9d9-4fdc-9d39-5f8752ec543d");
            this.dockPanelLeft.Location = new System.Drawing.Point(0, 158);
            this.dockPanelLeft.Name = "dockPanelLeft";
            this.dockPanelLeft.Options.AllowFloating = false;
            this.dockPanelLeft.Options.FloatOnDblClick = false;
            this.dockPanelLeft.Options.ShowCloseButton = false;
            this.dockPanelLeft.OriginalSize = new System.Drawing.Size(300, 200);
            this.dockPanelLeft.ShowCaption = true;
            this.dockPanelLeft.Size = new System.Drawing.Size(300, 780);
            this.dockPanelLeft.Text = "Navigation";
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Controls.Add(this.treeList1);
            this.dockPanel1_Container.Location = new System.Drawing.Point(3, 46);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(293, 731);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // treeList1
            // 
            this.treeList1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.treeList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeList1.Location = new System.Drawing.Point(0, 0);
            this.treeList1.MenuManager = this.ribbonControl1;
            this.treeList1.Name = "treeList1";
            this.treeList1.Size = new System.Drawing.Size(293, 731);
            this.treeList1.TabIndex = 0;
            // 
            // dockPanelContent
            // 
            this.dockPanelContent.Controls.Add(this.dockPanel2_Container);
            this.dockPanelContent.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.dockPanelContent.ID = new System.Guid("6b5fd466-2455-49e6-ac3b-735f8b84952d");
            this.dockPanelContent.Location = new System.Drawing.Point(300, 158);
            this.dockPanelContent.Name = "dockPanelContent";
            this.dockPanelContent.Options.AllowFloating = false;
            this.dockPanelContent.Options.FloatOnDblClick = false;
            this.dockPanelContent.Options.ShowAutoHideButton = false;
            this.dockPanelContent.Options.ShowCloseButton = false;
            this.dockPanelContent.Options.ShowMaximizeButton = false;
            this.dockPanelContent.Options.ShowMinimizeButton = false;
            this.dockPanelContent.OriginalSize = new System.Drawing.Size(1111, 200);
            this.dockPanelContent.ShowCaption = false;
            this.dockPanelContent.Size = new System.Drawing.Size(1111, 780);
            this.dockPanelContent.Text = "Content";
            // 
            // dockPanel2_Container
            // 
            this.dockPanel2_Container.Controls.Add(this.tabPane);
            this.dockPanel2_Container.Location = new System.Drawing.Point(3, 0);
            this.dockPanel2_Container.Name = "dockPanel2_Container";
            this.dockPanel2_Container.Size = new System.Drawing.Size(1105, 776);
            this.dockPanel2_Container.TabIndex = 0;
            // 
            // tabPane
            // 
            this.tabPane.AllowTransitionAnimation = DevExpress.Utils.DefaultBoolean.False;
            this.tabPane.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPane.Location = new System.Drawing.Point(0, 0);
            this.tabPane.Name = "tabPane";
            this.tabPane.RegularSize = new System.Drawing.Size(1105, 776);
            this.tabPane.SelectedPage = null;
            this.tabPane.Size = new System.Drawing.Size(1105, 776);
            this.tabPane.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1411, 938);
            this.Controls.Add(this.dockPanelContent);
            this.Controls.Add(this.dockPanelLeft);
            this.Controls.Add(this.ribbonControl1);
            this.IconOptions.SvgImage = global::BooksMaker.WindowsClient.Properties.Resources.doctor;
            this.Name = "MainForm";
            this.Ribbon = this.ribbonControl1;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Books Maker";
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
            this.dockPanelLeft.ResumeLayout(false);
            this.dockPanel1_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).EndInit();
            this.dockPanelContent.ResumeLayout(false);
            this.dockPanel2_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabPane)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.Ribbon.RibbonPage rpHome;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private BooksMaker.WindowsClient.Controls.CustomDockManager dockManager1;
        private BooksMaker.WindowsClient.Controls.CustomDockPanel dockPanelContent;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel2_Container;
        private BooksMaker.WindowsClient.Controls.CustomDockPanel dockPanelLeft;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private DevExpress.XtraTreeList.TreeList treeList1;
        private DevExpress.XtraBars.Navigation.TabPane tabPane;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
    }
}

