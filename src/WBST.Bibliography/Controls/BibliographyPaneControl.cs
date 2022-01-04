using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace WBST.Bibliography {
    public partial class BibliographyPaneControl : XtraUserControl {
        public BibliographyPaneControl() {
            InitializeComponent();
            LoadBibliography();
        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {

        }

        private void LoadBibliography() {
            var b = Globals.ThisAddIn.Application.ActiveDocument.Bibliography;
            foreach (Microsoft.Office.Interop.Word.Source item in b.Sources) {
                if (item != null && item.XML != null) {
                    var xml = XElement.Parse(item.XML);
                }
            }
        }
    }

    public class BibliographySourceInfo {
        public string Tag { get; set; }
        public SourceType Type { get; set; }
        public string Guid { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public string City { get; set; }
        public string Publisher { get; set; }
        public AuthorInfo Author { get; set; }
        public string ShortTitle { get; set; }
        public int Pages { get; set; }
        public string Edition { get; set; }
        public int RefOrder { get; set; }
        public string Url { get; set; }
    }
    public class AuthorInfo {
        public NameListInfo Authors { get; }
        public NameListInfo Translators { get; }
        public NameListInfo Editors { get; }
        public AuthorInfo() {
            Authors = new NameListInfo();
            Translators = new NameListInfo();
            Editors = new NameListInfo();
        }
    }
    public class NameListInfo : List<PersonInfo> { }
    public class PersonInfo {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
    }
    public enum SourceType {
        [Description("Artykuł z magazynu")]
        ArticleInAPeriodical,
        [Description("Książka")]
        Book,
        [Description("Fragment książki")]
        BookSection,
        [Description("Artykuł z czasopisma")]
        JournalArticle,
        [Description("Materiały konferencyjne")]
        ConferenceProceedings,
        [Description("Raport")]
        Report,
        [Description("Książka")]
        SoundRecording,
        [Description("Przedstawienie")]
        Performance,
        [Description("Sztuka")]
        Art,
        [Description("Dokument z witryny sieci Web")]
        DocumentFromInternetSite,
        [Description("Witryna sieci Web")]
        InternetSite,
        [Description("Film")]
        Film,
        [Description("Wywiad")]
        Interview,
        [Description("Patent")]
        Patent,
        [Description("Książka")]
        ElectronicSource,
        [Description("Sprawa")]
        Case,
        [Description("Różne")]
        Misc
    }
}
