
namespace EIB.CommentaryEditor {
    partial class CommentaryItemDialog {
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
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnApply = new DevExpress.XtraEditors.SimpleButton();
            this.tablePanel1 = new DevExpress.Utils.Layout.TablePanel();
            this.txtVerseEnd = new DevExpress.XtraEditors.SpinEdit();
            this.txtVerseBegin = new DevExpress.XtraEditors.SpinEdit();
            this.txtChapterEnd = new DevExpress.XtraEditors.SpinEdit();
            this.txtChapterBegin = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).BeginInit();
            this.tablePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtVerseEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVerseBegin.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtChapterEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtChapterBegin.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.btnCancel);
            this.panelControl1.Controls.Add(this.btnApply);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 136);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(263, 38);
            this.panelControl1.TabIndex = 8;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(93, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnApply.Location = new System.Drawing.Point(174, 6);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 0;
            this.btnApply.Text = "Apply";
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // tablePanel1
            // 
            this.tablePanel1.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 5F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 55F)});
            this.tablePanel1.Controls.Add(this.txtVerseEnd);
            this.tablePanel1.Controls.Add(this.txtVerseBegin);
            this.tablePanel1.Controls.Add(this.txtChapterEnd);
            this.tablePanel1.Controls.Add(this.txtChapterBegin);
            this.tablePanel1.Controls.Add(this.labelControl4);
            this.tablePanel1.Controls.Add(this.labelControl3);
            this.tablePanel1.Controls.Add(this.labelControl2);
            this.tablePanel1.Controls.Add(this.labelControl1);
            this.tablePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePanel1.Location = new System.Drawing.Point(0, 0);
            this.tablePanel1.Name = "tablePanel1";
            this.tablePanel1.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
            this.tablePanel1.Size = new System.Drawing.Size(263, 136);
            this.tablePanel1.TabIndex = 9;
            // 
            // txtVerseEnd
            // 
            this.tablePanel1.SetColumn(this.txtVerseEnd, 1);
            this.txtVerseEnd.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtVerseEnd.Location = new System.Drawing.Point(83, 81);
            this.txtVerseEnd.Name = "txtVerseEnd";
            this.txtVerseEnd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtVerseEnd.Properties.IsFloatValue = false;
            this.txtVerseEnd.Properties.MaskSettings.Set("mask", "N00");
            this.tablePanel1.SetRow(this.txtVerseEnd, 3);
            this.txtVerseEnd.Size = new System.Drawing.Size(177, 20);
            this.txtVerseEnd.TabIndex = 7;
            // 
            // txtVerseBegin
            // 
            this.tablePanel1.SetColumn(this.txtVerseBegin, 1);
            this.txtVerseBegin.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtVerseBegin.Location = new System.Drawing.Point(83, 55);
            this.txtVerseBegin.Name = "txtVerseBegin";
            this.txtVerseBegin.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtVerseBegin.Properties.IsFloatValue = false;
            this.txtVerseBegin.Properties.MaskSettings.Set("mask", "N00");
            this.tablePanel1.SetRow(this.txtVerseBegin, 2);
            this.txtVerseBegin.Size = new System.Drawing.Size(177, 20);
            this.txtVerseBegin.TabIndex = 6;
            // 
            // txtChapterEnd
            // 
            this.tablePanel1.SetColumn(this.txtChapterEnd, 1);
            this.txtChapterEnd.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtChapterEnd.Location = new System.Drawing.Point(83, 29);
            this.txtChapterEnd.Name = "txtChapterEnd";
            this.txtChapterEnd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtChapterEnd.Properties.IsFloatValue = false;
            this.txtChapterEnd.Properties.MaskSettings.Set("mask", "N00");
            this.tablePanel1.SetRow(this.txtChapterEnd, 1);
            this.txtChapterEnd.Size = new System.Drawing.Size(177, 20);
            this.txtChapterEnd.TabIndex = 5;
            this.txtChapterEnd.TabStop = false;
            // 
            // txtChapterBegin
            // 
            this.tablePanel1.SetColumn(this.txtChapterBegin, 1);
            this.txtChapterBegin.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtChapterBegin.Location = new System.Drawing.Point(83, 3);
            this.txtChapterBegin.Name = "txtChapterBegin";
            this.txtChapterBegin.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtChapterBegin.Properties.IsFloatValue = false;
            this.txtChapterBegin.Properties.MaskSettings.Set("mask", "N00");
            this.tablePanel1.SetRow(this.txtChapterBegin, 0);
            this.txtChapterBegin.Size = new System.Drawing.Size(177, 20);
            this.txtChapterBegin.TabIndex = 4;
            this.txtChapterBegin.TabStop = false;
            // 
            // labelControl4
            // 
            this.tablePanel1.SetColumn(this.labelControl4, 0);
            this.labelControl4.Location = new System.Drawing.Point(3, 84);
            this.labelControl4.Name = "labelControl4";
            this.tablePanel1.SetRow(this.labelControl4, 3);
            this.labelControl4.Size = new System.Drawing.Size(51, 13);
            this.labelControl4.TabIndex = 3;
            this.labelControl4.Text = "Verse end";
            // 
            // labelControl3
            // 
            this.tablePanel1.SetColumn(this.labelControl3, 0);
            this.labelControl3.Location = new System.Drawing.Point(3, 58);
            this.labelControl3.Name = "labelControl3";
            this.tablePanel1.SetRow(this.labelControl3, 2);
            this.labelControl3.Size = new System.Drawing.Size(61, 13);
            this.labelControl3.TabIndex = 2;
            this.labelControl3.Text = "Verse begin";
            // 
            // labelControl2
            // 
            this.tablePanel1.SetColumn(this.labelControl2, 0);
            this.labelControl2.Location = new System.Drawing.Point(3, 32);
            this.labelControl2.Name = "labelControl2";
            this.tablePanel1.SetRow(this.labelControl2, 1);
            this.labelControl2.Size = new System.Drawing.Size(64, 13);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "Chapter end";
            // 
            // labelControl1
            // 
            this.tablePanel1.SetColumn(this.labelControl1, 0);
            this.labelControl1.Location = new System.Drawing.Point(3, 6);
            this.labelControl1.Name = "labelControl1";
            this.tablePanel1.SetRow(this.labelControl1, 0);
            this.labelControl1.Size = new System.Drawing.Size(74, 13);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Chapter begin";
            // 
            // CommentaryItemDialog
            // 
            this.AcceptButton = this.btnApply;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(263, 174);
            this.Controls.Add(this.tablePanel1);
            this.Controls.Add(this.panelControl1);
            this.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.Shadow;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CommentaryItemDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Commentary Range";
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).EndInit();
            this.tablePanel1.ResumeLayout(false);
            this.tablePanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtVerseEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVerseBegin.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtChapterEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtChapterBegin.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnApply;
        private DevExpress.Utils.Layout.TablePanel tablePanel1;
        private DevExpress.XtraEditors.SpinEdit txtVerseEnd;
        private DevExpress.XtraEditors.SpinEdit txtVerseBegin;
        private DevExpress.XtraEditors.SpinEdit txtChapterEnd;
        private DevExpress.XtraEditors.SpinEdit txtChapterBegin;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}