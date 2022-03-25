using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBE.Ukrainian.UI.Controllers {
    public class UkrainianTransliterationController {
        private Dictionary<string, string> Alphabet { get; }
        public UkrainianTransliterationController() {
            Alphabet = new Dictionary<string, string> {
                { "А", "A" },
                { "Б", "B" },
                { "В", "W" },
                { "Г", "H" },
                { "Ґ", "G" },
                { "Д", "D" },
                { "Е", "E" },
                { "Є", "Je" },
                { "Ж", "Ż" },
                { "З", "Z" },
                { "И", "Y" },
                { "І", "I" },
                { "Ї", "Ji" },
                { "Й", "J" },
                { "К", "K" },
                { "Л", "Ł" },
                { "М", "M" },
                { "Н", "N" },
                { "О", "O" },
                { "П", "P" },
                { "Р", "R" },
                { "С", "S" },
                { "Т", "T" },
                { "У", "U" },
                { "Ф", "F" },
                { "Х", "Ch" },
                { "Ц", "C" },
                { "Ч", "Cz" },
                { "Ш", "Sz" },
                { "Щ", "Szcz" },
                { "Ь", "´" },
                { "Ю", "Ju" },
                { "Я", "Ja" },

                { "а", "a" },
                { "б", "b" },
                { "в", "w" },
                { "г", "h" },
                { "ґ", "g" },
                { "д", "d" },
                { "е", "e" },
                { "є", "je" },
                { "ж", "ż" },
                { "з", "z" },
                { "и", "y" },
                { "і", "i" },
                { "ї", "ji" },
                { "й", "j" },
                { "к", "k" },
                { "л", "ł" },
                { "м", "m" },
                { "н", "n" },
                { "о", "o" },
                { "п", "p" },
                { "р", "r" },
                { "с", "s" },
                { "т", "t" },
                { "у", "u" },
                { "ф", "f" },
                { "х", "ch" },
                { "ц", "c" },
                { "ч", "cz" },
                { "ш", "sz" },
                { "щ", "szcz" },
                { "ь", "´" },
                { "ю", "ju" },
                { "я", "ja" }
            };

        }
        public string ToLatin(string text) {
            var latin = string.Empty;

            foreach (var item in text) {
                var s = item.ToString();
                if (Alphabet.ContainsKey(s)) {
                    latin += Alphabet[s];
                }
                else {
                    latin += s;
                }
            }

            return latin;
        }
    }
}
