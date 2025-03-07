/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2025 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using System.ComponentModel;
using System.Reflection;
using System.Xml.Serialization;

namespace ChurchServices.Extensions {
    public static class EnumExtensions {
        public static string GetDescription(this Enum value) {
            try {
                FieldInfo fi = value.GetType().GetField(value.ToString());
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
            }
            catch {
                return string.Empty;
            }
        }
        public static string GetCategory(this Enum value) {
            try {
                FieldInfo fi = value.GetType().GetField(value.ToString());
                CategoryAttribute[] attributes = (CategoryAttribute[])fi.GetCustomAttributes(typeof(CategoryAttribute), false);
                return (attributes.Length > 0) ? attributes[0].Category : value.ToString();
            }
            catch {
                return string.Empty;
            }
        }

        public static IEnumerable<string> GetEnumCategories<T>() where T : IConvertible {
            var l = new List<string>();

            var v = GetEnumValues<T>();
            foreach (var e in v) {
                l.Add((e as Enum).GetCategory());
            }

            return l;
        }

        public static IEnumerable<string> GetEnumDescriptions<T>() where T : IConvertible {
            var l = new List<string>();

            var v = GetEnumValues<T>();
            foreach (var e in v) {
                l.Add((e as Enum).GetDescription());
            }

            return l;
        }

        public static IEnumerable<T> GetEnumValues<T>() where T : IConvertible {
            List<T> l = new List<T>();

            Array al = Enum.GetValues(typeof(T));
            foreach (var p in al) {
                T e = (T)p;
                l.Add(e);
            }

            return l;
        }

        public static T GetEnumByDescription<T>(this string description) {
            if (!typeof(T).IsEnum) {
                throw new InvalidEnumArgumentException("The specified type is not an enum");
            }

            foreach (T item in Enum.GetValues(typeof(T))) {
                if (string.Compare((item as Enum).GetDescription(), description, true) == 0) {
                    return item;
                }
            }
            return default(T);
        }

        public static T GetEnumByCategory<T>(this string category) {
            if (!typeof(T).IsEnum) {
                throw new InvalidEnumArgumentException("The specified type is not an enum");
            }

            foreach (T item in Enum.GetValues(typeof(T))) {
                if (string.Compare((item as Enum).GetCategory(), category, true) == 0) {
                    return item;
                }
            }
            return default(T);
        }

        public static T GetEnumByName<T>(this string name) {
            if (!typeof(T).IsEnum) {
                throw new InvalidEnumArgumentException("The specified type is not an enum");
            }

            return (T)Enum.Parse(typeof(T), name);
        }

        public static string GetXmlEnum<T>(this T value)  {
            if (!typeof(T).IsEnum) {
                throw new InvalidEnumArgumentException("The specified type is not an enum");
            }
            try {
                FieldInfo fi = value.GetType().GetField(value.ToString());
                XmlEnumAttribute[] attributes = (XmlEnumAttribute[])fi.GetCustomAttributes(typeof(XmlEnumAttribute), false);
                return (attributes.Length > 0) ? attributes[0].Name : value.ToString();
            }
            catch {
                return string.Empty;
            }
        }

        public static T GetEnumByXmlEnum<T>(this string xmlEnumValue) {
            if (!typeof(T).IsEnum) {
                throw new InvalidEnumArgumentException("The specified type is not an enum");
            }

            foreach (T item in Enum.GetValues(typeof(T))) {
                if (string.Compare(item.GetXmlEnum(), xmlEnumValue, true) == 0) {
                    return item;
                }
            }
            return default(T);
        }
    }
}
