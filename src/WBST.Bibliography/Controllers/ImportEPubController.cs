using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using IBE.ePubConverter.Common.Converters;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WBST.Bibliography.Controllers {
    internal class ImportEPubController : IController<string> {
        public void Dispose() { }

        public string Execute() {
            using (var dlg = new OpenFileDialog() {
                Filter = "Plik publikacji elektronicznej (*.epub)|*.epub"
            }) {
                if (dlg.ShowDialog() == DialogResult.OK) {
                    var docxFilePath = string.Empty;
                    var task = new Task<string>(() => {
                        var result = new WordConverter().Execute(dlg.FileName, false);
                        return result;
                    });

                    DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle(DevExpress.LookAndFeel.SkinStyle.Office2019Black);
                    SplashScreenManager.ShowDefaultWaitForm("Importowanie EPUB", "Proszę czekać...");

                    task.Start();
                    while (true) {
                        if (task.Status != TaskStatus.Running && task.Status != TaskStatus.WaitingToRun) {
                            break;
                        }
                        System.Threading.Thread.Sleep(500);
                    }

                    task.ContinueWith(t => {
                        if (t.Exception == null) {
                            docxFilePath = t.Result;
                            Globals.ThisAddIn.Application.Documents.Open(docxFilePath);
                            SplashScreenManager.Default.CloseWaitForm();
                        }
                    });

                    return docxFilePath;
                }
            }
            return default;
        }
    }
}
