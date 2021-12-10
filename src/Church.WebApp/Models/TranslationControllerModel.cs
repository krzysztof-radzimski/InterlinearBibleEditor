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


namespace Church.WebApp.Models {
    public class TranslationControllerModel {
        public Translation Translation { get; }
        public List<BookBase> Books { get; }
        public string Book { get; }
        public string Chapter { get; }
        public string Verse { get; }
        public int NTBookNumber { get; }
        public int LogosBookNumber { get; }
        public List<TranslationInfo> Translations { get; }
        public TranslationControllerModel(Translation t, string b = null, string c = null, string v = null, List<BookBase> books = null) {
            Translation = t;
            Book = b;
            Chapter = c;
            Verse = v;
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
