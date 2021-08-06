using DevExpress.Xpo;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using IBE.Common.Extensions;
using IBE.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IBE.WindowsClient.Controls {
    public partial class VerseEditorControl : XtraUserControl {
        public static int PANE_HEIGHT = 400;

        bool isLoading = false;
        // bool isLoaded = false;
        public Verse Verse { get; private set; }
        public bool LoadOtherTranslations { get; private set; }

        public event EventHandler<StrongCode> StrongClick;
        public event EventHandler<GrammarCode> GrammarCodeClick;

        public FlowLayoutPanel VerseWordsControl { get { return flowLayoutPanel; } }
        public VerseEditorControl() {
            InitializeComponent();
            SetEnabled(false);
            tabPane1.Height = PANE_HEIGHT;
        }

        public VerseEditorControl(Verse verse, bool loadOtherTranslations = true) : this() {
            Verse = verse;
            LoadOtherTranslations = loadOtherTranslations;
        }

        public bool IsModified() {
            if (Verse.IsNotNull() && Verse.Text.IsNullOrEmpty()) { return true; }
            foreach (var item in flowLayoutPanel.Controls) {
                if (item is VerseWordEditorControl) {
                    if ((item as VerseWordEditorControl).Modified) {
                        return true;
                    }
                }
            }
            return default;
        }

        private void VerseEditorControl_Load(object sender, EventArgs e) {
            // LoadData();
            wbGrammarCodes.EnsureCoreWebView2Async();
        }

        public VerseWordEditorControl CreateVerseWordControl(VerseWord word) {
            var control = new VerseWordEditorControl(word);

            control.StrongClick += Control_StrongClick;
            control.GrammarCodeClick += Control_GrammarCodeClick;
            control.DeleteClick += Control_DeleteClick;
            return control;
        }

        public void LoadData(Verse verse, bool loadOtherTranslations = true) {
            Verse = verse;
            LoadOtherTranslations = loadOtherTranslations;
            LoadData();
        }
        public void LoadData() {
            if (isLoading) { return; }
            isLoading = true;
            gridViewTranslations.LoadingPanelVisible = true;
            Application.DoEvents();

            cbStartFromNewLine.DataBindings.Add("Checked", Verse, "StartFromNewLine");
            var subtitle = Verse.ParentChapter.Subtitles.Where(x => x.BeforeVerseNumber == Verse.NumberOfVerse).FirstOrDefault();
            if (subtitle.IsNotNull()) {
                txtStoryText.Text = subtitle.Text;
                rgStoryLevel.SelectedIndex = subtitle.Level - 1;
            }

            //((System.ComponentModel.ISupportInitialize)(this.flowLayoutPanel)).BeginInit();
            VerseWordsControl.SuspendLayout();
            //Application.DoEvents();

            var getControls = Task.Factory.StartNew(() => {
                var wordControls = new List<VerseWordEditorControl>();
                var words = Verse.VerseWords.OrderBy(x => x.NumberOfVerseWord);
                foreach (var word in words) {
                    var control = CreateVerseWordControl(word);
                    wordControls.Add(control);
                }

                return wordControls;
            });

            getControls.ContinueWith((x) => {
                this.SafeInvoke(f => {
                    foreach (var control in x.Result) {
                        f.VerseWordsControl.Controls.Add(control);
                    }
                    f.VerseWordsControl.ResumeLayout(false);
                    f.VerseWordsControl.PerformLayout();

                });
            });

            if (LoadOtherTranslations) {
                var getTranslations = Task.Factory.StartNew(() => {
                    var list = new List<TranslationVerseInfo>();
                    var view = new XPView(Verse.Session, typeof(Translation)) {
                        CriteriaString = "[Type] = 4 AND [Hidden] = 0"
                    };
                    view.Properties.Add(new ViewProperty("Oid", SortDirection.None, "[Oid]", false, true));
                    view.Properties.Add(new ViewProperty("Name", SortDirection.None, "[Name]", false, true));
                    foreach (ViewRecord item in view) {
                        var id = item["Oid"].ToInt();
                        var name = item["Name"].ToString();
                        var tvi = new TranslationVerseInfo() {
                            TranslationName = name
                        };

                        var _view = new XPView(Verse.Session, typeof(Verse)) {
                            CriteriaString = $"[NumberOfVerse] = {Verse.NumberOfVerse} AND [ParentChapter.NumberOfChapter] = {Verse.ParentChapter.NumberOfChapter} AND [ParentChapter.ParentBook.NumberOfBook] = {Verse.ParentChapter.ParentBook.NumberOfBook} AND [ParentChapter.ParentBook.ParentTranslation.Name] = '{name.Replace("'", "''")}'"
                        };
                        _view.Properties.Add(new ViewProperty("Text", SortDirection.None, "[Text]", false, true));
                        foreach (ViewRecord _item in _view) {
                            tvi.VerseText = _item["Text"].ToString().Replace("<pb/>", "").Replace("<t>", "").Replace("<m>", "").Replace("</t>", "").Replace("</m>", "").Replace("<e>", "").Replace("</e>", "");
                        }
                        if (tvi.VerseText.IsNotNullOrEmpty()) {
                            list.Add(tvi);
                        }
                    }

                    return list;
                });

                getTranslations.ContinueWith((x) => {
                    this.SafeInvoke(f => {
                        f.gridTranslations.DataSource = x.Result;
                        f.gridViewTranslations.BestFitColumns();
                        f.gridViewTranslations.LoadingPanelVisible = false;

                        f.isLoading = false;

                        f.SetEnabled();
                    });
                });
            }
            else {
                tabTranslations.PageVisible = false;
                isLoading = false;
                gridViewTranslations.LoadingPanelVisible = false;
                SetEnabled();
            }
        }

        private void Control_DeleteClick(object sender, VerseWord e) {
            e.Delete();
            (e.Session as UnitOfWork).CommitChanges();

            var control = sender as VerseWordEditorControl;
            flowLayoutPanel.Controls.Remove(control);
            control.Dispose();
            control = null;
        }

        public void DeleteAll() {
            var controls = flowLayoutPanel.Controls.OfType<VerseWordEditorControl>();
            foreach (var item in controls) {
                Control_DeleteClick(item, item.Word);
            }
        }

        public void Clear() {
            flowLayoutPanel.Controls.Clear();
            gridTranslations.DataSource = null;
            if (wbGrammarCodes.CoreWebView2.IsNotNull()) {
                wbGrammarCodes.NavigateToString(@"<html><head></html><body></body></html>");
            }
            if (wbStrong.CoreWebView2.IsNotNull()) {
                wbStrong.NavigateToString(@"<html><head></html><body></body></html>");
            }
            txtDefinition.Text = String.Empty;
            txtShortDefinition.Text = String.Empty;
            txtStoryText.Text = String.Empty;
            cbStartFromNewLine.DataBindings.Clear();
            cbStartFromNewLine.Checked = false;
            rgStoryLevel.SelectedIndex = -1;
            this.Verse = null;
            GC.Collect();
        }

        public void Save() {
            gridViewTranslations.Focus();
            Application.DoEvents();

            foreach (var item in flowLayoutPanel.Controls) {
                if (item is VerseWordEditorControl) {
                    var word = (item as VerseWordEditorControl).Word;
                    word.Save();
                }
            }

            var text = String.Empty;
            foreach (var verseWord in Verse.VerseWords.OrderBy(x => x.NumberOfVerseWord)) {
                text += $"{verseWord.Translation} ";
            }

            Verse.Text = text.Trim();
            Verse.Save();

            var uow = Verse.Session as UnitOfWork;

            // save story 
            if (txtStoryText.Text.IsNotNullOrEmpty()) {
                var subtitle = Verse.ParentChapter.Subtitles.Where(x => x.BeforeVerseNumber == Verse.NumberOfVerse).FirstOrDefault();
                if (subtitle.IsNull()) {
                    subtitle = new Subtitle(uow) {
                        BeforeVerseNumber = Verse.NumberOfVerse,
                        ParentChapter = Verse.ParentChapter,
                        Level = rgStoryLevel.SelectedIndex + 1,
                        Text = txtStoryText.Text
                    };
                }
                else {
                    subtitle.BeforeVerseNumber = Verse.NumberOfVerse;
                    subtitle.ParentChapter = Verse.ParentChapter;
                    subtitle.Level = rgStoryLevel.SelectedIndex + 1;
                    subtitle.Text = txtStoryText.Text;
                }
                subtitle.Save();
            }


            if (uow.IsNotNull()) {
                uow.CommitChanges();
                uow.ReloadChangedObjects();
            }
        }

        private void Control_StrongClick(object sender, StrongCode e) {
            txtShortDefinition.EditValue = e.ShortDefinition;
            txtShortDefinition.Tag = e;
            txtDefinition.EditValue = e.Definition;

            wbStrong.Source = new Uri($"https://www.blueletterbible.org/lang/lexicon/lexicon.cfm?Strongs=G{e.Code}&t=MGNT");
            tabPane1.SelectedPage = tabNavigationPage2;
            if (StrongClick.IsNotNull()) {
                StrongClick(this, e);
            }
        }

        private void Control_GrammarCodeClick(object sender, GrammarCode e) {
            if (e.GrammarCodeDescription.IsNotNullOrEmpty()) {
                var htmlString = $@"
                <!DOCTYPE html>

                <html lang=""en"" xmlns=""http://www.w3.org/1999/xhtml"">
                <head>
                    <meta charset=""utf-8"" />
                    <title>Ggammar code</title>
                </head>
                <body>
                <h2>{e.GrammarCodeVariant1}</h2>
                <h3>{e.ShortDefinition}</h3>
                <p>{e.GrammarCodeDescription}</p>
                </body>
                </html>
                ";

                wbGrammarCodes.NavigateToString(htmlString);
            }
            else {
                wbGrammarCodes.Source = new Uri($"http://www.modernliteralversion.org/bibles/bs2/RMAC/{e.GrammarCodeVariant1}.htm");
            }

            tabPane1.SelectedPage = tabNavigationPage3;
            if (GrammarCodeClick.IsNotNull()) {
                GrammarCodeClick(this, e);
            }
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
        }

        public void SetColumnWidth(GridView view, GridColumn column, int percent) {
            var viewInfo = view.GetViewInfo() as GridViewInfo;
            int totalWidth = viewInfo.ViewRects.ColumnPanelWidth;
            column.Width = (totalWidth * percent) / 100;
        }

        class TranslationVerseInfo {
            public string TranslationName { get; set; }
            public string VerseText { get; set; }
        }

        private void wbStrong_CoreWebView2InitializationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs e) {
            wbStrong.CoreWebView2.DOMContentLoaded += CoreWebView2_DOMContentLoaded;
        }

        private void CoreWebView2_DOMContentLoaded(object sender, Microsoft.Web.WebView2.Core.CoreWebView2DOMContentLoadedEventArgs e) {
            var script = @"
var appBar = document.getElementById('appBar');
if (appBar != undefined) {
    appBar.parentElement.removeChild(appBar);
}

var menuTop = document.getElementById('menuTop');
if (menuTop != undefined) {
    menuTop.parentElement.removeChild(menuTop);
}

var contextBarT = document.getElementById('contextBarT');
if (contextBarT != undefined) {
    contextBarT.parentElement.removeChild(contextBarT);
}

var bodyCol2 = document.getElementById('bodyCol2');
if (bodyCol2 != undefined) {
    bodyCol2.parentElement.removeChild(bodyCol2);
}

var theFoot = document.getElementById('theFoot');
if (theFoot != undefined) {
    theFoot.parentElement.removeChild(theFoot);
}

var cookiewrapper = document.getElementById('cookie-wrapper');
if (cookiewrapper != undefined) {
    cookiewrapper.parentElement.removeChild(cookiewrapper);
}

var yuigen80 = document.getElementById('yui-gen80');
if (yuigen80 != undefined) {
    yuigen80.parentElement.removeChild(yuigen80);
}
var dView = document.getElementById('dView');
if (dView != undefined) {
    dView.parentElement.removeChild(dView);
}

var whole = document.getElementById('whole');
if (whole != undefined) {
    whole.setAttribute('style', 'padding-top: 0;');
}";
            wbStrong.CoreWebView2.ExecuteScriptAsync(script);
        }

        private void tabPane1_SizeChanged(object sender, EventArgs e) {
            PANE_HEIGHT = tabPane1.Height;
        }

        private void gcStrongShortDefinition_CustomButtonClick(object sender, DevExpress.XtraBars.Docking2010.BaseButtonEventArgs e) {
            // save strong changes
            var sc = txtShortDefinition.Tag as StrongCode;
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

        private void gridViewTranslations_DoubleClick(object sender, EventArgs e) {
            var info = gridViewTranslations.GetFocusedRow() as TranslationVerseInfo;
            if (info.IsNotNull() && info.TranslationName.Contains("TRO")) {
                System.Diagnostics.Process.Start(Verse.GetOblubienicaUrl());
            }
        }

        private void SetEnabled(bool flag = true) {
            txtDefinition.Enabled = txtShortDefinition.Enabled = txtStoryText.Enabled = gridTranslations.Enabled = wbGrammarCodes.Enabled = wbStrong.Enabled = cbStartFromNewLine.Enabled = rgStoryLevel.Enabled =
                   gcStrongDefinition.Enabled = flag;
        }
    }
}
