using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace WBST.Bibliography.Model {
    [XmlRoot(ElementName = "Source", Namespace = "http://schemas.openxmlformats.org/officeDocument/2006/bibliography")]
    public class BibliographySource {
        [Browsable(false)] [DisplayName("Nazwa tagu")] public string Tag { get; set; }
        [DisplayName("Typ źródła")] public SourceTypeEnum SourceType { get; set; }
        [Browsable(false)] [DisplayName("ID")] public string Guid { get; set; }
        [DisplayName("Tytuł")] public string Title { get; set; }
        [DisplayName("Rok")] public string Year { get; set; }
        [DisplayName("Miesiąc")] public string Month { get; set; }
        [DisplayName("Autor")] public BibliographyAuthor Author { get; set; }
        [DisplayName("Miejscowość")] public string City { get; set; }
        [DisplayName("Wydawca")] public string Publisher { get; set; }
        [DisplayName("Województwo")] public string StateProvince { get; set; }
        [DisplayName("Kraj")] public string CountryRegion { get; set; }
        [DisplayName("Tom")] public string Volume { get; set; }
        [DisplayName("Liczba tomów")] public string NumberVolumes { get; set; }
        [DisplayName("Krótki tytuł")] public string ShortTitle { get; set; }
        [Browsable(false)] [DisplayName("Typ numeru standardowego")] public string StandardNumber { get; set; }
        [DisplayName("Strony")] public string Pages { get; set; }
        [DisplayName("Wydanie")] public string Edition { get; set; }
        [DisplayName("Komentarze")] public string Comments { get; set; }
        [DisplayName("Nośnik")] public string Medium { get; set; }
        [Browsable(false)] [DisplayName("Dostęp (rok)")] public string YearAccessed { get; set; }
        [Browsable(false)] [DisplayName("Dostęp (miesiąc)")] public string MonthAccessed { get; set; }
        [Browsable(false)] [DisplayName("Dostęp (dzień)")] public string DayAccessed { get; set; }
        [XmlIgnore] [DisplayName("Dostęp")] public string Access => !String.IsNullOrEmpty(YearAccessed) ? $"{YearAccessed}.{MonthAccessed}.{DayAccessed}" : String.Empty;
        [DisplayName("Adres URL")] public string URL { get; set; }
        [Browsable(false)] [DisplayName("Identyfikator cyfrowy DOI")] public string DOI { get; set; }
        [Browsable(false)] [DisplayName("Język")] public string LCID { get; set; }
        [Browsable(false)] [DisplayName("Kolejność")] public string RefOrder { get; set; }
        [DisplayName("Nazwa magazynu")] public string JournalName { get; set; }
        [DisplayName("Numer magazynu")] public string Issue { get; set; }


        public bool ShouldSerializeTitle() { return !String.IsNullOrWhiteSpace(Title); }
        //public bool ShouldSerialize() { return !String.IsNullOrWhiteSpace(); }
        //public bool ShouldSerialize() { return !String.IsNullOrWhiteSpace(); }
        //public bool ShouldSerialize() { return !String.IsNullOrWhiteSpace(); }
        public bool ShouldSerializeYear() { return !String.IsNullOrWhiteSpace(Year); }
        public bool ShouldSerializeMonth() { return !String.IsNullOrWhiteSpace(Month); }
        public bool ShouldSerializeCity() { return !String.IsNullOrWhiteSpace(City); }
        public bool ShouldSerializePublisher() { return !String.IsNullOrWhiteSpace(Publisher); }
        public bool ShouldSerializeStateProvince() { return !String.IsNullOrWhiteSpace(StateProvince); }
        public bool ShouldSerializeCountryRegion() { return !String.IsNullOrWhiteSpace(CountryRegion); }
        public bool ShouldSerializeVolume() { return !String.IsNullOrWhiteSpace(Volume); }
        public bool ShouldSerializeNumberVolumes() { return !String.IsNullOrWhiteSpace(NumberVolumes); }
        public bool ShouldSerializeShortTitle() { return !String.IsNullOrWhiteSpace(ShortTitle); }
        public bool ShouldSerializeStandardNumber() { return !String.IsNullOrWhiteSpace(StandardNumber); }
        public bool ShouldSerializePages() { return !String.IsNullOrWhiteSpace(Pages); }
        public bool ShouldSerializeEdition() { return !String.IsNullOrWhiteSpace(Edition); }
        public bool ShouldSerializeComments() { return !String.IsNullOrWhiteSpace(Comments); }
        public bool ShouldSerializeMedium() { return !String.IsNullOrWhiteSpace(Medium); }
        public bool ShouldSerializeYearAccessed() { return !String.IsNullOrWhiteSpace(YearAccessed); }
        public bool ShouldSerializeMonthAccessed() { return !String.IsNullOrWhiteSpace(MonthAccessed); }
        public bool ShouldSerializeDayAccessed() { return !String.IsNullOrWhiteSpace(DayAccessed); }
        public bool ShouldSerializeURL() { return !String.IsNullOrWhiteSpace(URL); }
        public bool ShouldSerializeDOI() { return !String.IsNullOrWhiteSpace(DOI); }
        public bool ShouldSerializeJournalName() { return !String.IsNullOrWhiteSpace(JournalName); }
        public bool ShouldSerializeIssue() { return !String.IsNullOrWhiteSpace(Issue); }
    }
}
