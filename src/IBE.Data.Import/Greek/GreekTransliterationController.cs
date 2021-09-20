using IBE.Common.Extensions;
using IBE.Data.Import.Greek.Alphabet;
using System;
using System.Collections.Generic;

namespace IBE.Data.Import.Greek {
    public class GreekTransliterationController {
        public AlphabetController Alphabet { get; }
        public GreekTransliterationController() {
            Alphabet = new AlphabetController();
        }

        public string TransliterateSentence(string sentence) {
            var value = string.Empty;
            if (sentence.IsNotNullOrEmpty()) {
                var words = sentence.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in words) {
                    value += TransliterateWord(item) + " ";
                }
            }
            return value.Trim();
        }

        public string TransliterateWord(string word) {
            var value = string.Empty;
            if (word.IsNotNullOrEmpty()) {
                var letters = new List<GreekLetter>();
                foreach (var item in word) {
                    var letter = Alphabet.GetLetter(item);
                    if (letter.IsNotNull()) {
                        letters.Add(letter);
                    }
                    else {
                        throw new Exception($"Letter not found '{item}'");
                    }
                }

                var count = letters.Count;
                for (int i = 0; i < count; i++) {
                    var letter = letters[i];
                    var previous = i > 0 ? letters[i - 1] : null;

                    // Gamma
                    if (previous.IsNotNull() && letter.Small == Alphabet.Gamma.Small && (previous.Small == Alphabet.Gamma.Small || previous.Small == Alphabet.Kappa.Small || previous.Small == Alphabet.Ksi.Small)) {
                        letters[i - 1] = new Ni() { IsUpper = previous.IsUpper, CurrentGreek = previous.CurrentGreek, IsStrongBreath = previous.IsStrongBreath };
                    }

                    // Zeta
                    if (previous.IsNotNull() && letter.Small == Alphabet.Zeta.Small) {
                        letter.Transliteration = letter.AdditionalRoman;
                    }

                    // Sigma
                    if (previous.IsNotNull() && letter.Small == Alphabet.Sigma.Small && (previous.Small == Alphabet.Beta.Small || previous.Small == Alphabet.Gamma.Small || previous.Small == Alphabet.Delta.Small || previous.Small == Alphabet.Mi.Small)) {
                        letter.Transliteration = letter.AdditionalRoman;
                    }

                    // Dyftongi
                    // αι 
                    if (previous.IsNotNull() && letter.Small == Alphabet.Jota.Small && previous.Small == Alphabet.Alfa.Small) {
                        letter.Transliteration = letter.AdditionalRoman;
                        if (letter.IsStrongBreath) { previous.Transliteration = (previous.IsUpper ? "H" : "h") + previous.Transliteration.ToLower(); }
                    }
                    // ει 
                    if (previous.IsNotNull() && letter.Small == Alphabet.Jota.Small && previous.Small == Alphabet.Epsilon.Small) {
                        letter.Transliteration = letter.AdditionalRoman;
                        if (letter.IsStrongBreath) { previous.Transliteration = (previous.IsUpper ? "H" : "h") + previous.Transliteration.ToLower(); }
                    }
                    // οι 
                    if (previous.IsNotNull() && letter.Small == Alphabet.Jota.Small && previous.Small == Alphabet.Omikron.Small) {
                        letter.Transliteration = letter.AdditionalRoman;
                        if (letter.IsStrongBreath) { previous.Transliteration = (previous.IsUpper ? "H" : "h") + previous.Transliteration.ToLower(); }
                    }
                    // υι 
                    if (previous.IsNotNull() && letter.Small == Alphabet.Jota.Small && previous.Small == Alphabet.Ypsilon.Small) {
                        letter.Transliteration = letter.AdditionalRoman;
                        previous.Transliteration = previous.AdditionalRoman;
                        if (letter.IsStrongBreath) { previous.Transliteration = (previous.IsUpper ? "H" : "h") + previous.Transliteration.ToLower(); }
                    }
                    // ου 
                    if (previous.IsNotNull() && letter.Small == Alphabet.Ypsilon.Small && previous.Small == Alphabet.Omikron.Small) {
                        previous.Transliteration = string.Empty;
                        if (letter.IsStrongBreath) { previous.Transliteration = (previous.IsUpper ? "H" : "h") + previous.Transliteration.ToLower(); }
                    }

                    // Przydech samogłosek na początku wyrazu
                    if (previous.IsNull() && letter.IsStrongBreath && letter.Small != Alphabet.Ro.Small) {
                        letter.Transliteration = (letter.IsUpper ? "H" : "h") + letter.Transliteration.ToLower();
                    }
                }

                foreach (var item in letters) {
                    if (item.IsUpper) {
                        if (item.Transliteration.Length == 1) {
                            value += item.Transliteration.ToUpper();
                        }
                        else if (item.Transliteration.Length > 1) {
                            var t = item.Transliteration.Substring(0, 1).ToUpper() + item.Transliteration.Substring(1);
                            value += t;
                        }
                    }
                    else { value += item.Transliteration; }
                }
            }
            return value;
        }
    }
}
