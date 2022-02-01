using Word = Microsoft.Office.Interop.Word;
using System;
using System.IO;
using System.Linq;
using Office = Microsoft.Office.Core;

namespace WBST.Bibliography {
    public partial class ThisAddIn {
        public Ribbon Ribbon { get; private set; }
        public BibliographyPaneControl BibliographyPane {
            get {
                try {
                    var item = this.CustomTaskPanes.Where(x => x.Control is BibliographyPaneControl && (x.Control as BibliographyPaneControl).Document.ActiveWindow == Application.ActiveDocument.ActiveWindow).FirstOrDefault();
                    if (item.IsNotNullOrMissing()) {
                        return item.Control as BibliographyPaneControl;
                    }
                }
                catch { }
                return null;
            }
        }
        public Microsoft.Office.Tools.CustomTaskPane InitBibliographyPane(Word.Document doc) {
            var item = this.CustomTaskPanes.Where(x => x.Control is BibliographyPaneControl && (x.Control as BibliographyPaneControl).Document.IsNotNullOrMissing() && (x.Control as BibliographyPaneControl).Document.ActiveWindow == doc.ActiveWindow).FirstOrDefault();
            if (item == null) {
                item = this.CustomTaskPanes.Add(new BibliographyPaneControl(doc), "WBST Bibliografia", doc.ActiveWindow);
                item.Width = 500;
                item.Visible = true;
            }
            else {
                item.Visible = !item.Visible;
            }
            return item;
        }
        private void ThisAddIn_Startup(object sender, EventArgs e) {
            Application.WindowActivate += Application_WindowActivate;
            Application.DocumentBeforeClose += Application_DocumentBeforeClose;
            Application.DocumentOpen += Application_DocumentOpen;
            ((Word.ApplicationEvents4_Event)Application).NewDocument += ThisAddIn_NewDocument;
            var licPath = System.Configuration.ConfigurationManager.AppSettings["AsposeLic"];
            if (File.Exists(licPath)) { new Aspose.Words.License().SetLicense(licPath); }
        }

        private void ThisAddIn_NewDocument(Word.Document Doc) {
            ActivateAddinTabAndPane(Doc);
        }

        private void Application_DocumentOpen(Word.Document Doc) {
            ActivateAddinTabAndPane(Doc);
        }

        private void ActivateAddinTabAndPane(Word.Document Doc) {
            object text = "Wyższe Baptystyczne Seminarium Teologiczne w Warszawie";
            var found = Doc.ActiveWindow.Selection.Find.Execute(text);
            var item = this.CustomTaskPanes.Where(x => x.Control is BibliographyPaneControl && (x.Control as BibliographyPaneControl).Document.IsNotNullOrMissing() && (x.Control as BibliographyPaneControl).Document.ActiveWindow == Doc.ActiveWindow).FirstOrDefault();
            if (found && item == null) {
                InitBibliographyPane(Doc);
                Ribbon.ActivateTabMso("TabAddIns");
            }
            else if (found && item != null) {
                item.Visible = true;
                Ribbon.ActivateTabMso("TabAddIns");
            }
        }

        private void Application_DocumentBeforeClose(Word.Document Doc, ref bool Cancel) {
            try {
                var item = this.CustomTaskPanes.Where(x => x.Control is BibliographyPaneControl && (x.Control as BibliographyPaneControl).Document.IsNotNullOrMissing() && (x.Control as BibliographyPaneControl).Document.ActiveWindow == Doc.ActiveWindow).FirstOrDefault();
                if (item != null) {
                    this.CustomTaskPanes.Remove(item);
                }
            }
            catch { }
        }

        private void ThisAddIn_Shutdown(object sender, EventArgs e) {

        }

        protected override Office.IRibbonExtensibility CreateRibbonExtensibilityObject() {
            Ribbon = new Ribbon();
            return Ribbon;
        }

        private void Application_WindowActivate(Word.Document Doc, Word.Window Wn) {
            this.Ribbon.Invalidate();
        }

        #region VSTO generated code
        private void InternalStartup() {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }

        #endregion
    }
}
