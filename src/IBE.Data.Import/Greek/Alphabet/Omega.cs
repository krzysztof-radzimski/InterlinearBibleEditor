namespace IBE.Data.Import.Greek.Alphabet {
    public class Omega : GreekLetter {
        public override string DefaultRoman => "o";
        public override string Small => "ω";
        public override string SmallAtTheEnd => "";

        public override string BreathingDashDash => "ω";
        public override string BreathingDashDashSubscript => "ῳ";
        public override string BreathingDashAcute => "ώ";
        public override string BreathingDashAcuteSubscript => "ῴ";
        public override string BreathingDashGrave => "ὼ";
        public override string BreathingDashGraveSubscript => "ῲ";
        public override string BreathingDashCircumflex => "ῶ";
        public override string BreathingDashCircumflexSubscript => "ῷ";

        public override string BreathingSmoothDash => "ὠ";
        public override string BreathingSmoothDashSubscript => "ᾠ";
        public override string BreathingSmoothAcute => "ὤ";
        public override string BreathingSmoothAcuteSubscript => "ᾤ";
        public override string BreathingSmoothGrave => "ὢ";
        public override string BreathingSmoothGraveSubscript => "ᾢ";
        public override string BreathingSmoothCircumflex => "ὦ";
        public override string BreathingSmoothCircumflexSubscript => "ᾦ";

        public override string BreathingRoughDash => "ὡ";
        public override string BreathingRoughDashSubscript => "ᾡ";
        public override string BreathingRoughAcute => "ὥ";
        public override string BreathingRoughAcuteSubscript => "ᾥ";
        public override string BreathingRoughGrave => "ὣ";
        public override string BreathingRoughGraveSubscript => "ᾣ";
        public override string BreathingRoughCircumflex => "ὧ";
        public override string BreathingRoughCircumflexSubscript => "ᾧ";

        public override string BreathingDiaeresisDash => "";
        public override string BreathingDiaeresisAcute => "";
        public override string BreathingDiaeresisGrave => "";
        public override string BreathingDiaeresisCircumflex => "";

        public override string BreathingMacronDash => "";
        public override string BreathingBreveDash => "";
    }
}
