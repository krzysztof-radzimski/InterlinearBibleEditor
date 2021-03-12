namespace IBE.Data.Import.Greek {
    class PbpwVerseInfo {
        public int Book { get; set; }
        public int Chapter { get; set; }
        public int Verse { get; set; }
        public string VerseText { get; set; }

        public PbpwVerseInfo(int book, int chapter, int verse, string text) {
            Book = book;
            Chapter = chapter;
            Verse = verse;
            VerseText = text;
        }
    }
}
