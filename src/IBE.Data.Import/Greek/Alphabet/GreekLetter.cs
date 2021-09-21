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

        public virtual string Transliteration { get; set; }
        public virtual string CurrentGreek { get; set; }
        public virtual bool IsUpper { get; set; }
        public virtual bool IsStrongBreath { get; set; }
        public virtual LetterType Type { get; set; }
        public virtual bool IsFirst { get; set; }

        public GreekLetter() { Transliteration = DefaultRoman; }

        public LetterResult IsLetter(char e) { return IsLetter(e.ToString()); }
        public LetterResult IsLetter(string e) {
            if (e.IsNotNullOrWhiteSpace()) {
                var properties = this.GetType().GetProperties();
                foreach (var item in properties) {
                    var canWrite = item.CanWrite && item.SetMethod.IsNotNull() && item.SetMethod.IsPublic;
                    if (item.Name == "Large" || item.Name == "DefaultRoman" || item.Name == "AdditionalRoman" || canWrite) { continue; }
                    if (item.PropertyType == typeof(string)) {
                        var o = item.GetValue(this);
                        if (o.IsNotNull()) {
                            var val = o.ToString();
                            if (val.IsNullOrEmpty()) { continue; }
                            if (e.Equals(val.Trim())) {
                                var type = item.Name == "Small" ? LetterType.Default : item.Name.GetEnumByName<LetterType>();
                                return new LetterResult(true, type: type);
                            }
                            if (e.Equals(val.Trim().ToUpper())) {
                                var type = item.Name == "Small" ? LetterType.Default : item.Name.GetEnumByName<LetterType>();
                                return new LetterResult(true, true, type);
                            }
                        }
                    }
                }
            }
            return new LetterResult();
        }

        public override string ToString() {
            return Small;
        }
    }

    public class LetterResult {
        public LetterType Type { get; }
        public bool IsLetter { get; }
        public bool IsUpper { get; }
        private LetterResult() { }
        public LetterResult(bool isLetter = false, bool isUpper = false, LetterType type = LetterType.Default) {
            IsLetter = isLetter;
            IsUpper = isUpper;
            Type = type;
        }
    }

    public enum LetterType {
        Default,

        SmallAtTheEnd,

        BreathingDashDash,
        BreathingDashDashSubscript,
        BreathingDashAcute,
        BreathingDashAcuteSubscript,
        BreathingDashGrave,
        BreathingDashGraveSubscript,
        BreathingDashCircumflex,
        BreathingDashCircumflexSubscript,

        BreathingSmoothDash,
        BreathingSmoothDashSubscript,
        BreathingSmoothAcute,
        BreathingSmoothAcuteSubscript,
        BreathingSmoothGrave,
        BreathingSmoothGraveSubscript,
        BreathingSmoothCircumflex,
        BreathingSmoothCircumflexSubscript,

        BreathingRoughDash,
        BreathingRoughDashSubscript,
        BreathingRoughAcute,
        BreathingRoughAcuteSubscript,
        BreathingRoughGrave,
        BreathingRoughGraveSubscript,
        BreathingRoughCircumflex,
        BreathingRoughCircumflexSubscript,

        BreathingDiaeresisDash,
        BreathingDiaeresisAcute,
        BreathingDiaeresisGrave,
        BreathingDiaeresisCircumflex,

        BreathingMacronDash,
        BreathingBreveDash
    }
}
