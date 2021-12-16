using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIB.SNPPD.Importer.Controllers {
    internal class DocumentImportController {
        public const string BOOK_NAME_STYLE = "Nagwek1";
        public const string REFERENCES_STYLE = "Odnosniki";
        public const string SUBTITLE_STYLE = "Nagwek3";
        public const string CHAPTER_NUMBER_STYLE = "Inicja";

        public void Execute(string filePath) {
            using var wordDocument = WordprocessingDocument.Open(filePath, false);
            var body = wordDocument.MainDocumentPart.Document.Body;
            foreach (var line in body) {
                if (line is Paragraph) { 
                
                }
            }
        }
    }
}
