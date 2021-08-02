using System;
using System.Collections.Generic;

namespace BooksMaker.WindowsClient.Model {
    public class DocumentBibliography : List<BibliographyItem> { }
    public class BibliographyItem {
        public string Author { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Publisher { get; set; }
        public string SeriesName { get; set; }
        public string Url { get; set; }
        public DateTime UrlAccessDate { get; set; }
    }
}