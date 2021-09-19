namespace IBE.Data.Import.Greek.Alphabet {
    public class Ypsilon : IAlphabet {
        public string Polish => "y";
        public string Small => "υ";
        public string SmallAtTheEnd => "";
        public string Large => Small.ToUpper();

        public string BreathingDashDash => "υ";
        public string BreathingDashDashSubscript => "";
        public string BreathingDashAcute => "ύ";
        public string BreathingDashAcuteSubscript => "";
        public string BreathingDashGrave => "ὺ";
        public string BreathingDashGraveSubscript => "";
        public string BreathingDashCircumflex => "ῦ";
        public string BreathingDashCircumflexSubscript => "";

        public string BreathingSmoothDash => "ὐ";
        public string BreathingSmoothDashSubscript => "";
        public string BreathingSmoothAcute => "ὔ";
        public string BreathingSmoothAcuteSubscript => "";
        public string BreathingSmoothGrave => "ὒ";
        public string BreathingSmoothGraveSubscript => "";
        public string BreathingSmoothCircumflex => "ὖ";
        public string BreathingSmoothCircumflexSubscript => "";

        public string BreathingRoughDash => "ὑ";
        public string BreathingRoughDashSubscript => "";
        public string BreathingRoughAcute => "ὕ";
        public string BreathingRoughAcuteSubscript => "";
        public string BreathingRoughGrave => "ὓ";
        public string BreathingRoughGraveSubscript => "";
        public string BreathingRoughCircumflex => "ὗ";
        public string BreathingRoughCircumflexSubscript => "";

        public string BreathingDiaeresisDash => "ϋ";
        public string BreathingDiaeresisAcute => "ΰ";
        public string BreathingDiaeresisGrave => "ῢ";
        public string BreathingDiaeresisCircumflex => "ῧ";

        public string BreathingMacronDash => "ῡ";
        public string BreathingBreveDash => "ῠ";
    }
}
