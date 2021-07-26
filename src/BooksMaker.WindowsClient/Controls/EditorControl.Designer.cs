
namespace BooksMaker.WindowsClient.Controls {
    partial class EditorControl {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditorControl));
            this.editor = new DevExpress.XtraRichEdit.RichEditControl();
            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
            this.barToolbar = new DevExpress.XtraBars.Bar();
            this.btnSave = new DevExpress.XtraBars.BarButtonItem();
            this.btnSaveAndClose = new DevExpress.XtraBars.BarButtonItem();
            this.btnCancel = new DevExpress.XtraBars.BarButtonItem();
            this.lblCaption = new DevExpress.XtraBars.BarStaticItem();
            this.barStatusBar = new DevExpress.XtraBars.Bar();
            this.lblPageNumber = new DevExpress.XtraBars.BarStaticItem();
            this.lblWords = new DevExpress.XtraBars.BarStaticItem();
            this.btnZoom = new DevExpress.XtraBars.BarEditItem();
            this.zoomTrackBar = new DevExpress.XtraEditors.Repository.RepositoryItemZoomTrackBar();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.zoomTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // editor
            // 
            this.editor.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.editor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editor.Location = new System.Drawing.Point(0, 27);
            this.editor.MenuManager = this.barManager;
            this.editor.Name = "editor";
            this.editor.Size = new System.Drawing.Size(1124, 768);
            this.editor.TabIndex = 1;
            // 
            // barManager
            // 
            this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.barToolbar,
            this.barStatusBar});
            this.barManager.DockControls.Add(this.barDockControlTop);
            this.barManager.DockControls.Add(this.barDockControlBottom);
            this.barManager.DockControls.Add(this.barDockControlLeft);
            this.barManager.DockControls.Add(this.barDockControlRight);
            this.barManager.Form = this;
            this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btnSave,
            this.btnCancel,
            this.btnSaveAndClose,
            this.lblWords,
            this.lblPageNumber,
            this.barStaticItem1,
            this.btnZoom,
            this.lblCaption});
            this.barManager.MaxItemId = 8;
            this.barManager.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.zoomTrackBar});
            this.barManager.StatusBar = this.barStatusBar;
            // 
            // barToolbar
            // 
            this.barToolbar.BarName = "Tools";
            this.barToolbar.DockCol = 0;
            this.barToolbar.DockRow = 0;
            this.barToolbar.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.barToolbar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btnSave),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnSaveAndClose),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnCancel),
            new DevExpress.XtraBars.LinkPersistInfo(this.lblCaption)});
            this.barToolbar.OptionsBar.AllowQuickCustomization = false;
            this.barToolbar.OptionsBar.DrawBorder = false;
            this.barToolbar.OptionsBar.DrawDragBorder = false;
            this.barToolbar.OptionsBar.UseWholeRow = true;
            this.barToolbar.Text = "Tools";
            // 
            // btnSave
            // 
            this.btnSave.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnSave.Caption = "Save";
            this.btnSave.Id = 0;
            this.btnSave.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnSave.ImageOptions.SvgImage")));
            this.btnSave.Name = "btnSave";
            this.btnSave.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnSave_ItemClick);
            // 
            // btnSaveAndClose
            // 
            this.btnSaveAndClose.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnSaveAndClose.Caption = "Save and close";
            this.btnSaveAndClose.Id = 2;
            this.btnSaveAndClose.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnSaveAndClose.ImageOptions.SvgImage")));
            this.btnSaveAndClose.Name = "btnSaveAndClose";
            this.btnSaveAndClose.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnSaveAndClose_ItemClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnCancel.Caption = "Cancel";
            this.btnCancel.Id = 1;
            this.btnCancel.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnCancel.ImageOptions.SvgImage")));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnCancel_ItemClick);
            // 
            // lblCaption
            // 
            this.lblCaption.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.lblCaption.Caption = "Name of topic";
            this.lblCaption.Id = 7;
            this.lblCaption.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblCaption.ItemAppearance.Normal.Options.UseFont = true;
            this.lblCaption.Name = "lblCaption";
            // 
            // barStatusBar
            // 
            this.barStatusBar.BarName = "Status bar";
            this.barStatusBar.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.barStatusBar.DockCol = 0;
            this.barStatusBar.DockRow = 0;
            this.barStatusBar.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.barStatusBar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.lblPageNumber),
            new DevExpress.XtraBars.LinkPersistInfo(this.lblWords),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnZoom)});
            this.barStatusBar.OptionsBar.AllowQuickCustomization = false;
            this.barStatusBar.OptionsBar.DrawDragBorder = false;
            this.barStatusBar.OptionsBar.UseWholeRow = true;
            this.barStatusBar.Text = "Status bar";
            // 
            // lblPageNumber
            // 
            this.lblPageNumber.Caption = "Page {0} of {1}";
            this.lblPageNumber.Id = 4;
            this.lblPageNumber.Name = "lblPageNumber";
            // 
            // lblWords
            // 
            this.lblWords.Caption = "Words: {0}";
            this.lblWords.Id = 3;
            this.lblWords.Name = "lblWords";
            // 
            // btnZoom
            // 
            this.btnZoom.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnZoom.Edit = this.zoomTrackBar;
            this.btnZoom.Id = 6;
            this.btnZoom.Name = "btnZoom";
            this.btnZoom.Size = new System.Drawing.Size(200, 0);
            // 
            // zoomTrackBar
            // 
            this.zoomTrackBar.Name = "zoomTrackBar";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager;
            this.barDockControlTop.Size = new System.Drawing.Size(1124, 27);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 795);
            this.barDockControlBottom.Manager = this.barManager;
            this.barDockControlBottom.Size = new System.Drawing.Size(1124, 21);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 27);
            this.barDockControlLeft.Manager = this.barManager;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 768);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1124, 27);
            this.barDockControlRight.Manager = this.barManager;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 768);
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.Caption = "barStaticItem1";
            this.barStaticItem1.Id = 5;
            this.barStaticItem1.Name = "barStaticItem1";
            // 
            // EditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.editor);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "EditorControl";
            this.Size = new System.Drawing.Size(1124, 816);
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.zoomTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraRichEdit.RichEditControl editor;
        private DevExpress.XtraBars.BarManager barManager;
        private DevExpress.XtraBars.Bar barToolbar;
        private DevExpress.XtraBars.Bar barStatusBar;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem btnSave;
        private DevExpress.XtraBars.BarButtonItem btnSaveAndClose;
        private DevExpress.XtraBars.BarButtonItem btnCancel;
        private DevExpress.XtraBars.BarStaticItem lblPageNumber;
        private DevExpress.XtraBars.BarStaticItem lblWords;
        private DevExpress.XtraBars.BarEditItem btnZoom;
        private DevExpress.XtraEditors.Repository.RepositoryItemZoomTrackBar zoomTrackBar;
        private DevExpress.XtraBars.BarStaticItem barStaticItem1;
        private DevExpress.XtraBars.BarStaticItem lblCaption;
    }
}
