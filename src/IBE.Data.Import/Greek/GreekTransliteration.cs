using IBE.Common.Extensions;
using System;
using Unidecode.NET;

namespace IBE.Data.Import.Greek {
    public static class GreekTransliteration {
        private static readonly string[] LOWERS = new string[] {
            "ἁ","ἱ","ὑ","ἑ","ὁ","ἡ","ὡ",
            "ἃ","ἳ","ὓ","ἓ","ὃ","ἣ","ὣ",
            "ἅ","ἵ","ὕ","ἕ","ὅ","ἥ","ὥ",
            "εἁ","εἱ","εὑ","ιἑ","εὁ","εὡ",
            "υἁ","υἱ","υἑ","υἡ",
            "ἧ","οὗ", "οἱ",
            "ᾇ","ᾧ","ᾗ"
            };
        private static readonly string[] UPPERS = new string[] {
            "Ἁ","Ἱ","Ὑ","Ἑ","Ὁ","Ἡ","Ὡ",
            "Ἃ","Ἳ","Ὓ","Ἓ","Ὃ","Ἣ","Ὣ",
            "Ἅ","Ἵ","Ὕ","Ἕ","Ὅ","Ἥ","Ὥ",
            "Εἁ","Εἱ","Εὑ","Ιἑ","Εὁ","Εὡ",
            "Υἁ","Υἱ","Υἑ","Υἡ",
            "Ἧ","Οὗ", "Οἱ",
            "ᾏ","ᾯ","ᾟ"
            };
        public static string TransliterateAncientGreek(this string greekText) {
            if (greekText != null) {
                var prepared = PrepareString(greekText);
                var transliterit = prepared.Unidecode();
                transliterit = transliterit.FixChar_U().FixChar_OU().FixChar_KH().FixChar_PH().FixChar_X();
                return transliterit.Trim();
            }
            return default;
        }

        private static string PrepareString(string greekText) {
            var prepared = String.Empty;
            var table = greekText.Split(' ');
            foreach (var item in table) {
                if (item.StartWithAny(LOWERS)) {
                    prepared += $"h{item} ";
                }
                else if (item.StartWithAny(UPPERS)) {
                    prepared += $"H{item.ToLower()} ";
                }
                else {
                    prepared += $"{item} ";
                }
            }

            return prepared;
        }

        private static string FixChar_OU(this string text) {
            return text.Replace("ou", "u").Replace("Ou", "u");
        }
        private static string FixChar_KH(this string text) {
            return text.Replace("kh", "ch");
        }
        private static string FixChar_PH(this string text) {
            return text.Replace("ph", "f");
        }
        private static string FixChar_X(this string text) {
            return text.Replace("x", "ks");
        }
        private static string FixChar_U(this string text) {
            string[] table = new string[] { "w", "r", "t", "p", "s", "d", "f", "g", "h", "j", "k", "l", "z", "x", "c", "v", "b", "n", "m" };
            string[] tableUpper = new string[] { "W", "R", "T", "P", "S", "D", "F", "G", "H", "J", "K", "L", "Z", "X", "C", "V", "B", "N", "M" };
            foreach (var item in table) {
                text = text.Replace($"{item}u", $"{item}y");
            }
            foreach (var item in tableUpper) {
                text = text.Replace($"{item}u", $"{item}y");
            }
            return text;
        }        
    }
}
