using DevExpress.Xpo;
using DevExpress.XtraBars.Ribbon;
using IBE.Data;
using IBE.Data.Model;
using System;
using System.IO;

namespace IBE.WindowsClient {
    public partial class MainForm : RibbonForm {        
        public MainForm() {
            InitializeComponent();
            Text = "Interlinear Bible Editor";
            IconOptions.SvgImage = new DevExpress.Utils.Svg.SvgImage(new MemoryStream(Properties.Resources.bible));           
        }
        private void btnShowViewer_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var frm = new ViewerForm {
                MdiParent = this
            };
            frm.Show();
        }

        private void btnEditBook_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {

        }
    }
}
