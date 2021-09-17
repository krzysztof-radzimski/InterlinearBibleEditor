namespace IBE.Data.Import.Greek.Alphabet {
    public class Eta : IAlphabet {
        public string Polish => "e";
        public string Small => "η";
        public string Large => Small.ToUpper();

        public string BreathingDashDash => "η";
        public string BreathingDashDashSubscript => "ῃ";
        public string BreathingDashAcute => "ή";
        public string BreathingDashAcuteSubscript => "ῄ";
        public string BreathingDashGrave => "ὴ";
        public string BreathingDashGraveSubscript => "ῂ";
        public string BreathingDashCircumflex => "ῆ";
        public string BreathingDashCircumflexSubscript => "ῇ";

        public string BreathingSmoothDash => "ἠ";
        public string BreathingSmoothDashSubscript => "ᾐ";
        public string BreathingSmoothAcute => "ἤ";
        public string BreathingSmoothAcuteSubscript => "ᾔ";
        public string BreathingSmoothGrave => "ἢ";
        public string BreathingSmoothGraveSubscript => "ᾒ";
        public string BreathingSmoothCircumflex => "ἦ";
        public string BreathingSmoothCircumflexSubscript => "ᾖ";

        public string BreathingRoughDash => "ἡ";
        public string BreathingRoughDashSubscript => "ᾑ";
        public string BreathingRoughAcute => "ἥ";
        public string BreathingRoughAcuteSubscript => "ᾕ";
        public string BreathingRoughGrave => "ἣ";
        public string BreathingRoughGraveSubscript => "ᾓ";
        public string BreathingRoughCircumflex => "ἧ";
        public string BreathingRoughCircumflexSubscript => "ᾗ";

        public string BreathingDiaeresisDash => "";
        public string BreathingDiaeresisAcute => "";
        public string BreathingDiaeresisGrave => "";
        public string BreathingDiaeresisCircumflex => "";

        public string BreathingMacronDash => "";
        public string BreathingBreveDash => "";
    }
}
