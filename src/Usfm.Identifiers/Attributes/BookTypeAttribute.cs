/*=====================================================================================
	Author: (C)2009 - 2020 ITORG Krzysztof Radzimski
	http://itorg.pl
    License: MIT
  ===================================================================================*/

using System;

namespace Usfm.Identifiers.Attributes {
    [AttributeUsage(AttributeTargets.Field)]
    public class BookTypeAttribute : Attribute {
        public string BookNumber { get; set; }
        public BookType BookType { get; set; }
        public BookPart BookPart { get; set; }
        public LetterType LetterType { get; set; }
        public bool Deuterocanonical { get; set; }
        public BookTypeAttribute() { }
        public BookTypeAttribute(string bookNumber) : this() {
            BookNumber = bookNumber;
        }
    }
}
