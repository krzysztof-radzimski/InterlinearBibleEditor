
namespace ChurchServices.WinApp.Controls {
    partial class VerseWordEditorControl {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VerseWordEditorControl));
            this.tablePanel = new DevExpress.Utils.Layout.TablePanel();
            this.btnDelete = new DevExpress.XtraEditors.SimpleButton();
            this.standaloneBarDockControl1 = new DevExpress.XtraBars.StandaloneBarDockControl();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.btnInsertEmptyString = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.lblNumberOfVerseWord = new DevExpress.XtraEditors.LabelControl();
            this.txtFootnoteText = new DevExpress.XtraEditors.MemoEdit();
            this.cbCitation = new DevExpress.XtraEditors.CheckEdit();
            this.cbWordOfJesus = new DevExpress.XtraEditors.CheckEdit();
            this.txtTranslation = new DevExpress.XtraEditors.TextEdit();
            this.lblGrammarCode = new DevExpress.XtraEditors.LabelControl();
            this.lblTransliteration = new DevExpress.XtraEditors.LabelControl();
            this.lblStrong = new DevExpress.XtraEditors.LabelControl();
            this.lblGreekWord = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).BeginInit();
            this.tablePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFootnoteText.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbCitation.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbWordOfJesus.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTranslation.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // tablePanel
            // 
            this.tablePanel.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F)});
            this.tablePanel.Controls.Add(this.btnDelete);
            this.tablePanel.Controls.Add(this.standaloneBarDockControl1);
            this.tablePanel.Controls.Add(this.lblNumberOfVerseWord);
            this.tablePanel.Controls.Add(this.txtFootnoteText);
            this.tablePanel.Controls.Add(this.cbCitation);
            this.tablePanel.Controls.Add(this.cbWordOfJesus);
            this.tablePanel.Controls.Add(this.txtTranslation);
            this.tablePanel.Controls.Add(this.lblGrammarCode);
            this.tablePanel.Controls.Add(this.lblTransliteration);
            this.tablePanel.Controls.Add(this.lblStrong);
            this.tablePanel.Controls.Add(this.lblGreekWord);
            this.tablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePanel.Location = new System.Drawing.Point(0, 0);
            this.tablePanel.Name = "tablePanel";
            this.tablePanel.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
            this.tablePanel.Size = new System.Drawing.Size(270, 262);
            this.tablePanel.TabIndex = 0;
            // 
            // btnDelete
            // 
            this.tablePanel.SetColumn(this.btnDelete, 1);
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnDelete.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnDelete.ImageOptions.SvgImage")));
            this.btnDelete.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.btnDelete.Location = new System.Drawing.Point(246, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.tablePanel.SetRow(this.btnDelete, 0);
            this.btnDelete.Size = new System.Drawing.Size(21, 23);
            this.btnDelete.TabIndex = 9;
            this.btnDelete.TabStop = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // standaloneBarDockControl1
            // 
            this.standaloneBarDockControl1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.standaloneBarDockControl1.AutoSize = true;
            this.standaloneBarDockControl1.CausesValidation = false;
            this.tablePanel.SetColumn(this.standaloneBarDockControl1, 0);
            this.tablePanel.SetColumnSpan(this.standaloneBarDockControl1, 2);
            this.standaloneBarDockControl1.Location = new System.Drawing.Point(3, 147);
            this.standaloneBarDockControl1.Manager = this.barManager1;
            this.standaloneBarDockControl1.Name = "standaloneBarDockControl1";
            this.tablePanel.SetRow(this.standaloneBarDockControl1, 5);
            this.standaloneBarDockControl1.Size = new System.Drawing.Size(169, 24);
            this.standaloneBarDockControl1.Text = "standaloneBarDockControl1";
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.DockControls.Add(this.standaloneBarDockControl1);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btnInsertEmptyString});
            this.barManager1.MaxItemId = 1;
            // 
            // bar1
            // 
            this.bar1.BarName = "Custom 2";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Standalone;
            this.bar1.FloatLocation = new System.Drawing.Point(454, 278);
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnInsertEmptyString, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.DrawBorder = false;
            this.bar1.OptionsBar.DrawDragBorder = false;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.StandaloneBarDockControl = this.standaloneBarDockControl1;
            this.bar1.Text = "Custom 2";
            // 
            // btnInsertEmptyString
            // 
            this.btnInsertEmptyString.Caption = "Insert empty line string";
            this.btnInsertEmptyString.Id = 0;
            this.btnInsertEmptyString.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnInsertEmptyString.ImageOptions.SvgImage")));
            this.btnInsertEmptyString.Name = "btnInsertEmptyString";
            this.btnInsertEmptyString.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnInsertEmptyString_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(270, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 262);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(270, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 262);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(270, 0);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 262);
            // 
            // lblNumberOfVerseWord
            // 
            this.lblNumberOfVerseWord.Appearance.Options.UseTextOptions = true;
            this.lblNumberOfVerseWord.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.tablePanel.SetColumn(this.lblNumberOfVerseWord, 0);
            this.lblNumberOfVerseWord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNumberOfVerseWord.Location = new System.Drawing.Point(3, 3);
            this.lblNumberOfVerseWord.Name = "lblNumberOfVerseWord";
            this.tablePanel.SetRow(this.lblNumberOfVerseWord, 0);
            this.lblNumberOfVerseWord.Size = new System.Drawing.Size(129, 23);
            this.lblNumberOfVerseWord.TabIndex = 8;
            this.lblNumberOfVerseWord.Text = "1";
            this.lblNumberOfVerseWord.DoubleClick += new System.EventHandler(this.lblNumberOfVerseWord_DoubleClick);
            // 
            // txtFootnoteText
            // 
            this.tablePanel.SetColumn(this.txtFootnoteText, 0);
            this.tablePanel.SetColumnSpan(this.txtFootnoteText, 2);
            this.txtFootnoteText.Location = new System.Drawing.Point(3, 201);
            this.txtFootnoteText.Name = "txtFootnoteText";
            this.tablePanel.SetRow(this.txtFootnoteText, 7);
            this.txtFootnoteText.Size = new System.Drawing.Size(264, 51);
            this.txtFootnoteText.TabIndex = 7;
            this.txtFootnoteText.TabStop = false;
            // 
            // cbCitation
            // 
            this.tablePanel.SetColumn(this.cbCitation, 1);
            this.cbCitation.Location = new System.Drawing.Point(138, 177);
            this.cbCitation.Name = "cbCitation";
            this.cbCitation.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.cbCitation.Properties.Appearance.Options.UseFont = true;
            this.cbCitation.Properties.Caption = "In citation";
            this.tablePanel.SetRow(this.cbCitation, 6);
            this.cbCitation.Size = new System.Drawing.Size(129, 18);
            this.cbCitation.TabIndex = 6;
            this.cbCitation.TabStop = false;
            // 
            // cbWordOfJesus
            // 
            this.tablePanel.SetColumn(this.cbWordOfJesus, 0);
            this.cbWordOfJesus.Location = new System.Drawing.Point(3, 177);
            this.cbWordOfJesus.Name = "cbWordOfJesus";
            this.cbWordOfJesus.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.cbWordOfJesus.Properties.Appearance.ForeColor = System.Drawing.Color.Red;
            this.cbWordOfJesus.Properties.Appearance.Options.UseBackColor = true;
            this.cbWordOfJesus.Properties.Appearance.Options.UseForeColor = true;
            this.cbWordOfJesus.Properties.Caption = "Word of God";
            this.tablePanel.SetRow(this.cbWordOfJesus, 6);
            this.cbWordOfJesus.Size = new System.Drawing.Size(129, 18);
            this.cbWordOfJesus.TabIndex = 5;
            this.cbWordOfJesus.TabStop = false;
            // 
            // txtTranslation
            // 
            this.tablePanel.SetColumn(this.txtTranslation, 0);
            this.tablePanel.SetColumnSpan(this.txtTranslation, 2);
            this.txtTranslation.Location = new System.Drawing.Point(3, 113);
            this.txtTranslation.Name = "txtTranslation";
            this.txtTranslation.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.txtTranslation.Properties.Appearance.Options.UseFont = true;
            this.tablePanel.SetRow(this.txtTranslation, 4);
            this.txtTranslation.Size = new System.Drawing.Size(264, 28);
            this.txtTranslation.TabIndex = 4;
            this.txtTranslation.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtTranslation_KeyUp);
            // 
            // lblGrammarCode
            // 
            this.lblGrammarCode.Appearance.ForeColor = System.Drawing.Color.Red;
            this.lblGrammarCode.Appearance.Options.UseForeColor = true;
            this.lblGrammarCode.Appearance.Options.UseTextOptions = true;
            this.lblGrammarCode.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.tablePanel.SetColumn(this.lblGrammarCode, 1);
            this.lblGrammarCode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblGrammarCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGrammarCode.Location = new System.Drawing.Point(138, 59);
            this.lblGrammarCode.Name = "lblGrammarCode";
            this.lblGrammarCode.Padding = new System.Windows.Forms.Padding(3);
            this.tablePanel.SetRow(this.lblGrammarCode, 2);
            this.lblGrammarCode.Size = new System.Drawing.Size(129, 19);
            this.lblGrammarCode.TabIndex = 3;
            this.lblGrammarCode.Text = "none";
            this.lblGrammarCode.Click += new System.EventHandler(this.lblGrammarCode_Click);
            this.lblGrammarCode.DoubleClick += new System.EventHandler(this.lblGrammarCode_DoubleClick);
            // 
            // lblTransliteration
            // 
            this.lblTransliteration.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblTransliteration.Appearance.ForeColor = System.Drawing.Color.Green;
            this.lblTransliteration.Appearance.Options.UseFont = true;
            this.lblTransliteration.Appearance.Options.UseForeColor = true;
            this.lblTransliteration.Appearance.Options.UseTextOptions = true;
            this.lblTransliteration.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.tablePanel.SetColumn(this.lblTransliteration, 0);
            this.tablePanel.SetColumnSpan(this.lblTransliteration, 2);
            this.lblTransliteration.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblTransliteration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTransliteration.Location = new System.Drawing.Point(3, 84);
            this.lblTransliteration.Name = "lblTransliteration";
            this.lblTransliteration.Padding = new System.Windows.Forms.Padding(3);
            this.tablePanel.SetRow(this.lblTransliteration, 3);
            this.lblTransliteration.Size = new System.Drawing.Size(264, 23);
            this.lblTransliteration.TabIndex = 2;
            this.lblTransliteration.Text = "none";
            this.lblTransliteration.Click += new System.EventHandler(this.lblTransliteration_Click);
            // 
            // lblStrong
            // 
            this.lblStrong.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.lblStrong.Appearance.Options.UseForeColor = true;
            this.lblStrong.Appearance.Options.UseTextOptions = true;
            this.lblStrong.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.tablePanel.SetColumn(this.lblStrong, 0);
            this.lblStrong.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblStrong.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStrong.Location = new System.Drawing.Point(3, 59);
            this.lblStrong.Name = "lblStrong";
            this.lblStrong.Padding = new System.Windows.Forms.Padding(3);
            this.tablePanel.SetRow(this.lblStrong, 2);
            this.lblStrong.Size = new System.Drawing.Size(129, 19);
            this.lblStrong.TabIndex = 1;
            this.lblStrong.Text = "none";
            this.lblStrong.Click += new System.EventHandler(this.lblStrong_Click);
            this.lblStrong.DoubleClick += new System.EventHandler(this.lblStrong_DoubleClick);
            // 
            // lblGreekWord
            // 
            this.lblGreekWord.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblGreekWord.Appearance.Options.UseFont = true;
            this.lblGreekWord.Appearance.Options.UseTextOptions = true;
            this.lblGreekWord.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.tablePanel.SetColumn(this.lblGreekWord, 0);
            this.tablePanel.SetColumnSpan(this.lblGreekWord, 2);
            this.lblGreekWord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGreekWord.Location = new System.Drawing.Point(3, 32);
            this.lblGreekWord.Name = "lblGreekWord";
            this.tablePanel.SetRow(this.lblGreekWord, 1);
            this.lblGreekWord.Size = new System.Drawing.Size(264, 21);
            this.lblGreekWord.TabIndex = 0;
            this.lblGreekWord.Text = "none";
            this.lblGreekWord.DoubleClick += new System.EventHandler(this.lblGreekWord_DoubleClick);
            // 
            // VerseWordEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tablePanel);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "VerseWordEditorControl";
            this.Size = new System.Drawing.Size(270, 262);
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).EndInit();
            this.tablePanel.ResumeLayout(false);
            this.tablePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFootnoteText.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbCitation.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbWordOfJesus.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTranslation.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.Utils.Layout.TablePanel tablePanel;
        private DevExpress.XtraEditors.TextEdit txtTranslation;
        private DevExpress.XtraEditors.LabelControl lblGrammarCode;
        private DevExpress.XtraEditors.LabelControl lblTransliteration;
        private DevExpress.XtraEditors.LabelControl lblStrong;
        private DevExpress.XtraEditors.LabelControl lblGreekWord;
        private DevExpress.XtraEditors.CheckEdit cbCitation;
        private DevExpress.XtraEditors.CheckEdit cbWordOfJesus;
        private DevExpress.XtraEditors.MemoEdit txtFootnoteText;
        private DevExpress.XtraEditors.LabelControl lblNumberOfVerseWord;
        private DevExpress.XtraBars.StandaloneBarDockControl standaloneBarDockControl1;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarButtonItem btnInsertEmptyString;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraEditors.SimpleButton btnDelete;
    }
}
