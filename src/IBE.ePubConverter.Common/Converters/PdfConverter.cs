namespace IBE.ePubConverter.Common.Converters {
    public class PdfConverter : IConverter<string> {
        public string Execute(string fileName, bool loadLicenseKey = true) {
            var converter = new WordConverter();
            var doc = converter.GetDocument(fileName, loadLicenseKey);
            if (doc != null) {
                var pdfFilePath = Path.Combine(Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName) + ".pdf");
                doc.Save(pdfFilePath, new Aspose.Words.Saving.PdfSaveOptions() {
                    UseHighQualityRendering = true,
                    ExportDocumentStructure = true,
                    EmbedFullFonts = true,
                    Compliance = Aspose.Words.Saving.PdfCompliance.PdfA2a,
                    JpegQuality = 100,
                    CreateNoteHyperlinks = true
                });
                return pdfFilePath;
            }

            return null;
        }
    }
}
