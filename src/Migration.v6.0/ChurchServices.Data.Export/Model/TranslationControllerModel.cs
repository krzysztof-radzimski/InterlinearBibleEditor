﻿/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/


namespace ChurchServices.Data.Export.Model {
    public class TranslationControllerModel {
        public Translation Translation { get; }
        public List<BookBaseInfo> Books { get; }
        public string Book { get; }
        public string Chapter { get; }
        public string Verse { get; }
        public int NTBookNumber { get; }
        public int LogosBookNumber { get; }
        public List<TranslationInfo> Translations { get; }
        public TranslationControllerModel(Translation t, string book = null, string chapter = null, string verse = null, List<BookBaseInfo> books = null) {
            Translation = t;
            Book = book;
            Chapter = chapter;
            Verse = verse;
            Translations = new List<TranslationInfo>();
            Books = books;
            NTBookNumber = GetNTBookNumber();
            LogosBookNumber = GetLogosBookNumber();
        }

        private int GetNTBookNumber() {
            if (Translation.Name.Replace("+", String.Empty) == "IPD") { return 67; }
            var book = Book.ToInt();
            var r = 1;
            for (int i = 470; i <= 730; i += 10) {
                if (i == book) {
                    return r;
                }
                r++;
            }
            return r;
        }
        private int GetLogosBookNumber() {
            var book = Book.ToInt();
            var r = 1;
            if (book < 470) {
                foreach (var item in Books) {
                    if (item.NumberOfBook < 470) {
                        if (book == item.NumberOfBook) { return r; }
                    }
                    else {
                        break;
                    }
                    r++;
                }
            }
            else {
                r = 61;
                foreach (var item in Books) {
                    if (item.NumberOfBook >= 470) {
                        if (book == item.NumberOfBook) { return r; }
                        r++;
                    }
                    else {
                        continue;
                    }
                }
            }

            return r;
        }
    }
}
