/*=====================================================================================
	Author: (C)2009 - 2020 ITORG Krzysztof Radzimski
	http://itorg.pl
    License: MIT
  ===================================================================================*/

using System;

namespace Usfm.Identifiers.Attributes {
    [AttributeUsage(AttributeTargets.Field)]
    public class NumberAttribute : Attribute {
        public string Number { get; set; }
        public NumberAttribute() { }
        public NumberAttribute(string number) : this() {
            Number = number;
        }
    }
}
