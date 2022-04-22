using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using WBST.Bibliography.Model;
using WBST.Bibliography.Utils;

namespace WBST.Bibliography.Forms {
    public partial class NameListForm : XtraForm {
        public BibliographyNameList NameList { get; private set; }
        private BarManager barManager;
        private PopupMenu popupMenu;
        private NameListForm(List<BibliographySource> sources = null) {
            InitializeComponent();
            barManager = new BarManager {
                Form = this
            };
            popupMenu = new PopupMenu(barManager);
            btnAdd.DropDownControl = popupMenu;

            var names = GetBibliographyPeople(sources);
            if (names.Count > 0) {
                foreach (var name in names.OrderBy(x=>x.Last)) {
                    var btn = new BarButtonItem(barManager, name.ToString()) { Tag = name };
                    btn.ItemClick += AddNameButton_ItemClick;
                    popupMenu.AddItem(btn);
                }
            }
        }

        private void AddNameButton_ItemClick(object sender, ItemClickEventArgs e) {
            NameList.People.Add(e.Item.Tag as BibliographyPerson);
            grid.RefreshDataSource();
        }

        public NameListForm(BibliographyNameList nameLists, List<BibliographySource> sources = null) : this(sources) {
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

            txtFirst.Text = txtLast.Text = txtMiddle.Text = String.Empty;
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

        private List<BibliographyPerson> GetBibliographyPeople(List<BibliographySource> sources = null) {
            var result = new List<BibliographyPerson>();
            try {
                if (sources.IsNullOrMissing()) {
                    sources = new List<BibliographySource>();
                    var globalBibliography = Globals.ThisAddIn.Application.Bibliography;
                    foreach (Microsoft.Office.Interop.Word.Source item in globalBibliography.Sources) {
                        if (item != null) {
                            var xml = item.XML;
                            if (xml != null) {
                                var serializer = new XmlSerializer(typeof(BibliographySource));
                                var o = serializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(xml)));
                                if (o is BibliographySource) {
                                    sources.Add(o as BibliographySource);
                                }
                            }
                        }
                    }
                }

                foreach (var s in sources) {
                    if (s.Author != null && s.Author.Author != null) {
                        var names = s.Author.Author.NamesList;
                        if (names != null && names.People != null) {
                            foreach (var name in names.People) {
                                if (!result.Where(x => x.ToString() == name.ToString()).Any()) {
                                    result.Add(name);
                                }
                            }
                        }
                    }
                }
            }
            catch { }
            return result;
        }
    }
}
