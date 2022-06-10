using System;

namespace WBST.Bibliography.Controllers {
    internal class RecognizeSiglumAndInsertHiperlinkController : IController<object> {
        public void Dispose() { }

        public object Execute() {
            var selection = Globals.ThisAddIn.Application.ActiveWindow.Selection;
            var text = selection.Text;
            var apiUrl = $"https://kosciol-jezusa.pl/api/GetSiglumUrl?q={text}";

            using (var client = new System.Net.WebClient()) {
                object url = client.DownloadString(apiUrl);
                if (url.ToString().IsNotNullOrEmpty() && url.ToString().StartsWith("https")) {
                    object anchor = selection.Range;
                    object missObj = Type.Missing;
                    Globals.ThisAddIn.Application.ActiveDocument.Hyperlinks.Add(anchor, ref url, ref missObj, ref missObj, ref missObj, ref missObj);
                }
            }

            return default;
        }
    }
}
