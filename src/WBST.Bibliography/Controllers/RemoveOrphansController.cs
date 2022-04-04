namespace WBST.Bibliography.Controllers {
    public class RemoveOrphansController : IController<object> {
        public void Dispose() { }

        public object Execute() {
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
            return default;
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
