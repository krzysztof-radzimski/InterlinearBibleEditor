using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Serialization;
using WBST.Bibliography.Forms;
using WBST.Bibliography.Model;

namespace WBST.Bibliography {
    public partial class BibliographyPaneControl : XtraUserControl {
        public string Current { get; private set; }
        public Microsoft.Office.Interop.Word.Document Document { get; }
        public Controllers.IBibliographyController FootnoteController { get; }
        private BibliographyPaneControl() { InitializeComponent(); }
        public BibliographyPaneControl(Microsoft.Office.Interop.Word.Document document) : this() {
            this.Document = document;
            FootnoteController = new Controllers.BibliographyController(document);
            LoadBibliography();
        }

        private void btnAddFootnote_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            InsertFootNote();
        }

        private void view_DoubleClick(object sender, EventArgs e) {
            InsertFootNote();
        }
        private void InsertFootNote() {
            var item = view.GetFocusedRow() as BibliographySource;
            FootnoteController.InsertFootNote(item);
        }

        public void LoadBibliography() {
            var list = new List<BibliographySource>();
            Microsoft.Office.Interop.Word.Bibliography b = null;
            if (Document != null) {
                b = Document.Bibliography;
            }
            if (b != null) {
                foreach (Microsoft.Office.Interop.Word.Source item in b.Sources) {
                    if (item != null) {
                        var xml = item.XML;
                        if (xml != null) {
                            var serializer = new XmlSerializer(typeof(BibliographySource));
                            var o = serializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(xml)));
                            if (o is BibliographySource) {
                                list.Add(o as BibliographySource);
                            }
                        }
                    }
                }

                grid.DataSource = list;
                btnAddFootnote.Enabled = list.Count > 0;

                view.Columns["SourceType"].Group();
                view.Columns["Author"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                view.Columns["Author"].VisibleIndex = 1;

                view.BestFitColumns();
                view.ExpandAllGroups();
            }
        }

        private void btnAddSource_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            using (var dlg = new SourceForm(grid.DataSource as List<BibliographySource>)) {
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    if (dlg.Source != null) {
                        dlg.Save();
                        var xml = GetXml(dlg.Source);

                        Document.Bibliography.Sources.Add(xml.Trim());
                        LoadBibliography();
                    }
                }
            }
        }

        private string GetXml(BibliographySource source) {
            var stream = new MemoryStream();
            var serializer = new XmlSerializer(typeof(BibliographySource));
            serializer.Serialize(stream, source);
            var xml = Encoding.UTF8.GetString(stream.GetBuffer());

            xml = xml.Replace(@"xmlns=""http://schemas.openxmlformats.org/officeDocument/2006/bibliography""", @"xmlns:b=""http://schemas.openxmlformats.org/officeDocument/2006/bibliography""");
            xml = xml.Replace("</", "</b:");
            xml = Regex.Replace(xml, @"\<(?<first>[A-Z])", delegate (Match m) {
                return "<b:" + m.Groups["first"].Value;
            });
            xml = xml.Replace(@"<?xml version=""1.0"" encoding=""UTF-8""?>", "");
            xml = xml.Replace(@"<?xml version=""1.0""?>", "");
            return xml;
        }

        private void btnEditSource_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var item = view.GetFocusedRow() as BibliographySource;
            if (item.IsNotNullOrMissing()) {
                using (var dlg = new SourceForm(item, grid.DataSource as List<BibliographySource>)) {
                    if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                        dlg.Save();

                        Microsoft.Office.Interop.Word.Source src = null;
                        foreach (Microsoft.Office.Interop.Word.Source source in Document.Bibliography.Sources) {
                            if (source.Tag == dlg.Source.Tag) {
                                src = source;
                                break;
                            }
                        }
                        if (src != null) {
                            var xml = GetXml(dlg.Source);

                            src.Delete();
                            Document.Bibliography.Sources.Add(xml.Trim());
                            LoadBibliography();
                        }
                    }
                }
            }
        }

        private void btnDeleteSource_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var item = view.GetFocusedRow() as BibliographySource;
            if (item.IsNotNullOrMissing()) {
                Microsoft.Office.Interop.Word.Source src = null;
                foreach (Microsoft.Office.Interop.Word.Source source in Document.Bibliography.Sources) {
                    if (source.Tag == item.Tag) {
                        src = source;
                        break;
                    }
                }
                if (src != null) {
                    if (XtraMessageBox.Show($"Czy usunąć źródło '{item.Title}'?", "WBST Bibliografia", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes) {
                        src.Delete();
                        LoadBibliography();
                    }
                }
            }
        }

        private void btnAppendBibliography_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var sources = grid.DataSource as List<BibliographySource>;
            if (sources != null) {
                FootnoteController.AppendBibliography(sources);
            }
        }

        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            LoadBibliography();
        }

        private void view_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e) {
            if (e.HitInfo.InRow) {
                System.Drawing.Point p2 = Control.MousePosition;
                this.popupMenu1.ShowPopup(p2);
            }
        }

        private void btnAppendBibliographyItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var item = view.GetFocusedRow() as BibliographySource;
            if (item.IsNotNullOrMissing()) {
                FootnoteController.AppendBibliography(item);
            }
        }
    }
}
