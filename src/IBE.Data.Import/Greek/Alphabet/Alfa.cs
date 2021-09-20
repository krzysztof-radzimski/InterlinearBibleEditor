namespace IBE.Data.Import.Greek.Alphabet {
    public class Alfa : GreekLetter {
        public override string DefaultRoman => "a";
        public override string Small => "α";
        public override string SmallAtTheEnd => "";
       
        public override string BreathingDashDash => "α";
        public override string BreathingDashDashSubscript => "ᾳ";
        public override string BreathingDashAcute => "ά";
        public override string BreathingDashAcuteSubscript => "ᾴ";
        public override string BreathingDashGrave => "ὰ";
        public override string BreathingDashGraveSubscript => "ᾲ";
        public override string BreathingDashCircumflex => "ᾶ";
        public override string BreathingDashCircumflexSubscript => "ᾷ";

        public override string BreathingSmoothDash => "ἀ";
        public override string BreathingSmoothDashSubscript => "ᾀ";
        public override string BreathingSmoothAcute => "ἄ";
        public override string BreathingSmoothAcuteSubscript => "ᾄ";
        public override string BreathingSmoothGrave => "ἂ";
        public override string BreathingSmoothGraveSubscript => "ᾂ";
        public override string BreathingSmoothCircumflex => "ἆ";
        public override string BreathingSmoothCircumflexSubscript => "ᾆ";

        public override string BreathingRoughDash => "ἁ";
        public override string BreathingRoughDashSubscript => "ᾁ";
        public override string BreathingRoughAcute => "ἅ";
        public override string BreathingRoughAcuteSubscript => "ᾅ";
        public override string BreathingRoughGrave => "ἃ";
        public override string BreathingRoughGraveSubscript => "ᾃ";
        public override string BreathingRoughCircumflex => "ἇ";
        public override string BreathingRoughCircumflexSubscript => "ᾇ";

        public override string BreathingDiaeresisDash => "";
        public override string BreathingDiaeresisAcute => "";
        public override string BreathingDiaeresisGrave => "";
        public override string BreathingDiaeresisCircumflex => "";

        public override string BreathingMacronDash => "ᾱ";
        public override string BreathingBreveDash => "ᾰ";
    }
}
