/*=====================================================================================
	Author: (C)2009 - 2020 ITORG Krzysztof Radzimski
	http://itorg.pl
    License: MIT

    https://ubsicap.github.io/usfm/usfm2.5/identification/books.html
  ===================================================================================*/

using IBE.Identifiers.Attributes;

namespace IBE.Identifiers {
    public enum BookIdentifiers {
        [BookName("Genesis", "Gen")] [BookName("Rodzaju", "Rdz", Language = Language.pl)] [BookType("01", BookType = BookType.Book)] OT_GEN,
        [BookName("Exodus", "Exo")] [BookName("Wyjścia", "Wj", Language = Language.pl)] [BookType("02", BookType = BookType.Book)] OT_EXO,
        [BookName("Leviticus", "Lev")] [BookName("Kapłańska", "Kpł", Language = Language.pl)] [BookType("03", BookType = BookType.Book)] OT_LEV,
        [BookName("Numbers", "Num")] [BookName("Liczb", "Lb", Language = Language.pl)] [BookType("04", BookType = BookType.Book)] OT_NUM,
        [BookName("Deuteronomy", "Deu")] [BookName("Powtórzonego Prawa", "Pwt", Language = Language.pl)] [BookType("05", BookType = BookType.Book)] OT_DEU,

        [BookName("Joshua", "Jos")] [BookName("Jozuego", "Joz", Language = Language.pl)] [BookType("06", BookType = BookType.Book)] OT_JOS,
        [BookName("Judges", "Jdg")] [BookName("Sędziów", "Sdz", Language = Language.pl)] [BookType("07", BookType = BookType.Book)] OT_JDG,
        [BookName("Ruth", "Rut")] [BookName("Rut", "Rt", Language = Language.pl)] [BookType("08", BookType = BookType.Book)] OT_RUT,

        [BookName("1 Samuel", "1Sa")] [BookName("1 Samuela", "1Sm", Language = Language.pl)] [BookType("09", BookType = BookType.Book)] OT_1SA,
        [BookName("2 Samuel", "2Sa")] [BookName("2 Samuela", "2Sm", Language = Language.pl)] [BookType("10", BookType = BookType.Book)] OT_2SA,

        [BookName("1 Kings", "1Ki")] [BookName("1 Królewska", "1Krl", Language = Language.pl)] [BookType("11", BookType = BookType.Book)] OT_1KI,
        [BookName("2 Kings", "2Ki")] [BookName("2 Królewska", "2Krl", Language = Language.pl)] [BookType("12", BookType = BookType.Book)] OT_2KI,

        [BookName("1 Chronicles", "1Ch")] [BookName("1 Kronik", "1Krn", Language = Language.pl)] [BookType("13", BookType = BookType.Book)] OT_1CH,
        [BookName("2 Chronicles", "2Ch")] [BookName("2 Kronik", "2Krn", Language = Language.pl)] [BookType("14", BookType = BookType.Book)] OT_2CH,

        [BookName("Ezra", "Ezr")] [BookName("Ezdrasza", "Ezd", Language = Language.pl)] [BookType("15", BookType = BookType.Book)] OT_EZR,
        [BookName("Nehemiah", "Neh")] [BookName("Nehemiasza", "Ne", Language = Language.pl)] [BookType("16", BookType = BookType.Book)] OT_NEH,
        [BookName("Esther", "Est")] [BookName("Estery", "Est", Language = Language.pl)] [BookType("17", BookType = BookType.Book)] OT_EST,
        [BookName("Job", "Job")] [BookName("Joba", "Jb", Language = Language.pl)] [BookType("18", BookType = BookType.Book)] OT_JOB,

        [BookName("Psalms", "Psa")] [BookName("Psalmy", "Ps", Language = Language.pl)] [BookType("19")] OT_PSA,

        [BookName("Proverbs", "Pro")] [BookName("Przysłów", "Prz", Language = Language.pl)] [BookType("20", BookType = BookType.Book)] OT_PRO,
        [BookName("Ecclesiastes", "Ecc")] [BookName("Kaznodziei", "Kzn", Language = Language.pl)] [BookType("21", BookType = BookType.Book)] OT_ECC,
        [BookName("Song of Songs", "Sng")] [BookName("Pieśń nad pieśniami", "Pnp", Language = Language.pl)] [BookType("22")] OT_SNG,

        [BookName("Isaiah", "Isa")] [BookName("Izajasza", "Iz", Language = Language.pl)] [BookType("23", BookType = BookType.Book)] OT_ISA,
        [BookName("Jeremiah", "Jer")] [BookName("Jeremiasza", "Jr", Language = Language.pl)] [BookType("24", BookType = BookType.Book)] OT_JER,
        [BookName("Lamentations", "Lam")] [BookName("Treny", "Tr", Language = Language.pl)] [BookType("25")] OT_LAM,
        [BookName("Ezekiel", "Ezk")] [BookName("Ezechiela", "Ez", Language = Language.pl)] [BookType("26", BookType = BookType.Book)] OT_EZK,
        [BookName("Daniel", "Dan")] [BookName("Daniela", "Dn", Language = Language.pl)] [BookType("27", BookType = BookType.Book)] OT_DAN,
        [BookName("Hosea", "Hos")] [BookName("Ozeasza", "Oz", Language = Language.pl)] [BookType("28", BookType = BookType.Book)] OT_HOS,
        [BookName("Joel", "Jol")] [BookName("Joela", "Jl", Language = Language.pl)] [BookType("29", BookType = BookType.Book)] OT_JOL,
        [BookName("Amos", "Amo")] [BookName("Amosa", "Am", Language = Language.pl)] [BookType("30", BookType = BookType.Book)] OT_AMO,
        [BookName("Obadiah", "Oba")] [BookName("Abdiasza", "Ab", Language = Language.pl)] [BookType("31", BookType = BookType.Book)] OT_OBA,
        [BookName("Jonah", "Jon")] [BookName("Jonasza", "Jo", Language = Language.pl)] [BookType("32", BookType = BookType.Book)] OT_JON,
        [BookName("Micah", "Mic")] [BookName("Micheasza", "Mi", Language = Language.pl)] [BookType("33", BookType = BookType.Book)] OT_MIC,
        [BookName("Nahum", "Nam")] [BookName("Nahuma", "Na", Language = Language.pl)] [BookType("34", BookType = BookType.Book)] OT_NAM,
        [BookName("Habakkuk", "Hab")] [BookName("Habakuka", "Ha", Language = Language.pl)] [BookType("35", BookType = BookType.Book)] OT_HAB,
        [BookName("Zephaniah", "Zep")] [BookName("Sofoniasza", "So", Language = Language.pl)] [BookType("36", BookType = BookType.Book)] OT_ZEP,
        [BookName("Haggai", "Hag")] [BookName("Aggeusza", "Ag", Language = Language.pl)] [BookType("37", BookType = BookType.Book)] OT_HAG,
        [BookName("Zechariah", "Zec")] [BookName("Zachariasza", "Za", Language = Language.pl)] [BookType("38", BookType = BookType.Book)] OT_ZEC,
        [BookName("Malachi", "Mal")] [BookName("Malachiasza", "Ma", Language = Language.pl)] [BookType("39", BookType = BookType.Book)] OT_MAL,

        [BookName("Matthew", "Mat")] [BookName("Mateusza", "Ma", Language = Language.pl)] [BookType("41", BookType = BookType.Gospel)] NT_MAT,
        [BookName("Mark", "Mrk")] [BookName("Marka", "Mk", Language = Language.pl)] [BookType("42", BookType = BookType.Gospel)] NT_MRK,
        [BookName("Luke", "Luk")] [BookName("Łukasza", "Łk", Language = Language.pl)] [BookType("43", BookType = BookType.Gospel)] NT_LUK,
        [BookName("John", "Jhn")] [BookName("Jana", "J", Language = Language.pl)] [BookType("44", BookType = BookType.Gospel)] NT_JHN,

        [BookName("Acts", "Act")] [BookName("Dzieje Apostolskie", "Dz", Language = Language.pl)] [BookType("45")] NT_ACT,

        [BookName("Romans", "Rom")] [BookName("Rzymian", "Rz", Language = Language.pl)] [BookType("46", BookType = BookType.PaulsLetter, LetterType = LetterType.Addressed)] NT_ROM,
        [BookName("Corinthians", "1Co")] [BookName("Koryntian", "1Kor", Language = Language.pl)] [BookType("47", BookType = BookType.PaulsLetter, BookPart = BookPart.First, LetterType = LetterType.Addressed)] NT_1CO,
        [BookName("Corinthians", "2Co")] [BookName("Koryntian", "2Kor", Language = Language.pl)] [BookType("48", BookType = BookType.PaulsLetter, BookPart = BookPart.Second, LetterType = LetterType.Addressed)] NT_2CO,
        [BookName("Galatians", "Gal")] [BookName("Galacjan", "Ga", Language = Language.pl)] [BookType("49", BookType = BookType.PaulsLetter, LetterType = LetterType.Addressed)] NT_GAL,
        [BookName("Ephesians", "Eph")] [BookName("Efezjan", "Ef", Language = Language.pl)] [BookType("50", BookType = BookType.PaulsLetter, LetterType = LetterType.Addressed)] NT_EPH,
        [BookName("Philippians", "Php")] [BookName("Filipian", "Flp", Language = Language.pl)] [BookType("51", BookType = BookType.PaulsLetter, LetterType = LetterType.Addressed)] NT_PHP,
        [BookName("Colossians", "Col")] [BookName("Kolosan", "Kol", Language = Language.pl)] [BookType("52", BookType = BookType.PaulsLetter, LetterType = LetterType.Addressed)] NT_COL,
        [BookName("1 Thessalonians", "1Th")] [BookName("Tesaloniczan", "1Ts", Language = Language.pl)] [BookType("53", BookType = BookType.PaulsLetter, LetterType = LetterType.Addressed, BookPart = BookPart.First)] NT_1TH,
        [BookName("2 Thessalonians", "2Th")] [BookName("Tesaloniczan", "2Ts", Language = Language.pl)] [BookType("54", BookType = BookType.PaulsLetter, LetterType = LetterType.Addressed, BookPart = BookPart.Second)] NT_2TH,
        [BookName("1 Timothy", "1Ti")] [BookName("Tymoteusza", "1Tm", Language = Language.pl)] [BookType("55", BookType = BookType.PaulsLetter, LetterType = LetterType.Addressed, BookPart = BookPart.First)] NT_1TI,
        [BookName("2 Timothy", "2Ti")] [BookName("Tymoteusza", "2Tm", Language = Language.pl)] [BookType("56", BookType = BookType.PaulsLetter, LetterType = LetterType.Addressed, BookPart = BookPart.Second)] NT_2TI,
        [BookName("Titus", "Tit")] [BookName("Tytusa", "Tt", Language = Language.pl)] [BookType("57", BookType = BookType.PaulsLetter, LetterType = LetterType.Addressed)] NT_TIT,
        [BookName("Philemon", "Phm")] [BookName("Filemona", "Flm", Language = Language.pl)] [BookType("58", BookType = BookType.PaulsLetter, LetterType = LetterType.Addressed)] NT_PHM,
        [BookName("Hebrews", "Heb")] [BookName("Hebrajczyków", "Hbr", Language = Language.pl)] [BookType("59", BookType = BookType.Letter, LetterType = LetterType.Addressed)] NT_HEB,
        [BookName("James", "Jas")] [BookName("Jakuba", "Jk", Language = Language.pl)] [BookType("60", BookType = BookType.JamesLetter, LetterType = LetterType.Ordinary)] NT_JAS,
        [BookName("1 Peter", "1Pe")] [BookName("Piotra", "1P", Language = Language.pl)] [BookType("61", BookType = BookType.PetersLetter, LetterType = LetterType.Ordinary, BookPart = BookPart.First)] NT_1PE,
        [BookName("2 Peter", "2Pe")] [BookName("Piotra", "2P", Language = Language.pl)] [BookType("62", BookType = BookType.PetersLetter, LetterType = LetterType.Ordinary, BookPart = BookPart.Second)] NT_2PE,
        [BookName("1 John", "1Jn")] [BookName("Jana", "1J", Language = Language.pl)] [BookType("63", BookType = BookType.JohnsLetter, LetterType = LetterType.Ordinary, BookPart = BookPart.First)] NT_1JN,
        [BookName("2 John", "2Jn")] [BookName("Jana", "2J", Language = Language.pl)] [BookType("64", BookType = BookType.JohnsLetter, LetterType = LetterType.Ordinary, BookPart = BookPart.Second)] NT_2JN,
        [BookName("3 John", "3Jn")] [BookName("Jana", "3J", Language = Language.pl)] [BookType("65", BookType = BookType.JohnsLetter, LetterType = LetterType.Ordinary, BookPart = BookPart.Third)] NT_3JN,
        [BookName("Jude", "Jud")] [BookName("Judy", "Jd", Language = Language.pl)] [BookType("66", BookType = BookType.JudsLetter, LetterType = LetterType.Ordinary)] NT_JUD,
        [BookName("John", "Rev")] [BookName("Jana", "Obj", Language = Language.pl)] [BookType("67", BookType = BookType.Eschatology)] NT_REV,

        // -----------------------------------------
        // Deuterocanonical books and apocrites
        // -----------------------------------------

        [BookName("Tobit", "Tob")] [BookName("Tobiasza", "Tob", Language = Language.pl)] [BookType("68", Deuterocanonical = DeuterocanonicalType.Deuterocanonical, BookType = BookType.Book)] DC_TOB,
        [BookName("Judith", "Jdt")] [BookName("Judyty", "Jdt", Language = Language.pl)] [BookType("69", Deuterocanonical = DeuterocanonicalType.Deuterocanonical, BookType = BookType.Book)] DC_JDT,
        [BookName("Esther", "Esg")] [BookName("Estery", "Esg", Language = Language.pl)] [BookType("70", Deuterocanonical = DeuterocanonicalType.Apocrypha, BookType = BookType.Book)] DC_ESG,
        [BookName("Wisdom of Solomon", "Wis")] [BookName("Mądrości", "Mdr", Language = Language.pl)] [BookType("71", Deuterocanonical = DeuterocanonicalType.Deuterocanonical, BookType = BookType.Book)] DC_WIS,
        // Ecclesiasticus or Jesus son of Sirach
        [BookName("Sirach", "Sir")] [BookName("Mądrość Syraha", "Syr", Language = Language.pl)] [BookType("72", Deuterocanonical = DeuterocanonicalType.Deuterocanonical)] DC_SIR,
        // 5 chapters in Orthodox Bibles (LJE is separate); 6 chapters in Catholic Bibles (includes LJE); called 1 Baruch in Syriac Bibles
        [BookName("Baruch", "Bar")] [BookName("Barucha", "Bar", Language = Language.pl)] [BookType("73", Deuterocanonical = DeuterocanonicalType.Deuterocanonical, BookType = BookType.Book)] DC_BAR,
        // Sometimes included in Baruch; called ‘Rest of Jeremiah’ in Ethiopia
        [BookName("Jeremiah", "Lje")] [BookName("Jeremiasza", "Lje", Language = Language.pl)] [BookType("74", Deuterocanonical = DeuterocanonicalType.Apocrypha, BookType = BookType.Letter, LetterType = LetterType.Ordinary)] DC_LJE,

        // Daniel appendix
        // Includes the Prayer of Azariah; sometimes included in Greek Daniel
        [BookName("Song of the 3 Young Men", "S3y")] [BookName("Pieśń 3 młodzieńców", "S3y", Language = Language.pl)] [BookType("75", Deuterocanonical = DeuterocanonicalType.Deuterocanonical)] DC_S3Y,
        // Sometimes included in Greek Daniel
        [BookName("Susanna", "Sus")] [BookName("Zuzanna", "Zuz", Language = Language.pl)] [BookType("76", Deuterocanonical = DeuterocanonicalType.Deuterocanonical)] DC_SUS,
        // Sometimes included in Greek Daniel; called ‘Rest of Daniel’ in Ethiopia
        [BookName("Bel and the Dragon", "Bel")] [BookName("Bel i Wąż", "Bar", Language = Language.pl)] [BookType("77", Deuterocanonical = DeuterocanonicalType.Deuterocanonical)] DC_BEL,

        [BookName("Maccabees", "1Ma")] [BookName("Machabejska", "1Ma", Language = Language.pl)] [BookType("78", Deuterocanonical = DeuterocanonicalType.Deuterocanonical, BookType = BookType.Book, BookPart = BookPart.First)] DC_1MA,
        [BookName("Maccabees", "2Ma")] [BookName("Machabejska", "2Ma", Language = Language.pl)] [BookType("79", Deuterocanonical = DeuterocanonicalType.Deuterocanonical, BookType = BookType.Book, BookPart = BookPart.Second)] DC_2MA,
        [BookName("Maccabees", "3Ma")] [BookName("Machabejska", "3Ma", Language = Language.pl)] [BookType("80", Deuterocanonical = DeuterocanonicalType.Apocrypha, BookType = BookType.Book, BookPart = BookPart.Third)] DC_3MA,
        [BookName("Maccabees", "4Ma")] [BookName("Machabejska", "4Ma", Language = Language.pl)] [BookType("81", Deuterocanonical = DeuterocanonicalType.Apocrypha, BookType = BookType.Book, BookPart = BookPart.Fourth)] DC_4MA,

        // The 9 chapter book of Greek Ezra in the LXX, called ‘2 Esdras’ in Russian Bibles, and called ‘3 Esdras’ in the Vulgate; when Ezra - Nehemiah is one book use EZR
        [BookName("Esdras", "1Es")] [BookName("Ezdrasza", "1Es", Language = Language.pl)] [BookType("82", Deuterocanonical = DeuterocanonicalType.Apocrypha, BookType = BookType.Book, BookPart = BookPart.First)] DC_1ES,
        // The 16 chapter book of Latin Esdras called ‘3 Esdras’ in Russian Bibles and called ‘4 Esdras’ in the Vulgate; for the 12 chapter Apocalypse of Ezra use EZA
        [BookName("Esdras", "2Es")] [BookName("Ezdrasza", "2Es", Language = Language.pl)] [BookType("83", Deuterocanonical = DeuterocanonicalType.Apocrypha, BookType = BookType.Book, BookPart = BookPart.Second)] DC_2ES,

        // Sometimes appended to 2 Chronicles, included in Orthodox Bibles
        [BookName("Prayer of Manasseh", "Man")] [BookName("Modlitwa Manassesa", "Man", Language = Language.pl)] [BookType("84", Deuterocanonical = DeuterocanonicalType.Apocrypha)] DC_MAN,
        // An additional Psalm in the Septuagint, appended to Psalms in Orthodox Bibles
        [BookName("Psalm", "Ps2")] [BookName("Psalmy", "Ps2", Language = Language.pl)] [BookType("85", Deuterocanonical = DeuterocanonicalType.Apocrypha)] DC_PS2,
        // A book in some editions of the Septuagint; Odes has different contents in Greek, Russian, and Syriac traditions
        [BookName("Odae/Odes", "Oda")] [BookName("Ody", "Oda", Language = Language.pl)] [BookType("86", Deuterocanonical = DeuterocanonicalType.Apocrypha)] DC_ODA,
        // A book in some editions of the Septuagint, but not printed in modern Bibles
        [BookName("Psalms of Solomon", "Pss")] [BookName("Psalmy Salomona", "Pss", Language = Language.pl)] [BookType("87", Deuterocanonical = DeuterocanonicalType.Apocrypha)] DC_PSS,

        // 12 chapter book of Ezra Apocalypse; called ‘3 Ezra’ in the Armenian Bible, called ‘Ezra Shealtiel’ in the Ethiopian Bible; formerly called 4ES; called ‘2 Esdras’ when it includes 5 Ezra and 6 Ezra
        [BookName("Ezra Apocalypse", "Eza")] [BookName("Apokalipsa Ezdrasza", "Eza", Language = Language.pl)] [BookType("A4", Deuterocanonical = DeuterocanonicalType.Apocrypha)] DC_EZA,
        //2 chapter Latin preface to Ezra Apocalypse; formerly called 5ES
        [BookName("Esdras", "5Ez")] [BookName("5 Ezdrasza", "5Ez")] [BookType("A5", Deuterocanonical = DeuterocanonicalType.Apocrypha, BookType = BookType.Book, BookPart = BookPart.Fifth)] DC_5EZ,
        //2 chapter Latin conclusion to Ezra Apocalypse; formerly called 6ES
        [BookName("Esdras", "6Ez")] [BookType("A6", Deuterocanonical = DeuterocanonicalType.Apocrypha, BookType = BookType.Book, BookPart = BookPart.Sixth)] DC_6EZ,

        // The 14 chapter version of Daniel from the Septuagint including Greek additions
        [BookName("Daniel", "Dag")] [BookName("Daniela", "Dag")] [BookType("B2", Deuterocanonical = DeuterocanonicalType.Apocrypha)] DC_DAG,
        // Additional Psalms 152-155 found in West Syriac manuscripts
        [BookName("Psalms", "Ps3")] [BookName("Psalmy", "Ps3", Language = Language.pl)] [BookType("B3", Deuterocanonical = DeuterocanonicalType.Apocrypha, BookType = BookType.Book)] DC_PS3,
        // The Apocalypse of Baruch in Syriac Bibles
        [BookName("Baruch", "2Ba")] [BookName("Barucha", "2Ba")] [BookType("B4", Deuterocanonical = DeuterocanonicalType.Apocrypha, BookType = BookType.Book, BookPart = BookPart.Second)] DC_2BA,
        // Sometimes appended to 2 Baruch; sometimes separate in Syriac Bibles
        [BookName("Letter of Baruch", "Lba")] [BookType("B5", Deuterocanonical = DeuterocanonicalType.Apocrypha)] DC_LBA,
        // Ancient Hebrew book used in the Ethiopian Bible
        [BookName("Jubilees", "Jub")] [BookName("Jubileuszów", "Jub", Language = Language.pl)] [BookType("B6", Deuterocanonical = DeuterocanonicalType.Apocrypha, BookType = BookType.Book)] DC_JUB,
        // Sometimes called ‘1 Enoch’; ancient Hebrew book in the Ethiopian Bible
        [BookName("Enoch", "Eno")] [BookName("Henoha", "Hen", Language = Language.pl)] [BookType("B7", Deuterocanonical = DeuterocanonicalType.Apocrypha, BookType = BookType.Book)] DC_ENO,
        // Book of Mekabis of Benjamin in the Ethiopian Bible
        [BookName("1 Meqabyan/Mekabis", "1Mq")] [BookName("Mekabiego", "1Mq", Language = Language.pl)] [BookType("B8", Deuterocanonical = DeuterocanonicalType.Apocrypha, BookType = BookType.Book, BookPart = BookPart.First)] DC_1MQ,
        // Book of Mekabis of Moab in the Ethiopian Bible
        [BookName("2 Meqabyan/Mekabis", "2Mq")] [BookName("Mekabiego", "2Mq", Language = Language.pl)] [BookType("B9", Deuterocanonical = DeuterocanonicalType.Apocrypha, BookType = BookType.Book, BookPart = BookPart.Second)] DC_2MQ,
        // Book of Meqabyan in the Ethiopian Bible
        [BookName("3 Meqabyan/Mekabis", "3Mq")] [BookName("Mekabiego", "3Mq", Language = Language.pl)] [BookType("C0", Deuterocanonical = DeuterocanonicalType.Apocrypha, BookType = BookType.Book, BookPart = BookPart.Third)] DC_3MQ,

        // Proverbs part 2: Used in the Ethiopian Bible
        [BookName("Reproof", "Rep")] [BookName("Nagany", "Rep", Language = Language.pl)] [BookType("C1", Deuterocanonical = DeuterocanonicalType.Apocrypha, BookType = BookType.Book)] DC_REP,
        // Paralipomenon of Jeremiah, called ‘Rest of the Words of Baruch’ in Ethiopia; may include or exclude the Letter of Jeremiah as chapter 1, used in the Ethiopian Bible
        [BookName("Baruch", "4Ba")] [BookName("Barucha", "4Ba", Language = Language.pl)] [BookType("C2", Deuterocanonical = DeuterocanonicalType.Apocrypha, BookType = BookType.Book, BookPart = BookPart.Fourth)] DC_4BA,
        // A Latin Vulgate book, found in the Vulgate and some medieval Catholic translations
        [BookName("Letter to the Laodiceans", "Lao")] [BookName("Laodycejczyków", "Lao", Language = Language.pl)] [BookType("C3", Deuterocanonical = DeuterocanonicalType.Apocrypha, BookType = BookType.Letter, LetterType = LetterType.Addressed)] DC_LAO
    }
}
