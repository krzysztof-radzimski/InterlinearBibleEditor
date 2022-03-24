/*=====================================================================================

	Interlinear Bible Editor
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using DevExpress.Xpo;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Layout;
using DevExpress.XtraGrid.Views.Layout.ViewInfo;
using DevExpress.XtraRichEdit.Services;
using IBE.Common.Extensions;
using IBE.Data.Import.Greek;
using IBE.Data.Model;
using IBE.WindowsClient.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IBE.WindowsClient.Controls {
    public partial class VerseGridControl : XtraUserControl {
        private GreekTransliterationController TransliterationController { get; }
        private List<VerseWordInfo> Words { get; set; }
        public Verse Verse { get; private set; }

        public VerseGridControl() {
            InitializeComponent();
            TransliterationController = new GreekTransliterationController();
            txtDefinition.ReplaceService<ISyntaxHighlightService>(new HTMLSyntaxHighlightService(txtDefinition));
        }
        public VerseGridControl(Verse verse, bool loadOtherTranslations = true) : this() {
            LoadData(verse, loadOtherTranslations);
        }

        public void LoadData(Verse verse, bool loadOtherTranslations = true) {
            Verse = verse;
            Words = new List<VerseWordInfo>();
            if (verse.IsNotNull()) {
                foreach (var item in verse.VerseWords.OrderBy(x => x.NumberOfVerseWord)) {
                    Words.Add(new VerseWordInfo(item));
                }
            }
            gridControl.DataSource = Words;

            cbStartFromNewLine.Checked = Verse.StartFromNewLine;

            txtStoryTextLevel1.Text = String.Empty;
            txtStoryTextLevel2.Text = String.Empty;

            var subtitles = Verse.ParentChapter.Subtitles.Where(x => x.BeforeVerseNumber == Verse.NumberOfVerse);
            foreach (var subtitle in subtitles) {
                if (subtitle.Level == 1) {
                    txtStoryTextLevel1.Text = subtitle.Text;
                }
                else if (subtitle.Level == 2) {
                    txtStoryTextLevel2.Text = subtitle.Text;
                }
            }

            if (loadOtherTranslations) {
                var getTranslations = Task.Factory.StartNew(() => {
                    var index = new VerseIndex(Verse.Index);
                    var list = new List<TranslationVerseInfo>();
                    var view = new XPView(Verse.Session, typeof(Translation)) {
                        CriteriaString = "(([Type] = 4) OR ([Type] = 2)) AND [Hidden] = 0"
                    };
                    view.Properties.Add(new ViewProperty("Oid", SortDirection.None, "[Oid]", false, true));
                    view.Properties.Add(new ViewProperty("Name", SortDirection.None, "[Name]", false, true));
                    view.Properties.Add(new ViewProperty("Type", SortDirection.None, "[Type]", false, true));

                    var indexes = new List<string>();
                    foreach (ViewRecord item in view) {
                        var name = item["Name"].ToString();
                        var _index = $"{name.Replace("'", "").Replace("+", "")}.{index.NumberOfBook}.{index.NumberOfChapter}.{index.NumberOfVerse}";
                        indexes.Add(_index);

                        var tvi = new TranslationVerseInfo() {
                            TranslationName = name.Replace("'", "").Replace("+", ""),
                            SortIndex = ((TranslationType)item["Type"]).GetCategory().ToInt()
                        };
                        list.Add(tvi);
                    }

                    var _view = new XPView(Verse.Session, typeof(Verse)) {
                        Criteria = new DevExpress.Data.Filtering.InOperator("Index", indexes)
                    };

                    _view.Properties.Add(new ViewProperty("Index", SortDirection.None, "[Index]", false, true));
                    _view.Properties.Add(new ViewProperty("Text", SortDirection.None, "[Text]", false, true));

                    foreach (ViewRecord item in _view) {
                        var idx = new VerseIndex(item["Index"].ToString());
                        var tvi = list.Where(x => x.TranslationName == idx.TranslationName).FirstOrDefault();
                        if (tvi.IsNotNull()) {
                            tvi.VerseText = item["Text"].ToString().Replace("<pb/>", "").Replace("<t>", "").Replace("<m>", "").Replace("</t>", "").Replace("</m>", "").Replace("<e>", "").Replace("</e>", "");
                        };
                    }

                    return list.Where(x=>x.VerseText.IsNotNullOrEmpty()).OrderBy(x => x.SortIndex).ToList();
                });

                getTranslations.ContinueWith((x) => {
                    this.SafeInvoke(f => {
                        f.gridTranslations.DataSource = x.Result;
                        f.viewTranslations.BestFitColumns();
                        f.viewTranslations.LoadingPanelVisible = false;
                    });
                });
            }
            else {
                tabVerseTranslations.PageVisible = false;
                viewTranslations.LoadingPanelVisible = false;
            }
        }

        public void Save() {
            Verse.StartFromNewLine = cbStartFromNewLine.Checked;
            // save story level 1
            if (txtStoryTextLevel1.Text.IsNotNullOrEmpty()) {
                var subtitle = Verse.ParentChapter.Subtitles.Where(x => x.BeforeVerseNumber == Verse.NumberOfVerse && x.Level == 1).FirstOrDefault();
                if (subtitle.IsNull()) {
                    subtitle = new Subtitle(Verse.Session) {
                        BeforeVerseNumber = Verse.NumberOfVerse,
                        ParentChapter = Verse.ParentChapter,
                        Level = 1,
                        Text = txtStoryTextLevel1.Text
                    };
                }
                else {
                    subtitle.BeforeVerseNumber = Verse.NumberOfVerse;
                    subtitle.ParentChapter = Verse.ParentChapter;
                    subtitle.Level = 1;
                    subtitle.Text = txtStoryTextLevel1.Text;
                }
                subtitle.Save();
            }
            else {
                var subtitle = Verse.ParentChapter.Subtitles.Where(x => x.BeforeVerseNumber == Verse.NumberOfVerse && x.Level == 1).FirstOrDefault();
                if (subtitle.IsNotNull()) {
                    subtitle.Delete();
                }
            }

            // save story level 2
            if (txtStoryTextLevel2.Text.IsNotNullOrEmpty()) {
                var subtitle = Verse.ParentChapter.Subtitles.Where(x => x.BeforeVerseNumber == Verse.NumberOfVerse && x.Level == 2).FirstOrDefault();
                if (subtitle.IsNull()) {
                    subtitle = new Subtitle(Verse.Session) {
                        BeforeVerseNumber = Verse.NumberOfVerse,
                        ParentChapter = Verse.ParentChapter,
                        Level = 2,
                        Text = txtStoryTextLevel2.Text
                    };
                }
                else {
                    subtitle.BeforeVerseNumber = Verse.NumberOfVerse;
                    subtitle.ParentChapter = Verse.ParentChapter;
                    subtitle.Level = 2;
                    subtitle.Text = txtStoryTextLevel2.Text;
                }
                subtitle.Save();
            }
            else {
                var subtitle = Verse.ParentChapter.Subtitles.Where(x => x.BeforeVerseNumber == Verse.NumberOfVerse && x.Level == 2).FirstOrDefault();
                if (subtitle.IsNotNull()) {
                    subtitle.Delete();
                }
            }

            Words.ForEach(x => { x.Save(); });

            var text = String.Empty;
            foreach (var verseWord in Verse.VerseWords.OrderBy(x => x.NumberOfVerseWord)) {
                text += $"{verseWord.Translation} ";
            }

            Verse.Text = text.Trim();
            Verse.Save();

            var uow = Verse.Session as UnitOfWork;
            if (uow.IsNotNull()) {
                uow.CommitChanges();
                uow.ReloadChangedObjects();
            }
        }

        public bool IsModified() {
            foreach (var item in Words) {
                if (item.IsModified()) { return true; }
            }
            return default;
        }

        public void DeleteAll() {
            Verse.Session.Delete(Verse.VerseWords);
            Verse.Session.Save(Verse.VerseWords);
            Words.Clear();
            LoadData(Verse, false);
        }

        public void RenumerateAll() {
            for (int i = 0; i < Words.Count; i++) {
                Words[i].Number = i;
            }
            layoutView1.RefreshData();
        }
        public void WordOfGodAll() {
            foreach (var word in Words) {
                word.WordOfGod = true;
            }
            layoutView1.RefreshData();
        }

        public class VerseWordInfo {
            [System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)] public VerseWord Word { get; }

            [DisplayName("Number")] public int Number { get; set; }
            [DisplayName("Strong's Code")] public int? StrongsCode { get; set; }
            [DisplayName("Grammar Code")] public string GrammarCode { get; set; }
            [DisplayName("Greek word")] public string SourceWord { get; set; }
            [DisplayName("Transliteration")] public string Transliteration { get; set; }
            [DisplayName("Translation")] public string Translation { get; set; }
            [DisplayName("Citation")] public bool Citation { get; set; }
            [DisplayName("Word of God")] public bool WordOfGod { get; set; }
            [DisplayName("Footnote")] public string Footnote { get; set; }

            private VerseWordInfo() { }
            public VerseWordInfo(VerseWord word) : this() {
                Number = word.NumberOfVerseWord;
                StrongsCode = word.StrongCode?.Code;
                GrammarCode = word.GrammarCode?.GrammarCodeVariant1;
                SourceWord = word.SourceWord;
                Translation = word.Translation;
                Transliteration = word.Transliteration;
                Citation = word.Citation;
                WordOfGod = word.WordOfJesus;
                Footnote = word.FootnoteText;
                Word = word;
            }

            public void Save() {
                Word.FootnoteText = Footnote;
                Word.WordOfJesus = WordOfGod;
                Word.NumberOfVerseWord = Number;
                Word.SourceWord = SourceWord;
                Word.Translation = Translation;
                Word.Transliteration = Transliteration;
                Word.Citation = Citation;
                if (GrammarCode != Word.GrammarCode?.GrammarCodeVariant1) {
                    if (GrammarCode.IsNullOrEmpty()) {
                        Word.GrammarCode = null;
                    }
                    else {
                        Word.GrammarCode = new XPQuery<GrammarCode>(Word.Session).Where(x => x.GrammarCodeVariant1 == GrammarCode).FirstOrDefault();
                    }
                }
                var differentLang = false;
                if (Word.StrongCode.IsNotNull()) {
                    differentLang = Word.StrongCode.Lang != Language.Greek;
                }
                if (StrongsCode.HasValue && (StrongsCode.Value != Word.StrongCode?.Code || differentLang)) {
                    if (!StrongsCode.HasValue) {
                        Word.StrongCode = null;
                    }
                    else {
                        Word.StrongCode = new XPQuery<StrongCode>(Word.Session).Where(x => x.Code == StrongsCode.Value && x.Lang == Language.Greek).FirstOrDefault();
                    }
                }
                Word.Save();
            }

            public bool IsModified() {
                if (Word.FootnoteText != Footnote) { return true; }
                if (Word.WordOfJesus != WordOfGod) { return true; }
                if (Word.NumberOfVerseWord != Number) { return true; }
                if (Word.SourceWord != SourceWord) { return true; }
                if (Word.Translation != Translation) { return true; }
                if (Word.Transliteration != Transliteration) { return true; }
                if (Word.Citation != Citation) { return true; }
                if (Word.GrammarCode?.GrammarCodeVariant1 != GrammarCode) { return true; }
                if (Word.StrongCode?.Code != StrongsCode) { return true; }
                return default;
            }
        }
        public class TranslationVerseInfo {
            public string TranslationName { get; set; }
            public string VerseText { get; set; }

            [System.ComponentModel.Browsable(false)] public int SortIndex { get; set; }
        }

        private void layoutView1_DoubleClick(object sender, System.EventArgs e) {
            MouseEventArgs args = e as MouseEventArgs;
            var view = sender as LayoutView;
            LayoutViewHitInfo hi = view.CalcHitInfo(args.Location);
            if (hi.InField) {
                var word = view.GetRow(hi.RowHandle) as VerseWordInfo;
                if (word.IsNotNull()) {
                    if (hi.Column.FieldName == "StrongsCode") {
                        var value = word.StrongsCode.HasValue ? word.StrongsCode.Value.ToString() : String.Empty;
                        var strongCode = XtraInputBox.Show("Insert Strong's code:", "Strong Codes", value);
                        if (strongCode.IsNotNullOrEmpty()) {
                            var sc = new XPQuery<StrongCode>(Verse.Session).Where(x => x.Code == strongCode.ToInt() && x.Lang == Language.Greek).FirstOrDefault();
                            if (sc.IsNotNull()) {
                                word.StrongsCode = sc.Code;
                                layoutView1.RefreshData();
                            }
                        }
                    }
                    else if (hi.Column.FieldName == "GrammarCode") {
                        if (word.Word.GrammarCode.IsNotNull()) {
                            using (var dlg = new GrammarCodeFindDialog(word.Word.GrammarCode)) {
                                if (dlg.ShowDialog() == DialogResult.OK && dlg.Selected.IsNotNull()) {
                                    word.GrammarCode = dlg.Selected.GrammarCodeVariant1;
                                    layoutView1.RefreshData();
                                }
                            }
                        }
                        else {
                            using (var dlg = new GrammarCodeFindDialog(word.Word.Session)) {
                                if (dlg.ShowDialog() == DialogResult.OK && dlg.Selected.IsNotNull()) {
                                    word.GrammarCode = dlg.Selected.GrammarCodeVariant1;
                                    layoutView1.RefreshData();
                                }
                            }
                        }
                    }
                    else if (hi.Column.FieldName == "Number") {
                        var index = XtraInputBox.Show("Set word index:", "Word Index", word.Number);
                        if (index != 0) {
                            word.Number = index;
                            layoutView1.RefreshData();
                        }
                    }
                }
            }
        }

        private void gridControl_ProcessGridKey(object sender, KeyEventArgs e) {
            var gridControl = sender as GridControl;
            var layoutView = gridControl.FocusedView as LayoutView;
            if (e.KeyCode == Keys.Tab && layoutView.IsNotNull()) {
                BeginInvoke(new MethodInvoker(() => {
                    if (layoutView.IsNotNull() && layoutView.FocusedColumn.IsNotNull() && layoutView.FocusedColumn.FieldName != "Translation") {
                        SendKeys.Send("{TAB}");
                    }
                }));
            }
            if (e.KeyCode == Keys.F12 && layoutView.IsNotNull()) {
                BeginInvoke(new MethodInvoker(() => {
                    if (layoutView.FocusedColumn.IsNotNull() && layoutView.FocusedColumn.FieldName == "Translation") {
                        layoutView.SetFocusedValue("―");
                    }
                }));
            }
        }

        private void layoutView1_Click(object sender, System.EventArgs e) {
            MouseEventArgs args = e as MouseEventArgs;
            var view = sender as LayoutView;
            LayoutViewHitInfo hi = view.CalcHitInfo(args.Location);
            if (hi.InField) {
                var word = view.GetRow(hi.RowHandle) as VerseWordInfo;
                if (word.IsNotNull()) {
                    //
                    // Strong's code
                    //
                    if (hi.Column.FieldName == "StrongsCode" && word.StrongsCode.HasValue) {
                        tabPane.SelectedPage = tabStrongDictionary;
                        lblStrongCode.Text = $"G{word.StrongsCode}";
                        txtDefinition.Text = word.Word.StrongCode.Definition;
                        txtShortDefinition.Text = word.Word.StrongCode.ShortDefinition;
                        btnSaveStrongDefinition.Tag = word.Word.StrongCode;

                        var htmlString = $@"
                                <!DOCTYPE html>

                                <html lang=""en"" xmlns=""http://www.w3.org/1999/xhtml"">
                                <head>
                                    <meta charset=""utf-8"" />
                                    <title>Strongs code</title>                                    
                                </head>
                                <body style=""color: White;"">
                                <h2>G{word.StrongsCode}</h2>
                                <h3>{word.Word.StrongCode.ShortDefinition}</h3>
                                <div>{word.Word.StrongCode.Definition}</div>
                                </body>
                                </html>
                                ";
                        vwStrongLocal.NavigateToString(htmlString);

                        wvStrong.Source = new Uri($"https://biblehub.com/greek/{word.StrongsCode}.htm");
                    }
                    else if (hi.Column.FieldName == "StrongsCode" && !word.StrongsCode.HasValue) {
                        var value = word.StrongsCode.HasValue ? word.StrongsCode.Value.ToString() : String.Empty;
                        var strongCode = XtraInputBox.Show("Insert Strong's code:", "Strong Codes", value);
                        if (strongCode.IsNotNullOrEmpty()) {
                            var sc = new XPQuery<StrongCode>(Verse.Session).Where(x => x.Code == strongCode.ToInt() && x.Lang == Language.Greek).FirstOrDefault();
                            if (sc.IsNotNull()) {
                                word.StrongsCode = sc.Code;
                                layoutView1.RefreshData();
                            }
                        }
                    }
                    //
                    // Grammar code
                    //
                    else if (hi.Column.FieldName == "GrammarCode" && word.GrammarCode.IsNotNullOrEmpty()) {
                        var gc = word.Word.GrammarCode;
                        if (gc.GrammarCodeDescription.IsNotNullOrEmpty()) {
                            var htmlString = $@"
                                <!DOCTYPE html>

                                <html lang=""en"" xmlns=""http://www.w3.org/1999/xhtml"">
                                <head>
                                    <meta charset=""utf-8"" />
                                    <title>Grammar code</title>
                                </head>
                                <body>
                                <h2>{gc.GrammarCodeVariant1}</h2>
                                <h3>{gc.ShortDefinition}</h3>
                                <p>{gc.GrammarCodeDescription}</p>
                                </body>
                                </html>
                                ";

                            wbGrammarCodes.NavigateToString(htmlString);
                        }
                        else {
                            wbGrammarCodes.Source = new Uri($"http://www.modernliteralversion.org/bibles/bs2/RMAC/{gc.GrammarCodeVariant1}.htm");
                        }
                        tabPane.SelectedPage = tabGrammarCode;
                    }
                    else if (hi.Column.FieldName == "GrammarCode" && word.GrammarCode.IsNullOrEmpty()) {
                        using (var dlg = new GrammarCodeFindDialog(word.Word.Session)) {
                            if (dlg.ShowDialog() == DialogResult.OK && dlg.Selected.IsNotNull()) {
                                word.GrammarCode = dlg.Selected.GrammarCodeVariant1;
                                layoutView1.RefreshData();
                            }
                        }
                    }
                    //
                    // Transliteration
                    //
                    else if (hi.Column.FieldName == "Transliteration") {
                        var result = XtraInputBox.Show("Transliteration", "Transliteration", word.Transliteration);
                        if (result != word.Transliteration && result.IsNotNullOrEmpty()) {
                            word.Transliteration = result;
                            layoutView1.RefreshData();
                        }
                    }
                    //
                    // SourceWord
                    //
                    else if (hi.Column.FieldName == "SourceWord") {
                        var result = XtraInputBox.Show("SourceWord", "SourceWord", word.SourceWord);
                        if (result != word.SourceWord && result.IsNotNullOrEmpty()) {
                            word.SourceWord = result;
                            word.Transliteration = TransliterationController.TransliterateSentence(result);
                            layoutView1.RefreshData();
                        }
                    }
                }
            }
        }

        private void wvStrong_CoreWebView2InitializationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs e) {
            wvStrong.CoreWebView2.DOMContentLoaded += CoreWebView2_DOMContentLoaded;
        }

        private void CoreWebView2_DOMContentLoaded(object sender, Microsoft.Web.WebView2.Core.CoreWebView2DOMContentLoadedEventArgs e) {
            var script = @"
var body = document.body;
for(var i = 0; i < 3; i++){
    var el = body.getElementsByTagName('div')[0];
    if (el != undefined) {
       body.removeChild(el);
    }
}
";
            wvStrong.CoreWebView2.ExecuteScriptAsync(script);
        }

        private void btnSaveStrongDefinition_Click(object sender, EventArgs e) {
            var sc = btnSaveStrongDefinition.Tag as StrongCode;
            if (sc.IsNotNull()) {
                sc.Definition = txtDefinition.Text;
                sc.ShortDefinition = txtShortDefinition.Text;
                sc.Save();

                var uow = sc.Session as UnitOfWork;
                if (uow.IsNotNull()) {
                    uow.CommitChanges();
                    uow.ReloadChangedObjects();
                }
            }
        }

        private void VerseGridControl_Load(object sender, EventArgs e) {
            wbGrammarCodes.EnsureCoreWebView2Async();
            vwStrongLocal.EnsureCoreWebView2Async();
        }
    }
}
