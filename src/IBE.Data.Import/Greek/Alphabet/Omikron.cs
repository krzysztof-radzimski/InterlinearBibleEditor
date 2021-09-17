namespace IBE.Data.Import.Greek.Alphabet {
    public class Omikron : IAlphabet {
        public string Polish => "o";
        public string Small => "ο";
        public string Large => Small.ToUpper();

        public string BreathingDashDash => "ο";
        public string BreathingDashDashSubscript => "";
        public string BreathingDashAcute => "ό";
        public string BreathingDashAcuteSubscript => "";
        public string BreathingDashGrave => "ὸ";
        public string BreathingDashGraveSubscript => "";
        public string BreathingDashCircumflex => "";
        public string BreathingDashCircumflexSubscript => "";

        public string BreathingSmoothDash => "ὀ";
        public string BreathingSmoothDashSubscript => "";
        public string BreathingSmoothAcute => "ὄ";
        public string BreathingSmoothAcuteSubscript => "";
        public string BreathingSmoothGrave => "ὂ";
        public string BreathingSmoothGraveSubscript => "";
        public string BreathingSmoothCircumflex => "";
        public string BreathingSmoothCircumflexSubscript => "";

        public string BreathingRoughDash => "ὁ";
        public string BreathingRoughDashSubscript => "";
        public string BreathingRoughAcute => "ὅ";
        public string BreathingRoughAcuteSubscript => "";
        public string BreathingRoughGrave => "ὃ";
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
