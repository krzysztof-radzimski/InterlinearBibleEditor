/*=====================================================================================
	Author: (C)2009 - 2020 ITORG Krzysztof Radzimski
	http://itorg.pl
    License: MIT
  ===================================================================================*/

using System;

namespace IBE.Identifiers.Attributes {
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class BaseNameAttribute : Attribute {
        public string Name { get; set; }
        public Language Language { get; set; } = Language.en;
    }
}
