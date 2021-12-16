namespace EIB.SNPPD.Importer.Model {
    internal class VerseRunFootnoteModel : List<VerseRunFootnoteRunModel> {
        public int Number { get; set; }
    }
    public class VerseRunFootnoteRunModel {
        public bool Hebrew { get; set; }
        public bool Greek { get; set; }
        public string Text { get; set; }
        public FormattingModel Formatting { get; set; }
    }
    public class FormattingModel {
        public bool NewLine { get; set; }
        public bool Bold { get; set; }
        public bool Italic { get; set; }
        public bool Underline { get; set; }
        public bool Poetry { get; set; }
    }
}
