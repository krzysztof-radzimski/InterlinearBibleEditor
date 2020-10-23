/*=====================================================================================
	Author: (C)2009 - 2020 ITORG Krzysztof Radzimski
	http://itorg.pl
    License: MIT
  ===================================================================================*/

using Usfm.Identifiers.Attributes;

namespace Usfm.Identifiers {
    public enum BookType {
        None = 0,
        [BaseName(Name = "Book of")]
        [BaseName(Name = "Księga", Language = Language.pl)]
        Book,

        [BaseName(Name = "Gospel of")]
        [BaseName(Name = "Ewangelia wg św.", Language = Language.pl)]
        Gospel,
        
        [BaseName(Name = "")]
        [BaseName(Name = "List", Language = Language.pl)]
        Letter,
        
        [BaseName(Name = "")]
        [BaseName(Name = "List św. Pawła", Language = Language.pl)]
        PaulsLetter,

        [BaseName(Name = "")]
        [BaseName(Name = "List św. Jakuba", Language = Language.pl)]
        JamesLetter,

        [BaseName(Name = "")]
        [BaseName(Name = "List św. Piotra", Language = Language.pl)]
        PetersLetter,

        [BaseName(Name = "")]
        [BaseName(Name = "List św. Jana", Language = Language.pl)]
        JohnsLetter,

        [BaseName(Name = "")]
        [BaseName(Name = "List św. Judy", Language = Language.pl)]
        JudsLetter,

        [BaseName(Name = "Revelation")]
        [BaseName(Name = "Objawienie", Language = Language.pl)]
        Eschatology
    }
}
