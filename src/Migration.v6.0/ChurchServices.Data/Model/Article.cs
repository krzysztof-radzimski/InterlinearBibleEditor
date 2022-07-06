/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.Data.Model {
    public class Article : XPObject {
        private string text;
        private string subject;
        private string authorName;
        private string lead;
        private DateTime date;
        private byte[] documentData;
        private ArticleType type;
        private byte[] authorPicture;
        private bool hidden;

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
        public string Lead {
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

        public ArticleType Type {
            get { return type; }
            set { SetPropertyValue(nameof(Type), ref type, value); }
        }

        public byte[] AuthorPicture {
            get { return authorPicture; }
            set { SetPropertyValue(nameof(AuthorPicture), ref authorPicture, value); }
        }

        public bool Hidden {
            get { return hidden; }
            set { SetPropertyValue(nameof(Hidden), ref hidden, value); }
        }

        public Article(Session session) : base(session) { }
    }
}
