using Aspose.Pdf.Facades;
using System.Text;

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
else if (args.Length == 1) {
    var result = Convert.ToBase64String(Encoding.UTF8.GetBytes(args[0]));
    Console.WriteLine(result);
}