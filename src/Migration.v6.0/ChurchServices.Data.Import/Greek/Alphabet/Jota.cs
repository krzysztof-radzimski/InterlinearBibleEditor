namespace ChurchServices.Data.Import.Greek.Alphabet {
    public class Jota : GreekLetter {
        public override string DefaultRoman => "i";
        public override string AdditionalRoman => "i";
        public override string Small => "ι";
        public override string SmallAtTheEnd => "";

        public override string BreathingDashDash => "ι";
        public override string BreathingDashDashSubscript => "";
        public override string BreathingDashAcute => "ί";
        public override string BreathingDashAcuteSubscript => "ί";
        public override string BreathingDashGrave => "ὶ";
        public override string BreathingDashGraveSubscript => "";
        public override string BreathingDashCircumflex => "ῖ";
        public override string BreathingDashCircumflexSubscript => "";

        public override string BreathingSmoothDash => "ἰ";
        public override string BreathingSmoothDashSubscript => "";
        public override string BreathingSmoothAcute => "ἴ";
        public override string BreathingSmoothAcuteSubscript => "";
        public override string BreathingSmoothGrave => "ἲ";
        public override string BreathingSmoothGraveSubscript => "";
        public override string BreathingSmoothCircumflex => "ἶ";
        public override string BreathingSmoothCircumflexSubscript => "";

        public override string BreathingRoughDash => "ἱ";
        public override string BreathingRoughDashSubscript => "";
        public override string BreathingRoughAcute => "ἵ";
        public override string BreathingRoughAcuteSubscript => "";
        public override string BreathingRoughGrave => "ἳ";
        public override string BreathingRoughGraveSubscript => "";
        public override string BreathingRoughCircumflex => "ἷ";
        public override string BreathingRoughCircumflexSubscript => "";

        public override string BreathingDiaeresisDash => "ϊ";
        public override string BreathingDiaeresisAcute => "ΐ";
        public override string BreathingDiaeresisGrave => "ῒ";
        public override string BreathingDiaeresisCircumflex => "ῗ";

        public override string BreathingMacronDash => "ῑ";
        public override string BreathingBreveDash => "ῐ";
    }
}
