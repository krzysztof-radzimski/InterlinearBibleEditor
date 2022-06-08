using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using WBST.Bibliography.Model;

namespace WBST.Bibliography.Forms {
    public partial class SourceForm : XtraForm {
        public static List<BibliographySource> Sources { get; private set; }
        public BibliographySource Source { get; private set; }
        public SourceForm() {
            InitializeComponent();
            SetComboBoxLists();
            Source = GetDefault();
            LoadData();
            AddGroups();
        }
        public SourceForm(BibliographySource source) {
            InitializeComponent();
            SetComboBoxLists();
            Source = source;
            LoadData();
            AddGroups();
        }

        private void AddGroups() {
            if (Sources != null) {
                var list = Sources.Where(x => x.Comments.IsNotNullOrEmpty()).Select(x => x.Comments).Distinct();
                txtGroup.Properties.Items.Clear();
                foreach (var item in list) {
                    txtGroup.Properties.Items.Add(item);
                }
            }
        }

        private BibliographySource GetDefault() {
            var guid = Guid.NewGuid().ToString();
            return new BibliographySource() {
                Author = new BibliographyAuthor() {
                    Author = new Author() {
                        Objects = new List<object> {
                            new BibliographyNameList() {
                                People = new List<BibliographyPerson>()
                            }
                        }
                    },
                    Editor = new Author() {
                        Objects = new List<object> {
                            new BibliographyNameList() {
                                People = new List<BibliographyPerson>()
                            }
                        }
                    },
                    Translator = new Author() {
                        Objects = new List<object> {
                            new BibliographyNameList() {
                                People = new List<BibliographyPerson>()
                            }
                        }
                    }
                },
                City = "",
                CountryRegion = "Polska",
                Guid = guid,
                Publisher = "",
                SourceType = SourceTypeEnum.Book,
                Tag = "W" + guid.Substring(0, 5).ToLower(),
                Title = "",
                Year = DateTime.Now.Year.ToString(),
                ShortTitle = "",
                Comments = ""
            };
        }
        private void LoadData() {
            if (Source.SourceType == SourceTypeEnum.Book) { txtSourceType.SelectedIndex = 0; }
            else if (Source.SourceType == SourceTypeEnum.ArticleInAPeriodical) { txtSourceType.SelectedIndex = 1; }
            else if (Source.SourceType == SourceTypeEnum.InternetSite) { txtSourceType.SelectedIndex = 2; }

            if (Source.Author != null) {
                if (Source.Author.Author != null) {
                    if (!String.IsNullOrEmpty(Source.Author.Author.Corporates)) {
                        cbCorporateAuthor.Checked = true;
                        txtCorporateAuthor.Text = Source.Author.Author.Corporates;
                    }
                    else {
                        cbCorporateAuthor.Checked = false;
                        txtCorporateAuthor.Text = String.Empty;

                        if (Source.Author.Author.NamesList != null) {
                            txtAuthor.Text = Source.Author.Author.NamesList.ToString();
                        }
                    }
                }

                if (Source.Author.Editor != null) {
                    txtEditor.Text = Source.Author.Editor.NamesList.ToString();
                }
                if (Source.Author.Translator != null) {
                    txtTranslator.Text = Source.Author.Translator.NamesList.ToString();
                }
            }

            if (Source.DayAccessed.IsNotNullOrEmpty() && Source.MonthAccessed.IsNotNullOrEmpty() && Source.YearAccessed.IsNotNullOrEmpty()) {
                txtAccess.DateTime = new DateTime(Source.YearAccessed.ToInt(), Source.MonthAccessed.ToInt(), Source.DayAccessed.ToInt());
            }

            txtCity.Text = Source.City;
            txtEdition.Text = Source.Edition;
            txtJournalName.Text = Source.JournalName;
            txtJournalNumber.Text = Source.Issue;
            txtPublisher.Text = Source.Publisher;
            txtShortTitle.Text = Source.ShortTitle;
            txtTitle.Text = Source.Title;
            txtUrl.Text = Source.URL;
            txtVolume.Text = Source.Volume;
            txtVolumes.Text = Source.NumberVolumes;
            txtYear.Text = Source.Year;
            txtGroup.Text = Source.Comments;
        }
        public BibliographySource Save() {
            Source.City = txtCity.Text;
            Source.Edition = txtEdition.Text;
            Source.Issue = txtJournalNumber.Text;
            Source.JournalName = txtJournalName.Text;
            Source.NumberVolumes = txtVolumes.Text;
            Source.Volume = txtVolume.Text;
            Source.Year = txtYear.Text;
            Source.Title = txtTitle.Text;
            Source.ShortTitle = txtShortTitle.Text;
            Source.Publisher = txtPublisher.Text;
            Source.URL = txtUrl.Text;
            Source.Comments = txtGroup.Text;
            switch (txtSourceType.SelectedIndex) {
                case 0: { Source.SourceType = SourceTypeEnum.Book; break; }
                case 1: { Source.SourceType = SourceTypeEnum.ArticleInAPeriodical; break; }
                case 2: { Source.SourceType = SourceTypeEnum.InternetSite; break; }
            }

            if (txtAccess.DateTime != DateTime.MinValue) {
                Source.DayAccessed = txtAccess.DateTime.Day.ToString().PadLeft(2, '0');
                Source.MonthAccessed = txtAccess.DateTime.Month.ToString().PadLeft(2, '0');
                Source.YearAccessed = txtAccess.DateTime.Year.ToString();
            }
            else {
                Source.DayAccessed = "";
                Source.MonthAccessed = "";
                Source.YearAccessed = "";
            }

            return Source;
        }

        private void btnShortUrl_Click(object sender, EventArgs e) {
            using (var client = new System.Net.WebClient()) {
                var url = Convert.ToBase64String(Encoding.UTF8.GetBytes(txtUrl.Text));
                txtUrl.Text = client.DownloadString("https://kosciol-jezusa.pl/api/UrlShortener?url=" + url);
            }
        }

        private void cbCorporateAuthor_CheckedChanged(object sender, EventArgs e) {
            txtCorporateAuthor.Enabled = cbCorporateAuthor.Checked;
            btnEditAuthor.Enabled = !cbCorporateAuthor.Checked;
        }

        private void txtSourceType_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                table.Visible = false;

                switch (txtSourceType.SelectedIndex) {
                    case 0: {
                            SetControlVisibility(false,
                            lblSourceType, txtSourceType,
                            lblAuthor, txtAuthor, btnEditAuthor,
                            txtCorporateAuthor, cbCorporateAuthor,
                            lblTitle, txtTitle,
                            lblShortTitle, txtShortTitle,
                            lblPublisher, txtPublisher,
                            lblYear, txtYear,
                            lblCity, txtCity,
                            lblGroup, txtGroup);
                            break;
                        }
                    case 1: {
                            SetControlVisibility(false,
                            lblSourceType, txtSourceType,
                            lblAuthor, txtAuthor, btnEditAuthor,
                            txtCorporateAuthor, cbCorporateAuthor,
                            lblTitle, txtTitle,
                            lblShortTitle, txtShortTitle,
                            lblJournalName, txtJournalName,
                            lblJournalNumber, txtJournalNumber,
                            lblGroup, txtGroup);
                            break;
                        }
                    case 2: {
                            SetControlVisibility(false,
                            lblSourceType, txtSourceType,
                            lblAuthor, txtAuthor, btnEditAuthor,
                            txtCorporateAuthor, cbCorporateAuthor,
                            lblTitle, txtTitle,
                            lblShortTitle, txtShortTitle,
                            lblAccess, txtAccess,
                            lblUrl, txtUrl,
                            lblGroup, txtGroup);

                            break;
                        }
                }
            }
            finally {
                table.Visible = true;
            }
        }

        private void SetControlVisibility(bool show = false, params Control[] visibleControls) {
            foreach (Control item in table.Controls) {
                //if (item is CheckEdit || item is SimpleButton || item is XtraScrollableControl) { continue; }
                if (visibleControls != null && visibleControls.Where(x => x.Name == item.Name).Any()) { item.Visible = true; continue; }
                item.Visible = show;
            }
        }

        private void cbShowAllFields_CheckedChanged(object sender, EventArgs e) {
            if (cbShowAllFields.Checked) {
                SetControlVisibility(true);
            }
            else {
                txtSourceType_SelectedIndexChanged(sender, e);
            }
        }

        private void btnEditAuthor_Click(object sender, EventArgs e) {
            if (Source.Author == null) {
                Source.Author = new BibliographyAuthor();
            }
            if (Source.Author.Author == null) {
                Source.Author.Author = new Author() {
                    Objects = new List<object>() {
                        new BibliographyNameList() {
                             People = new List<BibliographyPerson>()
                        }
                    }
                };
            }
            using (var dlg = new NameListForm(Source.Author.Author.NamesList)) {
                if (dlg.ShowDialog() == DialogResult.OK) {
                    txtAuthor.Text = Source.Author.Author.NamesList.ToString();
                }
            }
        }

        private void btnEditEditor_Click(object sender, EventArgs e) {
            if (Source.Author == null) {
                Source.Author = new BibliographyAuthor();
            }
            if (Source.Author.Editor == null) {
                Source.Author.Editor = new Author() {
                    Objects = new List<object>() {
                        new BibliographyNameList() {
                             People = new List<BibliographyPerson>()
                        }
                    }
                };
            }
            using (var dlg = new NameListForm(Source.Author.Editor.NamesList)) {
                if (dlg.ShowDialog() == DialogResult.OK) {
                    txtEditor.Text = Source.Author.Editor.NamesList.ToString();
                }
            }
        }

        private void btnEditTranslator_Click(object sender, EventArgs e) {
            if (Source.Author == null) {
                Source.Author = new BibliographyAuthor();
            }
            if (Source.Author.Translator == null) {
                Source.Author.Translator = new Author() {
                    Objects = new List<object>() {
                        new BibliographyNameList() {
                             People = new List<BibliographyPerson>()
                        }
                    }
                };
            }
            using (var dlg = new NameListForm(Source.Author.Translator.NamesList)) {
                if (dlg.ShowDialog() == DialogResult.OK) {
                    txtTranslator.Text = Source.Author.Translator.NamesList.ToString();
                }
            }
        }

        private void SetComboBoxLists() {
            try {
                var listOfCities = new List<string>();
                var listOfPublishers = new List<string>();

                if (Sources == null || Sources.Count == 0) {
                    Sources = new List<BibliographySource>();
                    var globalBibliography = Globals.ThisAddIn.Application.Bibliography;
                    foreach (Microsoft.Office.Interop.Word.Source item in globalBibliography.Sources) {
                        if (item != null) {
                            var xml = item.XML;
                            if (xml != null) {
                                var serializer = new XmlSerializer(typeof(BibliographySource));
                                var o = serializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(xml)));
                                if (o is BibliographySource) {
                                    Sources.Add(o as BibliographySource);
                                }
                            }
                        }
                    }
                }

                foreach (var s in Sources) {
                    if (s.City.IsNotNullOrEmpty() && !listOfCities.Contains(s.City.Trim())) {
                        listOfCities.Add(s.City.Trim());
                    }
                    if (s.Publisher.IsNotNullOrEmpty() && !listOfPublishers.Contains(s.Publisher.Trim())) {
                        listOfPublishers.Add(s.Publisher.Trim());
                    }
                }

                txtCity.Properties.Items.AddRange(listOfCities.OrderBy(x => x).ToArray());
                txtPublisher.Properties.Items.AddRange(listOfPublishers.OrderBy(x => x).ToArray());
            }
            catch { }
        }
    }
}
