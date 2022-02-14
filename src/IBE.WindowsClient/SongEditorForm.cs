using DevExpress.XtraEditors;
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
    public partial class SongEditorForm : XtraForm {
        private Song This = null;
        private SongEditorForm() {
            InitializeComponent();
        }
        public SongEditorForm(Song song) : this() {
            This = song;
            grid.DataSource = song.SongVerses;
            gridView.BestFitColumns();
        }
    }
}
