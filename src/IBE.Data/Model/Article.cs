using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Text;

namespace IBE.Data.Model {
    public class Article : XPObject {
        private string text;
        private string subject;
        private string authorName;
        private string lead;
        private DateTime date;
        private byte[] documentData;

        [Size(200)]
        public string Subject {
            get { return subject; }
            set { SetPropertyValue(nameof(Subject), ref subject, value); }
        }

        [Size(100)]
        public string AuthorName {
            get { return authorName; }
            set { SetPropertyValue(nameof(AuthorName), ref authorName, value); }
        }

        public DateTime Date {
            get { return date; }
            set { SetPropertyValue(nameof(Date), ref date, value); }
        }

        [Size(1000)]
        public string Lead{
            get { return lead; }
            set { SetPropertyValue(nameof(Lead), ref lead, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Text {
            get { return text; }
            set { SetPropertyValue(nameof(Text), ref text, value); }
        }

        public byte[] DocumentData {
            get { return documentData; }
            set { SetPropertyValue(nameof(DocumentData), ref documentData, value); }
        }

        public Article(Session session) : base(session) { }
    }
}
