using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WBST.Bibliography.Forms {
    public partial class Form1 : XtraForm {
        public Form1() {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e) {
            dockPanel6.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible;
        }
    }
}
