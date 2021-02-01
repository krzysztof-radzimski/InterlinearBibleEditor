using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace IBE.Data.Import.Extensions {
   public static class XElementExtensions {
        public static XElement GetElement(this XElement element, params string[] names) {
            if (element.IsNotNull() && element.HasElements) {
                return element.Elements().Where(xe => xe.Name.LocalName.ContainsInTable(true, names)).FirstOrDefault();
            }

            return null;
        }
        public static string Value(this XElement e) {
            if (e == null) {
                return String.Empty;
            }

            return e.Value(String.Empty);
        }
        public static string Value(this XElement e, string defaultValue) {
            if (e != null) {
                if (!String.IsNullOrEmpty(e.Value)) {
                    return e.Value;
                }
                else {
                    return defaultValue;
                }
            }
            if (defaultValue != null) {
                return defaultValue;
            }

            return String.Empty;
        }

        public static string Value(this XAttribute attribute) {
            if (attribute != null) {
                return attribute.Value(String.Empty);
            }

            return String.Empty;
        }
        public static string Value(this XAttribute attribute, string defaultValue) {
            if (attribute != null) {
                if (!String.IsNullOrEmpty(attribute.Value)) {
                    return attribute.Value;
                }
            }

            return defaultValue;
        }
    }
}
