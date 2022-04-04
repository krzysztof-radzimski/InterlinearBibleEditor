using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;
using WBST.Bibliography.Controllers;
using Office = Microsoft.Office.Core;

// TODO:  Follow these steps to enable the Ribbon (XML) item:

// 1: Copy the following code block into the ThisAddin, ThisWorkbook, or ThisDocument class.

//  protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
//  {
//      return new Ribbon1();
//  }

// 2. Create callback methods in the "Ribbon Callbacks" region of this class to handle user
//    actions, such as clicking a button. Note: if you have exported this Ribbon from the Ribbon designer,
//    move your code from the event handlers to the callback methods and modify the code to work with the
//    Ribbon extensibility (RibbonX) programming model.

// 3. Assign attributes to the control tags in the Ribbon XML file to identify the appropriate callback methods in your code.  

// For more information, see the Ribbon XML documentation in the Visual Studio Tools for Office Help.


namespace WBST.Bibliography {
    [ComVisible(true)]
    public class Ribbon : Office.IRibbonExtensibility {
        private Office.IRibbonUI ribbon;

        public Ribbon() {
        }

        #region IRibbonExtensibility Members

        public string GetCustomUI(string ribbonID) {
            return GetCustomUI();
        }
        public string GetCustomUI() {
            var ribbonXmlText = GetResourceText("WBST.Bibliography.Ribbon.xml");
            if (!String.IsNullOrEmpty(ribbonXmlText)) {
                if (IsWord2007()) {
                    var nameSpace = @"http://schemas.microsoft.com/office/2006/01/customui";
                    var orginal = @"http://schemas.microsoft.com/office/2009/07/customui";
                    ribbonXmlText = ribbonXmlText.Replace(orginal, nameSpace);
                    // skasowanie backstageview
                    try {
                        var xml = XElement.Parse(ribbonXmlText);
                        xml.Elements().Where(x => x.Name.LocalName == "backstage").Remove();
                        ribbonXmlText = xml.ToString();
                    }
                    catch { }
                }
                else {
                    // skasowanie officeMenu
                    try {
                        var xml = XElement.Parse(ribbonXmlText);
                        xml.Descendants().Where(x => x.Name.LocalName == "officeMenu").Remove();
                        ribbonXmlText = xml.ToString();
                    }
                    catch { }
                }
            }
            return ribbonXmlText;
        }
        private bool IsWord2007() {
            using (var controller = new GetWordApplicationVersionController()) {
                return controller.Execute() == 12;
            }
        }
        #endregion

        #region Ribbon Callbacks      
        public void Ribbon_Load(Office.IRibbonUI ribbonUI) {
            this.ribbon = ribbonUI;
        }

        public bool GetPressed(Office.IRibbonControl control) {
            if (control != null && !String.IsNullOrEmpty(control.Id)) {
                switch (control.Id) {
                    case "btnShowPane": {
                            if (Globals.ThisAddIn.BibliographyPane != null) {
                                return Globals.ThisAddIn.BibliographyPane.Visible;
                            }
                            return false;
                        }
                }
            }

            return false;
        }

        public void ToggleButtonOnAction(Office.IRibbonControl control, bool pressed) {
            if (control != null && !String.IsNullOrEmpty(control.Id)) {
                switch (control.Id) {
                    case "btnShowPane": {
                            Globals.ThisAddIn.InitBibliographyPane(Globals.ThisAddIn.Application.ActiveDocument);
                            break;
                        }
                }
            }
        }

        public void OnAction(Office.IRibbonControl control) {
            if (control != null && !String.IsNullOrEmpty(control.Id)) {
                switch (control.Id) {
                    case "btnUrlShortener": {
                            var urlText = Globals.ThisAddIn.Application.ActiveWindow.Selection.Text.Trim();
                            if (urlText.IsNotNullOrEmpty() && urlText.StartsWith("http")) {
                                using (var client = new System.Net.WebClient()) {
                                    var url = Convert.ToBase64String(Encoding.UTF8.GetBytes(urlText));
                                    Globals.ThisAddIn.Application.ActiveWindow.Selection.Text = client.DownloadString("https://kosciol-jezusa.pl/api/UrlShortener?url=" + url);
                                }
                            }
                            else {
                                DevExpress.XtraEditors.XtraMessageBox.Show("Wskazany ciąg nie jest adresem Url", "WBST", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                            }
                            break;
                        }
                    case "btnPublisAsEPub": {
                            using (var controller = new ExportAsEPubController()) { controller.Execute(); }
                            break;
                        }
                    case "btnImportEPub": {
                            using (var controller = new ImportEPubController()) { controller.Execute(); }
                            break;
                        }
                    case "btnImportMobi": {
                            using (var controller = new ImportMobiController()) { controller.Execute(); }
                            break;
                        }
                    case "btnRemoveOrphans": { RemoveOrphans(); break; }
                }
            }
        }

        public void Invalidate() {
            try {
                if (ribbon.IsNotNullOrMissing()) {
                    ribbon.Invalidate();
                    ribbon.InvalidateControl("TabShare");
                }
            }
            catch { }
        }

        public void ActivateTabMso(string name) { ribbon.ActivateTabMso(name); }
        #endregion

        #region Helpers

        private static string GetResourceText(string resourceName) {
            Assembly asm = Assembly.GetExecutingAssembly();
            string[] resourceNames = asm.GetManifestResourceNames();
            for (int i = 0; i < resourceNames.Length; ++i) {
                if (string.Compare(resourceName, resourceNames[i], StringComparison.OrdinalIgnoreCase) == 0) {
                    using (StreamReader resourceReader = new StreamReader(asm.GetManifestResourceStream(resourceNames[i]))) {
                        if (resourceReader != null) {
                            return resourceReader.ReadToEnd();
                        }
                    }
                }
            }
            return null;
        }

        #endregion

        private void RemoveOrphans() {
            RemoveOrphans(
                "o", "a", "i", "u", "w", "z", "a", 
                
                "(z", "(o", "(a", "(i", "(u", "(w", "(a", "o", "i", 
                "[z", "[o", "[a", "[i", "[u", "[w", "[a", "[o", "[i",

                "O", "A", "I", "U", "W", "Z", "A",

                "(Z", "(O", "(A", "(I", "(U", "(W", "(A", "O", "I",
                "[Z", "[O", "[A", "[I", "[U", "[W", "[A", "[O", "[I",

                "1", "2", "3", "4", "5", "6", "7", "8", "9",
                "10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
                "20", "21", "22", "23", "24", "25", "26", "27", "28", "29",
                "30", "31", "32", "33", "34", "35", "36", "37", "38", "39",
                "40", "41", "42", "43", "44", "45", "46", "47", "48", "49",
                "50", "51", "52", "53", "54", "55", "56", "57", "58", "59",
                "60", "61", "62", "63", "64", "65", "66", "67", "68", "69",

                "1.", "2.", "3.", "4.", "5.", "6.", "7.", "8.", "9.",
                "10.", "11.", "12.", "13.", "14.", "15.", "16.", "17.", "18.", "19.",
                "20.", "21.", "22.", "23.", "24.", "25.", "26.", "27.", "28.", "29.",
                "30.", "31.", "32.", "33.", "34.", "35.", "36.", "37.", "38.", "39.",
                "40.", "41.", "42.", "43.", "44.", "45.", "46.", "47.", "48.", "49.",
                "50.", "51.", "52.", "53.", "54.", "55.", "56.", "57.", "58.", "59.",
                "60.", "61.", "62.", "63.", "64.", "65.", "66.", "67.", "68.", "69.");
        }

        private void RemoveOrphans(params string[] table) {
            var doc = Globals.ThisAddIn.Application.ActiveDocument;
            foreach (string s in table) {
                string findText = $" {s} ";
                string replaceWith = $" {s}\u00A0";
                var find = doc.Range().Find;
                find.ClearFormatting();
                find.Text = findText;
                find.MatchCase = true;
                find.Replacement.ClearFormatting();
                find.Replacement.Text = replaceWith;
                find.Execute(Replace: Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll, Forward: true, Wrap: Microsoft.Office.Interop.Word.WdFindWrap.wdFindContinue);
            }
        }
    }
}
