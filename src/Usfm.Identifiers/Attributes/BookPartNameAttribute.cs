/*=====================================================================================
	Author: (C)2009 - 2020 ITORG Krzysztof Radzimski
	http://itorg.pl
    License: MIT
  ===================================================================================*/

using System;

namespace Usfm.Identifiers.Attributes {
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class BookPartNameAttribute : BaseNameAttribute {
        public string NameFeminine { get; set; }
        public BookPartNameAttribute(string name, string feminine) {
            Name = name;
            NameFeminine = feminine;
        }
    }
}
