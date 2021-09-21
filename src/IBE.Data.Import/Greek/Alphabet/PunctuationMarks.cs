namespace IBE.Data.Import.Greek.Alphabet.Marks {
    public class Dot : GreekLetter {
        public override string DefaultRoman => ".";
        public override string Small => ".";
    }
    public class Semicolon : GreekLetter {
        public override string DefaultRoman => ";";
        public override string Small => "·";
    }
    public class Comma : GreekLetter {
        public override string DefaultRoman => ",";
        public override string Small => ",";
    }
    public class Question : GreekLetter {
        public override string DefaultRoman => "?";
        public override string Small => ";";
    }
    public class Colon : GreekLetter {
        public override string DefaultRoman => ":";
        public override string Small => "·";
    }
    public class Apostrophe : GreekLetter {
        public override string DefaultRoman => "\"";
        public override string Small => "\"";
    }
    public class Other : GreekLetter {
        public override string DefaultRoman => "";
        public override string Small => "";        
    }
}
