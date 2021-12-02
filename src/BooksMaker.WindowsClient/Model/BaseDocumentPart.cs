namespace BooksMaker.WindowsClient.Model {
    public interface IBaseDocumentPart {
        string NameOfPart { get; set; }
        byte[] OpenXmlBytes { get; set; }
        DocumentPartType PartType { get; }
        string DefaultFileName { get; }
    }

    public abstract class BaseDocumentPart : IBaseDocumentPart {
        public byte[] OpenXmlBytes { get; set; }
        public string NameOfPart { get; set; }
        public abstract DocumentPartType PartType { get; }
        public string DefaultFileName => $"{PartType}.xml";
    }

    public enum DocumentPartType {
        TitlePage,
        Introduction,
        Content,
        Summary,
        Bibliography,
        Properties
    }
}
