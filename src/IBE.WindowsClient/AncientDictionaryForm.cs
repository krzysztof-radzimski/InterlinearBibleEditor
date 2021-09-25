using DevExpress.Xpo;
using DevExpress.XtraBars.Ribbon;
using IBE.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IBE.WindowsClient {
    public partial class AncientDictionaryForm : RibbonForm {
        UnitOfWork uow;
        public AncientDictionaryForm() {
            InitializeComponent();
            uow = new UnitOfWork();
            gridControl1.DataSource = new XPCollection<AncientDictionaryItem>(uow);
            gridView1.BestFitColumns();
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            uow.CommitChanges();
        }
    }
}
