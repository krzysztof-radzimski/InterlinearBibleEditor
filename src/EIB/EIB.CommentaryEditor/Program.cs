using DevExpress.Xpo;
using EIB.CommentaryEditor.Db;
using EIB.CommentaryEditor.Db.Model;
using System;
using System.Linq;
using System.Windows.Forms;

namespace EIB.CommentaryEditor {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ConnectionHelper.Connect();

            int commentaryOid = -1;

            using (var uow = new UnitOfWork()) {
                if (!new XPQuery<Commentary>(uow).Any()) {
                    var commentary = new Commentary(uow) {
                        Abbreviation = String.Empty,
                        Information = String.Empty,
                        Version = 4,
                        Title = String.Empty
                    };

                    using (var dlg = new CommentaryDialog(commentary)) {
                        if (dlg.ShowDialog() == DialogResult.OK) {
                            commentary.Save();
                            uow.CommitChanges();

                            commentaryOid = commentary.Oid;
                        }
                        else {
                            return;
                        }
                    }
                }
                else {
                    using (var dlg = new ChooseCommentary()) {
                        if (dlg.ShowDialog() == DialogResult.OK) {
                            commentaryOid = dlg.Commentary.Oid;
                        }
                        else {
                            return;
                        }
                    }
                }
            }

            Application.Run(new MainForm(commentaryOid));
        }
    }
}
