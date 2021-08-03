using BooksMaker.WindowsClient.Controls;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraRichEdit;
using System;
using System.Windows.Forms;

namespace BooksMaker.WindowsClient {
    public partial class MainForm : RibbonForm {
        public MainForm() {
            InitializeComponent();
        }

        private TabNavigationPage AddEditor(string caption, byte[] openXmlFileData = null, string rtfFileData = null, string plainTextFileData = null) {
            var tab = tabPane.AddPage(new EditorControl() {
                Dock = DockStyle.Fill
            }) as TabNavigationPage;

            tab.Caption = caption;
            tab.PageVisible = true;
            var editor = tab.Controls[0] as EditorControl;
            editor.Caption = caption;
            editor.Editor.Text = String.Empty;

            if (openXmlFileData != null) {
                editor.Editor.LoadDocument(openXmlFileData, DocumentFormat.OpenXml);
            }
            else if (plainTextFileData != null) {
                editor.Editor.Text = plainTextFileData;
            }
            else if (rtfFileData != null) {
                editor.Editor.RtfText = rtfFileData;
            }

            editor.CancelButtonClicked += new ItemClickEventHandler(delegate (object sender, ItemClickEventArgs e) {
                var editorControl = sender as EditorControl;
                var parentTab = editorControl.Parent as TabNavigationPage;
                parentTab.Controls.Clear();
                tabPane.Pages.Remove(parentTab);
            });

            return tab;
        }

        

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var tab = AddEditor("Test of topic");
            tabPane.SelectedPage = tab;
        }
    }
}
