using System.Xml.Serialization;

namespace ChurchServices.Data.Import.EIB.Model.Osis {
    public class OsisTitle {
        [XmlText(typeof(string))] public string Title { get; set; }
    }
}
