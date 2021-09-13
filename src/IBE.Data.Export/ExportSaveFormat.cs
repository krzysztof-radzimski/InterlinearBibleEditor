using System.ComponentModel;

namespace IBE.Data.Export {
    public enum ExportSaveFormat {
        [Description("application/vnd.openxmlformats-officedocument.wordprocessingml.document")]
        [Category("file.docx")]
        Docx, 
        [Description("application/pdf")]
        [Category("file.pdf")]
        Pdf
    }
}
