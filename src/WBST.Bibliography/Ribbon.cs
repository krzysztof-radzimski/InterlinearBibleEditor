using Microsoft.Office.Tools.Ribbon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WBST.Bibliography {
    public partial class Ribbon {
        private void Ribbon_Load(object sender, RibbonUIEventArgs e) {

        }

        private void btnShowPane_Click(object sender, RibbonControlEventArgs e) {
            Globals.ThisAddIn.InitBibliographyPane();
            Globals.ThisAddIn.BibliographyPane.Visible = !Globals.ThisAddIn.BibliographyPane.Visible;
        }
    }
}
