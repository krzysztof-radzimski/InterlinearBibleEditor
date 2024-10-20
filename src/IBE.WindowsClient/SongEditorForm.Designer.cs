﻿namespace IBE.WindowsClient {
    partial class SongEditorForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SongEditorForm));
            this.tablePanel1 = new DevExpress.Utils.Layout.TablePanel();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnPaste = new DevExpress.XtraEditors.DropDownButton();
            this.mnuPaste = new DevExpress.XtraBars.PopupMenu(this.components);
            this.btnPasteVerse = new DevExpress.XtraBars.BarButtonItem();
            this.btnPasteChorus = new DevExpress.XtraBars.BarButtonItem();
            this.btnPasteBridge = new DevExpress.XtraBars.BarButtonItem();
            this.btnPasteChords = new DevExpress.XtraBars.BarButtonItem();
            this.btnDeleteVerse = new DevExpress.XtraBars.BarButtonItem();
            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.grid = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.txtType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.txtYouTube = new DevExpress.XtraEditors.TextEdit();
            this.txtNumber = new DevExpress.XtraEditors.SpinEdit();
            this.txtBPM = new DevExpress.XtraEditors.SpinEdit();
            this.txtSignature = new DevExpress.XtraEditors.ComboBoxEdit();
            this.txtName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btnInsertVerseBefore = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).BeginInit();
            this.tablePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mnuPaste)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtYouTube.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNumber.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBPM.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSignature.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // tablePanel1
            // 
            this.tablePanel1.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 5F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 55F)});
            this.tablePanel1.Controls.Add(this.panelControl1);
            this.tablePanel1.Controls.Add(this.grid);
            this.tablePanel1.Controls.Add(this.txtType);
            this.tablePanel1.Controls.Add(this.txtYouTube);
            this.tablePanel1.Controls.Add(this.txtNumber);
            this.tablePanel1.Controls.Add(this.txtBPM);
            this.tablePanel1.Controls.Add(this.txtSignature);
            this.tablePanel1.Controls.Add(this.txtName);
            this.tablePanel1.Controls.Add(this.labelControl6);
            this.tablePanel1.Controls.Add(this.labelControl5);
            this.tablePanel1.Controls.Add(this.labelControl4);
            this.tablePanel1.Controls.Add(this.labelControl3);
            this.tablePanel1.Controls.Add(this.labelControl2);
            this.tablePanel1.Controls.Add(this.labelControl1);
            this.tablePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePanel1.Location = new System.Drawing.Point(0, 0);
            this.tablePanel1.Name = "tablePanel1";
            this.tablePanel1.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 100F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 50F)});
            this.tablePanel1.Size = new System.Drawing.Size(798, 778);
            this.tablePanel1.TabIndex = 0;
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.tablePanel1.SetColumn(this.panelControl1, 0);
            this.tablePanel1.SetColumnSpan(this.panelControl1, 2);
            this.panelControl1.Controls.Add(this.btnPaste);
            this.panelControl1.Controls.Add(this.btnCancel);
            this.panelControl1.Controls.Add(this.btnOk);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(3, 731);
            this.panelControl1.Name = "panelControl1";
            this.tablePanel1.SetRow(this.panelControl1, 7);
            this.panelControl1.Size = new System.Drawing.Size(792, 44);
            this.panelControl1.TabIndex = 12;
            // 
            // btnPaste
            // 
            this.btnPaste.DropDownControl = this.mnuPaste;
            this.btnPaste.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnPaste.ImageOptions.SvgImage")));
            this.btnPaste.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.btnPaste.Location = new System.Drawing.Point(9, 9);
            this.btnPaste.MenuManager = this.barManager;
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(80, 23);
            this.btnPaste.TabIndex = 3;
            this.btnPaste.Text = "Paste";
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // mnuPaste
            // 
            this.mnuPaste.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btnPasteVerse),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnPasteChorus),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnPasteBridge),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnPasteChords),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnDeleteVerse),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnInsertVerseBefore)});
            this.mnuPaste.Manager = this.barManager;
            this.mnuPaste.Name = "mnuPaste";
            // 
            // btnPasteVerse
            // 
            this.btnPasteVerse.Caption = "Paste verse";
            this.btnPasteVerse.Id = 0;
            this.btnPasteVerse.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnPasteVerse.ImageOptions.SvgImage")));
            this.btnPasteVerse.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F1);
            this.btnPasteVerse.Name = "btnPasteVerse";
            this.btnPasteVerse.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnPasteVerse_ItemClick);
            // 
            // btnPasteChorus
            // 
            this.btnPasteChorus.Caption = "Paste chorus";
            this.btnPasteChorus.Id = 1;
            this.btnPasteChorus.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnPasteChorus.ImageOptions.SvgImage")));
            this.btnPasteChorus.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F2);
            this.btnPasteChorus.Name = "btnPasteChorus";
            this.btnPasteChorus.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnPasteChorus_ItemClick);
            // 
            // btnPasteBridge
            // 
            this.btnPasteBridge.Caption = "Paste bridge";
            this.btnPasteBridge.Id = 2;
            this.btnPasteBridge.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnPasteBridge.ImageOptions.SvgImage")));
            this.btnPasteBridge.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F3);
            this.btnPasteBridge.Name = "btnPasteBridge";
            this.btnPasteBridge.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnPasteBridge_ItemClick);
            // 
            // btnPasteChords
            // 
            this.btnPasteChords.Caption = "Paste chords";
            this.btnPasteChords.Id = 3;
            this.btnPasteChords.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnPasteChords.ImageOptions.SvgImage")));
            this.btnPasteChords.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F4);
            this.btnPasteChords.Name = "btnPasteChords";
            this.btnPasteChords.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnPasteChords_ItemClick);
            // 
            // btnDeleteVerse
            // 
            this.btnDeleteVerse.Caption = "Delete verse";
            this.btnDeleteVerse.Id = 4;
            this.btnDeleteVerse.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnDeleteVerse.ImageOptions.SvgImage")));
            this.btnDeleteVerse.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete));
            this.btnDeleteVerse.Name = "btnDeleteVerse";
            this.btnDeleteVerse.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnDeleteVerse_ItemClick);
            // 
            // barManager
            // 
            this.barManager.DockControls.Add(this.barDockControlTop);
            this.barManager.DockControls.Add(this.barDockControlBottom);
            this.barManager.DockControls.Add(this.barDockControlLeft);
            this.barManager.DockControls.Add(this.barDockControlRight);
            this.barManager.Form = this;
            this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btnPasteVerse,
            this.btnPasteChorus,
            this.btnPasteBridge,
            this.btnPasteChords,
            this.btnDeleteVerse,
            this.btnInsertVerseBefore});
            this.barManager.MaxItemId = 6;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager;
            this.barDockControlTop.Size = new System.Drawing.Size(798, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 778);
            this.barDockControlBottom.Manager = this.barManager;
            this.barDockControlBottom.Size = new System.Drawing.Size(798, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.barManager;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 778);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(798, 0);
            this.barDockControlRight.Manager = this.barManager;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 778);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(627, 9);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(708, 9);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "Apply";
            // 
            // grid
            // 
            this.tablePanel1.SetColumn(this.grid, 0);
            this.tablePanel1.SetColumnSpan(this.grid, 2);
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.Location = new System.Drawing.Point(3, 159);
            this.grid.MainView = this.gridView;
            this.grid.Name = "grid";
            this.tablePanel1.SetRow(this.grid, 6);
            this.grid.Size = new System.Drawing.Size(792, 566);
            this.grid.TabIndex = 1;
            this.grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            // 
            // gridView
            // 
            this.gridView.Appearance.Row.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.gridView.Appearance.Row.Options.UseFont = true;
            this.gridView.GridControl = this.grid;
            this.gridView.Name = "gridView";
            this.gridView.OptionsView.ColumnAutoWidth = false;
            this.gridView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            this.gridView.OptionsView.ShowGroupPanel = false;
            // 
            // txtType
            // 
            this.tablePanel1.SetColumn(this.txtType, 1);
            this.txtType.Location = new System.Drawing.Point(80, 133);
            this.txtType.Name = "txtType";
            this.txtType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.tablePanel1.SetRow(this.txtType, 5);
            this.txtType.Size = new System.Drawing.Size(715, 20);
            this.txtType.TabIndex = 11;
            // 
            // txtYouTube
            // 
            this.tablePanel1.SetColumn(this.txtYouTube, 1);
            this.txtYouTube.Location = new System.Drawing.Point(80, 107);
            this.txtYouTube.Name = "txtYouTube";
            this.tablePanel1.SetRow(this.txtYouTube, 4);
            this.txtYouTube.Size = new System.Drawing.Size(715, 20);
            this.txtYouTube.TabIndex = 10;
            // 
            // txtNumber
            // 
            this.tablePanel1.SetColumn(this.txtNumber, 1);
            this.txtNumber.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtNumber.Location = new System.Drawing.Point(80, 81);
            this.txtNumber.Name = "txtNumber";
            this.txtNumber.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtNumber.Properties.IsFloatValue = false;
            this.txtNumber.Properties.MaskSettings.Set("mask", "N00");
            this.txtNumber.Properties.MaxValue = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.txtNumber.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtNumber.Properties.SpinStyle = DevExpress.XtraEditors.Controls.SpinStyles.Horizontal;
            this.tablePanel1.SetRow(this.txtNumber, 3);
            this.txtNumber.Size = new System.Drawing.Size(715, 20);
            this.txtNumber.TabIndex = 9;
            // 
            // txtBPM
            // 
            this.tablePanel1.SetColumn(this.txtBPM, 1);
            this.txtBPM.EditValue = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.txtBPM.Location = new System.Drawing.Point(80, 55);
            this.txtBPM.Name = "txtBPM";
            this.txtBPM.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtBPM.Properties.IsFloatValue = false;
            this.txtBPM.Properties.MaskSettings.Set("mask", "N00");
            this.txtBPM.Properties.MaxValue = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.txtBPM.Properties.MinValue = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.txtBPM.Properties.SpinStyle = DevExpress.XtraEditors.Controls.SpinStyles.Horizontal;
            this.tablePanel1.SetRow(this.txtBPM, 2);
            this.txtBPM.Size = new System.Drawing.Size(715, 20);
            this.txtBPM.TabIndex = 8;
            // 
            // txtSignature
            // 
            this.tablePanel1.SetColumn(this.txtSignature, 1);
            this.txtSignature.Location = new System.Drawing.Point(80, 29);
            this.txtSignature.Name = "txtSignature";
            this.txtSignature.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtSignature.Properties.Items.AddRange(new object[] {
            "4/4",
            "3/4",
            "6/8",
            "12/8"});
            this.txtSignature.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.tablePanel1.SetRow(this.txtSignature, 1);
            this.txtSignature.Size = new System.Drawing.Size(715, 20);
            this.txtSignature.TabIndex = 7;
            // 
            // txtName
            // 
            this.tablePanel1.SetColumn(this.txtName, 1);
            this.txtName.Location = new System.Drawing.Point(80, 3);
            this.txtName.Name = "txtName";
            this.tablePanel1.SetRow(this.txtName, 0);
            this.txtName.Size = new System.Drawing.Size(715, 20);
            this.txtName.TabIndex = 6;
            // 
            // labelControl6
            // 
            this.tablePanel1.SetColumn(this.labelControl6, 0);
            this.labelControl6.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelControl6.Location = new System.Drawing.Point(47, 133);
            this.labelControl6.Name = "labelControl6";
            this.tablePanel1.SetRow(this.labelControl6, 5);
            this.labelControl6.Size = new System.Drawing.Size(27, 20);
            this.labelControl6.TabIndex = 5;
            this.labelControl6.Text = "Type:";
            // 
            // labelControl5
            // 
            this.tablePanel1.SetColumn(this.labelControl5, 0);
            this.labelControl5.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelControl5.Location = new System.Drawing.Point(3, 107);
            this.labelControl5.Name = "labelControl5";
            this.tablePanel1.SetRow(this.labelControl5, 4);
            this.labelControl5.Size = new System.Drawing.Size(71, 20);
            this.labelControl5.TabIndex = 4;
            this.labelControl5.Text = "YouTube URL:";
            // 
            // labelControl4
            // 
            this.tablePanel1.SetColumn(this.labelControl4, 0);
            this.labelControl4.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelControl4.Location = new System.Drawing.Point(30, 81);
            this.labelControl4.Name = "labelControl4";
            this.tablePanel1.SetRow(this.labelControl4, 3);
            this.labelControl4.Size = new System.Drawing.Size(44, 20);
            this.labelControl4.TabIndex = 3;
            this.labelControl4.Text = "Number:";
            // 
            // labelControl3
            // 
            this.tablePanel1.SetColumn(this.labelControl3, 0);
            this.labelControl3.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelControl3.Location = new System.Drawing.Point(49, 55);
            this.labelControl3.Name = "labelControl3";
            this.tablePanel1.SetRow(this.labelControl3, 2);
            this.labelControl3.Size = new System.Drawing.Size(25, 20);
            this.labelControl3.TabIndex = 2;
            this.labelControl3.Text = "BPM:";
            // 
            // labelControl2
            // 
            this.tablePanel1.SetColumn(this.labelControl2, 0);
            this.labelControl2.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelControl2.Location = new System.Drawing.Point(21, 29);
            this.labelControl2.Name = "labelControl2";
            this.tablePanel1.SetRow(this.labelControl2, 1);
            this.labelControl2.Size = new System.Drawing.Size(53, 20);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "Signature:";
            // 
            // labelControl1
            // 
            this.tablePanel1.SetColumn(this.labelControl1, 0);
            this.labelControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelControl1.Location = new System.Drawing.Point(42, 3);
            this.labelControl1.Name = "labelControl1";
            this.tablePanel1.SetRow(this.labelControl1, 0);
            this.labelControl1.Size = new System.Drawing.Size(32, 20);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Name:";
            // 
            // btnInsertVerseBefore
            // 
            this.btnInsertVerseBefore.Caption = "Insert verse before";
            this.btnInsertVerseBefore.Id = 5;
            this.btnInsertVerseBefore.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnInsertVerseBefore.ImageOptions.SvgImage")));
            this.btnInsertVerseBefore.ItemShortcut = new DevExpress.XtraBars.BarShortcut(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                | System.Windows.Forms.Keys.B));
            this.btnInsertVerseBefore.Name = "btnInsertVerseBefore";
            this.btnInsertVerseBefore.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnInsertVerseBefore_ItemClick);
            // 
            // SongEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(798, 778);
            this.Controls.Add(this.tablePanel1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SongEditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Song Editor";
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).EndInit();
            this.tablePanel1.ResumeLayout(false);
            this.tablePanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mnuPaste)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtYouTube.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNumber.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBPM.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSignature.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.Utils.Layout.TablePanel tablePanel1;
        private DevExpress.XtraEditors.SpinEdit txtBPM;
        private DevExpress.XtraEditors.ComboBoxEdit txtSignature;
        private DevExpress.XtraEditors.TextEdit txtName;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ComboBoxEdit txtType;
        private DevExpress.XtraEditors.TextEdit txtYouTube;
        private DevExpress.XtraEditors.SpinEdit txtNumber;
        private DevExpress.XtraGrid.GridControl grid;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnOk;
        private DevExpress.XtraEditors.DropDownButton btnPaste;
        private DevExpress.XtraBars.PopupMenu mnuPaste;
        private DevExpress.XtraBars.BarButtonItem btnPasteVerse;
        private DevExpress.XtraBars.BarManager barManager;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem btnPasteChorus;
        private DevExpress.XtraBars.BarButtonItem btnPasteBridge;
        private DevExpress.XtraBars.BarButtonItem btnPasteChords;
        private DevExpress.XtraBars.BarButtonItem btnDeleteVerse;
        private DevExpress.XtraBars.BarButtonItem btnInsertVerseBefore;
    }
}