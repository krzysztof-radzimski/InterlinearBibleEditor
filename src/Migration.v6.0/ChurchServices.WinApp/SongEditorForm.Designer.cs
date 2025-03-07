namespace ChurchServices.WinApp {
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SongEditorForm));
            tablePanel1 = new DevExpress.Utils.Layout.TablePanel();
            panelControl1 = new DevExpress.XtraEditors.PanelControl();
            btnPaste = new DevExpress.XtraEditors.DropDownButton();
            mnuPaste = new DevExpress.XtraBars.PopupMenu(components);
            btnPasteVerse = new DevExpress.XtraBars.BarButtonItem();
            btnPasteChorus = new DevExpress.XtraBars.BarButtonItem();
            btnPasteBridge = new DevExpress.XtraBars.BarButtonItem();
            btnPasteChords = new DevExpress.XtraBars.BarButtonItem();
            btnDeleteVerse = new DevExpress.XtraBars.BarButtonItem();
            btnInsertVerseBefore = new DevExpress.XtraBars.BarButtonItem();
            barManager = new DevExpress.XtraBars.BarManager(components);
            barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            btnCancel = new DevExpress.XtraEditors.SimpleButton();
            btnOk = new DevExpress.XtraEditors.SimpleButton();
            grid = new DevExpress.XtraGrid.GridControl();
            gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            txtType = new DevExpress.XtraEditors.ComboBoxEdit();
            txtYouTube = new DevExpress.XtraEditors.TextEdit();
            txtNumber = new DevExpress.XtraEditors.SpinEdit();
            txtBPM = new DevExpress.XtraEditors.SpinEdit();
            txtSignature = new DevExpress.XtraEditors.ComboBoxEdit();
            txtName = new DevExpress.XtraEditors.TextEdit();
            labelControl6 = new DevExpress.XtraEditors.LabelControl();
            labelControl5 = new DevExpress.XtraEditors.LabelControl();
            labelControl4 = new DevExpress.XtraEditors.LabelControl();
            labelControl3 = new DevExpress.XtraEditors.LabelControl();
            labelControl2 = new DevExpress.XtraEditors.LabelControl();
            labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)tablePanel1).BeginInit();
            tablePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)panelControl1).BeginInit();
            panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)mnuPaste).BeginInit();
            ((System.ComponentModel.ISupportInitialize)barManager).BeginInit();
            ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtType.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtYouTube.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtNumber.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtBPM.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtSignature.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtName.Properties).BeginInit();
            SuspendLayout();
            // 
            // tablePanel1
            // 
            tablePanel1.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] { new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 5F), new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 55F) });
            tablePanel1.Controls.Add(panelControl1);
            tablePanel1.Controls.Add(grid);
            tablePanel1.Controls.Add(txtType);
            tablePanel1.Controls.Add(txtYouTube);
            tablePanel1.Controls.Add(txtNumber);
            tablePanel1.Controls.Add(txtBPM);
            tablePanel1.Controls.Add(txtSignature);
            tablePanel1.Controls.Add(txtName);
            tablePanel1.Controls.Add(labelControl6);
            tablePanel1.Controls.Add(labelControl5);
            tablePanel1.Controls.Add(labelControl4);
            tablePanel1.Controls.Add(labelControl3);
            tablePanel1.Controls.Add(labelControl2);
            tablePanel1.Controls.Add(labelControl1);
            tablePanel1.Dock = DockStyle.Fill;
            tablePanel1.Location = new Point(0, 0);
            tablePanel1.Name = "tablePanel1";
            tablePanel1.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] { new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 26F), new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 26F), new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 26F), new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 26F), new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 26F), new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 26F), new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 100F), new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 50F) });
            tablePanel1.Size = new Size(798, 778);
            tablePanel1.TabIndex = 0;
            // 
            // panelControl1
            // 
            panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            tablePanel1.SetColumn(panelControl1, 0);
            tablePanel1.SetColumnSpan(panelControl1, 2);
            panelControl1.Controls.Add(btnPaste);
            panelControl1.Controls.Add(btnCancel);
            panelControl1.Controls.Add(btnOk);
            panelControl1.Dock = DockStyle.Fill;
            panelControl1.Location = new Point(3, 731);
            panelControl1.Name = "panelControl1";
            tablePanel1.SetRow(panelControl1, 7);
            panelControl1.Size = new Size(792, 44);
            panelControl1.TabIndex = 12;
            // 
            // btnPaste
            // 
            btnPaste.DropDownControl = mnuPaste;
            btnPaste.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("btnPaste.ImageOptions.SvgImage");
            btnPaste.ImageOptions.SvgImageSize = new Size(16, 16);
            btnPaste.Location = new Point(9, 9);
            btnPaste.MenuManager = barManager;
            btnPaste.Name = "btnPaste";
            btnPaste.Size = new Size(80, 23);
            btnPaste.TabIndex = 3;
            btnPaste.Text = "Paste";
            btnPaste.Click += btnPaste_Click;
            // 
            // mnuPaste
            // 
            mnuPaste.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] { new DevExpress.XtraBars.LinkPersistInfo(btnPasteVerse), new DevExpress.XtraBars.LinkPersistInfo(btnPasteChorus), new DevExpress.XtraBars.LinkPersistInfo(btnPasteBridge), new DevExpress.XtraBars.LinkPersistInfo(btnPasteChords), new DevExpress.XtraBars.LinkPersistInfo(btnDeleteVerse), new DevExpress.XtraBars.LinkPersistInfo(btnInsertVerseBefore) });
            mnuPaste.Manager = barManager;
            mnuPaste.Name = "mnuPaste";
            // 
            // btnPasteVerse
            // 
            btnPasteVerse.Caption = "Paste verse";
            btnPasteVerse.Id = 0;
            btnPasteVerse.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("btnPasteVerse.ImageOptions.SvgImage");
            btnPasteVerse.ItemShortcut = new DevExpress.XtraBars.BarShortcut(Keys.F1);
            btnPasteVerse.Name = "btnPasteVerse";
            btnPasteVerse.ItemClick += btnPasteVerse_ItemClick;
            // 
            // btnPasteChorus
            // 
            btnPasteChorus.Caption = "Paste chorus";
            btnPasteChorus.Id = 1;
            btnPasteChorus.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("btnPasteChorus.ImageOptions.SvgImage");
            btnPasteChorus.ItemShortcut = new DevExpress.XtraBars.BarShortcut(Keys.F2);
            btnPasteChorus.Name = "btnPasteChorus";
            btnPasteChorus.ItemClick += btnPasteChorus_ItemClick;
            // 
            // btnPasteBridge
            // 
            btnPasteBridge.Caption = "Paste bridge";
            btnPasteBridge.Id = 2;
            btnPasteBridge.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("btnPasteBridge.ImageOptions.SvgImage");
            btnPasteBridge.ItemShortcut = new DevExpress.XtraBars.BarShortcut(Keys.F3);
            btnPasteBridge.Name = "btnPasteBridge";
            btnPasteBridge.ItemClick += btnPasteBridge_ItemClick;
            // 
            // btnPasteChords
            // 
            btnPasteChords.Caption = "Paste chords";
            btnPasteChords.Id = 3;
            btnPasteChords.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("btnPasteChords.ImageOptions.SvgImage");
            btnPasteChords.ItemShortcut = new DevExpress.XtraBars.BarShortcut(Keys.F4);
            btnPasteChords.Name = "btnPasteChords";
            btnPasteChords.ItemClick += btnPasteChords_ItemClick;
            // 
            // btnDeleteVerse
            // 
            btnDeleteVerse.Caption = "Delete verse";
            btnDeleteVerse.Id = 4;
            btnDeleteVerse.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("btnDeleteVerse.ImageOptions.SvgImage");
            btnDeleteVerse.ItemShortcut = new DevExpress.XtraBars.BarShortcut(Keys.Control | Keys.Delete);
            btnDeleteVerse.Name = "btnDeleteVerse";
            btnDeleteVerse.ItemClick += btnDeleteVerse_ItemClick;
            // 
            // btnInsertVerseBefore
            // 
            btnInsertVerseBefore.Caption = "Insert verse before";
            btnInsertVerseBefore.Id = 5;
            btnInsertVerseBefore.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("btnInsertVerseBefore.ImageOptions.SvgImage");
            btnInsertVerseBefore.ItemShortcut = new DevExpress.XtraBars.BarShortcut(Keys.Control | Keys.Shift | Keys.B);
            btnInsertVerseBefore.Name = "btnInsertVerseBefore";
            btnInsertVerseBefore.ItemClick += btnInsertVerseBefore_ItemClick;
            // 
            // barManager
            // 
            barManager.DockControls.Add(barDockControlTop);
            barManager.DockControls.Add(barDockControlBottom);
            barManager.DockControls.Add(barDockControlLeft);
            barManager.DockControls.Add(barDockControlRight);
            barManager.Form = this;
            barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] { btnPasteVerse, btnPasteChorus, btnPasteBridge, btnPasteChords, btnDeleteVerse, btnInsertVerseBefore });
            barManager.MaxItemId = 6;
            // 
            // barDockControlTop
            // 
            barDockControlTop.CausesValidation = false;
            barDockControlTop.Dock = DockStyle.Top;
            barDockControlTop.Location = new Point(0, 0);
            barDockControlTop.Manager = barManager;
            barDockControlTop.Size = new Size(798, 0);
            // 
            // barDockControlBottom
            // 
            barDockControlBottom.CausesValidation = false;
            barDockControlBottom.Dock = DockStyle.Bottom;
            barDockControlBottom.Location = new Point(0, 778);
            barDockControlBottom.Manager = barManager;
            barDockControlBottom.Size = new Size(798, 0);
            // 
            // barDockControlLeft
            // 
            barDockControlLeft.CausesValidation = false;
            barDockControlLeft.Dock = DockStyle.Left;
            barDockControlLeft.Location = new Point(0, 0);
            barDockControlLeft.Manager = barManager;
            barDockControlLeft.Size = new Size(0, 778);
            // 
            // barDockControlRight
            // 
            barDockControlRight.CausesValidation = false;
            barDockControlRight.Dock = DockStyle.Right;
            barDockControlRight.Location = new Point(798, 0);
            barDockControlRight.Manager = barManager;
            barDockControlRight.Size = new Size(0, 778);
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(627, 9);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Cancel";
            // 
            // btnOk
            // 
            btnOk.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnOk.DialogResult = DialogResult.OK;
            btnOk.Location = new Point(708, 9);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(75, 23);
            btnOk.TabIndex = 0;
            btnOk.Text = "Apply";
            // 
            // grid
            // 
            tablePanel1.SetColumn(grid, 0);
            tablePanel1.SetColumnSpan(grid, 2);
            grid.Dock = DockStyle.Fill;
            grid.Location = new Point(3, 159);
            grid.MainView = gridView;
            grid.Name = "grid";
            tablePanel1.SetRow(grid, 6);
            grid.Size = new Size(792, 566);
            grid.TabIndex = 1;
            grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { gridView });
            // 
            // gridView
            // 
            gridView.Appearance.Row.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            gridView.Appearance.Row.Options.UseFont = true;
            gridView.GridControl = grid;
            gridView.Name = "gridView";
            gridView.OptionsView.ColumnAutoWidth = false;
            gridView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            gridView.OptionsView.ShowGroupPanel = false;
            // 
            // txtType
            // 
            tablePanel1.SetColumn(txtType, 1);
            txtType.Location = new Point(77, 133);
            txtType.Name = "txtType";
            txtType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            txtType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            tablePanel1.SetRow(txtType, 5);
            txtType.Size = new Size(718, 20);
            txtType.TabIndex = 11;
            // 
            // txtYouTube
            // 
            tablePanel1.SetColumn(txtYouTube, 1);
            txtYouTube.Location = new Point(77, 107);
            txtYouTube.Name = "txtYouTube";
            tablePanel1.SetRow(txtYouTube, 4);
            txtYouTube.Size = new Size(718, 20);
            txtYouTube.TabIndex = 10;
            // 
            // txtNumber
            // 
            tablePanel1.SetColumn(txtNumber, 1);
            txtNumber.EditValue = new decimal(new int[] { 1, 0, 0, 0 });
            txtNumber.Location = new Point(77, 81);
            txtNumber.Name = "txtNumber";
            txtNumber.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            txtNumber.Properties.IsFloatValue = false;
            txtNumber.Properties.MaskSettings.Set("mask", "N00");
            txtNumber.Properties.MaxValue = new decimal(new int[] { 5000, 0, 0, 0 });
            txtNumber.Properties.MinValue = new decimal(new int[] { 1, 0, 0, 0 });
            txtNumber.Properties.SpinStyle = DevExpress.XtraEditors.Controls.SpinStyles.Horizontal;
            tablePanel1.SetRow(txtNumber, 3);
            txtNumber.Size = new Size(718, 20);
            txtNumber.TabIndex = 9;
            // 
            // txtBPM
            // 
            tablePanel1.SetColumn(txtBPM, 1);
            txtBPM.EditValue = new decimal(new int[] { 50, 0, 0, 0 });
            txtBPM.Location = new Point(77, 55);
            txtBPM.Name = "txtBPM";
            txtBPM.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            txtBPM.Properties.IsFloatValue = false;
            txtBPM.Properties.MaskSettings.Set("mask", "N00");
            txtBPM.Properties.MaxValue = new decimal(new int[] { 300, 0, 0, 0 });
            txtBPM.Properties.MinValue = new decimal(new int[] { 50, 0, 0, 0 });
            txtBPM.Properties.SpinStyle = DevExpress.XtraEditors.Controls.SpinStyles.Horizontal;
            tablePanel1.SetRow(txtBPM, 2);
            txtBPM.Size = new Size(718, 20);
            txtBPM.TabIndex = 8;
            // 
            // txtSignature
            // 
            tablePanel1.SetColumn(txtSignature, 1);
            txtSignature.Location = new Point(77, 29);
            txtSignature.Name = "txtSignature";
            txtSignature.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            txtSignature.Properties.Items.AddRange(new object[] { "4/4", "3/4", "6/8", "12/8" });
            txtSignature.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            tablePanel1.SetRow(txtSignature, 1);
            txtSignature.Size = new Size(718, 20);
            txtSignature.TabIndex = 7;
            // 
            // txtName
            // 
            tablePanel1.SetColumn(txtName, 1);
            txtName.Location = new Point(77, 3);
            txtName.Name = "txtName";
            tablePanel1.SetRow(txtName, 0);
            txtName.Size = new Size(718, 20);
            txtName.TabIndex = 6;
            // 
            // labelControl6
            // 
            tablePanel1.SetColumn(labelControl6, 0);
            labelControl6.Dock = DockStyle.Right;
            labelControl6.Location = new Point(43, 133);
            labelControl6.Name = "labelControl6";
            tablePanel1.SetRow(labelControl6, 5);
            labelControl6.Size = new Size(28, 20);
            labelControl6.TabIndex = 5;
            labelControl6.Text = "Type:";
            // 
            // labelControl5
            // 
            tablePanel1.SetColumn(labelControl5, 0);
            labelControl5.Dock = DockStyle.Right;
            labelControl5.Location = new Point(3, 107);
            labelControl5.Name = "labelControl5";
            tablePanel1.SetRow(labelControl5, 4);
            labelControl5.Size = new Size(68, 20);
            labelControl5.TabIndex = 4;
            labelControl5.Text = "YouTube URL:";
            // 
            // labelControl4
            // 
            tablePanel1.SetColumn(labelControl4, 0);
            labelControl4.Dock = DockStyle.Right;
            labelControl4.Location = new Point(30, 81);
            labelControl4.Name = "labelControl4";
            tablePanel1.SetRow(labelControl4, 3);
            labelControl4.Size = new Size(41, 20);
            labelControl4.TabIndex = 3;
            labelControl4.Text = "Number:";
            // 
            // labelControl3
            // 
            tablePanel1.SetColumn(labelControl3, 0);
            labelControl3.Dock = DockStyle.Right;
            labelControl3.Location = new Point(47, 55);
            labelControl3.Name = "labelControl3";
            tablePanel1.SetRow(labelControl3, 2);
            labelControl3.Size = new Size(24, 20);
            labelControl3.TabIndex = 2;
            labelControl3.Text = "BPM:";
            // 
            // labelControl2
            // 
            tablePanel1.SetColumn(labelControl2, 0);
            labelControl2.Dock = DockStyle.Right;
            labelControl2.Location = new Point(21, 29);
            labelControl2.Name = "labelControl2";
            tablePanel1.SetRow(labelControl2, 1);
            labelControl2.Size = new Size(50, 20);
            labelControl2.TabIndex = 1;
            labelControl2.Text = "Signature:";
            // 
            // labelControl1
            // 
            tablePanel1.SetColumn(labelControl1, 0);
            labelControl1.Dock = DockStyle.Right;
            labelControl1.Location = new Point(40, 3);
            labelControl1.Name = "labelControl1";
            tablePanel1.SetRow(labelControl1, 0);
            labelControl1.Size = new Size(31, 20);
            labelControl1.TabIndex = 0;
            labelControl1.Text = "Name:";
            // 
            // SongEditorForm
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(798, 778);
            Controls.Add(tablePanel1);
            Controls.Add(barDockControlLeft);
            Controls.Add(barDockControlRight);
            Controls.Add(barDockControlBottom);
            Controls.Add(barDockControlTop);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SongEditorForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Song Editor";
            ((System.ComponentModel.ISupportInitialize)tablePanel1).EndInit();
            tablePanel1.ResumeLayout(false);
            tablePanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)panelControl1).EndInit();
            panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)mnuPaste).EndInit();
            ((System.ComponentModel.ISupportInitialize)barManager).EndInit();
            ((System.ComponentModel.ISupportInitialize)grid).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridView).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtType.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtYouTube.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtNumber.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtBPM.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtSignature.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtName.Properties).EndInit();
            ResumeLayout(false);
            PerformLayout();
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