namespace IBE.Data.Import.Greek.Alphabet {
    public class Alfa : IAlphabet {
        public string Polish => "a";
        public string Small => "α";
        public string SmallAtTheEnd => "";
        public string Large => Small.ToUpper();
       
        public string BreathingDashDash => "α";
        public string BreathingDashDashSubscript => "ᾳ";
        public string BreathingDashAcute => "ά";
        public string BreathingDashAcuteSubscript => "ᾴ";
        public string BreathingDashGrave => "ὰ";
        public string BreathingDashGraveSubscript => "ᾲ";
        public string BreathingDashCircumflex => "ᾶ";
        public string BreathingDashCircumflexSubscript => "ᾷ";

        public string BreathingSmoothDash => "ἀ";
        public string BreathingSmoothDashSubscript => "ᾀ";
        public string BreathingSmoothAcute => "ἄ";
        public string BreathingSmoothAcuteSubscript => "ᾄ";
        public string BreathingSmoothGrave => "ἂ";
        public string BreathingSmoothGraveSubscript => "ᾂ";
        public string BreathingSmoothCircumflex => "ἆ";
        public string BreathingSmoothCircumflexSubscript => "ᾆ";

        public string BreathingRoughDash => "ἁ";
        public string BreathingRoughDashSubscript => "ᾁ";
        public string BreathingRoughAcute => "ἅ";
        public string BreathingRoughAcuteSubscript => "ᾅ";
        public string BreathingRoughGrave => "ἃ";
        public string BreathingRoughGraveSubscript => "ᾃ";
        public string BreathingRoughCircumflex => "ἇ";
        public string BreathingRoughCircumflexSubscript => "ᾇ";

        public string BreathingDiaeresisDash => "";
        public string BreathingDiaeresisAcute => "";
        public string BreathingDiaeresisGrave => "";
        public string BreathingDiaeresisCircumflex => "";

        public string BreathingMacronDash => "ᾱ";
        public string BreathingBreveDash => "ᾰ";
    }
}
