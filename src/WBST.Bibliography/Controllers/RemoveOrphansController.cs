using DevExpress.XtraEditors;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WBST.Bibliography.Controllers {
    public class RemoveOrphansController : IController<object> {
        public void Dispose() { }

        public object Execute() {
            var hyphens = new[] { "o", "a", "i", "u", "w", "z", "a" };
            var orphans = new List<string>();
            foreach (var item in hyphens) {
                orphans.Add($"{item}");
            }
            foreach (var item in hyphens) {
                orphans.Add($"({item}");
            }
            foreach (var item in hyphens) {
                orphans.Add($"[{item}");
            }
            foreach (var item in hyphens) {
                orphans.Add($"({item})");
            }

            foreach (var item in hyphens) {
                orphans.Add($"{item.ToUpper()}");
            }
            foreach (var item in hyphens) {
                orphans.Add($"({item.ToUpper()}");
            }
            foreach (var item in hyphens) {
                orphans.Add($"[{item.ToUpper()}");
            }
            foreach (var item in hyphens) {
                orphans.Add($"({item.ToUpper()})");
            }

            for (var i = 1; i < 100; i++) {
                orphans.Add($"{i}");
            }
            for (var i = 1; i < 100; i++) {
                orphans.Add($"{i}.");
            }
            for (var i = 1; i < 100; i++) {
                orphans.Add($"({i})");
            }
            for (var i = 1; i < 100; i++) {
                orphans.Add($"[{i}");
            }
            for (var i = 1; i < 100; i++) {
                orphans.Add($"({i}");
            }

            orphans.Add("dr");
            orphans.Add("prof.");
            orphans.Add("mgr");
            orphans.Add("tłum.");
            orphans.Add("przekł.");
            orphans.Add("wyd.");
            orphans.Add("red.");
            orphans.Add("s.");
            orphans.Add("str.");
            orphans.Add("nr.");

            RemoveOrphans(orphans.ToArray());

            //RemoveOrphans(
            //       "o", "a", "i", "u", "w", "z", "a",

            //       "(z", "(o", "(a", "(i", "(u", "(w", "(a", "o", "i",
            //       "[z", "[o", "[a", "[i", "[u", "[w", "[a", "[o", "[i",

            //       "O", "A", "I", "U", "W", "Z", "A",

            //       "(Z", "(O", "(A", "(I", "(U", "(W", "(A", "O", "I",
            //       "[Z", "[O", "[A", "[I", "[U", "[W", "[A", "[O", "[I",

            //       "1", "2", "3", "4", "5", "6", "7", "8", "9",
            //       "10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
            //       "20", "21", "22", "23", "24", "25", "26", "27", "28", "29",
            //       "30", "31", "32", "33", "34", "35", "36", "37", "38", "39",
            //       "40", "41", "42", "43", "44", "45", "46", "47", "48", "49",
            //       "50", "51", "52", "53", "54", "55", "56", "57", "58", "59",
            //       "60", "61", "62", "63", "64", "65", "66", "67", "68", "69",

            //       "1.", "2.", "3.", "4.", "5.", "6.", "7.", "8.", "9.",
            //       "10.", "11.", "12.", "13.", "14.", "15.", "16.", "17.", "18.", "19.",
            //       "20.", "21.", "22.", "23.", "24.", "25.", "26.", "27.", "28.", "29.",
            //       "30.", "31.", "32.", "33.", "34.", "35.", "36.", "37.", "38.", "39.",
            //       "40.", "41.", "42.", "43.", "44.", "45.", "46.", "47.", "48.", "49.",
            //       "50.", "51.", "52.", "53.", "54.", "55.", "56.", "57.", "58.", "59.",
            //       "60.", "61.", "62.", "63.", "64.", "65.", "66.", "67.", "68.", "69.");
            return default;
        }

        private void RemoveOrphans(params string[] table) {
            var dlg = GetProgressForm(table.Length);
            dlg.Show();

            var progress = 0;
            var doc = Globals.ThisAddIn.Application.ActiveDocument;
            RemoveOrphans(doc.Range(), table, dlg, progress);
            progress = 0;
            foreach (Microsoft.Office.Interop.Word.Footnote item in doc.Footnotes) {
                RemoveOrphans(item.Range, table, dlg, progress, "z przypisów");
                break;
            }
            dlg.Close();
            dlg = null;
        }

        private void RemoveOrphans(Microsoft.Office.Interop.Word.Range range, string[] table, XtraForm dlg, int progress, string fromText = "z treści dokumentu") {
            foreach (string s in table) {
                progress++;
                SetProgress(dlg, progress, fromText: fromText);

                string findText = $" {s} ";
                string replaceWith = $" {s}\u00A0";
                var find = range.Find;
                find.ClearFormatting();
                find.Text = findText;
                find.MatchCase = true;
                find.Replacement.ClearFormatting();
                find.Replacement.Text = replaceWith;
                find.Execute(
                    Replace: Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll, 
                    Forward: true, 
                    Wrap: Microsoft.Office.Interop.Word.WdFindWrap.wdFindContinue);
            }
        }

        private XtraForm GetProgressForm(int max) {
            // Create a form to show a progress bar,
            // and adjust its properties.
            var form = new XtraForm() {
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MinimizeBox = false,
                MaximizeBox = false,
                CloseBox = false,
                Size = new System.Drawing.Size(350, 50),
                ShowInTaskbar = false,
                StartPosition = FormStartPosition.CenterScreen,
                TopMost = true,
                Text = "Usuwanie sierotek"
            };


            ProgressBarControl progressBar = new ProgressBarControl() {
                Name = "progressBar"
            };
            progressBar.Properties.Maximum = max;

            // Add a progress bar to a form and show it.
            form.Controls.Add(progressBar);
            progressBar.Dock = DockStyle.Fill;

            return form;
        }

        private void SetProgress(XtraForm form, int progress, string fromText = "z treści dokumentu") {
            if (form != null) {                
                var progressBar = form.Controls["progressBar"] as ProgressBarControl;
                if (progressBar != null) {
                    progressBar.EditValue = progress;
                    form.Text = $"Usuwanie sierotek {fromText} ({progress} z {progressBar.Properties.Maximum})";
                    Application.DoEvents();
                }
            }

        }
    }
}
