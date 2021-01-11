/*=====================================================================================
	Author: (C)2009 - 2020 ITORG Krzysztof Radzimski
	http://itorg.pl
    License: MIT
  ===================================================================================*/

using System;

namespace IBE.Identifiers.Attributes {
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class BookNameAttribute : BaseNameAttribute {
        public string Description { get; set; }
        public string ShortName { get; set; }
        public BookNameAttribute(string name, string shortName) {
            Name = name;
            ShortName = shortName;
        }
    }
}
