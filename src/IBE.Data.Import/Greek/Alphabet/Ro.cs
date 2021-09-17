namespace IBE.Data.Import.Greek.Alphabet {
    public class Ro : IAlphabet {
        public string Polish => "r";
        public string Small => "ρ";
        public string Large => Small.ToUpper();

        public string BreathingDashDash => "ρ";
        public string BreathingDashDashSubscript => "";
        public string BreathingDashAcute => "";
        public string BreathingDashAcuteSubscript => "";
        public string BreathingDashGrave => "";
        public string BreathingDashGraveSubscript => "";
        public string BreathingDashCircumflex => "";
        public string BreathingDashCircumflexSubscript => "";

        public string BreathingSmoothDash => "ῤ";
        public string BreathingSmoothDashSubscript => "";
        public string BreathingSmoothAcute => "";
        public string BreathingSmoothAcuteSubscript => "";
        public string BreathingSmoothGrave => "";
        public string BreathingSmoothGraveSubscript => "";
        public string BreathingSmoothCircumflex => "";
        public string BreathingSmoothCircumflexSubscript => "";

        public string BreathingRoughDash => "ῥ";
        public string BreathingRoughDashSubscript => "";
        public string BreathingRoughAcute => "";
        public string BreathingRoughAcuteSubscript => "";
        public string BreathingRoughGrave => "";
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
