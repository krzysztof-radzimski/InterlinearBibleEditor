using DevExpress.Xpo;
using EIB.Data.Utils;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace EIB.Data.Model {
    public class Verse : XPObject {
        private int numberOfVerse;
        private Chapter parentChapter;
        private string text;
        private string index;
        private bool startFromNewLine;
        private bool isTitle;

        public int NumberOfVerse {
            get { return numberOfVerse; }
            set { SetPropertyValue(nameof(NumberOfVerse), ref numberOfVerse, value); }
        }

        [Association]
        public Chapter ParentChapter {
            get { return parentChapter; }
            set { SetPropertyValue(nameof(ParentChapter), ref parentChapter, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Text {
            get { return text; }
            set { SetPropertyValue(nameof(Text), ref text, value); }
        }
        public bool StartFromNewLine {
            get { return startFromNewLine; }
            set { SetPropertyValue(nameof(StartFromNewLine), ref startFromNewLine, value); }
        }
        public bool IsTitle {
            get { return isTitle; }
            set { SetPropertyValue(nameof(IsTitle), ref isTitle, value); }
        }
        public string Index {
            get { return index; }
            set { SetPropertyValue(nameof(Index), ref index, value); }
        }

        public Verse(Session session) : base(session) { }

        public VerseIndex GetVerseIndex() => new VerseIndex(Index);
        public override string ToString() => NumberOfVerse.ToString();

        public XElement GetXml() => XElement.Parse(Text);

        public VerseModel GetVerseModel(string text) {
            if (text != null) {
                XmlSerializer serializer = new XmlSerializer(typeof(VerseModel));
                using (Stream reader = new MemoryStream(Encoding.UTF8.GetBytes(text))) {
                    return (VerseModel)serializer.Deserialize(reader);
                }

            }
            return default;
        }
        public string GetText(VerseModel verseModel) {
            if (verseModel != null) {
                var serializer = new XmlSerializer(typeof(VerseModel));

                using (StringWriter writer = new Utf8StringWriter()) {
                    serializer.Serialize(writer, verseModel);
                    return writer.ToString();
                }
            }
            return default;
        }
    }
    public class Utf8StringWriter : StringWriter { public override Encoding Encoding => Encoding.UTF8; }
}
