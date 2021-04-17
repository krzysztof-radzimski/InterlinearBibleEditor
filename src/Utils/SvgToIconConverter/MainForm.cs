using DevExpress.Utils.Svg;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SvgToIconConverter {
    public partial class MainForm : XtraForm {
        public MainForm() {
            InitializeComponent();
        }

        private void btnOpenSvg_Click(object sender, EventArgs e) {
            using (var dlg = new OpenFileDialog() { Filter = "SVG file (*.svg)|*.svg" }) {
                if (dlg.ShowDialog() == DialogResult.OK) {
                    svgImageBox.SvgImage = File.ReadAllBytes(dlg.FileName);
                    btnConvertToIcon.Enabled = true;
                }
            }
        }

        private void btnConvertToIcon_Click(object sender, EventArgs e) {
            if (svgImageBox.SvgImage != null) {
                SvgBitmap.SvgImageRenderingMode = SvgImageRenderingMode.HighQuality;
                var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                Directory.CreateDirectory(dir);
                var list = new List<Bitmap>();
                foreach (CheckedListBoxItem sizeItem in sizes.Items) {
                    if (sizeItem.CheckState == CheckState.Checked) {                       
                        var bmp = new SvgBitmap(svgImageBox.SvgImage);
                        var size = new Size(Convert.ToInt32(sizeItem.Value), Convert.ToInt32(sizeItem.Value));
                        var img = bmp.Render(size, null);
                        var fileName = Path.Combine(dir, $"{sizeItem.Value}.png");
                        img.Save(fileName, ImageFormat.Png);
                        list.Add((Bitmap)Bitmap.FromFile(fileName));
                    }
                }
                
                var ms = new MemoryStream();
                IconFactory.SavePngsAsIcon(list, ms);

                using (var dlg = new SaveFileDialog() { Filter = "Icon file (*.ico)|*.ico" }) {
                    if (dlg.ShowDialog() == DialogResult.OK) {
                        File.WriteAllBytes(dlg.FileName, ms.ToArray());
                    }
                }
            }
        }
    }
}
