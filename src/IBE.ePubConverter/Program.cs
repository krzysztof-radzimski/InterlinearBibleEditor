using IBE.ePubConverter.Converters;

if (args.Length > 1) {
    var fileName = args[1];
    if (File.Exists(fileName)) {
        if (args[0].ToLower() == "word" || args[0].ToLower() == "-word") {
            new WordConverter().Execute(fileName);
        }
        else if (args[0].ToLower() == "pdf" || args[0].ToLower() == "-pdf") {
            new PdfConverter().Execute(fileName);
        }
        else {
            throw new Exception("Nieobsługiwany przełącznik!");
        }
    }
    else {
        throw new FileNotFoundException($"Nie znaleziono pliku '{fileName}'!");
    }
}
