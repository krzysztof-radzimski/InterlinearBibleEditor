﻿using DevExpress.Xpo;
using IBE.Data.Export.Model;

namespace IBE.Data.Export.Controllers {
    public interface IBibleTagController {
        string AppendNonBreakingSpaces(string text);
        string CleanVerseText(string text);

        string GetInternalVerseRangeHtml(string input, TranslationControllerModel model);
        string GetInternalVerseRangeText(string input, TranslationControllerModel model);

        string GetInternalVerseHtml(string input, TranslationControllerModel model);
        string GetInternalVerseText(string input, TranslationControllerModel model);

        string GetExternalVerseRangeHtml(string input, TranslationControllerModel model);
        string GetExternalVerseRangeText(string input, TranslationControllerModel model);

        string GetExternalVerseHtml(string input, TranslationControllerModel model);
        string GetExternalVerseText(string input, TranslationControllerModel model);

        string GetInternalVerseListHtml(string input, TranslationControllerModel model);
        string GetInternalVerseListText(string input, TranslationControllerModel model);

        string GetMultiChapterRangeHtml(string input, TranslationControllerModel model);
        string GetMultiChapterRangeText(string input, TranslationControllerModel model);

        string GetVerseTranslation(Session session, int numberOfBook, int numberOfChapter, int verseStart, int verseEnd = 0, string translationName = "NPI");
    }
}