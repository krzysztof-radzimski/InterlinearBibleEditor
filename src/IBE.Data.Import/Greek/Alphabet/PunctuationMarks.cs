namespace IBE.Data.Import.Greek.Alphabet.Marks {
    public class Dot : GreekLetter {
        public override string DefaultRoman => ".";
        public override string Small => ".";
        public override bool IsMark { get => true; set => base.IsMark = value; }
    }
    public class Semicolon : GreekLetter {
        public override string DefaultRoman => ";";
        public override string Small => "·"; 
        public override bool IsMark { get => true; set => base.IsMark = value; }
    }
    public class Comma : GreekLetter {
        public override string DefaultRoman => ",";
        public override string Small => ",";
        public override bool IsMark { get => true; set => base.IsMark = value; }
    }
    public class Question : GreekLetter {
        public override string DefaultRoman => "?";
        public override string Small => ";";
        public override bool IsMark { get => true; set => base.IsMark = value; }
    }
    public class Colon : GreekLetter {
        public override string DefaultRoman => ":";
        public override string Small => "·";
        public override bool IsMark { get => true; set => base.IsMark = value; }
    }
    public class Apostrophe : GreekLetter {
        public override string DefaultRoman => "\"";
        public override string Small => "\"";
        public override bool IsMark { get => true; set => base.IsMark = value; }
    }
    public class Other : GreekLetter {
        public override string DefaultRoman => "";
        public override string Small => "";        
    }
}
