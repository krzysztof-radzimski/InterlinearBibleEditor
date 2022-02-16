using DevExpress.Xpo;
using DevExpress.XtraEditors;
using IBE.Common.Extensions;
using IBE.Data.Model;
using System;
using System.Linq;
using System.Windows.Forms;

namespace IBE.WindowsClient {
    public partial class SongEditorForm : XtraForm {
        private Song This = null;
        private SongEditorForm() {
            InitializeComponent();
            foreach (Enum item in Enum.GetValues(typeof(SongGroupType))) {
                txtType.Properties.Items.Add(item.ToString());
            }
        }
        public SongEditorForm(Song song) : this() {
            This = song;

            txtBPM.Value = song.BPM;
            txtName.Text = song.Name;
            txtNumber.Value = song.Number;
            txtSignature.Text = song.Signature;
            txtType.SelectedIndex = (int)song.Type;
            txtYouTube.Text = song.YouTube;

            grid.DataSource = song.SongVerses;
            gridView.BestFitColumns();
            gridView.Columns["Oid"].Visible = false;
            gridView.Columns["Index"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;

        }

        public Song Save() {
            This.YouTube = txtYouTube.Text;
            This.Signature = txtSignature.Text;
            This.Type = (SongGroupType)txtType.SelectedIndex;
            This.Number = txtNumber.Value.ToInt();
            This.Name = txtName.Text;
            This.BPM = txtBPM.Value.ToInt();
            This.Save();
            (This.Session as UnitOfWork).CommitChanges();
            return This;
        }

        private void btnPaste_Click(object sender, EventArgs e) {
            var text = Clipboard.GetText();
            if (text.IsNotNullOrEmpty()) {
                var t = text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Length > 0) {

                    if (gridView.FocusedColumn.FieldName == "Text") {
                        var emptyVerses = This.SongVerses.Where(x => x.Text.IsNullOrEmpty());
                        if (emptyVerses.Any()) {
                            var index = 0;
                            foreach (var item in emptyVerses) {
                                if (index < t.Length) {
                                    item.Text = t[index];
                                    index++;
                                }
                            }
                        }
                        else {
                            foreach (var item in t) {
                                This.SongVerses.Add(new SongVerse(This.Session as UnitOfWork) {
                                    Text = item
                                });
                            }
                        }

                        gridView.RefreshData();
                        gridView.BestFitColumns();
                    }
                    else if (gridView.FocusedColumn.FieldName == "Chords") {
                        var emptyVerses = This.SongVerses.Where(x => x.Chords.IsNullOrEmpty());
                        if (emptyVerses.Any()) {
                            var index = 0;
                            foreach (var item in emptyVerses) {
                                if (index < t.Length) {
                                    item.Chords = t[index];
                                    index++;
                                }
                            }
                        }
                        else {
                            foreach (var item in t) {
                                This.SongVerses.Add(new SongVerse(This.Session as UnitOfWork) {
                                    Chords = item
                                });
                            }
                        }

                        gridView.RefreshData();
                        gridView.BestFitColumns();
                    }
                }

            }
        }

        private void btnPasteBridge_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            PasteAs(SongVerseType.Bridge);
        }
        private void btnPasteChorus_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            PasteAs(SongVerseType.Chorus);
        }
        private void btnPasteVerse_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            PasteAs(SongVerseType.Default);
        }

        private void PasteAs(SongVerseType type) {
            var text = Clipboard.GetText();
            if (text.IsNotNullOrEmpty()) {
                var t = text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Length > 0) {
                    var number = 1;
                    var last = This.SongVerses.Where(x => x.Type == type).LastOrDefault();
                    if (last.IsNotNull()) {
                        number = last.Number + 1;
                    }

                    var emptyVerses = This.SongVerses.Where(x => x.Text.IsNullOrEmpty());
                    if (emptyVerses.Any()) {
                        var idx = 0;
                        foreach (var item in emptyVerses) {
                            var index = This.SongVerses.Count > 0 ? This.SongVerses.Max(x => x.Index) + 1 : 1;
                            if (idx < t.Length) {
                                item.Text = t[idx];
                                item.Type = type;
                                item.Number = number;
                                item.Index = index;
                                idx++;
                            }
                        }
                    }
                    else {
                        foreach (var item in t) {
                            var index = This.SongVerses.Count > 0 ? This.SongVerses.Max(x => x.Index) + 1 : 1;
                            This.SongVerses.Add(new SongVerse(This.Session as UnitOfWork) {
                                Text = item,
                                Type = type,
                                Number = number,
                                Index = index
                            });
                        }
                    }

                    gridView.RefreshData();
                    gridView.BestFitColumns();
                }

            }
        }

        private void btnPasteChords_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var text = Clipboard.GetText();
            if (text.IsNotNullOrEmpty()) {
                var t = text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Length > 0) {
                    var emptyVerses = This.SongVerses.Where(x => x.Chords.IsNullOrEmpty());
                    if (emptyVerses.Any()) {
                        var index = 0;
                        foreach (var item in emptyVerses) {
                            if (index < t.Length) {
                                item.Chords = t[index];
                                index++;
                            }
                        }
                    }
                    else {
                        foreach (var item in t) {
                            This.SongVerses.Add(new SongVerse(This.Session as UnitOfWork) {
                                Chords = item
                            });
                        }
                    }

                    gridView.RefreshData();
                    gridView.BestFitColumns();
                }
            }
        }

        private void btnDeleteVerse_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var item = gridView.GetFocusedRow() as SongVerse;
            if (item != null) {
                item.Delete();
            }
        }

        private void btnInsertVerseBefore_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var current = gridView.GetFocusedRow() as SongVerse;
            if (current != null) {
                var currentIndex = current.Index;
                This.SongVerses.Add(new SongVerse(This.Session as UnitOfWork) {
                    Index = currentIndex,
                    Number = current.Number,
                    Type = current.Type
                });

                var start = false;
                foreach (var item in This.SongVerses.OrderBy(x => x.Index)) {
                    if (item == current) {
                        start = true;
                    }
                    if (start) {
                        if (item.Oid < 0) { continue; }
                        item.Index++;
                    }
                }

                gridView.RefreshData();
                gridView.BestFitColumns();
            }
        }
    }
}
