/*=====================================================================================
	Author: (C)2009 - 2020 ITORG Krzysztof Radzimski
	http://itorg.pl
    License: MIT
  ===================================================================================*/

using Usfm.Identifiers.Attributes;

namespace Usfm.Identifiers {
    public enum BookPart {
        None = 0,

        [BookPartName("First", "First")]
        [BookPartName("Pierwszy", "Pierwsza", Language = Language.pl)]
        First = 1,

        [BookPartName("Second", "Second")]
        [BookPartName("Drugi", "Druga", Language = Language.pl)]
        Second = 2,

        [BookPartName("Third", "Third")]
        [BookPartName("Trzeci", "Trzecia", Language = Language.pl)]
        Third = 3,

        [BookPartName("Fourth", "Fourth")]
        [BookPartName("Czwarty", "Czwarta", Language = Language.pl)]
        Fourth = 4,

        [BookPartName("Fifth", "Fifth")]
        [BookPartName("Piąty", "Piąta", Language = Language.pl)]
        Fifth = 5,

        [BookPartName("Sixth", "Sixth")]
        [BookPartName("Szósty", "Szósta", Language = Language.pl)]
        Sixth = 6
    }
}
