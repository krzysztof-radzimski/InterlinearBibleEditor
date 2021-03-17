using DevExpress.Xpo;
using DevExpress.XtraBars.Ribbon;
using IBE.Data;
using IBE.Data.Import;
using IBE.Data.Model;
using System;
using System.IO;

namespace IBE.WindowsClient {
    public partial class MainForm : RibbonForm {        
        public MainForm() {
            InitializeComponent();
            Text = "Interlinear Bible Editor";
            IconOptions.SvgImage = new DevExpress.Utils.Svg.SvgImage(new MemoryStream(Properties.Resources.bible));

            ConnectionHelper.Connect();
        }

        private void btnTranslations_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var frm = new TranslationsForm();
            frm.IconOptions.SvgImage = e.Item.ImageOptions.SvgImage;
            frm.MdiParent = this;
            frm.Show();
        }

        private void btnInterlinearEditor_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var frm = new InterlinearEditorForm();
            frm.IconOptions.SvgImage = e.Item.ImageOptions.SvgImage;
            frm.MdiParent = this;
            frm.Show();
        }
    }
}
