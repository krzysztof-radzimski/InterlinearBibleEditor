namespace IBE.Data.Import.Greek.Alphabet {
    public class Jota : IAlphabet {
        public string Polish => "j";
        public string Small => "ι";
        public string SmallAtTheEnd => "";
        public string Large => Small.ToUpper();

        public string BreathingDashDash => "ι";
        public string BreathingDashDashSubscript => "";
        public string BreathingDashAcute => "ί";
        public string BreathingDashAcuteSubscript => "";
        public string BreathingDashGrave => "ὶ";
        public string BreathingDashGraveSubscript => "";
        public string BreathingDashCircumflex => "ῖ";
        public string BreathingDashCircumflexSubscript => "";

        public string BreathingSmoothDash => "ἰ";
        public string BreathingSmoothDashSubscript => "";
        public string BreathingSmoothAcute => "ἴ";
        public string BreathingSmoothAcuteSubscript => "";
        public string BreathingSmoothGrave => "ἲ";
        public string BreathingSmoothGraveSubscript => "";
        public string BreathingSmoothCircumflex => "ἶ";
        public string BreathingSmoothCircumflexSubscript => "";

        public string BreathingRoughDash => "ἱ";
        public string BreathingRoughDashSubscript => "";
        public string BreathingRoughAcute => "ἵ";
        public string BreathingRoughAcuteSubscript => "";
        public string BreathingRoughGrave => "ἳ";
        public string BreathingRoughGraveSubscript => "";
        public string BreathingRoughCircumflex => "ἷ";
        public string BreathingRoughCircumflexSubscript => "";

        public string BreathingDiaeresisDash => "ϊ";
        public string BreathingDiaeresisAcute => "ΐ";
        public string BreathingDiaeresisGrave => "ῒ";
        public string BreathingDiaeresisCircumflex => "ῗ";

        public string BreathingMacronDash => "ῑ";
        public string BreathingBreveDash => "ῐ";
    }
}
