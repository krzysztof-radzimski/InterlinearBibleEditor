/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using ChurchServices.Data.Export.Controllers;

namespace ChurchServices.Data.Export {
    public class CompareVersesExporter : BaseExporter {
        private int footNoteIndex = 1;
        private CompareVersesExporter() : base() { }
        public CompareVersesExporter(byte[] asposeLicense, string host) : base(asposeLicense, host) { }

        public byte[] Export(CompareVerseModel model, ExportSaveFormat saveFormat, IBibleTagController bibleTag, TranslationControllerModel transModel) {
            if (model.IsNull()) { throw new ArgumentNullException("model"); }
            var builder = GetDocumentBuilder();

            var title = $"Porównanie tłumaczeń {model.BookName} {model.Index.NumberOfChapter}:{model.Index.NumberOfVerse}";
            {
                var par = builder.CurrentParagraph;
                par.ParagraphFormat.Style = builder.Document.Styles["Nagłówek 1"];
                par.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.Write(title);
            }

            {
                var par = builder.InsertParagraph();
                par.ParagraphFormat.Style = builder.Document.Styles["Normal"];
                par.ParagraphFormat.Alignment = ParagraphAlignment.Left;
            }

            var table = builder.StartTable();

            builder.Font.Bold = true;
            // Header
            {
                builder.InsertCell();
                builder.Writeln("Przekład");
                builder.InsertCell();
                builder.Writeln("Rodzaj");
                builder.InsertCell();
                builder.Writeln("Nazwa");
                builder.InsertCell();
                builder.Writeln("Treść");
            }

            builder.Font.Bold = false;
            builder.EndRow();

            foreach (var item in model.Verses) {
                var index = item.Index;
                if (index.IsNull()) { continue; }
                var translationName = item.TranslationName;
                var translationDesc = item.TranslationDescription;
                var translationType = item.TranslationType.GetDescription();
                //Dictionary<int, string> footNotes = null;
                //var itemHtml = GetVerseHtml(model, item, out footNotes);

                var itemHtml = GetVerseHtml(model, item);

                builder.InsertCell();
                builder.Writeln(translationName);
                builder.InsertCell();
                builder.Writeln(translationType);
                builder.InsertCell();
                builder.Writeln(translationDesc);
                builder.InsertCell();

                foreach (var hItem in itemHtml) {
                    if (hItem.Footnote) {
                        var footnoteText = hItem.Text;
                        if (footnoteText.Contains("<x>")) {
                            footnoteText = bibleTag.GetInternalVerseRangeText(footnoteText, transModel);
                            footnoteText = bibleTag.GetInternalVerseText(footnoteText, transModel);
                            footnoteText = bibleTag.GetExternalVerseRangeText(footnoteText, transModel);
                            footnoteText = bibleTag.GetExternalVerseText(footnoteText, transModel);
                            footnoteText = bibleTag.GetInternalVerseListText(footnoteText, transModel);
                            footnoteText = bibleTag.GetMultiChapterRangeText(footnoteText, transModel);
                        }
                        footnoteText = footnoteText.Replace("&nbsp;", " ");
                        builder.InsertFootnote(FootnoteType.Footnote, footnoteText, $"{hItem.FootNoteNumber})");
                    }
                    else {
                        builder.InsertHtml(hItem.Text);
                    }
                }

                //builder.InsertHtml($"{itemHtml}");
                //if (footNotes.IsNotNull() && footNotes.Count > 0) {
                //    foreach (var footNote in footNotes) {
                //        builder.InsertFootnote(FootnoteType.Footnote, footNote.Value, $"{footNote.Key})");
                //    }
                //}

                builder.EndRow();
            }

            table.AutoFit(Aspose.Words.Tables.AutoFitBehavior.AutoFitToWindow);
            builder.EndTable();

            return SaveBuilder(saveFormat, builder);
        }

        // https://localhost:44378/CompareVerse/PBD/470/24/14

        private List<VerseHtmlItemModel> GetVerseHtml(CompareVerseModel model, CompareVerseInfo verse) {
            var result = new List<VerseHtmlItemModel>();
            var footNotes = new Dictionary<int, string>();

            var text = verse.Text;
            if (text.Contains("<n>") && text.Contains("*")) {
                var footNoteTextPattern = GetFootnotesPattern();

                text = Regex.Replace(text, footNoteTextPattern, delegate (Match m) {
                    if (m.Groups != null && m.Groups.Count > 0) {
                        for (var i = 1; i < 8; i++) {
                            var groupName = $"f{i}";
                            if (m.Groups[groupName] != null && m.Groups[groupName].Success) {
                                var groupValue = m.Groups[groupName].Value;
                                footNotes.Add(footNoteIndex, groupValue);
                                footNoteIndex++;
                            }
                        }
                    }

                    var result = String.Empty;
                    return result;
                }, RegexOptions.IgnoreCase);
            }

            if (footNotes.Count > 0) {
                var t = text.Split(" ");
                foreach (var item in t) {
                    if (item.Contains("*")) {

                        var n = 0;
                        if (item.ContainsNonMark('*')) {
                            n = item.CountMark('*');
                            result.Add(new VerseHtmlItemModel() { Text = $"<span> {FormatVerseText(item.Replace("*",""), model.Part)}</span>" });
                        }
                        else {
                            n = item.Trim().Length;
                        }
                        
                        var note = String.Empty;
                        footNotes.TryGetValue(n, out note);
                        result.Add(new VerseHtmlItemModel() {
                            FootNoteNumber = n,
                            Footnote = true,
                            Text = FormatVerseText(note, model.Part)
                        });
                    }
                    else {
                        result.Add(new VerseHtmlItemModel() { Text = $"<span> {FormatVerseText(item, model.Part)}</span>"  });
                    }
                }
            }
            else {
                result.Add(new VerseHtmlItemModel() { Text = $"<span> {FormatVerseText(text, model.Part)}</span>" });
            }

            return result;
        }
        //private string GetVerseHtml(CompareVerseModel model, CompareVerseInfo verse, out Dictionary<int, string> notes) {
        //    notes = null;
        //    var footNotes = new Dictionary<int, string>();

        //    var text = " " + verse.Text;
        //    if (text.Contains("<n>") && text.Contains("*")) {
        //        var footNoteTextPattern = GetFootnotesPattern();

        //        text = Regex.Replace(text, footNoteTextPattern, delegate (Match m) {
        //            if (m.Groups != null && m.Groups.Count > 0) {
        //                for (var i = 1; i < 8; i++) {
        //                    var groupName = $"f{i}";
        //                    if (m.Groups[groupName] != null && m.Groups[groupName].Success) {
        //                        var groupValue = m.Groups[groupName].Value;
        //                        footNotes.Add(footNoteIndex, groupValue);
        //                        footNoteIndex++;
        //                    }
        //                }
        //            }

        //            var result = String.Empty;
        //            return result;
        //        }, RegexOptions.IgnoreCase);
        //    }

        //    text = FormatVerseText(text, model.Part);

        //    notes = footNotes;
        //    return text;
        //}
    }

    class VerseHtmlItemModel {
        public bool Footnote { get; set; }
        public int FootNoteNumber { get; set; } = 0;
        public string Text { get; set; }
    }
}
