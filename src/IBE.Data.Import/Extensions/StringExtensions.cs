using System;
using System.Collections.Generic;
using System.Linq;

namespace IBE.Data.Import.Extensions {
    public static  class StringExtensions {
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
    }
}
