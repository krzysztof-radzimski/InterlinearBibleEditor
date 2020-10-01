/*=====================================================================================
	Author: (C)2009 - 2020 ITORG Krzysztof Radzimski
	http://itorg.pl
    License: MIT
  ===================================================================================*/

using System;

namespace Usfm.Identifiers.Attributes {
    [AttributeUsage(AttributeTargets.Field)]
    public class EnglishNameAttribute : Attribute {
        public string Name { get; set; }
        public EnglishNameAttribute() { }
        public EnglishNameAttribute(string name) : this() {
            Name = name;
        }
    }
}
