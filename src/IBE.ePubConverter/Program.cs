using IBE.ePubConverter.Converters;
try {
    if (args.Length == 1) {
        if (args[0] == "/?" || args[0] == "help" || args[0] == "-?" || args[0] == "?" || args[0] == "-h" || args[0] == "-help") {
            Console.WriteLine("ePUB Converter");
            Console.WriteLine("Domyslnie program konwertuje pliki .epub do formatu .docx");
            Console.WriteLine("Dostępne przełączniki:");
            Console.WriteLine("-word \"ścieżka pliku\"");
            Console.WriteLine("-pdf \"ścieżka pliku\"");
            Console.WriteLine("\"ścieżka pliku\"");
            Console.WriteLine("\"ścieżka pliku\" \"ścieżka pliku2\" \"ścieżka pliku3\"");
        }
        else if (File.Exists(args[0])) {
            var fileName = args[0];
            Console.WriteLine($"Konwertowanie pliku {fileName} do formatu DOCX...");
            new WordConverter().Execute(fileName);
        }
        else {
            Console.WriteLine($"Nieznany argument '{args[0]}'!");
        }
    }
    else if (args.Length == 2) {
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
    else if (args.Length > 2) {
        IConverter converter = null;
        if (args[0].ToLower() == "word" || args[0].ToLower() == "-word" || args[0].ToLower() == "pdf" || args[0].ToLower() == "-pdf") {
            if (args[0].ToLower() == "word" || args[0].ToLower() == "-word") {
                converter = new WordConverter();
            }
            else if (args[0].ToLower() == "pdf" || args[0].ToLower() == "-pdf") {
                converter = new PdfConverter();
            }
        }

        if (converter == null) { converter = new WordConverter(); }
        foreach (var arg in args) {
            if (File.Exists(arg)) {
                Console.WriteLine($"Konwertowanie pliku {arg} do formatu DOCX...");
                converter.Execute(arg);
            }
        }
    }
}
catch (Exception ex) {
    Console.WriteLine(ex.ToString());
    // Console.ReadLine();
}
