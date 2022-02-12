using DevExpress.Xpo;

namespace IBE.Data.Model {
    public class UrlShort : XPObject {
        private string url;
        private string shortUrl;
        public string Url {
            get { return url; }
            set { SetPropertyValue(nameof(url), ref url, value); }
        }
        public string ShortUrl {
            get { return shortUrl; }
            set { SetPropertyValue(nameof(shortUrl), ref shortUrl, value); }
        }
        public UrlShort(Session session) : base(session) { }
    }

    public class UrlShortInfo {
        public string Url { get; set; }
        public string Short { get; set; }
    }
}
