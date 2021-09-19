namespace IBE.Data.Import.Greek.Alphabet {
    public class Omega : IAlphabet {
        public string Polish => "o";
        public string Small => "ω";
        public string SmallAtTheEnd => "";
        public string Large => Small.ToUpper();

        public string BreathingDashDash => "ω";
        public string BreathingDashDashSubscript => "ῳ";
        public string BreathingDashAcute => "ώ";
        public string BreathingDashAcuteSubscript => "ῴ";
        public string BreathingDashGrave => "ὼ";
        public string BreathingDashGraveSubscript => "ῲ";
        public string BreathingDashCircumflex => "ῶ";
        public string BreathingDashCircumflexSubscript => "ῷ";

        public string BreathingSmoothDash => "ὠ";
        public string BreathingSmoothDashSubscript => "ᾠ";
        public string BreathingSmoothAcute => "ὤ";
        public string BreathingSmoothAcuteSubscript => "ᾤ";
        public string BreathingSmoothGrave => "ὢ";
        public string BreathingSmoothGraveSubscript => "ᾢ";
        public string BreathingSmoothCircumflex => "ὦ";
        public string BreathingSmoothCircumflexSubscript => "ᾦ";

        public string BreathingRoughDash => "ὡ";
        public string BreathingRoughDashSubscript => "ᾡ";
        public string BreathingRoughAcute => "ὥ";
        public string BreathingRoughAcuteSubscript => "ᾥ";
        public string BreathingRoughGrave => "ὣ";
        public string BreathingRoughGraveSubscript => "ᾣ";
        public string BreathingRoughCircumflex => "ὧ";
        public string BreathingRoughCircumflexSubscript => "ᾧ";

        public string BreathingDiaeresisDash => "";
        public string BreathingDiaeresisAcute => "";
        public string BreathingDiaeresisGrave => "";
        public string BreathingDiaeresisCircumflex => "";

        public string BreathingMacronDash => "";
        public string BreathingBreveDash => "";
    }
}
