using IBE.Common.Extensions;

namespace IBE.Data.Import.Greek.Alphabet {
    public abstract class GreekLetter : IAlphabet {
        public abstract string DefaultRoman { get; }
        public virtual string AdditionalRoman { get; }
        public abstract string Small { get; }
        public virtual string SmallAtTheEnd { get; }
        public string Large => Small.ToUpper();

        public virtual string BreathingDashDash { get; }
        public virtual string BreathingDashDashSubscript { get; }
        public virtual string BreathingDashAcute { get; }
        public virtual string BreathingDashAcuteSubscript { get; }
        public virtual string BreathingDashGrave { get; }
        public virtual string BreathingDashGraveSubscript { get; }
        public virtual string BreathingDashCircumflex { get; }
        public virtual string BreathingDashCircumflexSubscript { get; }

        public virtual string BreathingSmoothDash { get; }
        public virtual string BreathingSmoothDashSubscript { get; }
        public virtual string BreathingSmoothAcute { get; }
        public virtual string BreathingSmoothAcuteSubscript { get; }
        public virtual string BreathingSmoothGrave { get; }
        public virtual string BreathingSmoothGraveSubscript { get; }
        public virtual string BreathingSmoothCircumflex { get; }
        public virtual string BreathingSmoothCircumflexSubscript { get; }

        public virtual string BreathingRoughDash { get; }
        public virtual string BreathingRoughDashSubscript { get; }
        public virtual string BreathingRoughAcute { get; }
        public virtual string BreathingRoughAcuteSubscript { get; }
        public virtual string BreathingRoughGrave { get; }
        public virtual string BreathingRoughGraveSubscript { get; }
        public virtual string BreathingRoughCircumflex { get; }
        public virtual string BreathingRoughCircumflexSubscript { get; }

        public virtual string BreathingDiaeresisDash { get; }
        public virtual string BreathingDiaeresisAcute { get; }
        public virtual string BreathingDiaeresisGrave { get; }
        public virtual string BreathingDiaeresisCircumflex { get; }

        public virtual string BreathingMacronDash { get; }
        public virtual string BreathingBreveDash { get; }

        public string Transliteration { get; set; }
        public string CurrentGreek { get; set; }
        public bool IsUpper { get; set; }
        public bool IsStrongBreath { get; set; }

        public GreekLetter() { Transliteration = DefaultRoman; }

        public bool IsLetter(char e, out bool isUpper) { return IsLetter(e.ToString(), out isUpper); }
        public bool IsLetter(string e, out bool isUpper) {
            isUpper = false;
            if (e.IsNotNullOrWhiteSpace()) {
                var properties = this.GetType().GetProperties();
                foreach (var item in properties) {
                    if (item.Name == "Large" || item.Name == "DefaultRoman" || item.Name == "AdditionalRoman" || item.Name == "Transliteration" || item.Name == "CurrentGreek" || item.Name == "IsUpper" || item.Name == "IsStrongBreath") { continue; }
                    if (item.PropertyType == typeof(string)) {
                        var o = item.GetValue(this);
                        if (o.IsNotNull()) {
                            var val = o.ToString();
                            if (val.IsNullOrEmpty()) { continue; }
                            if (e.Equals(val)) { 
                                return true; 
                            }
                            if (e.Equals(val.ToUpper())) { 
                                isUpper = true; return true; 
                            }
                        }
                    }
                }
            }
            return default;
        }
    }
}
