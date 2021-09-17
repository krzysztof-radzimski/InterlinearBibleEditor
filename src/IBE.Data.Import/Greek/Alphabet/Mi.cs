namespace IBE.Data.Import.Greek.Alphabet {
    public class Mi : IAlphabet {
        public string Polish => "m";
        public string Small => "μ";
        public string SmallAtTheEnd => "";
        public string Large => Small.ToUpper();

        public string BreathingDashDash => "";

        public string BreathingDashDashSubscript => "";

        public string BreathingDashAcute => "";

        public string BreathingDashAcuteSubscript => "";

        public string BreathingDashGrave => "";

        public string BreathingDashGraveSubscript => "";

        public string BreathingDashCircumflex => "";

        public string BreathingDashCircumflexSubscript => "";

        public string BreathingSmoothDash => "";

        public string BreathingSmoothDashSubscript => "";

        public string BreathingSmoothAcute => "";

        public string BreathingSmoothAcuteSubscript => "";

        public string BreathingSmoothGrave => "";

        public string BreathingSmoothGraveSubscript => "";

        public string BreathingSmoothCircumflex => "";

        public string BreathingSmoothCircumflexSubscript => "";

        public string BreathingRoughDash => "";

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

        public string ReplaceWhen(string previous) {
            return default;
        }
    }
}
