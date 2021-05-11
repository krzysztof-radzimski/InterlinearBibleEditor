/*=====================================================================================

	Interlinear Bible Editor
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace IBE.Common.Extensions {
    public static class StringExtensions {
        public static bool IsNotNullOrEmpty(this string text) {
            return !String.IsNullOrEmpty(text);
        }
        public static bool IsNotNullOrWhiteSpace(this string text) {
            return !String.IsNullOrWhiteSpace(text);
        }
        public static bool IsNullOrEmpty(this string text) {
            return String.IsNullOrEmpty(text);
        }
        public static bool IsNullOrWhiteSpace(this string text) {
            return String.IsNullOrWhiteSpace(text);
        }
        public static bool ContainsInTable(this char c, params char[] chars) {
            foreach (var ch in chars) {
                if (ch == c) {
                    return true;
                }
            }

            return false;
        }
        public static bool ContainsInTable(this string text, bool equals, IEnumerable<string> strings) {
            return text.ContainsInTable(true, equals, strings.ToArray());
        }
        public static bool ContainsInTable(this string text, bool ignoreCase, bool equals, IEnumerable<string> strings) {
            return text.ContainsInTable(ignoreCase, equals, strings.ToArray());
        }
        public static bool ContainsInTable(this string text, bool equals, params string[] strings) {
            return text.ContainsInTable(true, equals, strings);
        }
        public static bool ContainsInTable(this string text, bool ignoreCase, bool equals, params string[] strings) {
            if (!String.IsNullOrEmpty(text)) {
                if (strings != null) {
                    foreach (string s in strings) {
                        if (ignoreCase) {
                            if (equals) {
                                if (s.ToLower() == text.ToLower()) {
                                    return true;
                                }
                            }
                            else {
                                if (text.ToLower().Contains(s.ToLower())) {
                                    return true;
                                }
                            }
                        }
                        else {
                            if (equals) {
                                if (s == text) {
                                    return true;
                                }
                            }
                            else {
                                if (text.Contains(s)) {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        public static bool ContainsInTable(this string text, bool ignoreCase, bool equals, params char[] chars) {
            if (!String.IsNullOrEmpty(text)) {
                if (chars != null) {
                    foreach (char c in chars) {
                        if (ignoreCase) {
                            if (equals) {
                                if (c.ToString().ToLower() == text.ToLower()) {
                                    return true;
                                }
                            }
                            else {
                                if (text.ToLower().Contains(c.ToString().ToLower())) {
                                    return true;
                                }
                            }
                        }
                        else {
                            if (equals) {
                                if (c.ToString() == text) {
                                    return true;
                                }
                            }
                            else {
                                if (text.Contains(c.ToString())) {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }
        public static bool ContainsSomethingFromTable(this string text, string[] strings) {
            if (!String.IsNullOrEmpty(text) && strings != null) {
                string s = text.ToLower().Trim();
                var q = from xe in strings
                        where s.Contains(xe.ToLower())
                        select xe;

                return q.Count() > 0;
            }

            return false;
        }
        public static bool HasPolishChars(this string source) {
            return source.ContainsInTable(true, false, "ą", "ę", "ź", "ć", "ż", "ń", "ł", "ó", "ś");
        }
        public static string PadLeft(this string text, string paddingString, int count) {
            string s = String.Empty;

            for (int i = 0; i < count; i++) {
                s += paddingString;
            }

            return s + text;
        }
        public static bool StartWithAny(this string text, params string[] starts) {
            if (text != null && starts != null && starts.Length > 0) {
                foreach (var start in starts) {
                    if (text.StartsWith(start)) {
                        return true;
                    }
                }
            }
            return default;
        }
        public static bool EndsWithAny(this string text, params string[] ends) {
            if (text != null && ends != null && ends.Length > 0) {
                foreach (var end in ends) {
                    if (text.EndsWith(end)) {
                        return true;
                    }
                }
            }
            return default;
        }

        public static string RemoveAny(this string text, params string[] e) {
            if (text.IsNotNull() && e.IsNotNull() && e.Length > 0) {
                foreach (var c in e) {
                    text = text.Replace(c, String.Empty);
                }
            }
            return text;
        }

        public static string RemovePolishChars(this string source) {
            string s = "";

            if (source != null) {
                if (source.HasPolishChars()) {
                    foreach (char c in source) {
                        switch (c) {
                            case 'ą': { s += 'a'; break; }
                            case 'ę': { s += 'e'; break; }
                            case 'ź': { s += 'z'; break; }
                            case 'ć': { s += 'c'; break; }
                            case 'ż': { s += 'z'; break; }
                            case 'ń': { s += 'n'; break; }
                            case 'ł': { s += 'l'; break; }
                            case 'ó': { s += 'o'; break; }
                            case 'ś': { s += 's'; break; }

                            case 'Ą': { s += 'A'; break; }
                            case 'Ę': { s += 'E'; break; }
                            case 'Ź': { s += 'Z'; break; }
                            case 'Ć': { s += 'C'; break; }
                            case 'Ż': { s += 'Z'; break; }
                            case 'Ń': { s += 'N'; break; }
                            case 'Ł': { s += 'L'; break; }
                            case 'Ó': { s += 'O'; break; }
                            case 'Ś': { s += 'S'; break; }

                            default: { s += c; break; }
                        }
                    }
                }
                else {
                    s = source;
                }
            }

            return s;
        }

        public static bool ContainsAllInTable(this string text, string[] strings) {
            if (!String.IsNullOrEmpty(text) && strings != null) {
                string s = text.ToLower().Trim();
                var q = from xe in strings
                        where s.Contains(xe.ToLower())
                        select xe;

                return q.Count() == strings.Length;
            }

            return false;
        }

        public static string GetInitials(this string text) {
            if (text.IsNotNullOrEmpty()) {
                var t = text.Split(' ');
                var result = string.Empty;
                foreach (var item in t) {
                    result += $"{item.Substring(0, 1).ToUpper()}.";
                }
                return result.Trim();
            }
            return default;
        }

        public static string ArabicToRoman(this int arabicNumeral) {

            string romanNumeral = "";

            if ((arabicNumeral / 1000) > 0) {
                for (int i = 1; i <= (arabicNumeral / 1000); i++) {
                    romanNumeral = romanNumeral + 'M';
                    arabicNumeral = arabicNumeral % 1000;
                }
            }

            if ((arabicNumeral / 100) > 0) {
                romanNumeral = romanNumeral + Paste(arabicNumeral / 100, "C", "D", "M");
                arabicNumeral = arabicNumeral % 100;
            }

            if ((arabicNumeral / 10) > 0) {
                romanNumeral = romanNumeral + Paste(arabicNumeral / 10, "X", "L", "C");
                arabicNumeral = arabicNumeral % 10;
            }

            if (arabicNumeral > 0) {
                romanNumeral = romanNumeral + Paste(arabicNumeral, "I", "V", "X");
            }
            return romanNumeral;
        }

        public static string Paste(int num, string one, string five, string ten) {

            string T;
            T = "";

            if (num < 4) { for (int i = 1; i <= num; i++) { T = T + one; } }
            if (num == 4) { T = one + five; }
            if (num == 5) { T = five; };
            if ((num > 5) && (num < 9)) {
                T = five;
                for (int i = 1; i <= num - 5; i++) { T = T + one; }
            }
            if (num == 9) { T = one + ten; }

            return T;
        }
    }

    public static class DateTimeExtensions {
        public static string GetDatePl(this DateTime date) {
            var dateText = $"{date.Day} {date.Month.GenitivePolishMonthName()} {date.Year}";
            
            return dateText;
        }
        static string GenitivePolishMonthName(this int month) {
            switch (month) {
                case 1:
                    return "stycznia";
                case 2:
                    return "lutego";
                case 3:
                    return "marca";
                case 4:
                    return "kwietnia";
                case 5:
                    return "maja";
                case 6:
                    return "czerwca";
                case 7:
                    return "lipca";
                case 8:
                    return "sierpnia";
                case 9:
                    return "września";
                case 10:
                    return "października";
                case 11:
                    return "listopada";
                case 12:
                    return "grudnia";

            }
            return "";
        }
    }
}
