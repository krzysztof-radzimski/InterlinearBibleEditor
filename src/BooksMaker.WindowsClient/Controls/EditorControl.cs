using DevExpress.XtraBars;
using DevExpress.XtraRichEdit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BooksMaker.WindowsClient.Controls {
    public partial class EditorControl : UserControl {
        public string Caption { get => lblCaption.Caption; set => lblCaption.Caption = value; }
        public RichEditControl Editor => this.editor;

        public event ItemClickEventHandler SaveButtonClicked;
        public event ItemClickEventHandler SaveAndCloseButtonClicked;
        public event ItemClickEventHandler CancelButtonClicked;

        public EditorControl() {
            InitializeComponent(); 
        }

        private void btnSave_ItemClick(object sender, ItemClickEventArgs e) {
            if (SaveButtonClicked != null) { SaveButtonClicked(this, e); }
        }

        private void btnSaveAndClose_ItemClick(object sender, ItemClickEventArgs e) {
            if (SaveAndCloseButtonClicked != null) { SaveAndCloseButtonClicked(this, e); }
        }

        private void btnCancel_ItemClick(object sender, ItemClickEventArgs e) {
            if (CancelButtonClicked != null) { CancelButtonClicked(this, e); }
        }
    }
}
