namespace ChurchServices.Extensions {
    public abstract class UnicodeHebrewLetters {
        // Aleph to Tav (Alef to Tav)
        public const char Aleph = '\u05D0';
        public const char Bet = '\u05D1';
        public const char Gimel = '\u05D2';
        public const char Dalet = '\u05D3';
        public const char He = '\u05D4';
        public const char Vav = '\u05D5';
        public const char Zayin = '\u05D6';
        public const char Het = '\u05D7';
        public const char Tet = '\u05D8';
        public const char Yod = '\u05D9';
        public const char FinalKaf = '\u05DA';
        public const char Kaf = '\u05DB';
        public const char Lamed = '\u05DC';
        public const char FinalMem = '\u05DD';
        public const char Mem = '\u05DE';
        public const char FinalNun = '\u05DF';
        public const char Nun = '\u05E0';
        public const char Samekh = '\u05E1';
        public const char Ayin = '\u05E2';
        public const char FinalPe = '\u05E3';
        public const char Pe = '\u05E4';
        public const char FinalTsadi = '\u05E5';
        public const char Tsadi = '\u05E6';
        public const char Qof = '\u05E7';
        public const char Resh = '\u05E8';
        public const char Shin = '\u05E9';
        public const char Tav = '\u05EA';

        // Additional letters
        public const char SofPasuk = '\u05C2';
        public const char Geresh = '\u05F3';
        public const char Gershayim = '\u05F4';

        // Vowel Points
        public const char Sheva = '\u05B0';
        public const char HatafSegol = '\u05B1';
        public const char HatafPatah = '\u05B2';
        public const char HatafKamatz = '\u05B3';
        public const char Hiriq = '\u05B4';
        public const char Tsere = '\u05B5';
        public const char Segol = '\u05B6';
        public const char Patah = '\u05B7';
        public const char Kamatz = '\u05B8';
        public const char Holam = '\u05B9';
        public const char HolamHaser = '\u05BA';
        public const char Qubuts = '\u05BB';
        public const char Dagesh = '\u05BC';

        // Punctuation Marks
        public const char Maqaf = '\u05BE';
        public const char Paseq = '\u05C0';
        public const char Rafe = '\u05BF';
        public const char ShinDot = '\u05C1';
        public const char SinDot = '\u05C2';
    }

    public abstract class HtmlHebrewLetters {
        // Aleph to Tav (Alef to Tav)
        public const string Aleph = "&#1488;";
        public const string Bet = "&#1489;";
        public const string Gimel = "&#1490;";
        public const string Dalet = "&#1491;";
        public const string He = "&#1492;";
        public const string Vav = "&#1493;";
        public const string Zayin = "&#1494;";
        public const string Het = "&#1495;";
        public const string Tet = "&#1496;";
        public const string Yod = "&#1497;";
        public const string FinalKaf = "&#1498;";
        public const string Kaf = "&#1499;";
        public const string Lamed = "&#1500;";
        public const string FinalMem = "&#1501;";
        public const string Mem = "&#1502;";
        public const string FinalNun = "&#1503;";
        public const string Nun = "&#1504;";
        public const string Samekh = "&#1505;";
        public const string Ayin = "&#1506;";
        public const string FinalPe = "&#1507;";
        public const string Pe = "&#1508;";
        public const string FinalTsadi = "&#1509;";
        public const string Tsadi = "&#1510;";
        public const string Qof = "&#1511;";
        public const string Resh = "&#1512;";
        public const string Shin = "&#1513;";
        public const string Tav = "&#1514;";

        // Additional letters
        public const string SofPasuk = "&#1466;";
        public const string Geresh = "&#1467;";
        public const string Gershayim = "&#1468;";

        // Vowel Points
        public const string Sheva = "&#1456;";
        public const string HatafSegol = "&#1457;";
        public const string HatafPatah = "&#1458;";
        public const string HatafKamatz = "&#1459;";
        public const string Hiriq = "&#1460;";
        public const string Tsere = "&#1461;";
        public const string Segol = "&#1462;";
        public const string Patah = "&#1463;";
        public const string Kamatz = "&#1464;";
        public const string Holam = "&#1465;";
        public const string HolamHaser = "&#1469;";
        public const string Qubuts = "&#1470;";
        public const string Dagesh = "&#1462;";

        // Punctuation Marks
        public const string Maqaf = "&#1470;";
        public const string Paseq = "&#1472;";
        public const string Rafe = "&#1471;";
        public const string ShinDot = "&#1473;";
        public const string SinDot = "&#1474;";
    }

    public abstract class HebrewLetters {
        // Aleph to Tav (Alef to Tav)
        public const char Aleph = 'א';
        public const char Bet = 'ב';
        public const char Gimel = 'ג';
        public const char Dalet = 'ד';
        public const char He = 'ה';
        public const char Vav = 'ו';
        public const char Zayin = 'ז';
        public const char Het = 'ח';
        public const char Tet = 'ט';
        public const char Yod = 'י';
        public const char FinalKaf = 'ך';
        public const char Kaf = 'כ';
        public const char Lamed = 'ל';
        public const char FinalMem = 'ם';
        public const char Mem = 'מ';
        public const char FinalNun = 'ן';
        public const char Nun = 'נ';
        public const char Samekh = 'ס';
        public const char Ayin = 'ע';
        public const char FinalPe = 'ף';
        public const char Pe = 'פ';
        public const char FinalTsadi = 'ץ';
        public const char Tsadi = 'צ';
        public const char Qof = 'ק';
        public const char Resh = 'ר';
        public const char Shin = 'ש';
        public const char Tav = 'ת';
    }

    public static class HebrewAlphabetExtensions {
        public static char HebrewToUnicode(char hebrewChar) {
            if (hebrewChar >= HebrewLetters.Aleph && hebrewChar <= HebrewLetters.Tav) {
                int unicodeOffset = hebrewChar - HebrewLetters.Aleph;
                char unicodeChar = (char)(UnicodeHebrewLetters.Aleph + unicodeOffset);
                return unicodeChar;
            }
            else {
                // Return the input character if it is not a Hebrew letter
                return hebrewChar;
            }
        }
    }
}
