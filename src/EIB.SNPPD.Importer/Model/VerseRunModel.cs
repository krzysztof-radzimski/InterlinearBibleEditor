namespace EIB.SNPPD.Importer.Model {
    internal class VerseRunModel {
        public FormattingModel Formatting { get; set; }
        public string Text { get; set; }
        public VerseRunFootnoteModel Footnote { get; set; }
        public VerseRunReferencesModel References { get; set; }
    }
}
