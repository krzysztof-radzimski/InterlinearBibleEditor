using System.IO;
using System.Windows.Forms;
namespace WBST.Bibliography.Controllers {
    internal class ImportMobiController : IController<string> {
        public void Dispose() { }

        public string Execute() {
            using (var dlg = new OpenFileDialog() {
                Filter = "Plik publikacji elektronicznej (*.mobi)|*.mobi"
            }) {
                if (dlg.ShowDialog() == DialogResult.OK) {
                    var document = new Aspose.Words.Document(dlg.FileName);
                    var docxFilePath = Path.Combine(Path.GetDirectoryName(dlg.FileName), Path.GetFileNameWithoutExtension(dlg.FileName) + ".docx");

                    if (File.Exists(docxFilePath)) {
                        if (MessageBox.Show("Czy nadpisać istniejący plik programu Word?", "Microsoft Word", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) {
                            return default;
                        }
                    }

                    document.Save(docxFilePath);

                    Globals.ThisAddIn.Application.Documents.Open(docxFilePath);

                    return docxFilePath;
                }
            }
            return default;
        }
    }
}
