using System;
using System.Collections.Generic;
using System.IO;
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
        public static string InnerText(this XElement element) {
            string s = String.Empty;

            if (element == null) {                return s;            }

            if (element.HasElements) {
                XElement x = element.Clone();
                x.Descendants().Remove();
                s = x.Value;
            }
            else {
                s = element.Value;
            }

            return s.Trim();
        }
        public static XElement Clone(this XElement x) {
            if (x.IsNotNull()) {
                try {
                    return XElement.Parse(x.ToString().Trim());
                }
                catch {}
            }

            return null;
        }
    }
}
