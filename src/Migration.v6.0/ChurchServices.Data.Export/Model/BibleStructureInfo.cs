namespace ChurchServices.Data.Export.Model {
    public class BibleStructureInfo {
        public TranslationType Type { get; set; }
        public string Name { get; set; }
        public List<BibleBookStructureInfo> Books { get; set; }
        public BibleStructureInfo() { }
    }
    public class BibleBookStructureInfo {     
        public int NumberOfBook { get; set; }
        public string Title { get; set; }
        public string BookShortcut { get; set; }
        public string BaseBookShortcut { get; set; }
        public int ChapterStartNumber { get; set; }
        public int ChapterEndNumber { get; set; }
        public bool IsNotTranslated { get; set; }
        public bool IsInterlinearBible { get; set; }
        public int FirstTranslatedChapter { get; set; }
        public string Color { get; set; }
        public List<BibleBookChapterStructureInfo> Chapters { get; set; }
        public BibleBookStructureInfo() { }
    }
    public class BibleBookChapterStructureInfo {
        public int ChapterNumber { get; set; }
        public int VerseStartNumber { get; set; }
        public int VerseEndNumber { get; set; }
        public BibleBookChapterStructureInfo() { }
    }

}
