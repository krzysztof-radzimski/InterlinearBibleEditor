using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using WBST.Bibliography.Forms;
using WBST.Bibliography.Model;

namespace WBST.Bibliography {
    public partial class BibliographyPaneControl : XtraUserControl {
        public string Current { get; private set; }
        public Microsoft.Office.Interop.Word.Document Document { get; }
        public Controllers.IBibliographyFootnoteController FootnoteController { get; }
        private BibliographyPaneControl() { InitializeComponent(); }
        public BibliographyPaneControl(Microsoft.Office.Interop.Word.Document document) : this() {
            this.Document = document;
            FootnoteController = new Controllers.BibliographyFootnoteController(document);
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
                            var serializer = new XmlSerializer(typeof(Model.BibliographySource));
                            var o = serializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(xml)));
                            if (o is Model.BibliographySource) {
                                list.Add(o as Model.BibliographySource);
                            }
                        }
                    }
                }

                grid.DataSource = list;
                btnAddFootnote.Enabled = list.Count > 0;

                view.Columns["SourceType"].Group();
                view.BestFitColumns();
                view.ExpandAllGroups();
            }
        }

        private void btnAddSource_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            using (var dlg = new SourceForm()) {
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    if (dlg.Source != null) {
                        var stream = new MemoryStream();
                        var serializer = new XmlSerializer(typeof(BibliographySource));
                        serializer.Serialize(stream, dlg.Source);
                        var xml = Encoding.UTF8.GetString(stream.GetBuffer());

                        xml = xml.Replace(@"xmlns=""http://schemas.openxmlformats.org/officeDocument/2006/bibliography""", @"xmlns:b=""http://schemas.openxmlformats.org/officeDocument/2006/bibliography""");
                        xml = xml.Replace("</", "</b:");
                        xml = Regex.Replace(xml, @"\<(?<first>[A-Z])", delegate (Match m) {
                            return "<b:" + m.Groups["first"].Value;
                        });
                        xml = xml.Replace(@"<?xml version=""1.0"" encoding=""UTF-8""?>", "");
                        xml = xml.Replace(@"<?xml version=""1.0""?>", "");

                        Document.Bibliography.Sources.Add(xml.Trim());
                        LoadBibliography();
                    }
                }
            }
        }


    }
}
