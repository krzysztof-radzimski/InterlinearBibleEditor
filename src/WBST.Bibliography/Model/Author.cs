﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace WBST.Bibliography.Model {
    public class BibliographyAuthor : IComparable {
        [DisplayName("Autor")] public Author Author { get; set; }
        [DisplayName("Redaktor")] public Author Editor { get; set; }
        [DisplayName("Tłumacz")] public Author Translator { get; set; }

        public int CompareTo(object obj) {
            var s1 = this.ToString();
            var s2 = obj.ToString();
            return s1.CompareTo(s2);
        }

        public override string ToString() {
            var str = "";
            if (Author != null) { str += $"{Author}; "; } 
            if (Editor != null) { str += $"red. {Editor}; "; }
            if (Translator != null) { str += $"przeł. {Translator};"; }
            var result = str.Trim();
            if (result.EndsWith(";")) {
                result = result.Substring(0, result.Length - 1);
            }
            return result;
        }
    }
    public class Author {
        [Browsable(false)]
        [XmlElement("NameList", typeof(BibliographyNameList))]
        [XmlElement("Corporate", typeof(string))]
        public List<object> Objects { get; set; }

        [XmlIgnore][Browsable(false)] public BibliographyNameList NamesList => Objects != null && Objects.Where(x => x is BibliographyNameList).Count() > 0 ? Objects.Where(x => x is BibliographyNameList).FirstOrDefault() as BibliographyNameList : null;
        [XmlIgnore][Browsable(false)] public string Corporates => Objects != null && Objects.Where(x => x is string).Count() > 0 ? Objects.Where(x => x is string).FirstOrDefault().ToString() : null;

        public override string ToString() {
            var str = "";
            foreach (var item in Objects) {
                str += item.ToString();
                if (item != Objects.Last()) { str += "; "; }
            }
            return str.Trim();
        }
    }
    public class BibliographyNameList {
        [DisplayName("Imie, Nazwisko")][XmlElement(ElementName = "Person")] public List<BibliographyPerson> People { get; set; }
        public override string ToString() {
            var str = "";
            foreach (var item in People) {
                str += $"{item.First} {item.Middle}".Trim();
                str += $" {item.Last}";
                if (item != People.Last()) { str += ", "; }
            }
            return str.Trim();
        }
    }
    public class BibliographyPerson {
        [DisplayName("Imię")] public string First { get; set; }
        [DisplayName("Drugie imię")] public string Middle { get; set; }
        [DisplayName("Nazwisko")] public string Last { get; set; }

        public override string ToString() {
            var str = string.Empty;
            str += $"{this.First} {this.Middle}".Trim();
            str += $" {this.Last}";
            return str.Trim();
        }
    }

}
