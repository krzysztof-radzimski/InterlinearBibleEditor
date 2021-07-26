
namespace IBE.WindowsClient {
    partial class StrongsCodesForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StrongsCodesForm));
            this.grid = new DevExpress.XtraGrid.GridControl();
            this.view = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colTopic = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSourceWord = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTrans = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colShortDefinition = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDefinition = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemRichTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit();
            this.repositoryItemHypertextLabel1 = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.view)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRichTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHypertextLabel1)).BeginInit();
            this.SuspendLayout();
            // 
            // grid
            // 
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.Location = new System.Drawing.Point(0, 0);
            this.grid.MainView = this.view;
            this.grid.Name = "grid";
            this.grid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemHypertextLabel1,
            this.repositoryItemRichTextEdit1});
            this.grid.Size = new System.Drawing.Size(800, 450);
            this.grid.TabIndex = 1;
            this.grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.view});
            // 
            // view
            // 
            this.view.Appearance.Row.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.view.Appearance.Row.Options.UseFont = true;
            this.view.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.view.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colTopic,
            this.colSourceWord,
            this.colTrans,
            this.colShortDefinition,
            this.colDefinition});
            this.view.GridControl = this.grid;
            this.view.Name = "view";
            this.view.OptionsBehavior.Editable = false;
            this.view.OptionsBehavior.ReadOnly = true;
            this.view.OptionsFind.AlwaysVisible = true;
            this.view.OptionsView.ColumnAutoWidth = false;
            this.view.OptionsView.RowAutoHeight = true;
            this.view.OptionsView.ShowDetailButtons = false;
            this.view.OptionsView.ShowGroupPanel = false;
            this.view.OptionsView.ShowIndicator = false;
            // 
            // colTopic
            // 
            this.colTopic.Caption = "Topic";
            this.colTopic.FieldName = "Topic";
            this.colTopic.Name = "colTopic";
            this.colTopic.Visible = true;
            this.colTopic.VisibleIndex = 0;
            // 
            // colSourceWord
            // 
            this.colSourceWord.Caption = "Source Word";
            this.colSourceWord.FieldName = "SourceWord";
            this.colSourceWord.Name = "colSourceWord";
            this.colSourceWord.Visible = true;
            this.colSourceWord.VisibleIndex = 2;
            // 
            // colTrans
            // 
            this.colTrans.Caption = "Transliteration";
            this.colTrans.FieldName = "Transliteration";
            this.colTrans.Name = "colTrans";
            this.colTrans.Visible = true;
            this.colTrans.VisibleIndex = 1;
            // 
            // colShortDefinition
            // 
            this.colShortDefinition.Caption = "Short Definition";
            this.colShortDefinition.FieldName = "ShortDefinition";
            this.colShortDefinition.Name = "colShortDefinition";
            this.colShortDefinition.Visible = true;
            this.colShortDefinition.VisibleIndex = 3;
            // 
            // colDefinition
            // 
            this.colDefinition.AppearanceCell.Options.UseTextOptions = true;
            this.colDefinition.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colDefinition.Caption = "Description";
            this.colDefinition.ColumnEdit = this.repositoryItemRichTextEdit1;
            this.colDefinition.FieldName = "Definition";
            this.colDefinition.Name = "colDefinition";
            this.colDefinition.Visible = true;
            this.colDefinition.VisibleIndex = 4;
            // 
            // repositoryItemRichTextEdit1
            // 
            this.repositoryItemRichTextEdit1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.repositoryItemRichTextEdit1.DocumentFormat = DevExpress.XtraRichEdit.DocumentFormat.Html;
            this.repositoryItemRichTextEdit1.EncodingWebName = "utf-8";
            this.repositoryItemRichTextEdit1.Name = "repositoryItemRichTextEdit1";
            this.repositoryItemRichTextEdit1.OptionsImport.FallbackFormat = DevExpress.XtraRichEdit.DocumentFormat.Html;
            this.repositoryItemRichTextEdit1.ReadOnly = true;
            this.repositoryItemRichTextEdit1.ShowCaretInReadOnly = false;
            // 
            // repositoryItemHypertextLabel1
            // 
            this.repositoryItemHypertextLabel1.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.repositoryItemHypertextLabel1.Appearance.Options.UseTextOptions = true;
            this.repositoryItemHypertextLabel1.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.repositoryItemHypertextLabel1.Name = "repositoryItemHypertextLabel1";
            // 
            // StrongsCodesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.grid);
            this.IconOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("StrongsCodesForm.IconOptions.SvgImage")));
            this.Name = "StrongsCodesForm";
            this.Text = "Strongs Codes";
            this.Load += new System.EventHandler(this.StrongsCodesForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.view)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRichTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHypertextLabel1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl grid;
        private DevExpress.XtraGrid.Views.Grid.GridView view;
        private DevExpress.XtraGrid.Columns.GridColumn colSourceWord;
        private DevExpress.XtraGrid.Columns.GridColumn colShortDefinition;
        private DevExpress.XtraGrid.Columns.GridColumn colDefinition;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel repositoryItemHypertextLabel1;
        private DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit repositoryItemRichTextEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn colTrans;
        private DevExpress.XtraGrid.Columns.GridColumn colTopic;
    }
}