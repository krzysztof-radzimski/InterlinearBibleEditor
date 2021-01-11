/*=====================================================================================
	Author: (C)2009 - 2020 ITORG Krzysztof Radzimski
	http://itorg.pl
    License: MIT
  ===================================================================================*/

using IBE.Identifiers.Attributes;

namespace IBE.Identifiers {
    public enum LetterType {
        None,

        [BaseName(Name = "")]
        [BaseName(Name = "", Language = Language.pl)]
        Ordinary,
        [BaseName(Name = " to the ")]
        [BaseName(Name = " do ", Language = Language.pl)]
        Addressed
    }
}
