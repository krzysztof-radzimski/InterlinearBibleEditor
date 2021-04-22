
namespace IBE.WindowsClient {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.btnTranslations = new DevExpress.XtraBars.BarButtonItem();
            this.btnInterlinearEditor = new DevExpress.XtraBars.BarButtonItem();
            this.btnArticles = new DevExpress.XtraBars.BarButtonItem();
            this.btnBaseBooks = new DevExpress.XtraBars.BarButtonItem();
            this.btnCopyDatabaseToWebFolder = new DevExpress.XtraBars.BarButtonItem();
            this.rpHome = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonStatusBar1 = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.xtraTabbedMdiManager1 = new DevExpress.XtraTabbedMdi.XtraTabbedMdiManager(this.components);
            this.spellChecker1 = new DevExpress.XtraSpellChecker.SpellChecker(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabbedMdiManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.CommandLayout = DevExpress.XtraBars.Ribbon.CommandLayout.Simplified;
            this.ribbonControl1.DrawGroupCaptions = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.ribbonControl1.SearchEditItem,
            this.btnTranslations,
            this.btnInterlinearEditor,
            this.btnArticles,
            this.btnBaseBooks,
            this.btnCopyDatabaseToWebFolder});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 10;
            this.ribbonControl1.MdiMergeStyle = DevExpress.XtraBars.Ribbon.RibbonMdiMergeStyle.Always;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.rpHome});
            this.ribbonControl1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.Office2019;
            this.ribbonControl1.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl1.ShowExpandCollapseButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl1.ShowPageHeadersInFormCaption = DevExpress.Utils.DefaultBoolean.True;
            this.ribbonControl1.ShowQatLocationSelector = false;
            this.ribbonControl1.ShowToolbarCustomizeItem = false;
            this.ribbonControl1.Size = new System.Drawing.Size(1125, 67);
            this.ribbonControl1.StatusBar = this.ribbonStatusBar1;
            this.ribbonControl1.Toolbar.ShowCustomizeItem = false;
            // 
            // btnTranslations
            // 
            this.btnTranslations.Caption = "Translations";
            this.btnTranslations.Id = 3;
            this.btnTranslations.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnTranslations.ImageOptions.SvgImage")));
            this.btnTranslations.Name = "btnTranslations";
            this.btnTranslations.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnTranslations_ItemClick);
            // 
            // btnInterlinearEditor
            // 
            this.btnInterlinearEditor.Caption = "InterlinearEditor";
            this.btnInterlinearEditor.Id = 6;
            this.btnInterlinearEditor.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnInterlinearEditor.ImageOptions.SvgImage")));
            this.btnInterlinearEditor.Name = "btnInterlinearEditor";
            this.btnInterlinearEditor.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnInterlinearEditor_ItemClick);
            // 
            // btnArticles
            // 
            this.btnArticles.Caption = "Articles";
            this.btnArticles.Id = 7;
            this.btnArticles.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnArticles.ImageOptions.SvgImage")));
            this.btnArticles.Name = "btnArticles";
            this.btnArticles.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnArticles_ItemClick);
            // 
            // btnBaseBooks
            // 
            this.btnBaseBooks.Caption = "Base books";
            this.btnBaseBooks.Id = 8;
            this.btnBaseBooks.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnBaseBooks.ImageOptions.SvgImage")));
            this.btnBaseBooks.Name = "btnBaseBooks";
            this.btnBaseBooks.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnBaseBooks_ItemClick);
            // 
            // btnCopyDatabaseToWebFolder
            // 
            this.btnCopyDatabaseToWebFolder.Caption = "Copy to web folder";
            this.btnCopyDatabaseToWebFolder.Id = 9;
            this.btnCopyDatabaseToWebFolder.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnCopyDatabaseToWebFolder.ImageOptions.SvgImage")));
            this.btnCopyDatabaseToWebFolder.Name = "btnCopyDatabaseToWebFolder";
            this.btnCopyDatabaseToWebFolder.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnCopyDatabaseToWebFolder_ItemClick);
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
            this.ribbonPageGroup1.ItemLinks.Add(this.btnTranslations);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnInterlinearEditor);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnBaseBooks);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnArticles);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnCopyDatabaseToWebFolder);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "ribbonPageGroup1";
            // 
            // ribbonStatusBar1
            // 
            this.ribbonStatusBar1.Location = new System.Drawing.Point(0, 727);
            this.ribbonStatusBar1.Name = "ribbonStatusBar1";
            this.ribbonStatusBar1.Ribbon = this.ribbonControl1;
            this.ribbonStatusBar1.Size = new System.Drawing.Size(1125, 20);
            // 
            // xtraTabbedMdiManager1
            // 
            this.xtraTabbedMdiManager1.ClosePageButtonShowMode = DevExpress.XtraTab.ClosePageButtonShowMode.InAllTabPagesAndTabControlHeader;
            this.xtraTabbedMdiManager1.MdiParent = this;
            this.xtraTabbedMdiManager1.UseFormIconAsPageImage = DevExpress.Utils.DefaultBoolean.True;
            // 
            // spellChecker1
            // 
            this.spellChecker1.CheckAsYouTypeOptions.CheckControlsInParentContainer = true;
            this.spellChecker1.Culture = new System.Globalization.CultureInfo("pl-PL");
            this.spellChecker1.ParentContainer = this;
            this.spellChecker1.SpellCheckMode = DevExpress.XtraSpellChecker.SpellCheckMode.AsYouType;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1125, 747);
            this.Controls.Add(this.ribbonStatusBar1);
            this.Controls.Add(this.ribbonControl1);
            this.IsMdiContainer = true;
            this.Name = "MainForm";
            this.Ribbon = this.ribbonControl1;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.StatusBar = this.ribbonStatusBar1;
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabbedMdiManager1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.Ribbon.RibbonPage rpHome;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar1;
        private DevExpress.XtraBars.BarButtonItem btnTranslations;
        private DevExpress.XtraTabbedMdi.XtraTabbedMdiManager xtraTabbedMdiManager1;
        private DevExpress.XtraBars.BarButtonItem btnInterlinearEditor;
        private DevExpress.XtraBars.BarButtonItem btnArticles;
        private DevExpress.XtraBars.BarButtonItem btnBaseBooks;
        private DevExpress.XtraBars.BarButtonItem btnCopyDatabaseToWebFolder;
        private DevExpress.XtraSpellChecker.SpellChecker spellChecker1;
    }
}

