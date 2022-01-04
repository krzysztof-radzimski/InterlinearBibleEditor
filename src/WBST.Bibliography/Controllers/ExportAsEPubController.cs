using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace WBST.Bibliography.Controllers {
    internal class ExportAsEPubController : IController<string> {
        public void Dispose() { }

        public string Execute() {
            var doc = Globals.ThisAddIn.Application.ActiveDocument;
            if (doc.IsNotNullOrMissing()) {
                var wordml = doc.WordOpenXML;
                if (wordml != null) {
                    var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".xml");
                    try {
                        XElement.Parse(wordml).Save(path);                    
                        var document = new Aspose.Words.Document(path);

                        using (var dlg = new SaveFileDialog() {
                            Filter = "Plik publikacji elektronicznej (*.epub)|*.epub"
                        }) {
                            if (dlg.ShowDialog() == DialogResult.OK) {
                                document.Save(dlg.FileName, Aspose.Words.SaveFormat.Epub);
                                return dlg.FileName;
                            }
                        }
                    }
                    finally {
                        try {
                            if (File.Exists(path)) { File.Delete(path); }
                        }
                        catch { }
                    }
                }
            }
            return default;
        }
    }
}
