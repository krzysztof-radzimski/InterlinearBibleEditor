using IBE.ePubConverter.Converters;

if (args.Length > 1) {
    var fileName = args[1];
    if (File.Exists(fileName)) {
        Console.WriteLine($"Konwertowanie pliku {fileName}...");
        if (args[0].ToLower() == "word" || args[0].ToLower() == "-word") {
            Console.Clear();
            Console.WriteLine($"Konwertowanie pliku {fileName} do formatu DOCX...");
            new WordConverter().Execute(fileName);
        }
        else if (args[0].ToLower() == "pdf" || args[0].ToLower() == "-pdf") {
            Console.Clear();
            Console.WriteLine($"Konwertowanie pliku {fileName} do formatu PDF...");
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
else if (args.Length == 1) {
    var fileName = args[0];
    if (File.Exists(fileName)) {
        Console.WriteLine($"Konwertowanie pliku {fileName} do formatu DOCX...");
        new WordConverter().Execute(fileName);
    }
    else {
        throw new FileNotFoundException($"Nie znaleziono pliku '{fileName}'!");
    }
}
