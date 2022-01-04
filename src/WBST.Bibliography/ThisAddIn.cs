using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Word;
using Microsoft.Office.Tools;

namespace WBST.Bibliography {
    public partial class ThisAddIn {
        public CustomTaskPane BibliographyPane { get; private set; }
        public void InitBibliographyPane() {
            if (BibliographyPane == null) {
                BibliographyPane = this.CustomTaskPanes.Add(new BibliographyPaneControl(), "WBST Bibliografia");
                BibliographyPane.Width = 500;
                BibliographyPane.Visible = false;
            }
        }
        private void ThisAddIn_Startup(object sender, System.EventArgs e) {
            
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e) {
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup() {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }

        #endregion
    }
}
