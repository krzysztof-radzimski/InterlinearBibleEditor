using ChurchServices.Data;
using ChurchServices.Data.Model;
using ChurchServices.Extensions;
using DevExpress.Xpo;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraSpellChecker;
using Newtonsoft.Json;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using Microsoft.Win32;
using DevExpress.LookAndFeel;

namespace ChurchServices.WinApp {
    public partial class MainForm : RibbonForm {
        public SpellChecker SpellChecker { get { return spellChecker1; } }
        public static MainForm Instance { get; private set; }
        public MainForm() {
            Instance = this;
            InitializeComponent();
            Text = "Church Services Content Manager";

            var imgData = Program.GetResource("bible.svg");
            if (imgData != null) {
                IconOptions.SvgImage = new DevExpress.Utils.Svg.SvgImage(new MemoryStream(imgData));
            }

            new ConnectionHelper().Connect();
            CreateSpellChecker();

            SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;
            SetTheme();
        }

        private void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e) {
            if (e != null) {
                if (e.Category == UserPreferenceCategory.General) {
                    SetTheme();
                }
            }
        }
        private void SetTheme() {
            if (ThemeIsLight()) {
                UserLookAndFeel.Default.SetSkinStyle(SkinSvgPalette.WXICompact.Calmness);
            }
            else {
                UserLookAndFeel.Default.SetSkinStyle(SkinSvgPalette.WXICompact.Darkness);
            }
        }
        public bool ThemeIsLight() {
            RegistryKey registry =
                Registry.CurrentUser.OpenSubKey(
                    @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
            return (int)registry.GetValue("SystemUsesLightTheme") == 1;
        }

        private void CreateSpellChecker() {
            var dir = Path.Combine(Path.GetTempPath(), "IBE");
            if (!Directory.Exists(dir)) { Directory.CreateDirectory(dir); }
            var dicPath = Path.Combine(dir, "pl_PL.dic");
            var affPath = Path.Combine(dir, "pl_PL.aff");
            var alfPath = Path.Combine(dir, "PolishAlphabet.dat");
            var cusPath = Path.Combine(dir, "CustomPolish.dic");
            if (!File.Exists(dicPath)) { File.WriteAllBytes(dicPath, Program.GetResource("pl_PL.dat")); }
            if (!File.Exists(affPath)) { File.WriteAllBytes(affPath, Program.GetResource("pl_PL.aff")); }
            if (!File.Exists(alfPath)) { File.WriteAllBytes(alfPath, Program.GetResource("PolishAlphabet.dat")); }

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
            var frm = GetForm<TranslationsForm>();
            if (frm != null) {
                frm.Activate();
            }
            else {
                frm = new TranslationsForm();
                frm.IconOptions.SvgImage = e.Item.ImageOptions.SvgImage;
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void btnInterlinearEditor_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var frm = new VerseGridForm();
            frm.IconOptions.SvgImage = e.Item.ImageOptions.SvgImage;
            frm.MdiParent = this;
            frm.Show();
        }

        private void btnArticles_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var frm = GetForm<ArticlesForm>();
            if (frm != null) {
                frm.Activate();
            }
            else {
                frm = new ArticlesForm();
                frm.IconOptions.SvgImage = e.Item.ImageOptions.SvgImage;
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void btnBaseBooks_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var frm = GetForm<BaseBooksForm>();
            if (frm != null) {
                frm.Activate();
            }
            else {
                frm = new BaseBooksForm();
                frm.IconOptions.SvgImage = e.Item.ImageOptions.SvgImage;
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void btnCopyDatabaseToWebFolder_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var path = "../../../../../../db/IBE.SQLite3";
            var path2 = "../../../../ChurchServices.WebApp/Data/IBE.SQLite3";
            var info = new FileInfo(path);
            if (info.Exists) {
                var info2 = new FileInfo(path2);
                if (!info2.Directory.Exists) { info2.Directory.Create(); }
                info.CopyTo(path2, true);
            }
            //File.Copy(path, path2, true);
        }

        private void btnStrongsCodes_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var frm = GetForm<StrongsCodesForm>();
            if (frm != null) {
                frm.Activate();
            }
            else {
                frm = new StrongsCodesForm();
                frm.IconOptions.SvgImage = e.Item.ImageOptions.SvgImage;
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void btnAncientDictionary_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var frm = GetForm<AncientDictionaryForm>();
            if (frm != null) {
                frm.Activate();
            }
            else {
                frm = new AncientDictionaryForm();
                frm.IconOptions.SvgImage = e.Item.ImageOptions.SvgImage;
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void btnImportUrlShortenersList_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            using (var client = new WebClient()) {
                var dataString = client.DownloadString("https://kosciol-jezusa.pl/api/UrlShortList");
                if (dataString.IsNotNullOrEmpty()) {
                    var result = JsonConvert.DeserializeObject<List<UrlShortInfo>>(dataString);
                    if (result.IsNotNull()) {
                        var uow = new UnitOfWork();
                        foreach (var item in result) {
                            var q = new XPQuery<UrlShort>(uow).Where(x => x.ShortUrl == item.Short).Any();
                            if (!q) {
                                var s = new UrlShort(uow) {
                                    ShortUrl = item.Short,
                                    Url = item.Url
                                };
                                s.Save();
                                uow.CommitChanges();
                            }
                        }
                    }
                }
            }
        }

        private void btnSongs_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var frm = GetForm<SongsForm>();
            if (frm != null) {
                frm.Activate();
            }
            else {
                frm = new SongsForm();
                frm.IconOptions.SvgImage = e.Item.ImageOptions.SvgImage;
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void btnUpdateVersesIndex_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var uow = new UnitOfWork();
            var verses = new XPQuery<Verse>(uow).Where(x => x.Index == null || x.Index == string.Empty).ToList();
            foreach (var item in verses) {
                var translationName = item.ParentTranslation.Name.Replace("+", "").Replace("'", "");
                var index = $"{translationName}.{item.ParentChapter.ParentBook.NumberOfBook}.{item.ParentChapter.NumberOfChapter}.{item.NumberOfVerse}";
                item.Index = index;
                item.Save();
            }
            uow.CommitChanges();
        }

        private T GetForm<T>() where T : class {
            foreach (var form in MdiChildren) {
                if (form.GetType().Name == typeof(T).Name) {
                    return form as T;
                }
            }
            return default;
        }

        private void ribbonControl1_Merge(object sender, RibbonMergeEventArgs e) {
            RibbonControl parentRRibbon = sender as RibbonControl;
            RibbonControl childRibbon = e.MergedChild;
            parentRRibbon.StatusBar.MergeStatusBar(childRibbon.StatusBar);
        }

        private void ribbonControl1_UnMerge(object sender, RibbonMergeEventArgs e) {
            try {
                RibbonControl parentRRibbon = sender as RibbonControl;
                parentRRibbon.StatusBar.UnMergeStatusBar();
            }
            catch { }
        }
    }
}
