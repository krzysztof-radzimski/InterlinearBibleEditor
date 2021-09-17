namespace IBE.Data.Import.Greek.Alphabet {
    public class Epsilon : IAlphabet {
        public string Polish => "e";
        public string Small => "ε";
        public string Large => Small.ToUpper();

        public string BreathingDashDash => "ε";
        public string BreathingDashDashSubscript => "";
        public string BreathingDashAcute => "έ";
        public string BreathingDashAcuteSubscript => "";
        public string BreathingDashGrave => "ὲ";
        public string BreathingDashGraveSubscript => "";
        public string BreathingDashCircumflex => "";
        public string BreathingDashCircumflexSubscript => "";

        public string BreathingSmoothDash => "ἐ";
        public string BreathingSmoothDashSubscript => "";
        public string BreathingSmoothAcute => "ἔ";
        public string BreathingSmoothAcuteSubscript => "";
        public string BreathingSmoothGrave => "ἒ";
        public string BreathingSmoothGraveSubscript => "";
        public string BreathingSmoothCircumflex => "";
        public string BreathingSmoothCircumflexSubscript => "";

        public string BreathingRoughDash => "ἑ";
        public string BreathingRoughDashSubscript => "";
        public string BreathingRoughAcute => "ἕ";
        public string BreathingRoughAcuteSubscript => "";
        public string BreathingRoughGrave => "ἓ";
        public string BreathingRoughGraveSubscript => "";
        public string BreathingRoughCircumflex => "";
        public string BreathingRoughCircumflexSubscript => "";

        public string BreathingDiaeresisDash => "";
        public string BreathingDiaeresisAcute => "";
        public string BreathingDiaeresisGrave => "";
        public string BreathingDiaeresisCircumflex => "";

        public string BreathingMacronDash => "";

        public string BreathingBreveDash => "";
    }
}
