namespace IBE.Data.Import.Greek.Alphabet {
    public class Eta : GreekLetter {
        public override string DefaultRoman => "e";
        public override string Small => "η";
        public override string SmallAtTheEnd => "";

        public override string BreathingDashDash => "η";
        public override string BreathingDashDashSubscript => "ῃ";
        public override string BreathingDashAcute => "ή";
        public override string BreathingDashAcuteSubscript => "ῄ";
        public override string BreathingDashGrave => "ὴ";
        public override string BreathingDashGraveSubscript => "ῂ";
        public override string BreathingDashCircumflex => "ῆ";
        public override string BreathingDashCircumflexSubscript => "ῇ";

        public override string BreathingSmoothDash => "ἠ";
        public override string BreathingSmoothDashSubscript => "ᾐ";
        public override string BreathingSmoothAcute => "ἤ";
        public override string BreathingSmoothAcuteSubscript => "ᾔ";
        public override string BreathingSmoothGrave => "ἢ";
        public override string BreathingSmoothGraveSubscript => "ᾒ";
        public override string BreathingSmoothCircumflex => "ἦ";
        public override string BreathingSmoothCircumflexSubscript => "ᾖ";

        public override string BreathingRoughDash => "ἡ";
        public override string BreathingRoughDashSubscript => "ᾑ";
        public override string BreathingRoughAcute => "ἥ";
        public override string BreathingRoughAcuteSubscript => "ᾕ";
        public override string BreathingRoughGrave => "ἣ";
        public override string BreathingRoughGraveSubscript => "ᾓ";
        public override string BreathingRoughCircumflex => "ἧ";
        public override string BreathingRoughCircumflexSubscript => "ᾗ";

        public override string BreathingDiaeresisDash => "";
        public override string BreathingDiaeresisAcute => "ή";
        public override string BreathingDiaeresisGrave => "";
        public override string BreathingDiaeresisCircumflex => "";

        public override string BreathingMacronDash => "";
        public override string BreathingBreveDash => "";
    }
}
