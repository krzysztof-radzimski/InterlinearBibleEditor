using DevExpress.XtraEditors;
using System;
using WBST.Bibliography.Model;
using WBST.Bibliography.Utils;

namespace WBST.Bibliography.Forms {
    public partial class NameListForm : XtraForm {
        public BibliographyNameList NameList { get; private set; }

        private NameListForm() {
            InitializeComponent();
        }
        public NameListForm(BibliographyNameList nameLists) : this() {
            NameList = nameLists;

            grid.DataSource = nameLists.People;
            view.BestFitColumns();
        }

        private void btnAdd_Click(object sender, EventArgs e) {
            NameList.People.Add(new BibliographyPerson() {
                First = txtFirst.Text,
                Middle = txtMiddle.Text,
                Last = txtLast.Text
            });
            grid.RefreshDataSource();
        }

        private void btnUp_Click(object sender, EventArgs e) {
            var item = view.GetFocusedRow() as BibliographyPerson;
            if (item != null) {
                NameList.People.MoveUp(item);
                grid.RefreshDataSource();
            }
        }

        private void btnDown_Click(object sender, EventArgs e) {
            var item = view.GetFocusedRow() as BibliographyPerson;
            if (item != null) {
                NameList.People.MoveDown(item);
                grid.RefreshDataSource();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e) {
            var item = view.GetFocusedRow() as BibliographyPerson;
            if (item != null) {
                NameList.People.Remove(item);
                grid.RefreshDataSource();
            }
        }

        private void view_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e) {
            var item = view.GetFocusedRow() as BibliographyPerson;
            if (item != null) {
                btnUp.Enabled = NameList.People.IndexOf(item) > 0;
                btnDown.Enabled = NameList.People.IndexOf(item) < NameList.People.Count - 1;
                btnDelete.Enabled = true;
            }
            else {
                btnDelete.Enabled = false;
                btnUp.Enabled = false;
                btnDown.Enabled = false;
            }
        }
    }
}
