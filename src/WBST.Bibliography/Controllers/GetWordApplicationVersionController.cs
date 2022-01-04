using System;
using W = Microsoft.Office.Interop.Word;

namespace WBST.Bibliography.Controllers {
    internal class GetWordApplicationVersionController : IController<int> {
        public void Dispose() { }

        public int Execute() {
            W.Application application = Globals.ThisAddIn.Application;
            return Convert.ToInt32(application.Version.Replace(".0", ""));
        }
    }
}
