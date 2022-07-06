namespace ChurchServices.Data.Import.Greek.Alphabet {
    public class Epsilon : GreekLetter {
        public override string DefaultRoman => "e";
        public override string Small => "ε";
        public override string SmallAtTheEnd => "";

        public override string BreathingDashDash => "ε";
        public override string BreathingDashDashSubscript => "";
        public override string BreathingDashAcute => "έ";
        public override string BreathingDashAcuteSubscript => "έ";
        public override string BreathingDashGrave => "ὲ";
        public override string BreathingDashGraveSubscript => "";
        public override string BreathingDashCircumflex => "";
        public override string BreathingDashCircumflexSubscript => "";

        public override string BreathingSmoothDash => "ἐ";
        public override string BreathingSmoothDashSubscript => "";
        public override string BreathingSmoothAcute => "ἔ";
        public override string BreathingSmoothAcuteSubscript => "";
        public override string BreathingSmoothGrave => "ἒ";
        public override string BreathingSmoothGraveSubscript => "";
        public override string BreathingSmoothCircumflex => "";
        public override string BreathingSmoothCircumflexSubscript => "";

        public override string BreathingRoughDash => "ἑ";
        public override string BreathingRoughDashSubscript => "";
        public override string BreathingRoughAcute => "ἕ";
        public override string BreathingRoughAcuteSubscript => "";
        public override string BreathingRoughGrave => "ἓ";
        public override string BreathingRoughGraveSubscript => "";
        public override string BreathingRoughCircumflex => "";
        public override string BreathingRoughCircumflexSubscript => "";

        public override string BreathingDiaeresisDash => "";
        public override string BreathingDiaeresisAcute => "";
        public override string BreathingDiaeresisGrave => "";
        public override string BreathingDiaeresisCircumflex => "";

        public override string BreathingMacronDash => "";
        public override string BreathingBreveDash => "";
    }
}
