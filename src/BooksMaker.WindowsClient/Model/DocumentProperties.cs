using System.Xml.Serialization;

namespace BooksMaker.WindowsClient.Model {
    public class DocumentProperties : BaseDocumentPart {
        public override DocumentPartType PartType => DocumentPartType.Properties;
    }
}