
namespace eSword.CommentaryEditor {
    partial class CommentaryDialog {
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
            this.tablePanel1 = new DevExpress.Utils.Layout.TablePanel();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnApply = new DevExpress.XtraEditors.SimpleButton();
            this.txtInformation = new DevExpress.XtraRichEdit.RichEditControl();
            this.txtAbbreviation = new DevExpress.XtraEditors.TextEdit();
            this.txtTitle = new DevExpress.XtraEditors.TextEdit();
            this.lblInformation = new DevExpress.XtraEditors.LabelControl();
            this.lblAbbreviation = new DevExpress.XtraEditors.LabelControl();
            this.lblTitle = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).BeginInit();
            this.tablePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtAbbreviation.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTitle.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // tablePanel1
            // 
            this.tablePanel1.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 5F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 55F)});
            this.tablePanel1.Controls.Add(this.panelControl1);
            this.tablePanel1.Controls.Add(this.txtInformation);
            this.tablePanel1.Controls.Add(this.txtAbbreviation);
            this.tablePanel1.Controls.Add(this.txtTitle);
            this.tablePanel1.Controls.Add(this.lblInformation);
            this.tablePanel1.Controls.Add(this.lblAbbreviation);
            this.tablePanel1.Controls.Add(this.lblTitle);
            this.tablePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePanel1.Location = new System.Drawing.Point(0, 0);
            this.tablePanel1.Name = "tablePanel1";
            this.tablePanel1.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 100F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 100F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 26F)});
            this.tablePanel1.Size = new System.Drawing.Size(905, 660);
            this.tablePanel1.TabIndex = 0;
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.tablePanel1.SetColumn(this.panelControl1, 1);
            this.panelControl1.Controls.Add(this.btnCancel);
            this.panelControl1.Controls.Add(this.btnApply);
            this.panelControl1.Location = new System.Drawing.Point(78, 619);
            this.panelControl1.Name = "panelControl1";
            this.tablePanel1.SetRow(this.panelControl1, 4);
            this.panelControl1.Size = new System.Drawing.Size(824, 38);
            this.panelControl1.TabIndex = 6;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(659, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnApply.Location = new System.Drawing.Point(740, 6);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 0;
            this.btnApply.Text = "Apply";
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // txtInformation
            // 
            this.txtInformation.ActiveViewType = DevExpress.XtraRichEdit.RichEditViewType.Simple;
            this.tablePanel1.SetColumn(this.txtInformation, 1);
            this.txtInformation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInformation.LayoutUnit = DevExpress.XtraRichEdit.DocumentLayoutUnit.Pixel;
            this.txtInformation.Location = new System.Drawing.Point(78, 55);
            this.txtInformation.Name = "txtInformation";
            this.tablePanel1.SetRow(this.txtInformation, 2);
            this.tablePanel1.SetRowSpan(this.txtInformation, 2);
            this.txtInformation.Size = new System.Drawing.Size(824, 558);
            this.txtInformation.TabIndex = 5;
            // 
            // txtAbbreviation
            // 
            this.tablePanel1.SetColumn(this.txtAbbreviation, 1);
            this.txtAbbreviation.Location = new System.Drawing.Point(78, 29);
            this.txtAbbreviation.Name = "txtAbbreviation";
            this.tablePanel1.SetRow(this.txtAbbreviation, 1);
            this.txtAbbreviation.Size = new System.Drawing.Size(824, 20);
            this.txtAbbreviation.TabIndex = 4;
            // 
            // txtTitle
            // 
            this.tablePanel1.SetColumn(this.txtTitle, 1);
            this.txtTitle.Location = new System.Drawing.Point(78, 3);
            this.txtTitle.Name = "txtTitle";
            this.tablePanel1.SetRow(this.txtTitle, 0);
            this.txtTitle.Size = new System.Drawing.Size(824, 20);
            this.txtTitle.TabIndex = 3;
            // 
            // lblInformation
            // 
            this.tablePanel1.SetColumn(this.lblInformation, 0);
            this.lblInformation.Location = new System.Drawing.Point(3, 55);
            this.lblInformation.Name = "lblInformation";
            this.tablePanel1.SetRow(this.lblInformation, 2);
            this.lblInformation.Size = new System.Drawing.Size(56, 13);
            this.lblInformation.TabIndex = 2;
            this.lblInformation.Text = "Information";
            // 
            // lblAbbreviation
            // 
            this.tablePanel1.SetColumn(this.lblAbbreviation, 0);
            this.lblAbbreviation.Location = new System.Drawing.Point(3, 32);
            this.lblAbbreviation.Name = "lblAbbreviation";
            this.tablePanel1.SetRow(this.lblAbbreviation, 1);
            this.lblAbbreviation.Size = new System.Drawing.Size(61, 13);
            this.lblAbbreviation.TabIndex = 1;
            this.lblAbbreviation.Text = "Abbreviation";
            // 
            // lblTitle
            // 
            this.tablePanel1.SetColumn(this.lblTitle, 0);
            this.lblTitle.Location = new System.Drawing.Point(3, 6);
            this.lblTitle.Name = "lblTitle";
            this.tablePanel1.SetRow(this.lblTitle, 0);
            this.lblTitle.Size = new System.Drawing.Size(20, 13);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Title";
            // 
            // CommentaryDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(905, 660);
            this.Controls.Add(this.tablePanel1);
            this.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.Shadow;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CommentaryDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Commentary Details";
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).EndInit();
            this.tablePanel1.ResumeLayout(false);
            this.tablePanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtAbbreviation.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTitle.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.Utils.Layout.TablePanel tablePanel1;
        private DevExpress.XtraEditors.LabelControl lblAbbreviation;
        private DevExpress.XtraEditors.LabelControl lblTitle;
        private DevExpress.XtraRichEdit.RichEditControl txtInformation;
        private DevExpress.XtraEditors.TextEdit txtAbbreviation;
        private DevExpress.XtraEditors.TextEdit txtTitle;
        private DevExpress.XtraEditors.LabelControl lblInformation;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnApply;
    }
}