namespace ChurchServices.WebApp.Models {
    public class BibleByVerseModel {
        public List<BibleByVerseBookInfo> Books { get; set; }
        public int Book { get; set; }
        public string Title { get; set; }
        public string Shortcut { get; set; }
        public int Chapter { get; set; }
        public int Verse { get; set; }
        public string OrginalText { get; set; }
        public string VovelsText { get; set; }
        public string VovelsName { get; set; }
        public string Transliteration {  get; set; }
        public string SNPD { get; set; }
        public string BW { get; set; }
        public string BT { get; set; }
        public string UBG { get; set; }
        public string BG { get; set; }
        public int ChapterCount { get; set; }
        public int VerseCount { get; set; }
    }

    public class BibleByVerseBookInfo {
        public int Number { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Color { get; set; }
    }
}
