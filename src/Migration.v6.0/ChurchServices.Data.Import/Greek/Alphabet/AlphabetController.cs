using System;

namespace ChurchServices.Data.Import.Greek.Alphabet {
    public class AlphabetController {
        public Alfa Alfa { get; }
        public Beta Beta { get; }
        public Gamma Gamma { get; }
        public Delta Delta { get; }
        public Epsilon Epsilon { get; }
        public Zeta Zeta { get; }
        public Eta Eta { get; }
        public Theta Theta { get; }
        public Jota Jota { get; }
        public Kappa Kappa { get; }
        public Lambda Lambda { get; }
        public Mi Mi { get; }
        public Ni Ni { get; }
        public Ksi Ksi { get; }
        public Omikron Omikron { get; }
        public Pi Pi { get; }
        public Ro Ro { get; }
        public Sigma Sigma { get; }
        public Tau Tau { get; }
        public Ypsilon Ypsilon { get; }
        public Fi Fi { get; }
        public Chi Chi { get; }
        public Psi Psi { get; }
        public Omega Omega { get; }

        public Marks.Colon Colon { get; }
        public Marks.Comma Comma { get; }
        public Marks.Dot Dot { get; }
        public Marks.Question Question { get; }
        public Marks.Semicolon Semicolon { get; }
        public Marks.Apostrophe Apostrophe { get; }

        public AlphabetController() {
            Alfa = new Alfa();
            Beta = new Beta();
            Chi = new Chi();
            Delta = new Delta();
            Epsilon = new Epsilon();
            Eta = new Eta();
            Fi = new Fi();
            Gamma = new Gamma();
            Jota = new Jota();
            Kappa = new Kappa();
            Ksi = new Ksi();
            Lambda = new Lambda();
            Mi = new Mi();
            Ni = new Ni();
            Omega = new Omega();
            Omikron = new Omikron();
            Pi = new Pi();
            Psi = new Psi();
            Ro = new Ro();
            Sigma = new Sigma();
            Tau = new Tau();
            Theta = new Theta();
            Ypsilon = new Ypsilon();
            Zeta = new Zeta();

            Colon = new Marks.Colon();
            Comma = new Marks.Comma();
            Dot = new Marks.Dot();
            Question = new Marks.Question();
            Semicolon = new Marks.Semicolon();
            Apostrophe = new Marks.Apostrophe();
        }

        public GreekLetter GetLetter(char e) { return GetLetter(e.ToString()); }
        public GreekLetter GetLetter(string e) {
            var properties = this.GetType().GetProperties();
            foreach (var item in properties) {
                var letter = item.GetValue(this) as GreekLetter;
                var isLetter = letter.IsLetter(e);
                if (isLetter.IsLetter) {
                    var result =  Activator.CreateInstance(item.PropertyType) as GreekLetter;
                    result.IsUpper = isLetter.IsUpper;
                    result.Type = isLetter.Type;
                    result.IsMark = isLetter.IsMark;
                    result.CurrentGreek = e;

                    if (isLetter.Type == LetterType.BreathingRoughAcute ||
                        isLetter.Type == LetterType.BreathingRoughAcuteSubscript ||
                        isLetter.Type == LetterType.BreathingRoughCircumflex ||
                        isLetter.Type == LetterType.BreathingRoughCircumflexSubscript||
                        isLetter.Type == LetterType.BreathingRoughDash||
                        isLetter.Type == LetterType.BreathingRoughDashSubscript||
                        isLetter.Type == LetterType.BreathingRoughGrave||
                        isLetter.Type == LetterType.BreathingRoughGraveSubscript) {
                        result.IsStrongBreath = true;
                    }
                    return result;
                }
            }
            return default;
        }
    }
}
