
namespace EIB.CommentaryEditor {
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
            this.fluentDesignFormContainer = new DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormContainer();
            this.accordionControl = new DevExpress.XtraBars.Navigation.AccordionControl();
            this.aceOT = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.accordionControlElement1 = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.aceNT = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.fluentDesignFormControl1 = new DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormControl();
            this.btnAbout = new DevExpress.XtraBars.BarButtonItem();
            this.mnuFile = new DevExpress.XtraBars.BarSubItem();
            this.btnImportWordFile = new DevExpress.XtraBars.BarButtonItem();
            this.btnSave = new DevExpress.XtraBars.BarButtonItem();
            this.btnSaveAsCMTI = new DevExpress.XtraBars.BarButtonItem();
            this.btnSaveAsCMTX = new DevExpress.XtraBars.BarButtonItem();
            this.btnClose = new DevExpress.XtraBars.BarButtonItem();
            this.btnAddCommentaryRange = new DevExpress.XtraBars.BarButtonItem();
            this.btnRemoveCommentaryRange = new DevExpress.XtraBars.BarButtonItem();
            this.fluentFormDefaultManager1 = new DevExpress.XtraBars.FluentDesignSystem.FluentFormDefaultManager(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.accordionControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fluentDesignFormControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fluentFormDefaultManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // fluentDesignFormContainer
            // 
            this.fluentDesignFormContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fluentDesignFormContainer.Location = new System.Drawing.Point(260, 31);
            this.fluentDesignFormContainer.Name = "fluentDesignFormContainer";
            this.fluentDesignFormContainer.Size = new System.Drawing.Size(1138, 768);
            this.fluentDesignFormContainer.TabIndex = 0;
            this.fluentDesignFormContainer.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.fluentDesignFormContainer_ControlAdded);
            // 
            // accordionControl
            // 
            this.accordionControl.AllowItemSelection = true;
            this.accordionControl.Dock = System.Windows.Forms.DockStyle.Left;
            this.accordionControl.Elements.AddRange(new DevExpress.XtraBars.Navigation.AccordionControlElement[] {
            this.aceOT,
            this.aceNT});
            this.accordionControl.Location = new System.Drawing.Point(0, 31);
            this.accordionControl.Name = "accordionControl";
            this.accordionControl.ScrollBarMode = DevExpress.XtraBars.Navigation.ScrollBarMode.Fluent;
            this.accordionControl.ShowFilterControl = DevExpress.XtraBars.Navigation.ShowFilterControl.Always;
            this.accordionControl.Size = new System.Drawing.Size(260, 768);
            this.accordionControl.TabIndex = 1;
            this.accordionControl.ViewType = DevExpress.XtraBars.Navigation.AccordionControlViewType.HamburgerMenu;
            this.accordionControl.ElementClick += new DevExpress.XtraBars.Navigation.ElementClickEventHandler(this.accordionControl_ElementClick);
            // 
            // aceOT
            // 
            this.aceOT.Elements.AddRange(new DevExpress.XtraBars.Navigation.AccordionControlElement[] {
            this.accordionControlElement1});
            this.aceOT.Expanded = true;
            this.aceOT.Name = "aceOT";
            this.aceOT.Text = "Old Testament";
            // 
            // accordionControlElement1
            // 
            this.accordionControlElement1.Name = "accordionControlElement1";
            this.accordionControlElement1.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.accordionControlElement1.Text = "Element1";
            // 
            // aceNT
            // 
            this.aceNT.Name = "aceNT";
            this.aceNT.Text = "New Testament";
            // 
            // fluentDesignFormControl1
            // 
            this.fluentDesignFormControl1.FluentDesignForm = this;
            this.fluentDesignFormControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btnAbout,
            this.mnuFile,
            this.btnSaveAsCMTI,
            this.btnSaveAsCMTX,
            this.btnClose,
            this.btnSave,
            this.btnAddCommentaryRange,
            this.btnRemoveCommentaryRange,
            this.btnImportWordFile});
            this.fluentDesignFormControl1.Location = new System.Drawing.Point(0, 0);
            this.fluentDesignFormControl1.Manager = this.fluentFormDefaultManager1;
            this.fluentDesignFormControl1.Name = "fluentDesignFormControl1";
            this.fluentDesignFormControl1.Size = new System.Drawing.Size(1398, 31);
            this.fluentDesignFormControl1.TabIndex = 2;
            this.fluentDesignFormControl1.TabStop = false;
            this.fluentDesignFormControl1.TitleItemLinks.Add(this.btnAbout);
            this.fluentDesignFormControl1.TitleItemLinks.Add(this.btnRemoveCommentaryRange);
            this.fluentDesignFormControl1.TitleItemLinks.Add(this.btnAddCommentaryRange, true);
            this.fluentDesignFormControl1.TitleItemLinks.Add(this.mnuFile);
            // 
            // btnAbout
            // 
            this.btnAbout.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnAbout.Caption = "About";
            this.btnAbout.Id = 0;
            this.btnAbout.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnAbout.ImageOptions.SvgImage")));
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            // 
            // mnuFile
            // 
            this.mnuFile.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.mnuFile.Caption = "&File";
            this.mnuFile.Id = 1;
            this.mnuFile.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnImportWordFile, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnSave),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnSaveAsCMTI, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnSaveAsCMTX),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnClose, true)});
            this.mnuFile.Name = "mnuFile";
            // 
            // btnImportWordFile
            // 
            this.btnImportWordFile.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnImportWordFile.Caption = "Import Word File";
            this.btnImportWordFile.Id = 8;
            this.btnImportWordFile.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnImportWordFile.ImageOptions.SvgImage")));
            this.btnImportWordFile.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O));
            this.btnImportWordFile.Name = "btnImportWordFile";
            this.btnImportWordFile.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.btnImportWordFile.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnImportWordFile_ItemClick);
            // 
            // btnSave
            // 
            this.btnSave.Caption = "Save changes";
            this.btnSave.Id = 5;
            this.btnSave.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnSave.ImageOptions.SvgImage")));
            this.btnSave.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S));
            this.btnSave.Name = "btnSave";
            this.btnSave.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnSave_ItemClick);
            // 
            // btnSaveAsCMTI
            // 
            this.btnSaveAsCMTI.Caption = "Save as CMTI file";
            this.btnSaveAsCMTI.Id = 2;
            this.btnSaveAsCMTI.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnSaveAsCMTI.ImageOptions.SvgImage")));
            this.btnSaveAsCMTI.ItemShortcut = new DevExpress.XtraBars.BarShortcut(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                | System.Windows.Forms.Keys.S));
            this.btnSaveAsCMTI.Name = "btnSaveAsCMTI";
            // 
            // btnSaveAsCMTX
            // 
            this.btnSaveAsCMTX.Caption = "Save as CMTX file";
            this.btnSaveAsCMTX.Id = 3;
            this.btnSaveAsCMTX.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnSaveAsCMTX.ImageOptions.SvgImage")));
            this.btnSaveAsCMTX.Name = "btnSaveAsCMTX";
            // 
            // btnClose
            // 
            this.btnClose.Caption = "Close";
            this.btnClose.Id = 4;
            this.btnClose.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnClose.ImageOptions.SvgImage")));
            this.btnClose.Name = "btnClose";
            this.btnClose.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnClose_ItemClick);
            // 
            // btnAddCommentaryRange
            // 
            this.btnAddCommentaryRange.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnAddCommentaryRange.Caption = "Add commentary range";
            this.btnAddCommentaryRange.Enabled = false;
            this.btnAddCommentaryRange.Id = 6;
            this.btnAddCommentaryRange.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnAddCommentaryRange.ImageOptions.SvgImage")));
            this.btnAddCommentaryRange.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F12);
            this.btnAddCommentaryRange.Name = "btnAddCommentaryRange";
            this.btnAddCommentaryRange.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.btnAddCommentaryRange.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnAddCommentaryRange_ItemClick);
            // 
            // btnRemoveCommentaryRange
            // 
            this.btnRemoveCommentaryRange.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnRemoveCommentaryRange.Caption = "Remove commentary range";
            this.btnRemoveCommentaryRange.Enabled = false;
            this.btnRemoveCommentaryRange.Id = 7;
            this.btnRemoveCommentaryRange.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnRemoveCommentaryRange.ImageOptions.SvgImage")));
            this.btnRemoveCommentaryRange.Name = "btnRemoveCommentaryRange";
            this.btnRemoveCommentaryRange.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.btnRemoveCommentaryRange.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.btnRemoveCommentaryRange.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnRemoveCommentaryRange_ItemClick);
            // 
            // fluentFormDefaultManager1
            // 
            this.fluentFormDefaultManager1.DockingEnabled = false;
            this.fluentFormDefaultManager1.Form = this;
            this.fluentFormDefaultManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btnAbout,
            this.mnuFile,
            this.btnSaveAsCMTI,
            this.btnSaveAsCMTX,
            this.btnClose,
            this.btnSave,
            this.btnAddCommentaryRange,
            this.btnRemoveCommentaryRange,
            this.btnImportWordFile});
            this.fluentFormDefaultManager1.MaxItemId = 9;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1398, 799);
            this.ControlContainer = this.fluentDesignFormContainer;
            this.Controls.Add(this.fluentDesignFormContainer);
            this.Controls.Add(this.accordionControl);
            this.Controls.Add(this.fluentDesignFormControl1);
            this.FluentDesignFormControl = this.fluentDesignFormControl1;
            this.Name = "MainForm";
            this.NavigationControl = this.accordionControl;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainForm";
            ((System.ComponentModel.ISupportInitialize)(this.accordionControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fluentDesignFormControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fluentFormDefaultManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormContainer fluentDesignFormContainer;
        private DevExpress.XtraBars.Navigation.AccordionControl accordionControl;
        private DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormControl fluentDesignFormControl1;
        private DevExpress.XtraBars.FluentDesignSystem.FluentFormDefaultManager fluentFormDefaultManager1;
        private DevExpress.XtraBars.Navigation.AccordionControlElement aceOT;
        private DevExpress.XtraBars.Navigation.AccordionControlElement aceNT;
        private DevExpress.XtraBars.BarButtonItem btnAbout;
        private DevExpress.XtraBars.Navigation.AccordionControlElement accordionControlElement1;
        private DevExpress.XtraBars.BarSubItem mnuFile;
        private DevExpress.XtraBars.BarButtonItem btnSaveAsCMTI;
        private DevExpress.XtraBars.BarButtonItem btnSaveAsCMTX;
        private DevExpress.XtraBars.BarButtonItem btnClose;
        private DevExpress.XtraBars.BarButtonItem btnSave;
        private DevExpress.XtraBars.BarButtonItem btnAddCommentaryRange;
        private DevExpress.XtraBars.BarButtonItem btnRemoveCommentaryRange;
        private DevExpress.XtraBars.BarButtonItem btnImportWordFile;
    }
}