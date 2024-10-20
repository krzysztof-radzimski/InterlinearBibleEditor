﻿
namespace IBE.WindowsClient {
    partial class InterlinearEditorForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InterlinearEditorForm));
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.btnSaveVerse = new DevExpress.XtraBars.BarButtonItem();
            this.txtBook = new DevExpress.XtraBars.BarEditItem();
            this.editBook = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.txtChapter = new DevExpress.XtraBars.BarEditItem();
            this.editChapter = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.txtVerse = new DevExpress.XtraBars.BarEditItem();
            this.editVerse = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
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
            this.rpHome = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.btnExportBookToPdf = new DevExpress.XtraBars.BarButtonItem();
            this.btnExportBookToDocx = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBook)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editChapter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editVerse)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.ribbonControl1.SearchEditItem,
            this.btnSaveVerse,
            this.txtBook,
            this.txtChapter,
            this.txtVerse,
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
            this.btnExportBookToDocx});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 19;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.rpHome});
            this.ribbonControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.editBook,
            this.editChapter,
            this.editVerse});
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
            // txtBook
            // 
            this.txtBook.Caption = "Book";
            this.txtBook.Edit = this.editBook;
            this.txtBook.EditWidth = 150;
            this.txtBook.Id = 2;
            this.txtBook.Name = "txtBook";
            // 
            // editBook
            // 
            this.editBook.AutoHeight = false;
            this.editBook.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFit;
            this.editBook.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.editBook.DropDownRows = 20;
            this.editBook.Name = "editBook";
            this.editBook.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            this.editBook.ShowFooter = false;
            this.editBook.ShowHeader = false;
            this.editBook.EditValueChanged += new System.EventHandler(this.editBook_EditValueChanged);
            // 
            // txtChapter
            // 
            this.txtChapter.Caption = "Chapter";
            this.txtChapter.Edit = this.editChapter;
            this.txtChapter.EditWidth = 150;
            this.txtChapter.Id = 3;
            this.txtChapter.Name = "txtChapter";
            // 
            // editChapter
            // 
            this.editChapter.AutoHeight = false;
            this.editChapter.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.editChapter.Name = "editChapter";
            this.editChapter.EditValueChanged += new System.EventHandler(this.editChapter_EditValueChanged);
            // 
            // txtVerse
            // 
            this.txtVerse.Caption = "Verse";
            this.txtVerse.Edit = this.editVerse;
            this.txtVerse.EditWidth = 150;
            this.txtVerse.Id = 4;
            this.txtVerse.Name = "txtVerse";
            // 
            // editVerse
            // 
            this.editVerse.AutoHeight = false;
            this.editVerse.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.editVerse.Name = "editVerse";
            this.editVerse.EditValueChanged += new System.EventHandler(this.editVerse_EditValueChanged);
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
            this.ribbonPageGroup1.ItemLinks.Add(this.txtBook, true);
            this.ribbonPageGroup1.ItemLinks.Add(this.txtChapter);
            this.ribbonPageGroup1.ItemLinks.Add(this.txtVerse);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnPreviousVerse, true);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnNextVerse);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnAddWord, true);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnAddWords);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnDeleteWords, true);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnRenumerateWords);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnSetAllAsJesusWords, true);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnOblubienicaEu, true);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnLogosSeptuagint);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnExportChapterToPDF, true);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnExportChapterToWord);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnExportBookToPdf, true);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnExportBookToDocx);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "ribbonPageGroup1";
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
            // InterlinearEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1163, 450);
            this.Controls.Add(this.ribbonControl1);
            this.Name = "InterlinearEditorForm";
            this.Ribbon = this.ribbonControl1;
            this.Text = "InterlinearEditorForm";
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBook)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editChapter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editVerse)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.Ribbon.RibbonPage rpHome;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.BarButtonItem btnSaveVerse;
        private DevExpress.XtraBars.BarEditItem txtBook;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit editBook;
        private DevExpress.XtraBars.BarEditItem txtChapter;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit editChapter;
        private DevExpress.XtraBars.BarEditItem txtVerse;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit editVerse;
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
    }
}