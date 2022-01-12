namespace WBST.Bibliography {
    partial class BibliographyPaneControl {
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BibliographyPaneControl));
            this.standaloneBarDockControl1 = new DevExpress.XtraBars.StandaloneBarDockControl();
            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.btnAddFootnote = new DevExpress.XtraBars.BarButtonItem();
            this.btnAddSource = new DevExpress.XtraBars.BarButtonItem();
            this.btnEditSource = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.grid = new DevExpress.XtraGrid.GridControl();
            this.view = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            this.btnDeleteSource = new DevExpress.XtraBars.BarButtonItem();
            this.btnAppendBibliography = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.view)).BeginInit();
            this.SuspendLayout();
            // 
            // standaloneBarDockControl1
            // 
            this.standaloneBarDockControl1.AutoSize = true;
            this.standaloneBarDockControl1.CausesValidation = false;
            this.standaloneBarDockControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.standaloneBarDockControl1.Location = new System.Drawing.Point(0, 0);
            this.standaloneBarDockControl1.Manager = this.barManager;
            this.standaloneBarDockControl1.Name = "standaloneBarDockControl1";
            this.standaloneBarDockControl1.Size = new System.Drawing.Size(663, 26);
            this.standaloneBarDockControl1.Text = "standaloneBarDockControl1";
            // 
            // barManager
            // 
            this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1});
            this.barManager.DockControls.Add(this.barDockControlTop);
            this.barManager.DockControls.Add(this.barDockControlBottom);
            this.barManager.DockControls.Add(this.barDockControlLeft);
            this.barManager.DockControls.Add(this.barDockControlRight);
            this.barManager.DockControls.Add(this.standaloneBarDockControl1);
            this.barManager.Form = this;
            this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btnAddFootnote,
            this.btnAddSource,
            this.btnEditSource,
            this.btnDeleteSource,
            this.btnAppendBibliography});
            this.barManager.MaxItemId = 5;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Standalone;
            this.bar1.FloatLocation = new System.Drawing.Point(501, 162);
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnAddFootnote, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnAddSource, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnEditSource, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnDeleteSource, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnAppendBibliography, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar1.OptionsBar.DrawBorder = false;
            this.bar1.OptionsBar.DrawDragBorder = false;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.StandaloneBarDockControl = this.standaloneBarDockControl1;
            this.bar1.Text = "Tools";
            // 
            // btnAddFootnote
            // 
            this.btnAddFootnote.Caption = "Dodaj przypis";
            this.btnAddFootnote.Enabled = false;
            this.btnAddFootnote.Id = 0;
            this.btnAddFootnote.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnAddFootnote.ImageOptions.SvgImage")));
            this.btnAddFootnote.Name = "btnAddFootnote";
            this.btnAddFootnote.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnAddFootnote_ItemClick);
            // 
            // btnAddSource
            // 
            this.btnAddSource.Caption = "Dodaj źródło";
            this.btnAddSource.Id = 1;
            this.btnAddSource.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnAddSource.ImageOptions.SvgImage")));
            this.btnAddSource.Name = "btnAddSource";
            this.btnAddSource.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnAddSource_ItemClick);
            // 
            // btnEditSource
            // 
            this.btnEditSource.Caption = "Edytuj źródło";
            this.btnEditSource.Id = 2;
            this.btnEditSource.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnEditSource.ImageOptions.SvgImage")));
            this.btnEditSource.Name = "btnEditSource";
            this.btnEditSource.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnEditSource_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager;
            this.barDockControlTop.Size = new System.Drawing.Size(663, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 663);
            this.barDockControlBottom.Manager = this.barManager;
            this.barDockControlBottom.Size = new System.Drawing.Size(663, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.barManager;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 663);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(663, 0);
            this.barDockControlRight.Manager = this.barManager;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 663);
            // 
            // grid
            // 
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.Location = new System.Drawing.Point(0, 26);
            this.grid.MainView = this.view;
            this.grid.Name = "grid";
            this.grid.Size = new System.Drawing.Size(663, 637);
            this.grid.TabIndex = 1;
            this.grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.view});
            // 
            // view
            // 
            this.view.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.view.GridControl = this.grid;
            this.view.Name = "view";
            this.view.OptionsBehavior.Editable = false;
            this.view.OptionsBehavior.ReadOnly = true;
            this.view.OptionsFind.AlwaysVisible = true;
            this.view.OptionsFind.FindMode = DevExpress.XtraEditors.FindMode.Always;
            this.view.OptionsView.ColumnAutoWidth = false;
            this.view.OptionsView.ShowGroupPanel = false;
            this.view.OptionsView.ShowIndicator = false;
            this.view.DoubleClick += new System.EventHandler(this.view_DoubleClick);
            // 
            // defaultLookAndFeel1
            // 
            this.defaultLookAndFeel1.LookAndFeel.SkinName = "Office 2019 Black";
            // 
            // btnDeleteSource
            // 
            this.btnDeleteSource.Caption = "Usuń źródło";
            this.btnDeleteSource.Id = 3;
            this.btnDeleteSource.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnDeleteSource.ImageOptions.SvgImage")));
            this.btnDeleteSource.Name = "btnDeleteSource";
            this.btnDeleteSource.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnDeleteSource_ItemClick);
            // 
            // btnAppendBibliography
            // 
            this.btnAppendBibliography.Caption = "Wstaw bibliografię";
            this.btnAppendBibliography.Id = 4;
            this.btnAppendBibliography.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnAppendBibliography.ImageOptions.SvgImage")));
            this.btnAppendBibliography.Name = "btnAppendBibliography";
            this.btnAppendBibliography.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnAppendBibliography_ItemClick);
            // 
            // BibliographyPaneControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grid);
            this.Controls.Add(this.standaloneBarDockControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "BibliographyPaneControl";
            this.Size = new System.Drawing.Size(663, 663);
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.view)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.StandaloneBarDockControl standaloneBarDockControl1;
        private DevExpress.XtraGrid.GridControl grid;
        private DevExpress.XtraGrid.Views.Grid.GridView view;
        private DevExpress.XtraBars.BarManager barManager;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarButtonItem btnAddFootnote;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
        private DevExpress.XtraBars.BarButtonItem btnAddSource;
        private DevExpress.XtraBars.BarButtonItem btnEditSource;
        private DevExpress.XtraBars.BarButtonItem btnDeleteSource;
        private DevExpress.XtraBars.BarButtonItem btnAppendBibliography;
    }
}
