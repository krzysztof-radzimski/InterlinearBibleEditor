
namespace IBE.WindowsClient.Controls {
    partial class VerseEditorControl {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VerseEditorControl));
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.tabPane1 = new DevExpress.XtraBars.Navigation.TabPane();
            this.tabNavigationPage1 = new DevExpress.XtraBars.Navigation.TabNavigationPage();
            this.gridTranslations = new DevExpress.XtraGrid.GridControl();
            this.gridViewTranslations = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colTranslationName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colVerseText = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tabNavigationPage2 = new DevExpress.XtraBars.Navigation.TabNavigationPage();
            this.wbStrong = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.tabNavigationPage3 = new DevExpress.XtraBars.Navigation.TabNavigationPage();
            this.wbGrammarCodes = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.tabNavigationPage4 = new DevExpress.XtraBars.Navigation.TabNavigationPage();
            this.tablePanel1 = new DevExpress.Utils.Layout.TablePanel();
            this.cbStartFromNewLine = new DevExpress.XtraEditors.CheckEdit();
            this.splitterControl1 = new DevExpress.XtraEditors.SplitterControl();
            ((System.ComponentModel.ISupportInitialize)(this.tabPane1)).BeginInit();
            this.tabPane1.SuspendLayout();
            this.tabNavigationPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridTranslations)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTranslations)).BeginInit();
            this.tabNavigationPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.wbStrong)).BeginInit();
            this.tabNavigationPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.wbGrammarCodes)).BeginInit();
            this.tabNavigationPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).BeginInit();
            this.tablePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbStartFromNewLine.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.AutoScroll = true;
            this.flowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Size = new System.Drawing.Size(974, 220);
            this.flowLayoutPanel.TabIndex = 0;
            // 
            // tabPane1
            // 
            this.tabPane1.Controls.Add(this.tabNavigationPage1);
            this.tabPane1.Controls.Add(this.tabNavigationPage2);
            this.tabPane1.Controls.Add(this.tabNavigationPage3);
            this.tabPane1.Controls.Add(this.tabNavigationPage4);
            this.tabPane1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabPane1.Location = new System.Drawing.Point(0, 230);
            this.tabPane1.Name = "tabPane1";
            this.tabPane1.Pages.AddRange(new DevExpress.XtraBars.Navigation.NavigationPageBase[] {
            this.tabNavigationPage1,
            this.tabNavigationPage2,
            this.tabNavigationPage3,
            this.tabNavigationPage4});
            this.tabPane1.RegularSize = new System.Drawing.Size(974, 400);
            this.tabPane1.SelectedPage = this.tabNavigationPage1;
            this.tabPane1.Size = new System.Drawing.Size(974, 400);
            this.tabPane1.TabIndex = 1;
            this.tabPane1.Text = "tabPane1";
            // 
            // tabNavigationPage1
            // 
            this.tabNavigationPage1.Caption = "Translations";
            this.tabNavigationPage1.Controls.Add(this.gridTranslations);
            this.tabNavigationPage1.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("tabNavigationPage1.ImageOptions.SvgImage")));
            this.tabNavigationPage1.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.tabNavigationPage1.ItemShowMode = DevExpress.XtraBars.Navigation.ItemShowMode.ImageAndText;
            this.tabNavigationPage1.Name = "tabNavigationPage1";
            this.tabNavigationPage1.Properties.ShowMode = DevExpress.XtraBars.Navigation.ItemShowMode.ImageAndText;
            this.tabNavigationPage1.Size = new System.Drawing.Size(974, 372);
            // 
            // gridTranslations
            // 
            this.gridTranslations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridTranslations.Location = new System.Drawing.Point(0, 0);
            this.gridTranslations.MainView = this.gridViewTranslations;
            this.gridTranslations.Name = "gridTranslations";
            this.gridTranslations.Size = new System.Drawing.Size(974, 372);
            this.gridTranslations.TabIndex = 0;
            this.gridTranslations.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewTranslations});
            // 
            // gridViewTranslations
            // 
            this.gridViewTranslations.Appearance.Row.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.gridViewTranslations.Appearance.Row.Options.UseFont = true;
            this.gridViewTranslations.Appearance.Row.Options.UseTextOptions = true;
            this.gridViewTranslations.Appearance.Row.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gridViewTranslations.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.gridViewTranslations.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colTranslationName,
            this.colVerseText});
            this.gridViewTranslations.GridControl = this.gridTranslations;
            this.gridViewTranslations.Name = "gridViewTranslations";
            this.gridViewTranslations.OptionsBehavior.AutoPopulateColumns = false;
            this.gridViewTranslations.OptionsBehavior.Editable = false;
            this.gridViewTranslations.OptionsBehavior.ReadOnly = true;
            this.gridViewTranslations.OptionsView.ColumnAutoWidth = false;
            this.gridViewTranslations.OptionsView.RowAutoHeight = true;
            this.gridViewTranslations.OptionsView.ShowColumnHeaders = false;
            this.gridViewTranslations.OptionsView.ShowDetailButtons = false;
            this.gridViewTranslations.OptionsView.ShowGroupPanel = false;
            this.gridViewTranslations.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewTranslations.OptionsView.ShowIndicator = false;
            this.gridViewTranslations.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewTranslations.RowSeparatorHeight = 10;
            // 
            // colTranslationName
            // 
            this.colTranslationName.AppearanceCell.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.colTranslationName.AppearanceCell.Options.UseFont = true;
            this.colTranslationName.Caption = "gridColumn1";
            this.colTranslationName.FieldName = "TranslationName";
            this.colTranslationName.Name = "colTranslationName";
            this.colTranslationName.Visible = true;
            this.colTranslationName.VisibleIndex = 0;
            // 
            // colVerseText
            // 
            this.colVerseText.Caption = "gridColumn2";
            this.colVerseText.FieldName = "VerseText";
            this.colVerseText.Name = "colVerseText";
            this.colVerseText.Visible = true;
            this.colVerseText.VisibleIndex = 1;
            // 
            // tabNavigationPage2
            // 
            this.tabNavigationPage2.Caption = "Strong";
            this.tabNavigationPage2.Controls.Add(this.wbStrong);
            this.tabNavigationPage2.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("tabNavigationPage2.ImageOptions.SvgImage")));
            this.tabNavigationPage2.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.tabNavigationPage2.ItemShowMode = DevExpress.XtraBars.Navigation.ItemShowMode.ImageAndText;
            this.tabNavigationPage2.Name = "tabNavigationPage2";
            this.tabNavigationPage2.Properties.ShowMode = DevExpress.XtraBars.Navigation.ItemShowMode.ImageAndText;
            this.tabNavigationPage2.Size = new System.Drawing.Size(974, 372);
            // 
            // wbStrong
            // 
            this.wbStrong.CreationProperties = null;
            this.wbStrong.DefaultBackgroundColor = System.Drawing.Color.White;
            this.wbStrong.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbStrong.Location = new System.Drawing.Point(0, 0);
            this.wbStrong.Name = "wbStrong";
            this.wbStrong.Size = new System.Drawing.Size(974, 372);
            this.wbStrong.TabIndex = 0;
            this.wbStrong.ZoomFactor = 1D;
            this.wbStrong.CoreWebView2InitializationCompleted += new System.EventHandler<Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs>(this.wbStrong_CoreWebView2InitializationCompleted);
            // 
            // tabNavigationPage3
            // 
            this.tabNavigationPage3.Caption = "Grammar Code";
            this.tabNavigationPage3.Controls.Add(this.wbGrammarCodes);
            this.tabNavigationPage3.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("tabNavigationPage3.ImageOptions.SvgImage")));
            this.tabNavigationPage3.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.tabNavigationPage3.ItemShowMode = DevExpress.XtraBars.Navigation.ItemShowMode.ImageAndText;
            this.tabNavigationPage3.Name = "tabNavigationPage3";
            this.tabNavigationPage3.Properties.ShowMode = DevExpress.XtraBars.Navigation.ItemShowMode.ImageAndText;
            this.tabNavigationPage3.Size = new System.Drawing.Size(974, 372);
            // 
            // wbGrammarCodes
            // 
            this.wbGrammarCodes.CreationProperties = null;
            this.wbGrammarCodes.DefaultBackgroundColor = System.Drawing.Color.White;
            this.wbGrammarCodes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbGrammarCodes.Location = new System.Drawing.Point(0, 0);
            this.wbGrammarCodes.Name = "wbGrammarCodes";
            this.wbGrammarCodes.Size = new System.Drawing.Size(974, 372);
            this.wbGrammarCodes.TabIndex = 0;
            this.wbGrammarCodes.ZoomFactor = 1D;
            this.wbGrammarCodes.CoreWebView2InitializationCompleted += new System.EventHandler<Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs>(this.wbGrammarCodes_CoreWebView2InitializationCompleted);
            // 
            // tabNavigationPage4
            // 
            this.tabNavigationPage4.Caption = "Settings";
            this.tabNavigationPage4.Controls.Add(this.tablePanel1);
            this.tabNavigationPage4.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("tabNavigationPage4.ImageOptions.SvgImage")));
            this.tabNavigationPage4.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.tabNavigationPage4.ItemShowMode = DevExpress.XtraBars.Navigation.ItemShowMode.ImageAndText;
            this.tabNavigationPage4.Name = "tabNavigationPage4";
            this.tabNavigationPage4.Properties.ShowMode = DevExpress.XtraBars.Navigation.ItemShowMode.ImageAndText;
            this.tabNavigationPage4.Size = new System.Drawing.Size(974, 372);
            // 
            // tablePanel1
            // 
            this.tablePanel1.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 1F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 99F)});
            this.tablePanel1.Controls.Add(this.cbStartFromNewLine);
            this.tablePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePanel1.Location = new System.Drawing.Point(0, 0);
            this.tablePanel1.Name = "tablePanel1";
            this.tablePanel1.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
            this.tablePanel1.Size = new System.Drawing.Size(974, 372);
            this.tablePanel1.TabIndex = 0;
            // 
            // cbStartFromNewLine
            // 
            this.tablePanel1.SetColumn(this.cbStartFromNewLine, 1);
            this.cbStartFromNewLine.Location = new System.Drawing.Point(13, 4);
            this.cbStartFromNewLine.Name = "cbStartFromNewLine";
            this.cbStartFromNewLine.Properties.Caption = "Start from new line";
            this.tablePanel1.SetRow(this.cbStartFromNewLine, 0);
            this.cbStartFromNewLine.Size = new System.Drawing.Size(958, 18);
            this.cbStartFromNewLine.TabIndex = 0;
            // 
            // splitterControl1
            // 
            this.splitterControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitterControl1.Location = new System.Drawing.Point(0, 220);
            this.splitterControl1.Name = "splitterControl1";
            this.splitterControl1.Size = new System.Drawing.Size(974, 10);
            this.splitterControl1.TabIndex = 2;
            this.splitterControl1.TabStop = false;
            // 
            // VerseEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flowLayoutPanel);
            this.Controls.Add(this.splitterControl1);
            this.Controls.Add(this.tabPane1);
            this.Name = "VerseEditorControl";
            this.Size = new System.Drawing.Size(974, 630);
            this.Load += new System.EventHandler(this.VerseEditorControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tabPane1)).EndInit();
            this.tabPane1.ResumeLayout(false);
            this.tabNavigationPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridTranslations)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTranslations)).EndInit();
            this.tabNavigationPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.wbStrong)).EndInit();
            this.tabNavigationPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.wbGrammarCodes)).EndInit();
            this.tabNavigationPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).EndInit();
            this.tablePanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cbStartFromNewLine.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
        private DevExpress.XtraBars.Navigation.TabPane tabPane1;
        private DevExpress.XtraBars.Navigation.TabNavigationPage tabNavigationPage1;
        private DevExpress.XtraGrid.GridControl gridTranslations;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewTranslations;
        private DevExpress.XtraBars.Navigation.TabNavigationPage tabNavigationPage2;
        private DevExpress.XtraBars.Navigation.TabNavigationPage tabNavigationPage3;
        private DevExpress.XtraEditors.SplitterControl splitterControl1;
        private DevExpress.XtraGrid.Columns.GridColumn colTranslationName;
        private DevExpress.XtraGrid.Columns.GridColumn colVerseText;
        private DevExpress.XtraBars.Navigation.TabNavigationPage tabNavigationPage4;
        private DevExpress.Utils.Layout.TablePanel tablePanel1;
        private DevExpress.XtraEditors.CheckEdit cbStartFromNewLine;
        private Microsoft.Web.WebView2.WinForms.WebView2 wbStrong;
        private Microsoft.Web.WebView2.WinForms.WebView2 wbGrammarCodes;
    }
}
