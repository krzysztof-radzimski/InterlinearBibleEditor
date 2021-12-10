using EIB.SNPPD.Importer.Controllers;

var config = new ConfigurationController();
var baseDirectory = config.GetBaseDirectory();
if (Directory.Exists(baseDirectory)) {
    foreach (var filePath in Directory.GetFiles(baseDirectory, "*.docx")) {
        new DocumentImportController().Execute(filePath);
    }
}