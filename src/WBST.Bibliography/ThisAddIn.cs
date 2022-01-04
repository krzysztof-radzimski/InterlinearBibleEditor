using Microsoft.Office.Tools;
using System;
using System.IO;
using Office = Microsoft.Office.Core;

namespace WBST.Bibliography {
    public partial class ThisAddIn {
        public Ribbon Ribbon { get; private set; }
        public CustomTaskPane BibliographyPane { get; private set; }
        public void InitBibliographyPane() {
            if (BibliographyPane == null) {
                BibliographyPane = this.CustomTaskPanes.Add(new BibliographyPaneControl(), "WBST Bibliografia");
                BibliographyPane.Width = 500;
                BibliographyPane.Visible = false;
            }
        }
        private void ThisAddIn_Startup(object sender, EventArgs e) {
            Application.WindowActivate += Application_WindowActivate;

            var licPath = System.Configuration.ConfigurationManager.AppSettings["AsposeLic"];
            if (File.Exists(licPath)) { new Aspose.Words.License().SetLicense(licPath); }
        }        

        private void ThisAddIn_Shutdown(object sender, EventArgs e) {

        }

        protected override Office.IRibbonExtensibility CreateRibbonExtensibilityObject() {
            Ribbon = new Ribbon();
            return Ribbon;
        }

        private void Application_WindowActivate(Microsoft.Office.Interop.Word.Document Doc, Microsoft.Office.Interop.Word.Window Wn) {
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
