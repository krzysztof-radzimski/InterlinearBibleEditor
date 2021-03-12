namespace IBE.Data.Import.Greek {
    class VerseWordInfo {
        public string Text { get; set; }
        public int StrongCode { get; set; }
        public string GrammarCode { get; set; }
        public int WordIndex { get; set; }

        public override string ToString() {
            return Text;
        }
    }
}
