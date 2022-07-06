using ChurchServices.Extensions;
using ChurchServices.Data.Import.Greek.Alphabet;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChurchServices.Data.Import.Greek {
    public class GreekTransliterationController {
        const string STRONG_BREATH_UPPER = "H";
        const string STRONG_BREATH_LOWER = "h";

        public AlphabetController Alphabet { get; }
        public GreekTransliterationController() {
            Alphabet = new AlphabetController();
        }

        public string GetSourceWordWithoutBreathAndAccent(string sourceWord, out bool isUpper) {
            isUpper = false;
            var isFirst = true;
            var letters = new List<GreekLetter>();
            foreach (var item in sourceWord) {
                var letter = Alphabet.GetLetter(item);
                if (letter.IsNotNull()) {
                    if (letter.IsMark) { continue; }
                    if (isFirst && letter.IsUpper) { isUpper = true; }
                    letters.Add(letter);
                }
                isFirst = false;
            }

            var result = string.Empty;
            foreach (var item in letters) {
                result += item.Small;
            }
            return result;
        }

        public TransliterateSentence GetTransliterateSentence(Model.VerseIndex index, DevExpress.Xpo.UnitOfWork uow) {
            if (index.IsNotNull()) {
                var verse = new DevExpress.Xpo.XPQuery<Model.Verse>(uow).Where(x => x.Index == index.Index).FirstOrDefault();
                if (verse.IsNotNull()) {
                    var value = string.Empty;
                    var svalue = string.Empty;
                    foreach (var item in verse.VerseWords) {
                        svalue += item.SourceWord + " ";
                        value += TransliterateWord(item.SourceWord) + " ";
                    }
                    return new TransliterateSentence() { SourceSentence = svalue.Trim(), TransliteritSentence = value.Trim(), Index = index };
                }
            }
            return default;
        }
        public string TransliterateSentenceByIndex(Model.VerseIndex index, DevExpress.Xpo.UnitOfWork uow) {
            if (index.IsNotNull()) {
                var verse = new DevExpress.Xpo.XPQuery<Model.Verse>(uow).Where(x => x.Index == index.Index).FirstOrDefault();
                if (verse.IsNotNull()) {
                    var value = string.Empty;
                    foreach (var item in verse.VerseWords) {
                        value += TransliterateWord(item.SourceWord) + " ";
                    }
                    return value.Trim();
                }
            }
            return default;
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

        public void TransliterateWord(Model.VerseWord word) {
            if (word.IsNotNull()) {
                word.Transliteration = TransliterateWord(word.SourceWord);
                word.Save();
            }
        }
        public string TransliterateWord(string word) {
            var value = string.Empty;
            if (word.IsNotNullOrEmpty()) {

                // God's name
                if (word == "יהוה") { return "JHWH"; }

                var letters = new List<GreekLetter>();
                foreach (var item in word) {
                    var letter = Alphabet.GetLetter(item);
                    if (letter.IsNotNull()) {
                        letters.Add(letter);
                    }
                    else {
                        // unknown characters       
                        letters.Add(new Alphabet.Marks.Other() { CurrentGreek = item.ToString(), Transliteration = item.ToString() });
                    }
                }

                var count = letters.Count;
                for (int i = 0; i < count; i++) {
                    var letter = letters[i];
                    var previous = i > 0 ? letters[i - 1] : null;
                    var next = count > i + 1 ? letters[i + 1] : null;

                    if (previous.IsNull()) { letter.IsFirst = true; }

                    // Gamma
                    if (previous.IsNotNull() && next.IsNotNull() && letter.Small == Alphabet.Gamma.Small &&
                       (next.Small == Alphabet.Gamma.Small || next.Small == Alphabet.Kappa.Small || next.Small == Alphabet.Chi.Small || next.Small == Alphabet.Ksi.Small)) {
                        letters[i] = new Ni() {
                            IsUpper = letter.IsUpper,
                            IsStrongBreath = letter.IsStrongBreath
                        };
                        //letters[i - 1] = new Ni() { IsUpper = previous.IsUpper, CurrentGreek = previous.CurrentGreek, IsStrongBreath = previous.IsStrongBreath };
                    }

                    // Zeta
                    if (previous.IsNotNull() && i < count - 1 && letter.Small == Alphabet.Zeta.Small) {
                        letter.Transliteration = letter.AdditionalRoman;
                    }

                    // Sigma
                    if (next.IsNotNull() && letter.Small == Alphabet.Sigma.Small &&
                       (next.Small == Alphabet.Beta.Small || next.Small == Alphabet.Gamma.Small || next.Small == Alphabet.Delta.Small || (previous.IsNotNull() && previous.Small == Alphabet.Alfa.Small && next.Small == Alphabet.Mi.Small))) {
                        letter.Transliteration = letter.AdditionalRoman;
                    }

                    // Dyftongi
                    // αι 
                    if (previous.IsNotNull() && letter.Small == Alphabet.Jota.Small && previous.Small == Alphabet.Alfa.Small) {
                        letter.Transliteration = letter.AdditionalRoman;
                        if (letter.IsStrongBreath && previous.IsFirst) { previous.Transliteration = (previous.IsUpper ? STRONG_BREATH_UPPER : STRONG_BREATH_LOWER) + previous.Transliteration.ToLower(); }
                    }
                    // αὐ
                    if (previous.IsNotNull() && letter.Small == Alphabet.Ypsilon.Small && previous.Small == Alphabet.Alfa.Small) {
                        letter.Transliteration = letter.AdditionalRoman;
                        if (letter.IsStrongBreath && previous.IsFirst) { previous.Transliteration = (previous.IsUpper ? STRONG_BREATH_UPPER : STRONG_BREATH_LOWER) + previous.Transliteration.ToLower(); }
                    }
                    // ηὐ
                    if (previous.IsNotNull() && letter.Small == Alphabet.Ypsilon.Small && previous.Small == Alphabet.Eta.Small) {
                        letter.Transliteration = letter.AdditionalRoman;
                        if (letter.IsStrongBreath && previous.IsFirst) { previous.Transliteration = (previous.IsUpper ? STRONG_BREATH_UPPER : STRONG_BREATH_LOWER) + previous.Transliteration.ToLower(); }
                    }
                    // ει 
                    if (previous.IsNotNull() && letter.Small == Alphabet.Jota.Small && previous.Small == Alphabet.Epsilon.Small) {
                        letter.Transliteration = letter.AdditionalRoman;
                        if (letter.IsStrongBreath && previous.IsFirst) { previous.Transliteration = (previous.IsUpper ? STRONG_BREATH_UPPER : STRONG_BREATH_LOWER) + previous.Transliteration.ToLower(); }
                    }
                    // εῦ
                    if (previous.IsNotNull() && letter.Small == Alphabet.Ypsilon.Small && previous.Small == Alphabet.Epsilon.Small) {
                        letter.Transliteration = letter.AdditionalRoman;
                        if (letter.IsStrongBreath && previous.IsFirst) { previous.Transliteration = (previous.IsUpper ? STRONG_BREATH_UPPER : STRONG_BREATH_LOWER) + previous.Transliteration.ToLower(); }
                    }
                    // οι 
                    if (previous.IsNotNull() && letter.Small == Alphabet.Jota.Small && previous.Small == Alphabet.Omikron.Small) {
                        letter.Transliteration = letter.AdditionalRoman;
                        if (letter.IsStrongBreath && previous.IsFirst) { previous.Transliteration = (previous.IsUpper ? STRONG_BREATH_UPPER : STRONG_BREATH_LOWER) + previous.Transliteration.ToLower(); }
                    }
                    // υι 
                    if (previous.IsNotNull() && letter.Small == Alphabet.Jota.Small && previous.Small == Alphabet.Ypsilon.Small) {
                        letter.Transliteration = letter.AdditionalRoman;
                        if (letter.IsStrongBreath && previous.IsFirst) { previous.Transliteration = (previous.IsUpper ? STRONG_BREATH_UPPER : STRONG_BREATH_LOWER) + previous.Transliteration.ToLower(); }
                    }
                    // ου 
                    if (previous.IsNotNull() && letter.Small == Alphabet.Ypsilon.Small && previous.Small == Alphabet.Omikron.Small) {
                        previous.Transliteration = string.Empty;
                        letter.Transliteration = letter.AdditionalRoman;
                        if (previous.IsUpper) {
                            letter.IsUpper = previous.IsUpper;
                        }
                        if (letter.IsStrongBreath && previous.IsFirst) { previous.Transliteration = (previous.IsUpper ? STRONG_BREATH_UPPER : STRONG_BREATH_LOWER) + previous.Transliteration.ToLower(); }
                    }
                    // ύο
                    if (previous.IsNotNull() && letter.Small == Alphabet.Omikron.Small && previous.Small == Alphabet.Ypsilon.Small) {
                        previous.Transliteration = previous.AdditionalRoman;
                        if (letter.IsStrongBreath && previous.IsFirst) { previous.Transliteration = (previous.IsUpper ? STRONG_BREATH_UPPER : STRONG_BREATH_LOWER) + previous.Transliteration.ToLower(); }
                    }
                }

                foreach (var item in letters) {
                    var strongBreath = string.Empty;

                    // Przydech samogłosek na początku wyrazu
                    if (item.IsFirst && item.IsStrongBreath && item.Small != Alphabet.Ro.Small &&
                        !item.Transliteration.StartsWith(STRONG_BREATH_UPPER) && !item.Transliteration.StartsWith(STRONG_BREATH_LOWER)) {
                        strongBreath = item.IsUpper ? STRONG_BREATH_UPPER : STRONG_BREATH_LOWER;
                    }

                    if (item.IsUpper) {
                        if (strongBreath.IsNotNullOrEmpty()) {
                            value += strongBreath + item.Transliteration.ToLower();
                        }
                        else if (item.Transliteration.Length == 1) {
                            value += strongBreath + item.Transliteration.ToUpper();
                        }
                        else if (item.Transliteration.Length > 1) {
                            var t = strongBreath + item.Transliteration.Substring(0, 1).ToUpper() + item.Transliteration.Substring(1);
                            value += t;
                        }
                    }
                    else { value += strongBreath + item.Transliteration; }
                }
            }
            return value;
        }
    }

    public class TransliterateSentence {
        public Model.VerseIndex Index { get; internal set; }
        public string SourceSentence { get; internal set; }
        public string TransliteritSentence { get; internal set; }
    }
}
