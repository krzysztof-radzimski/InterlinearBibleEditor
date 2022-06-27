
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
            this.btnStrongsCodes = new DevExpress.XtraBars.BarButtonItem();
            this.btnAncientDictionary = new DevExpress.XtraBars.BarButtonItem();
            this.btnImportUrlShortenersList = new DevExpress.XtraBars.BarButtonItem();
            this.btnSongs = new DevExpress.XtraBars.BarButtonItem();
            this.btnUpdateVersesIndex = new DevExpress.XtraBars.BarButtonItem();
            this.rpHome = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonStatusBar1 = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.xtraTabbedMdiManager1 = new DevExpress.XtraTabbedMdi.XtraTabbedMdiManager(this.components);
            this.spellChecker1 = new DevExpress.XtraSpellChecker.SpellChecker(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabbedMdiManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.DrawGroupCaptions = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl1.EmptyAreaImageOptions.ImagePadding = new System.Windows.Forms.Padding(45, 48, 45, 48);
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.ribbonControl1.SearchEditItem,
            this.btnTranslations,
            this.btnInterlinearEditor,
            this.btnArticles,
            this.btnBaseBooks,
            this.btnCopyDatabaseToWebFolder,
            this.btnStrongsCodes,
            this.btnAncientDictionary,
            this.btnImportUrlShortenersList,
            this.btnSongs,
            this.btnUpdateVersesIndex});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ribbonControl1.MaxItemId = 16;
            this.ribbonControl1.MdiMergeStyle = DevExpress.XtraBars.Ribbon.RibbonMdiMergeStyle.Always;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.OptionsMenuMinWidth = 495;
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.rpHome,
            this.ribbonPage1});
            this.ribbonControl1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.Office2019;
            this.ribbonControl1.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl1.ShowExpandCollapseButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl1.ShowPageHeadersInFormCaption = DevExpress.Utils.DefaultBoolean.True;
            this.ribbonControl1.ShowQatLocationSelector = false;
            this.ribbonControl1.Size = new System.Drawing.Size(1688, 213);
            this.ribbonControl1.StatusBar = this.ribbonStatusBar1;
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
            // btnStrongsCodes
            // 
            this.btnStrongsCodes.Caption = "Strongs Codes";
            this.btnStrongsCodes.Id = 10;
            this.btnStrongsCodes.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnStrongsCodes.ImageOptions.SvgImage")));
            this.btnStrongsCodes.Name = "btnStrongsCodes";
            this.btnStrongsCodes.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnStrongsCodes_ItemClick);
            // 
            // btnAncientDictionary
            // 
            this.btnAncientDictionary.Caption = "Dictionary";
            this.btnAncientDictionary.Id = 12;
            this.btnAncientDictionary.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnAncientDictionary.ImageOptions.SvgImage")));
            this.btnAncientDictionary.Name = "btnAncientDictionary";
            this.btnAncientDictionary.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnAncientDictionary_ItemClick);
            // 
            // btnImportUrlShortenersList
            // 
            this.btnImportUrlShortenersList.Caption = "Import url shorteners list";
            this.btnImportUrlShortenersList.Id = 13;
            this.btnImportUrlShortenersList.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnImportUrlShortenersList.ImageOptions.SvgImage")));
            this.btnImportUrlShortenersList.Name = "btnImportUrlShortenersList";
            this.btnImportUrlShortenersList.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnImportUrlShortenersList_ItemClick);
            // 
            // btnSongs
            // 
            this.btnSongs.Caption = "Songs";
            this.btnSongs.Id = 14;
            this.btnSongs.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnSongs.ImageOptions.SvgImage")));
            this.btnSongs.Name = "btnSongs";
            this.btnSongs.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnSongs_ItemClick);
            // 
            // btnUpdateVersesIndex
            // 
            this.btnUpdateVersesIndex.Caption = "Update Verses Index";
            this.btnUpdateVersesIndex.Id = 15;
            this.btnUpdateVersesIndex.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnUpdateVersesIndex.ImageOptions.SvgImage")));
            this.btnUpdateVersesIndex.Name = "btnUpdateVersesIndex";
            this.btnUpdateVersesIndex.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnUpdateVersesIndex_ItemClick);
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
            this.ribbonPageGroup1.ItemLinks.Add(this.btnArticles);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnCopyDatabaseToWebFolder, true);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "ribbonPageGroup1";
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup2});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "Additional";
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.btnSongs);
            this.ribbonPageGroup2.ItemLinks.Add(this.btnBaseBooks);
            this.ribbonPageGroup2.ItemLinks.Add(this.btnStrongsCodes);
            this.ribbonPageGroup2.ItemLinks.Add(this.btnAncientDictionary);
            this.ribbonPageGroup2.ItemLinks.Add(this.btnImportUrlShortenersList, true);
            this.ribbonPageGroup2.ItemLinks.Add(this.btnUpdateVersesIndex);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "ribbonPageGroup2";
            // 
            // ribbonStatusBar1
            // 
            this.ribbonStatusBar1.Location = new System.Drawing.Point(0, 1162);
            this.ribbonStatusBar1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ribbonStatusBar1.Name = "ribbonStatusBar1";
            this.ribbonStatusBar1.Ribbon = this.ribbonControl1;
            this.ribbonStatusBar1.Size = new System.Drawing.Size(1688, 45);
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
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1688, 1207);
            this.Controls.Add(this.ribbonStatusBar1);
            this.Controls.Add(this.ribbonControl1);
            this.IsMdiContainer = true;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
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
        private DevExpress.XtraBars.BarButtonItem btnStrongsCodes;
        private DevExpress.XtraBars.BarButtonItem btnAncientDictionary;
        private DevExpress.XtraBars.BarButtonItem btnImportUrlShortenersList;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.BarButtonItem btnSongs;
        private DevExpress.XtraBars.BarButtonItem btnUpdateVersesIndex;
    }
}

