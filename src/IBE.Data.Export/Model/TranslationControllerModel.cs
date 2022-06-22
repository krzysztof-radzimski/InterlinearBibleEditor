/*=====================================================================================

	Interlinear Bible Editor
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using IBE.Common.Extensions;
using IBE.Data.Model;
using System;
using System.Collections.Generic;

namespace IBE.Data.Export.Model {
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

    public class BookBaseInfo {
        public int NumberOfBook { get; set; }
        public string BookShortcut { get; set; }
        public string BookName { get; set; }
        public string BookTitle { get; set; }
        public string Color { get; set; }
        public string TimeOfWriting { get; set; }
        public string PlaceWhereBookWasWritten { get; set; }
        public string Purpose { get; set; }
        public string Subject { get; set; }
        public string AuthorName { get; set; }
        public string Preface { get; set; }
        public BiblePart StatusBiblePart { get; set; }
        public CanonType StatusCanonType { get; set; }
        public TheBookType StatusBookType { get; set; }
    }
}
