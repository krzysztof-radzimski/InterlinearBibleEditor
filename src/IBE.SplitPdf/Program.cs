using Aspose.Pdf.Facades;

if (args.Length == 2) {
    var path = args[0];
    var startPage = Convert.ToInt32(args[1]);

    if (File.Exists(path) && startPage > 0) {
        new Aspose.Pdf.License().SetLicense("../../../../../db/Aspose.Total.lic");

        var pdfEditor = new PdfFileEditor();
        var outputFilePath = Path.Combine(Path.GetDirectoryName(path), $"{Path.GetFileNameWithoutExtension(path)}.{startPage}.{Path.GetExtension(path)}");
        //var outputFilePath = @"C:\Users\krzysztof.radzimski\Documents\export.pdf";
        pdfEditor.SplitToEnd(path, startPage, outputFilePath);
    }
}