using DevExpress.Xpo;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraSpellChecker;
using IBE.Data;
using IBE.Data.Model;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace IBE.WindowsClient {
    public partial class MainForm : RibbonForm {
        public SpellChecker SpellChecker { get { return spellChecker1; } }
        public static MainForm Instance { get; private set; }
        public MainForm() {
            Instance = this;
            InitializeComponent();
            Text = "Interlinear Bible Editor";
            IconOptions.SvgImage = new DevExpress.Utils.Svg.SvgImage(new MemoryStream(Properties.Resources.bible));

            ConnectionHelper.Connect();
            CreateSpellChecker();
        }

        private void CreateSpellChecker() {
            var dir = Path.Combine(Path.GetTempPath(), "IBE");
            if (!Directory.Exists(dir)) { Directory.CreateDirectory(dir); }
            var dicPath = Path.Combine(dir, "pl_PL.dic");
            var affPath = Path.Combine(dir, "pl_PL.aff");
            var alfPath = Path.Combine(dir, "PolishAlphabet.dat");
            var cusPath = Path.Combine(dir, "CustomPolish.dic");
            if (!File.Exists(dicPath)) { File.WriteAllBytes(dicPath, Properties.Resources.pl_dic); }
            if (!File.Exists(affPath)) { File.WriteAllBytes(affPath, Properties.Resources.pl_aff); }
            if (!File.Exists(alfPath)) { File.WriteAllBytes(alfPath, Properties.Resources.PolishAlphabet); }

            this.SpellChecker.Dictionaries.Add(new HunspellDictionary(dicPath, affPath, CultureInfo.CurrentCulture));
            this.SpellChecker.Dictionaries.Add(new SpellCheckerCustomDictionary() {
                AlphabetPath = alfPath,
                CaseSensitive = false,
                Culture = CultureInfo.CurrentCulture,
                DictionaryPath = cusPath,
                Encoding = Encoding.UTF8
            });
        }

        private void btnTranslations_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var frm = new TranslationsForm();
            frm.IconOptions.SvgImage = e.Item.ImageOptions.SvgImage;
            frm.MdiParent = this;
            frm.Show();
        }

        private void btnInterlinearEditor_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            //var frm = new InterlinearEditorForm();
            var frm = new VerseGridForm();
            frm.IconOptions.SvgImage = e.Item.ImageOptions.SvgImage;
            frm.MdiParent = this;
            frm.Show();
        }

        private void btnArticles_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var frm = new ArticlesForm();
            frm.IconOptions.SvgImage = e.Item.ImageOptions.SvgImage;
            frm.MdiParent = this;
            frm.Show();
        }

        private void btnBaseBooks_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var frm = new BaseBooksForm();
            frm.IconOptions.SvgImage = e.Item.ImageOptions.SvgImage;
            frm.MdiParent = this;
            frm.Show();
        }

        private void btnCopyDatabaseToWebFolder_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var path = "../../../../db/IBE.SQLite3";
            var path2 = "../../../Church.WebApp/Data/IBE.SQLite3";
            File.Copy(path, path2, true);
        }

        private void btnStrongsCodes_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var frm = new StrongsCodesForm();
            frm.IconOptions.SvgImage = e.Item.ImageOptions.SvgImage;
            frm.MdiParent = this;
            frm.Show();
        }

        private void btnAncientDictionary_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var frm = new AncientDictionaryForm();
            frm.IconOptions.SvgImage = e.Item.ImageOptions.SvgImage;
            frm.MdiParent = this;
            frm.Show();
        }
    }
}
