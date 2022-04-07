namespace IBE.WindowsClient {
    partial class StrongsCodeEditForm {
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.richEditControl = new DevExpress.XtraRichEdit.RichEditControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.txtCode = new DevExpress.XtraEditors.TextEdit();
            this.txtLanguage = new DevExpress.XtraEditors.ComboBoxEdit();
            this.txtSourceWord = new DevExpress.XtraEditors.TextEdit();
            this.txtTransliteration = new DevExpress.XtraEditors.TextEdit();
            this.txtPronunciation = new DevExpress.XtraEditors.TextEdit();
            this.txtShortDefinition = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).BeginInit();
            this.tablePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLanguage.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSourceWord.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransliteration.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPronunciation.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtShortDefinition.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // tablePanel1
            // 
            this.tablePanel1.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 5F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 40F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 50F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F)});
            this.tablePanel1.Controls.Add(this.txtShortDefinition);
            this.tablePanel1.Controls.Add(this.txtPronunciation);
            this.tablePanel1.Controls.Add(this.txtTransliteration);
            this.tablePanel1.Controls.Add(this.txtSourceWord);
            this.tablePanel1.Controls.Add(this.txtLanguage);
            this.tablePanel1.Controls.Add(this.txtCode);
            this.tablePanel1.Controls.Add(this.panelControl1);
            this.tablePanel1.Controls.Add(this.richEditControl);
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
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 26F)});
            this.tablePanel1.Size = new System.Drawing.Size(961, 627);
            this.tablePanel1.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.tablePanel1.SetColumn(this.labelControl1, 0);
            this.labelControl1.Location = new System.Drawing.Point(3, 10);
            this.labelControl1.Name = "labelControl1";
            this.tablePanel1.SetRow(this.labelControl1, 0);
            this.labelControl1.Size = new System.Drawing.Size(39, 21);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Code:";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelControl2.Appearance.Options.UseFont = true;
            this.tablePanel1.SetColumn(this.labelControl2, 0);
            this.labelControl2.Location = new System.Drawing.Point(3, 44);
            this.labelControl2.Name = "labelControl2";
            this.tablePanel1.SetRow(this.labelControl2, 1);
            this.labelControl2.Size = new System.Drawing.Size(71, 21);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "Language:";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelControl3.Appearance.Options.UseFont = true;
            this.tablePanel1.SetColumn(this.labelControl3, 0);
            this.labelControl3.Location = new System.Drawing.Point(3, 78);
            this.labelControl3.Name = "labelControl3";
            this.tablePanel1.SetRow(this.labelControl3, 2);
            this.labelControl3.Size = new System.Drawing.Size(94, 21);
            this.labelControl3.TabIndex = 2;
            this.labelControl3.Text = "Source Word:";
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelControl4.Appearance.Options.UseFont = true;
            this.tablePanel1.SetColumn(this.labelControl4, 2);
            this.labelControl4.Location = new System.Drawing.Point(437, 10);
            this.labelControl4.Name = "labelControl4";
            this.tablePanel1.SetRow(this.labelControl4, 0);
            this.labelControl4.Size = new System.Drawing.Size(103, 21);
            this.labelControl4.TabIndex = 3;
            this.labelControl4.Text = "Transliteration:";
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelControl5.Appearance.Options.UseFont = true;
            this.tablePanel1.SetColumn(this.labelControl5, 2);
            this.labelControl5.Location = new System.Drawing.Point(437, 44);
            this.labelControl5.Name = "labelControl5";
            this.tablePanel1.SetRow(this.labelControl5, 1);
            this.labelControl5.Size = new System.Drawing.Size(100, 21);
            this.labelControl5.TabIndex = 4;
            this.labelControl5.Text = "Pronunciation:";
            // 
            // labelControl6
            // 
            this.labelControl6.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelControl6.Appearance.Options.UseFont = true;
            this.tablePanel1.SetColumn(this.labelControl6, 2);
            this.labelControl6.Location = new System.Drawing.Point(437, 78);
            this.labelControl6.Name = "labelControl6";
            this.tablePanel1.SetRow(this.labelControl6, 2);
            this.labelControl6.Size = new System.Drawing.Size(80, 21);
            this.labelControl6.TabIndex = 5;
            this.labelControl6.Text = "Translation:";
            // 
            // richEditControl
            // 
            this.richEditControl.ActiveViewType = DevExpress.XtraRichEdit.RichEditViewType.Simple;
            this.tablePanel1.SetColumn(this.richEditControl, 0);
            this.tablePanel1.SetColumnSpan(this.richEditControl, 4);
            this.richEditControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richEditControl.LayoutUnit = DevExpress.XtraRichEdit.DocumentLayoutUnit.Pixel;
            this.richEditControl.Location = new System.Drawing.Point(3, 105);
            this.richEditControl.Name = "richEditControl";
            this.tablePanel1.SetRow(this.richEditControl, 3);
            this.richEditControl.Size = new System.Drawing.Size(955, 465);
            this.richEditControl.TabIndex = 6;
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.tablePanel1.SetColumn(this.panelControl1, 0);
            this.tablePanel1.SetColumnSpan(this.panelControl1, 4);
            this.panelControl1.Controls.Add(this.btnCancel);
            this.panelControl1.Controls.Add(this.btnOk);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(3, 576);
            this.panelControl1.Name = "panelControl1";
            this.tablePanel1.SetRow(this.panelControl1, 4);
            this.panelControl1.Size = new System.Drawing.Size(955, 48);
            this.panelControl1.TabIndex = 14;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(790, 9);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(871, 9);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "Apply";
            // 
            // txtCode
            // 
            this.tablePanel1.SetColumn(this.txtCode, 1);
            this.txtCode.Location = new System.Drawing.Point(103, 3);
            this.txtCode.Name = "txtCode";
            this.txtCode.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.txtCode.Properties.Appearance.Options.UseFont = true;
            this.tablePanel1.SetRow(this.txtCode, 0);
            this.txtCode.Size = new System.Drawing.Size(328, 28);
            this.txtCode.TabIndex = 15;
            // 
            // txtLanguage
            // 
            this.tablePanel1.SetColumn(this.txtLanguage, 1);
            this.txtLanguage.Location = new System.Drawing.Point(103, 37);
            this.txtLanguage.Name = "txtLanguage";
            this.txtLanguage.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.txtLanguage.Properties.Appearance.Options.UseFont = true;
            this.txtLanguage.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtLanguage.Properties.Items.AddRange(new object[] {
            "Greek",
            "Hebrew"});
            this.tablePanel1.SetRow(this.txtLanguage, 1);
            this.txtLanguage.Size = new System.Drawing.Size(328, 28);
            this.txtLanguage.TabIndex = 16;
            // 
            // txtSourceWord
            // 
            this.tablePanel1.SetColumn(this.txtSourceWord, 1);
            this.txtSourceWord.Location = new System.Drawing.Point(103, 71);
            this.txtSourceWord.Name = "txtSourceWord";
            this.txtSourceWord.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.txtSourceWord.Properties.Appearance.Options.UseFont = true;
            this.tablePanel1.SetRow(this.txtSourceWord, 2);
            this.txtSourceWord.Size = new System.Drawing.Size(328, 28);
            this.txtSourceWord.TabIndex = 17;
            // 
            // txtTransliteration
            // 
            this.tablePanel1.SetColumn(this.txtTransliteration, 3);
            this.txtTransliteration.Location = new System.Drawing.Point(546, 3);
            this.txtTransliteration.Name = "txtTransliteration";
            this.txtTransliteration.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.txtTransliteration.Properties.Appearance.Options.UseFont = true;
            this.tablePanel1.SetRow(this.txtTransliteration, 0);
            this.txtTransliteration.Size = new System.Drawing.Size(412, 28);
            this.txtTransliteration.TabIndex = 18;
            // 
            // txtPronunciation
            // 
            this.tablePanel1.SetColumn(this.txtPronunciation, 3);
            this.txtPronunciation.Location = new System.Drawing.Point(546, 37);
            this.txtPronunciation.Name = "txtPronunciation";
            this.txtPronunciation.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.txtPronunciation.Properties.Appearance.Options.UseFont = true;
            this.tablePanel1.SetRow(this.txtPronunciation, 1);
            this.txtPronunciation.Size = new System.Drawing.Size(412, 28);
            this.txtPronunciation.TabIndex = 19;
            // 
            // txtTranslation
            // 
            this.tablePanel1.SetColumn(this.txtShortDefinition, 3);
            this.txtShortDefinition.Location = new System.Drawing.Point(546, 71);
            this.txtShortDefinition.Name = "txtTranslation";
            this.txtShortDefinition.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.txtShortDefinition.Properties.Appearance.Options.UseFont = true;
            this.tablePanel1.SetRow(this.txtShortDefinition, 2);
            this.txtShortDefinition.Size = new System.Drawing.Size(412, 28);
            this.txtShortDefinition.TabIndex = 20;
            // 
            // StrongsCodeEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(961, 627);
            this.Controls.Add(this.tablePanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.IconOptions.ShowIcon = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StrongsCodeEditForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "StrongsCodeEditForm";
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).EndInit();
            this.tablePanel1.ResumeLayout(false);
            this.tablePanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLanguage.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSourceWord.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransliteration.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPronunciation.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtShortDefinition.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.Utils.Layout.TablePanel tablePanel1;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraRichEdit.RichEditControl richEditControl;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.TextEdit txtShortDefinition;
        private DevExpress.XtraEditors.TextEdit txtPronunciation;
        private DevExpress.XtraEditors.TextEdit txtTransliteration;
        private DevExpress.XtraEditors.TextEdit txtSourceWord;
        private DevExpress.XtraEditors.ComboBoxEdit txtLanguage;
        private DevExpress.XtraEditors.TextEdit txtCode;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnOk;
    }
}