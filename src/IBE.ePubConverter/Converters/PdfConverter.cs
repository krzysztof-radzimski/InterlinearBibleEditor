namespace IBE.ePubConverter.Converters {
    internal class PdfConverter : IConverter {
        public void Execute(string fileName) {
            var converter = new WordConverter();
                 var doc = converter.GetDocument(fileName);
            if (doc != null) {
                doc.Save(fileName.Replace(".epub", ".pdf"), new Aspose.Words.Saving.PdfSaveOptions() {
                    UseHighQualityRendering = true,
                    ExportDocumentStructure = true,
                    EmbedFullFonts = true,
                    Compliance = Aspose.Words.Saving.PdfCompliance.PdfA2a,
                    JpegQuality = 100,
                    CreateNoteHyperlinks = true
                });
            }
        }
    }
}
