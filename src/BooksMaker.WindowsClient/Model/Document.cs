using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksMaker.WindowsClient.Model {
    public class Document {
        public DocumentProperties Properties { get; set; }
        public DocumentTitlePage TitlePage { get; set; }
        public DocumentToc Toc { get; set; }
        public DocumentIntroduction Introduction { get; set; }
        public List<DocumentSection> Sections { get; set; }
        public DocumentSummary Summary { get; set; }
        public DocumentBibliography Bibliography { get; set; }
        public DocumentImagesToc ImagesToc { get; set; }
    }
}
