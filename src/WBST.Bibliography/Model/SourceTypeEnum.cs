using System.ComponentModel;
using System.Xml.Serialization;

namespace WBST.Bibliography.Model {
    public enum SourceTypeEnum {
        [XmlEnum(Name = "ArticleInAPeriodical")] [Description("Artykuł z magazynu")] ArticleInAPeriodical,
        [XmlEnum(Name = "Book")] [Description("Książka")] Book,
        [XmlEnum(Name = "BookSection")] [Description("Fragment książki")] BookSection,
        [XmlEnum(Name = "JournalArticle")] [Description("Artykuł z czasopisma")] JournalArticle,
        [XmlEnum(Name = "ConferenceProceedings")] [Description("Materiały konferencyjne")] ConferenceProceedings,
        [XmlEnum(Name = "Report")] [Description("Raport")] Report,
        [XmlEnum(Name = "SoundRecording")] [Description("Książka")] SoundRecording,
        [XmlEnum(Name = "Performance")] [Description("Przedstawienie")] Performance,
        [XmlEnum(Name = "Art")] [Description("Sztuka")] Art,
        [XmlEnum(Name = "DocumentFromInternetSite")] [Description("Dokument z witryny sieci Web")] DocumentFromInternetSite,
        [XmlEnum(Name = "InternetSite")] [Description("Witryna sieci Web")] InternetSite,
        [XmlEnum(Name = "Film")] [Description("Film")] Film,
        [XmlEnum(Name = "Interview")] [Description("Wywiad")] Interview,
        [XmlEnum(Name = "Patent")] [Description("Patent")] Patent,
        [XmlEnum(Name = "ElectronicSource")] [Description("Książka")] ElectronicSource,
        [XmlEnum(Name = "Case")] [Description("Sprawa")] Case,
        [XmlEnum(Name = "Misc")] [Description("Różne")] Misc
    }
}
