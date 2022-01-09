
namespace IBE.WindowsClient {
    partial class VerseGridForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VerseGridForm));
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.btnSaveVerse = new DevExpress.XtraBars.BarButtonItem();
            this.btnNextVerse = new DevExpress.XtraBars.BarButtonItem();
            this.btnPreviousVerse = new DevExpress.XtraBars.BarButtonItem();
            this.btnAddWord = new DevExpress.XtraBars.BarButtonItem();
            this.btnAddWords = new DevExpress.XtraBars.BarButtonItem();
            this.btnDeleteWords = new DevExpress.XtraBars.BarButtonItem();
            this.btnRenumerateWords = new DevExpress.XtraBars.BarButtonItem();
            this.btnSetAllAsJesusWords = new DevExpress.XtraBars.BarButtonItem();
            this.btnOblubienicaEu = new DevExpress.XtraBars.BarButtonItem();
            this.btnExportChapterToPDF = new DevExpress.XtraBars.BarButtonItem();
            this.btnExportChapterToWord = new DevExpress.XtraBars.BarButtonItem();
            this.btnLogosSeptuagint = new DevExpress.XtraBars.BarButtonItem();
            this.btnExportBookToPdf = new DevExpress.XtraBars.BarButtonItem();
            this.btnExportBookToDocx = new DevExpress.XtraBars.BarButtonItem();
            this.btnAutoTranslateChapter = new DevExpress.XtraBars.BarButtonItem();
            this.btnUpdateDictionary = new DevExpress.XtraBars.BarButtonItem();
            this.btnDeleteWord = new DevExpress.XtraBars.BarButtonItem();
            this.btnAutoTranslateVerse = new DevExpress.XtraBars.BarButtonItem();
            this.rpHome = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.pnlTop = new DevExpress.XtraEditors.PanelControl();
            this.lblVerseCount = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtIndex = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtBooks = new DevExpress.XtraEditors.LookUpEdit();
            this.txtChapter = new DevExpress.XtraEditors.LookUpEdit();
            this.txtVerse = new DevExpress.XtraEditors.LookUpEdit();
            this.pnlContent = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTop)).BeginInit();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtIndex.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBooks.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtChapter.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVerse.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlContent)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.ribbonControl1.SearchEditItem,
            this.btnSaveVerse,
            this.btnNextVerse,
            this.btnPreviousVerse,
            this.btnAddWord,
            this.btnAddWords,
            this.btnDeleteWords,
            this.btnRenumerateWords,
            this.btnSetAllAsJesusWords,
            this.btnOblubienicaEu,
            this.btnExportChapterToPDF,
            this.btnExportChapterToWord,
            this.btnLogosSeptuagint,
            this.btnExportBookToPdf,
            this.btnExportBookToDocx,
            this.btnAutoTranslateChapter,
            this.btnUpdateDictionary,
            this.btnDeleteWord,
            this.btnAutoTranslateVerse});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 24;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.rpHome});
            this.ribbonControl1.Size = new System.Drawing.Size(1163, 130);
            // 
            // btnSaveVerse
            // 
            this.btnSaveVerse.Caption = "Save";
            this.btnSaveVerse.Id = 1;
            this.btnSaveVerse.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnSaveVerse.ImageOptions.SvgImage")));
            this.btnSaveVerse.Name = "btnSaveVerse";
            this.btnSaveVerse.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnSaveVerse_ItemClick);
            // 
            // btnNextVerse
            // 
            this.btnNextVerse.Caption = "Next verse";
            this.btnNextVerse.Enabled = false;
            this.btnNextVerse.Id = 5;
            this.btnNextVerse.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnNextVerse.ImageOptions.SvgImage")));
            this.btnNextVerse.Name = "btnNextVerse";
            this.btnNextVerse.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnNextVerse_ItemClick);
            // 
            // btnPreviousVerse
            // 
            this.btnPreviousVerse.Caption = "Previous verse";
            this.btnPreviousVerse.Enabled = false;
            this.btnPreviousVerse.Id = 6;
            this.btnPreviousVerse.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnPreviousVerse.ImageOptions.SvgImage")));
            this.btnPreviousVerse.Name = "btnPreviousVerse";
            this.btnPreviousVerse.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnPreviousVerse_ItemClick);
            // 
            // btnAddWord
            // 
            this.btnAddWord.Caption = "Add Word";
            this.btnAddWord.Id = 7;
            this.btnAddWord.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnAddWord.ImageOptions.SvgImage")));
            this.btnAddWord.Name = "btnAddWord";
            this.btnAddWord.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnAddWord_ItemClick);
            // 
            // btnAddWords
            // 
            this.btnAddWords.Caption = "Add words";
            this.btnAddWords.Id = 9;
            this.btnAddWords.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnAddWords.ImageOptions.SvgImage")));
            this.btnAddWords.Name = "btnAddWords";
            this.btnAddWords.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnAddWords_ItemClick);
            // 
            // btnDeleteWords
            // 
            this.btnDeleteWords.Caption = "Delete words";
            this.btnDeleteWords.Id = 10;
            this.btnDeleteWords.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnDeleteWords.ImageOptions.SvgImage")));
            this.btnDeleteWords.Name = "btnDeleteWords";
            this.btnDeleteWords.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnDeleteWords_ItemClick);
            // 
            // btnRenumerateWords
            // 
            this.btnRenumerateWords.Caption = "Renumerate words";
            this.btnRenumerateWords.Id = 11;
            this.btnRenumerateWords.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnRenumerateWords.ImageOptions.SvgImage")));
            this.btnRenumerateWords.Name = "btnRenumerateWords";
            this.btnRenumerateWords.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnRenumerateWords_ItemClick);
            // 
            // btnSetAllAsJesusWords
            // 
            this.btnSetAllAsJesusWords.Caption = "All as God\'s words";
            this.btnSetAllAsJesusWords.Id = 12;
            this.btnSetAllAsJesusWords.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnSetAllAsJesusWords.ImageOptions.SvgImage")));
            this.btnSetAllAsJesusWords.Name = "btnSetAllAsJesusWords";
            this.btnSetAllAsJesusWords.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnSetAllAsJesusWords_ItemClick);
            // 
            // btnOblubienicaEu
            // 
            this.btnOblubienicaEu.Caption = "Oblubienica.eu";
            this.btnOblubienicaEu.Id = 13;
            this.btnOblubienicaEu.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnOblubienicaEu.ImageOptions.SvgImage")));
            this.btnOblubienicaEu.Name = "btnOblubienicaEu";
            this.btnOblubienicaEu.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.btnOblubienicaEu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnOblubienicaEu_ItemClick);
            // 
            // btnExportChapterToPDF
            // 
            this.btnExportChapterToPDF.Caption = "Export chapter";
            this.btnExportChapterToPDF.Enabled = false;
            this.btnExportChapterToPDF.Id = 14;
            this.btnExportChapterToPDF.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnExportChapterToPDF.ImageOptions.SvgImage")));
            this.btnExportChapterToPDF.Name = "btnExportChapterToPDF";
            this.btnExportChapterToPDF.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnExportChapterToPDF_ItemClick);
            // 
            // btnExportChapterToWord
            // 
            this.btnExportChapterToWord.Caption = "Export chapter";
            this.btnExportChapterToWord.Enabled = false;
            this.btnExportChapterToWord.Id = 15;
            this.btnExportChapterToWord.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnExportChapterToWord.ImageOptions.SvgImage")));
            this.btnExportChapterToWord.Name = "btnExportChapterToWord";
            this.btnExportChapterToWord.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnExportChapterToWord_ItemClick);
            // 
            // btnLogosSeptuagint
            // 
            this.btnLogosSeptuagint.Caption = "Logos Setuagint";
            this.btnLogosSeptuagint.Id = 16;
            this.btnLogosSeptuagint.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnLogosSeptuagint.ImageOptions.SvgImage")));
            this.btnLogosSeptuagint.Name = "btnLogosSeptuagint";
            this.btnLogosSeptuagint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.btnLogosSeptuagint.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnLogosSeptuagint_ItemClick);
            // 
            // btnExportBookToPdf
            // 
            this.btnExportBookToPdf.Caption = "Export book";
            this.btnExportBookToPdf.Enabled = false;
            this.btnExportBookToPdf.Id = 17;
            this.btnExportBookToPdf.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnExportBookToPdf.ImageOptions.SvgImage")));
            this.btnExportBookToPdf.Name = "btnExportBookToPdf";
            this.btnExportBookToPdf.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnExportBookToPdf_ItemClick);
            // 
            // btnExportBookToDocx
            // 
            this.btnExportBookToDocx.Caption = "Export book";
            this.btnExportBookToDocx.Enabled = false;
            this.btnExportBookToDocx.Id = 18;
            this.btnExportBookToDocx.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnExportBookToDocx.ImageOptions.SvgImage")));
            this.btnExportBookToDocx.Name = "btnExportBookToDocx";
            this.btnExportBookToDocx.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnExportBookToDocx_ItemClick);
            // 
            // btnAutoTranslateChapter
            // 
            this.btnAutoTranslateChapter.Caption = "Auto-Translate chapter";
            this.btnAutoTranslateChapter.Enabled = false;
            this.btnAutoTranslateChapter.Id = 19;
            this.btnAutoTranslateChapter.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnAutoTranslateChapter.ImageOptions.SvgImage")));
            this.btnAutoTranslateChapter.Name = "btnAutoTranslateChapter";
            this.btnAutoTranslateChapter.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnAutoTranslateChapter_ItemClick);
            // 
            // btnUpdateDictionary
            // 
            this.btnUpdateDictionary.Caption = "Update dictionary";
            this.btnUpdateDictionary.Id = 20;
            this.btnUpdateDictionary.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnUpdateDictionary.ImageOptions.SvgImage")));
            this.btnUpdateDictionary.Name = "btnUpdateDictionary";
            this.btnUpdateDictionary.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnUpdateDictionary_ItemClick);
            // 
            // btnDeleteWord
            // 
            this.btnDeleteWord.Caption = "Delete word";
            this.btnDeleteWord.Id = 21;
            this.btnDeleteWord.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnDeleteWord.ImageOptions.SvgImage")));
            this.btnDeleteWord.Name = "btnDeleteWord";
            this.btnDeleteWord.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnDeleteWord_ItemClick);
            // 
            // btnAutoTranslateVerse
            // 
            this.btnAutoTranslateVerse.Caption = "Auto-Translate verse";
            this.btnAutoTranslateVerse.Enabled = false;
            this.btnAutoTranslateVerse.Id = 23;
            this.btnAutoTranslateVerse.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnAutoTranslateVerse.ImageOptions.SvgImage")));
            this.btnAutoTranslateVerse.Name = "btnAutoTranslateVerse";
            this.btnAutoTranslateVerse.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnAutoTranslateVerse_ItemClick);
            // 
            // rpHome
            // 
            this.rpHome.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1});
            this.rpHome.Name = "rpHome";
            this.rpHome.Text = "Home";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.btnSaveVerse);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnPreviousVerse, true);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnNextVerse);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnAddWord, true);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnAddWords);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnDeleteWords, true);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnDeleteWord);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnRenumerateWords);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnSetAllAsJesusWords, true);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnOblubienicaEu, true);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnLogosSeptuagint);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnExportChapterToPDF, true);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnExportChapterToWord);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnExportBookToPdf, true);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnExportBookToDocx);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnAutoTranslateChapter);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnAutoTranslateVerse);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnUpdateDictionary);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "ribbonPageGroup1";
            // 
            // pnlTop
            // 
            this.pnlTop.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pnlTop.Controls.Add(this.lblVerseCount);
            this.pnlTop.Controls.Add(this.labelControl4);
            this.pnlTop.Controls.Add(this.labelControl3);
            this.pnlTop.Controls.Add(this.labelControl2);
            this.pnlTop.Controls.Add(this.txtIndex);
            this.pnlTop.Controls.Add(this.labelControl1);
            this.pnlTop.Controls.Add(this.txtBooks);
            this.pnlTop.Controls.Add(this.txtChapter);
            this.pnlTop.Controls.Add(this.txtVerse);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 130);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1163, 33);
            this.pnlTop.TabIndex = 1;
            // 
            // lblVerseCount
            // 
            this.lblVerseCount.Location = new System.Drawing.Point(552, 10);
            this.lblVerseCount.Name = "lblVerseCount";
            this.lblVerseCount.Size = new System.Drawing.Size(9, 13);
            this.lblVerseCount.TabIndex = 8;
            this.lblVerseCount.Text = "...";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(449, 10);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(31, 13);
            this.labelControl4.TabIndex = 6;
            this.labelControl4.Text = "Verse:";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(333, 10);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(44, 13);
            this.labelControl3.TabIndex = 4;
            this.labelControl3.Text = "Chapter:";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(155, 10);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(29, 13);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "Book:";
            // 
            // txtIndex
            // 
            this.txtIndex.Location = new System.Drawing.Point(49, 7);
            this.txtIndex.MenuManager = this.ribbonControl1;
            this.txtIndex.Name = "txtIndex";
            this.txtIndex.Size = new System.Drawing.Size(100, 20);
            this.txtIndex.TabIndex = 1;
            this.txtIndex.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtIndex_KeyUp);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 10);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(31, 13);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Index:";
            // 
            // txtBooks
            // 
            this.txtBooks.Location = new System.Drawing.Point(190, 7);
            this.txtBooks.MenuManager = this.ribbonControl1;
            this.txtBooks.Name = "txtBooks";
            this.txtBooks.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtBooks.Properties.NullText = "";
            this.txtBooks.Properties.PopupSizeable = false;
            this.txtBooks.Size = new System.Drawing.Size(137, 20);
            this.txtBooks.TabIndex = 3;
            this.txtBooks.EditValueChanged += new System.EventHandler(this.editBook_EditValueChanged);
            // 
            // txtChapter
            // 
            this.txtChapter.Location = new System.Drawing.Point(383, 7);
            this.txtChapter.MenuManager = this.ribbonControl1;
            this.txtChapter.Name = "txtChapter";
            this.txtChapter.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtChapter.Properties.NullText = "";
            this.txtChapter.Properties.PopupSizeable = false;
            this.txtChapter.Size = new System.Drawing.Size(60, 20);
            this.txtChapter.TabIndex = 5;
            this.txtChapter.EditValueChanged += new System.EventHandler(this.editChapter_EditValueChanged);
            // 
            // txtVerse
            // 
            this.txtVerse.Location = new System.Drawing.Point(486, 7);
            this.txtVerse.MenuManager = this.ribbonControl1;
            this.txtVerse.Name = "txtVerse";
            this.txtVerse.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtVerse.Properties.NullText = "";
            this.txtVerse.Properties.PopupSizeable = false;
            this.txtVerse.Size = new System.Drawing.Size(60, 20);
            this.txtVerse.TabIndex = 7;
            this.txtVerse.EditValueChanged += new System.EventHandler(this.editVerse_EditValueChanged);
            // 
            // pnlContent
            // 
            this.pnlContent.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 163);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(1163, 482);
            this.pnlContent.TabIndex = 3;
            // 
            // VerseGridForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1163, 645);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.ribbonControl1);
            this.Name = "VerseGridForm";
            this.Ribbon = this.ribbonControl1;
            this.Text = "InterlinearEditorForm";
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTop)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtIndex.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBooks.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtChapter.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVerse.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlContent)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.Ribbon.RibbonPage rpHome;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.BarButtonItem btnSaveVerse;
        private DevExpress.XtraBars.BarButtonItem btnNextVerse;
        private DevExpress.XtraBars.BarButtonItem btnPreviousVerse;
        private DevExpress.XtraBars.BarButtonItem btnAddWord;
        private DevExpress.XtraBars.BarButtonItem btnAddWords;
        private DevExpress.XtraBars.BarButtonItem btnDeleteWords;
        private DevExpress.XtraBars.BarButtonItem btnRenumerateWords;
        private DevExpress.XtraBars.BarButtonItem btnSetAllAsJesusWords;
        private DevExpress.XtraBars.BarButtonItem btnOblubienicaEu;
        private DevExpress.XtraBars.BarButtonItem btnExportChapterToPDF;
        private DevExpress.XtraBars.BarButtonItem btnExportChapterToWord;
        private DevExpress.XtraBars.BarButtonItem btnLogosSeptuagint;
        private DevExpress.XtraBars.BarButtonItem btnExportBookToPdf;
        private DevExpress.XtraBars.BarButtonItem btnExportBookToDocx;
        private DevExpress.XtraBars.BarButtonItem btnAutoTranslateChapter;
        private DevExpress.XtraBars.BarButtonItem btnUpdateDictionary;
        private DevExpress.XtraBars.BarButtonItem btnDeleteWord;
        private DevExpress.XtraEditors.PanelControl pnlTop;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtIndex;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LookUpEdit txtBooks;
        private DevExpress.XtraEditors.LookUpEdit txtChapter;
        private DevExpress.XtraEditors.LookUpEdit txtVerse;
        private DevExpress.XtraEditors.PanelControl pnlContent;
        private DevExpress.XtraEditors.LabelControl lblVerseCount;
        private DevExpress.XtraBars.BarButtonItem btnAutoTranslateVerse;
    }
}