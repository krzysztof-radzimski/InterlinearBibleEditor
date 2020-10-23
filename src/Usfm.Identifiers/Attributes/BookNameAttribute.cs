/*=====================================================================================
	Author: (C)2009 - 2020 ITORG Krzysztof Radzimski
	http://itorg.pl
    License: MIT
  ===================================================================================*/

using System;

namespace Usfm.Identifiers.Attributes {
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class BaseNameAttribute : Attribute {
        public string Name { get; set; }
        public Language Language { get; set; } = Language.en;
    }


    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class BookNameAttribute : BaseNameAttribute {
        public string Description { get; set; }
        public string ShortName { get; set; }
        public BookNameAttribute(string name, string shortName) {
            Name = name;
            ShortName = shortName;
        }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class BookPartNameAttribute : BaseNameAttribute {
        public string NameFeminine { get; set; }
        public BookPartNameAttribute(string name, string feminine) {
            Name = name;
            NameFeminine = feminine;
        }
    }
}
