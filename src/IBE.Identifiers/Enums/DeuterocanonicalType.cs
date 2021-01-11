/*=====================================================================================
	Author: (C)2009 - 2020 ITORG Krzysztof Radzimski
	http://itorg.pl
    License: MIT
  ===================================================================================*/


using IBE.Identifiers.Attributes;

namespace IBE.Identifiers {
    public enum DeuterocanonicalType {
        None,
        [BaseName(Name = "Deuterocanonical")]
        [BaseName(Name = "Wtórnokanoniczne", Language = Language.pl)]
        Deuterocanonical,
        [BaseName(Name = "Apocrypha")]
        [BaseName(Name = "Apokryfy", Language = Language.pl)]
        Apocrypha
    }
}
