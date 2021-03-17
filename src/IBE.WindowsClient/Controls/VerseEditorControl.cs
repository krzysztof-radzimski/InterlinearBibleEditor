using DevExpress.Xpo;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using IBE.Common.Extensions;
using IBE.Data.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IBE.WindowsClient.Controls {
    public partial class VerseEditorControl : XtraUserControl {
        bool isLoading = false;
        bool isLoaded = false;
        public Verse Verse { get; }
        public event EventHandler<StrongCode> StrongClick;
        public event EventHandler<GrammarCode> GrammarCodeClick;
        public VerseEditorControl() {
            InitializeComponent();
        }

        public VerseEditorControl(Verse verse) : this() {
            Verse = verse;            
        }

        private void VerseEditorControl_Load(object sender, EventArgs e) {
            LoadData();
        }

        private void LoadData() {
            if (isLoading || isLoaded) { return; }
            isLoading = true;
            gridViewTranslations.LoadingPanelVisible = true;
            Application.DoEvents();
            
            cbStartFromNewLine.DataBindings.Add("Checked", Verse, "StartFromNewLine");

            var getControls = Task.Factory.StartNew(() => {
                var wordControls = new List<VerseWordEditorControl>();

                foreach (var word in Verse.VerseWords) {
                    var control = new VerseWordEditorControl(word);
                    control.StrongClick += Control_StrongClick;
                    control.GrammarCodeClick += Control_GrammarCodeClick;
                    wordControls.Add(control);
                }

                return wordControls;
            });

            getControls.ContinueWith((x) => {
                this.SafeInvoke(f => {
                    foreach (var control in x.Result) {
                        f.flowLayoutPanel.Controls.Add(control);                        
                    }
                });
            });


            var getTranslations = Task.Factory.StartNew(() => {
                var list = new List<TranslationVerseInfo>();
                var view = new XPView(Verse.Session, typeof(Translation));
                view.CriteriaString = "[Type] = 4";
                view.Properties.Add(new ViewProperty("Oid", SortDirection.None, "[Oid]", false, true));
                view.Properties.Add(new ViewProperty("Name", SortDirection.None, "[Name]", false, true));
                foreach (ViewRecord item in view) {
                    var id = item["Oid"].ToInt();
                    var name = item["Name"].ToString();
                    var tvi = new TranslationVerseInfo() {
                        TranslationName = name
                    };

                    var _view = new XPView(Verse.Session, typeof(Verse));
                    _view.CriteriaString = $"[NumberOfVerse] = {Verse.NumberOfVerse} AND [ParentChapter.NumberOfChapter] = {Verse.ParentChapter.NumberOfChapter} AND [ParentChapter.ParentBook.NumberOfBook] = {Verse.ParentChapter.ParentBook.NumberOfBook} AND [ParentChapter.ParentBook.ParentTranslation.Name] = '{name.Replace("'", "''")}'";
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
                    f.isLoaded = true;
                });
            });
        }

        

        public void Save() {
            gridViewTranslations.Focus();
            Application.DoEvents();

            foreach (var item in flowLayoutPanel.Controls) {
                if (item is VerseWordEditorControl) {
                    (item as VerseWordEditorControl).Word.Save();
                }
            }
            Verse.Save();
            var uow = Verse.Session as UnitOfWork;
            if (uow.IsNotNull()) {
                uow.CommitChanges();
                uow.ReloadChangedObjects();
            }
        }

        private void Control_StrongClick(object sender, StrongCode e) {
            wbStrong.DocumentText = $@"
<!DOCTYPE html>

<html lang=""en"" xmlns=""http://www.w3.org/1999/xhtml"">
<head>
    <meta charset=""utf-8"" />
    <title>Strong</title>
</head>
<body>
<h2>{e.SourceWord}</h2>
<h3>{e.ShortDefinition}</h3>
<p>{e.Definition}</p>
</body>
</html>
";
            tabPane1.SelectedPage = tabNavigationPage2;
            if (StrongClick.IsNotNull()) {
                StrongClick(this, e);
            }
        }

        private void Control_GrammarCodeClick(object sender, GrammarCode e) {
            wbGrammarCodes.DocumentText = $@"
<!DOCTYPE html>

<html lang=""en"" xmlns=""http://www.w3.org/1999/xhtml"">
<head>
    <meta charset=""utf-8"" />
    <title>Strong</title>
</head>
<body>
<h2>{e.GrammarCodeVariant1}</h2>
<h3>{e.ShortDefinition}</h3>
<p>{e.GrammarCodeDescription}</p>
</body>
</html>
";
            tabPane1.SelectedPage = tabNavigationPage3;
            if (GrammarCodeClick.IsNotNull()) {
                GrammarCodeClick(this, e);
            }
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            //gridTranslations.ForceInitialize();
            //SetColumnWidth(gridViewTranslations, colTranslationName, 15);
            //SetColumnWidth(gridViewTranslations, colVerseText, 85);
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

      
    }
}
