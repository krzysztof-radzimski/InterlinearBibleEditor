namespace ChurchServices.Data.Import.Greek.Alphabet {
    public class Ypsilon : GreekLetter {
        public override string DefaultRoman => "y";
        public override string AdditionalRoman => "u";
        public override string Small => "υ";
        public override string SmallAtTheEnd => "";

        public override string BreathingDashDash => "υ";
        public override string BreathingDashDashSubscript => "";
        public override string BreathingDashAcute => "ύ";
        public override string BreathingDashAcuteSubscript => "ύ";
        public override string BreathingDashGrave => "ὺ";
        public override string BreathingDashGraveSubscript => "";
        public override string BreathingDashCircumflex => "ῦ";
        public override string BreathingDashCircumflexSubscript => "";

        public override string BreathingSmoothDash => "ὐ";
        public override string BreathingSmoothDashSubscript => "";
        public override string BreathingSmoothAcute => "ὔ";
        public override string BreathingSmoothAcuteSubscript => "";
        public override string BreathingSmoothGrave => "ὒ";
        public override string BreathingSmoothGraveSubscript => "";
        public override string BreathingSmoothCircumflex => "ὖ";
        public override string BreathingSmoothCircumflexSubscript => "";

        public override string BreathingRoughDash => "ὑ";
        public override string BreathingRoughDashSubscript => "";
        public override string BreathingRoughAcute => "ὕ";
        public override string BreathingRoughAcuteSubscript => "";
        public override string BreathingRoughGrave => "ὓ";
        public override string BreathingRoughGraveSubscript => "";
        public override string BreathingRoughCircumflex => "ὗ";
        public override string BreathingRoughCircumflexSubscript => "";

        public override string BreathingDiaeresisDash => "ϋ";
        public override string BreathingDiaeresisAcute => "ΰ";
        public override string BreathingDiaeresisGrave => "ῢ";
        public override string BreathingDiaeresisCircumflex => "ῧ";

        public override string BreathingMacronDash => "ῡ";
        public override string BreathingBreveDash => "ῠ";
    }
}
