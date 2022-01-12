using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using WBST.Bibliography.Model;

namespace WBST.Bibliography.Forms {
    public partial class SourceForm : XtraForm {
        public BibliographySource Source { get; private set; }
        public SourceForm() {
            InitializeComponent();
            Source = GetDefault();
            LoadData();
        }
        public SourceForm(BibliographySource source) {
            InitializeComponent();
            Source = source;
            LoadData();
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
                //DayAccessed = DateTime.Now.Day.ToString().PadLeft(2, '0'),
                //MonthAccessed = DateTime.Now.Month.ToString().PadLeft(2, '0'),
                //YearAccessed = DateTime.Now.Year.ToString(),
                Guid = guid,
                Publisher = "",
                SourceType = SourceTypeEnum.Book,
                Tag = "W" + guid.Substring(0, 5).ToLower(),
                Title = "",
                Year = DateTime.Now.Year.ToString(),
                // Month = DateTime.Now.Month.ToString(),
                ShortTitle = ""
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
            Process.Start("https://app.bitly.com/");
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
                            SetControlVisibility();
                            lblSourceType.Visible = txtSourceType.Visible =
                            lblAuthor.Visible = txtAuthor.Visible = btnEditAuthor.Visible =
                            txtCorporateAuthor.Visible = cbCorporateAuthor.Visible =
                            lblTitle.Visible = txtTitle.Visible =
                            lblShortTitle.Visible = txtShortTitle.Visible =
                            lblPublisher.Visible = txtPublisher.Visible =
                            lblYear.Visible = txtYear.Visible =
                            lblCity.Visible = txtCity.Visible =
                            true;
                            break;
                        }
                    case 1: {
                            SetControlVisibility();
                            lblSourceType.Visible = txtSourceType.Visible =
                            lblAuthor.Visible = txtAuthor.Visible = btnEditAuthor.Visible =
                            txtCorporateAuthor.Visible = cbCorporateAuthor.Visible =
                            lblTitle.Visible = txtTitle.Visible =
                            lblShortTitle.Visible = txtShortTitle.Visible =
                            lblJournalName.Visible = txtJournalName.Visible =
                            lblJournalNumber.Visible = txtJournalNumber.Visible =
                            true;
                            break;
                        }
                    case 2: {
                            SetControlVisibility();
                            lblSourceType.Visible = txtSourceType.Visible =
                            lblAuthor.Visible = txtAuthor.Visible = btnEditAuthor.Visible =
                            txtCorporateAuthor.Visible = cbCorporateAuthor.Visible =
                            lblTitle.Visible = txtTitle.Visible =
                            lblShortTitle.Visible = txtShortTitle.Visible =
                            lblAccess.Visible = txtAccess.Visible =
                            lblUrl.Visible = txtUrl.Visible = true;
                            break;
                        }
                }
            }
            finally {
                table.Visible = true;
            }
        }

        private void SetControlVisibility(bool show = false) {
            foreach (Control item in table.Controls) {
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
            using (var dlg = new NameListForm(Source.Author.Author.NamesList)) {
                if (dlg.ShowDialog() == DialogResult.OK) {
                    txtAuthor.Text = Source.Author.Author.NamesList.ToString();
                }
            }
        }

        private void btnEditEditor_Click(object sender, EventArgs e) {
            using (var dlg = new NameListForm(Source.Author.Editor.NamesList)) {
                if (dlg.ShowDialog() == DialogResult.OK) {
                    txtEditor.Text = Source.Author.Editor.NamesList.ToString();
                }
            }
        }

        private void btnEditTranslator_Click(object sender, EventArgs e) {
            using (var dlg = new NameListForm(Source.Author.Translator.NamesList)) {
                if (dlg.ShowDialog() == DialogResult.OK) {
                    txtTranslator.Text = Source.Author.Translator.NamesList.ToString();
                }
            }
        }
    }
}
