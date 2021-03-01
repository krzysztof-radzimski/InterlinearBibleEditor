
namespace IBE.WindowsClient {
    partial class ViewerForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewerForm));
            this.WebBrowser = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.cbBooksList = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.txtChapter = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.rpHome = new DevExpress.XtraBars.Ribbon.RibbonPage();
            ((System.ComponentModel.ISupportInitialize)(this.WebBrowser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbBooksList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtChapter)).BeginInit();
            this.SuspendLayout();
            // 
            // WebBrowser
            // 
            this.WebBrowser.CreationProperties = null;
            this.WebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WebBrowser.Location = new System.Drawing.Point(0, 67);
            this.WebBrowser.Name = "WebBrowser";
            this.WebBrowser.Size = new System.Drawing.Size(800, 383);
            this.WebBrowser.TabIndex = 3;
            this.WebBrowser.ZoomFactor = 1D;
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.CommandLayout = DevExpress.XtraBars.Ribbon.CommandLayout.Simplified;
            this.ribbonControl1.DrawGroupCaptions = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.ribbonControl1.SearchEditItem});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 1;
            this.ribbonControl1.MdiMergeStyle = DevExpress.XtraBars.Ribbon.RibbonMdiMergeStyle.Always;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.rpHome});
            this.ribbonControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.cbBooksList,
            this.txtChapter});
            this.ribbonControl1.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl1.ShowExpandCollapseButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl1.Size = new System.Drawing.Size(800, 67);
            // 
            // cbBooksList
            // 
            this.cbBooksList.AutoHeight = false;
            this.cbBooksList.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbBooksList.Name = "cbBooksList";
            this.cbBooksList.NullValuePrompt = "Select book";
            this.cbBooksList.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbBooksList.SelectedIndexChanged += new System.EventHandler(this.cbBooksList_SelectedIndexChanged);
            // 
            // txtChapter
            // 
            this.txtChapter.AutoHeight = false;
            this.txtChapter.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtChapter.IsFloatValue = false;
            this.txtChapter.MaskSettings.Set("mask", "N00");
            this.txtChapter.Name = "txtChapter";
            // 
            // rpHome
            // 
            this.rpHome.Name = "rpHome";
            this.rpHome.Text = "Home";
            // 
            // ViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.WebBrowser);
            this.Controls.Add(this.ribbonControl1);
            this.IconOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("ViewerForm.IconOptions.SvgImage")));
            this.Name = "ViewerForm";
            this.Ribbon = this.ribbonControl1;
            this.Text = "ViewerForm";
            ((System.ComponentModel.ISupportInitialize)(this.WebBrowser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbBooksList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtChapter)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.Web.WebView2.WinForms.WebView2 WebBrowser;
        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.Ribbon.RibbonPage rpHome;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cbBooksList;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit txtChapter;
    }
}