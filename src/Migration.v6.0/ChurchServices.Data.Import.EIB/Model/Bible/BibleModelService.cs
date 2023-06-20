using ChurchServices.Extensions;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace ChurchServices.Data.Import.EIB.Model.Bible {
    public class BibleModelService : IDisposable {
        public BibleModelService() { }
        public BibleModel GetModelFromFile(string filePath) {
            if (filePath != null && File.Exists(filePath)) {
                XmlSerializer serializer = new(typeof(BibleModel));
                using (Stream reader = new FileStream(filePath, FileMode.Open)) {
                    return (BibleModel)serializer.Deserialize(reader);
                }
            }
            return default;
        }

        public BibleModel GetBibleModelFromOsisModel(Osis.OsisModel osisModel) {
            if (osisModel != null && osisModel.Text != null && osisModel.Text.WorkRef == "Bible" && osisModel.Text.Divisions != null && osisModel.Text.Divisions.Count > 0) {
                var model = new BibleModel() {
                    Shortcut = osisModel.Text.Header.Work.Work,
                    Name = osisModel.Text.Header.Work.Title,
                    Type = TranslationType.Literal,
                    Books = new List<BookModel>()
                };

                foreach (var div in osisModel.Text.Divisions) {
                    if (div.GetDivType() == Osis.OsisDivisionType.Book) {
                        var book = new BookModel() {
                            BookShortcut = Osis.OsisModelService.GetShortcutFromOsisAbbreviation(div.Id),
                            NumberOfBook = Osis.OsisModelService.GetBookNumberFromOsisAbbreviation(div.Id),
                            BookName = Osis.OsisModelService.GetBookName(div.Id),
                            Color = Osis.OsisModelService.GetBookColor(div.Id),
                            Chapters = new List<ChapterModel>()
                        };

                        model.Books.Add(book);

                        ChapterModel chapter = null;
                        PopulateDiv(div, chapter, book);
                    }
                }

                return model;
            }
            return default;
        }

        private ChapterModel PopulateDiv(Osis.OsisDivision div, ChapterModel chapter, BookModel book) {
            VerseModel verse = null;
            var chapterOrphans = new List<object>();
            var verseOrphans = new List<object>();
            foreach (var item in div.Items) {
                if (item is Osis.OsisVerse && (item as Osis.OsisVerse).ElementType == Osis.OsisElementType.Start) {
                    verse = new VerseModel() {
                        NumberOfVerse = (item as Osis.OsisVerse).Number,
                        Items = new List<object>()
                    };

                    if (verseOrphans.Count > 0) {
                        verse.Items.AddRange(verseOrphans);
                        verseOrphans = new List<object>();
                    }
                }
                else if (item is Osis.OsisVerse && (item as Osis.OsisVerse).ElementType == Osis.OsisElementType.End) {
                    if (verse != null) {
                        chapter.Items.Add(verse);
                        verse = null;
                    }
                }
                else if (item is Osis.OsisChapter && item is not Osis.OsisVerse && (item as Osis.OsisChapter).ElementType == Osis.OsisElementType.Start) {
                    chapter = new ChapterModel() {
                        NumberOfChapter = (item as Osis.OsisChapter).Number,
                        Items = new List<object>()
                    };

                    if (chapterOrphans.Count > 0) {
                        chapter.Items.AddRange(chapterOrphans);
                        chapterOrphans = new List<object>();
                    }
                }
                else if (item is Osis.OsisChapter && item is not Osis.OsisVerse && (item as Osis.OsisChapter).ElementType == Osis.OsisElementType.End) {
                    if (chapter != null) {
                        book.Chapters.Add(chapter);
                        chapter = null;
                    }
                }
                else if (item is Osis.OsisTitle) {
                    var title = new FormattedText((item as Osis.OsisTitle).Title);
                    if (chapter == null) { chapterOrphans.Add(title); }
                    else {
                        chapter.Items.Add(title);
                    }
                }
                else if (item is string) {
                    var span = new Span(item as string);
                    if (verse != null) {
                        verse.Items.Add(span);
                    }
                    else verseOrphans.Add(span);
                }
                else if (item is Osis.OsisNote) {
                    var note = PopulateNote(item as Osis.OsisNote);
                    if (verse != null) {
                        verse.Items.Add(note);
                    }
                    else verseOrphans.Add(note);
                }
                else if (item is Osis.OsisDivision) {
                    chapter = PopulateDiv(item as Osis.OsisDivision, chapter, book);
                }
            }
            return chapter;
        }

        private NoteModel PopulateNote(Osis.OsisNote osisNote) {
            var note = new NoteModel() {
                Number = osisNote.Number,
                Type = osisNote.IsCrossReference ? NoteType.CrossReference : NoteType.Default,
                Items = new List<object>()
            };
            foreach (var item in osisNote.Items) {
                if (item is string) {
                    var itemText = item as string;

                    // naprawa tekstu hebrajskiego
                    // https://en.wikipedia.org/wiki/Unicode_and_HTML_for_the_Hebrew_alphabet
                    // א ָר ָבּ zamieniamy na 
                    // <span lang="he" dir="rtl">&#64305;&#1464;&#1512;&#1464;&#1488;</span>
                    // oznacznikowanie tekstu hebrajskiego
                    var hrec = RecognizeHebrewAndGreekAndBibleTags(itemText);
                    if (hrec != null) {
                        note.Items.AddRange(hrec);
                    }
                    else {
                        note.Items.Add(new Span(itemText));
                    }
                }
                else if (item is Osis.OsisNoteReference) {
                    var refText = (item as Osis.OsisNoteReference).Ref;
                    refText = Osis.OsisModelService.ReplaceBookNames(refText);
                    note.Items.Add(new NoteReferenceModel() {
                        Ref = refText,
                        Text = (item as Osis.OsisNoteReference).Text
                    });
                }
            }

            for (int i = 0; i < note.Items.Count; i++) {
                var item = note.Items[i];
                if (item is string) {
                    var text = item as string;
                    text = Regex.Replace(text, @"\s\s", " ");
                    text = Regex.Replace(text, @"\(\s+", new MatchEvaluator(delegate (Match m) {
                        return "(";
                    }));
                    text = Regex.Replace(text, @"\s+\)", new MatchEvaluator(delegate (Match m) {
                        return ")";
                    }));
                    note.Items[i] = text;
                }
                else if (item is NoteReferenceModel) {
                    (item as NoteReferenceModel).Text = Regex.Replace((item as NoteReferenceModel).Text, @"\s\s", " ");
                }
            }

            return note;
        }
        private List<object> RecognizeGreek(object[] items) {
            if (items != null && items.Length > 0) {
                var result = new List<object>();
                foreach (var item in items) {
                    if (item is string) {
                        var r = RecognizeGreek(item.ToString());
                        if (r.IsNotNull()) {
                            result.AddRange(r);
                        }
                        else {
                            result.Add(item);
                        }
                    }
                    else {
                        result.Add(item);
                    }
                }
                return result.Count > items.Length ? result : null;
            }
            return null;
        }
        private object[] RecognizeGreek(string text) {
            var pattern = @"(?<greek>([\p{IsGreek}\p{IsGreekExtended}]\s?)+)";
            var matches = Regex.Matches(text, pattern);
            if (matches.Count > 0) {
                var result = new List<object>();
                int currentIndex = 0;
                foreach (Match match in matches) {
                    if (match.Success) {
                        var gr = match.Groups["greek"];
                        if (gr != null && gr.Success) {
                            if (match.Index > 0) {
                                var __text = text.Substring(currentIndex, match.Index - currentIndex);
                                result.Add(__text);
                            }
                            currentIndex = match.Index + match.Length;
                            var span = new Span(gr.Value);
                            span.MarkAsGreek();
                            result.Add(span);
                        }
                    }
                }
                if (currentIndex < text.Length) {
                    var __text = text.Substring(currentIndex);
                    result.Add(__text);
                }
                return result.Count > 0 ? result.ToArray() : null;
            }
            return null;
        }

#if DEBUG
        public object[] RecognizeHebrewAndGreekAndBibleTags(string text) {
#else
        private object[] RecognizeHebrewAndGreekAndBibleTags(string text) {
#endif
            var pattern = @"(?<he>([\u0590-\u05ff]\s?)+)";
            var matches = Regex.Matches(text, pattern);
            if (matches.Count > 0) {
                var result = new List<object>();
                var rmatches = new List<(string HtmlString, string NormalString, string MatchText, int IndexInOrginalString, int LengthInOrginalString)>();
                foreach (Match match in matches) {
                    if (match.Success) {
                        var gr = match.Groups["he"];
                        var g = gr.Value;

                        var hs = HtmlEncode(g);
                        if (hs != null) {
                            var ce = CSharpEncode(g);
                            var t = (HtmlString: hs, NormalString: ce, MatchText: g, IndexInOrginalString: gr.Index, LengthInOrginalString: gr.Length);
                            rmatches.Add(t);
                        }
                    }
                }

                if (rmatches.Count > 0) {
                    int currentIndex = 0;
                    foreach (var item in rmatches) {
                        if (item.IndexInOrginalString > 0) {
                            var __text = text.Substring(currentIndex, item.IndexInOrginalString - currentIndex);
                            if (result.Count > 0 && (result.Last() is Span || result.Last() is NoteReferenceModel)) {
                                __text = " " + __text;
                            }

                            var sig = RecognizeBibleTags(__text);
                            if (sig != null && sig.Length > 0) {
                                result.AddRange(sig);
                            }
                            else {
                                result.Add(__text);
                            }
                        }
                        currentIndex = item.IndexInOrginalString + item.LengthInOrginalString;
                        var span = new Span(item.NormalString) { Html = item.HtmlString };
                        span.MarkAsHebrew();
                        result.Add(span);
                    }
                    if (currentIndex < text.Length) {
                        var __text = text.Substring(currentIndex);
                        if (result.Count > 0 && (result.Last() is Span || result.Last() is NoteReferenceModel)) {
                            __text = " " + __text;
                        }

                        var sig = RecognizeBibleTags(__text);
                        if (sig != null && sig.Length > 0) {
                            result.AddRange(sig);
                        }
                        else {
                            result.Add(__text);
                        }
                    }

                    var gresults = RecognizeGreek(result.ToArray());
                    if (gresults != null && gresults.Count > 0) {
                        result = gresults;
                    }

                    return result.Count > 0 ? result.ToArray() : null;
                }
            }
            else {
                var sig = RecognizeBibleTags(text);
                if (sig != null && sig.Length > 0) {
                    var gresults = RecognizeGreek(sig);
                    if (gresults != null && gresults.Count > 0) {
                        sig = gresults.ToArray();
                    }
                    return sig;
                }
                {
                    var gresults = RecognizeGreek(text);
                    if (gresults != null && gresults.Length > 0) {
                        return gresults.ToArray();
                    }
                }
            }
            return null;
        }

        private bool IsVowel(int valueNext) {
            if (valueNext != 1463 && // Patach
                valueNext != 1464 && // Kamatz
                valueNext != 1461 && // Tzere
                valueNext != 1462 && // Segol
                valueNext != 1465 && // Holam haser
                valueNext != 1467 && // Kubutz                            
                valueNext != 1460 && // Hiriq haser
                valueNext != 1458 && // Hataf patach
                valueNext != 1459 && // Hataf kamatz
                valueNext != 1457 && // Hataf segol
                valueNext != 1456 && // Shwa
                valueNext != 1468 // Dagesh
                ) { return false; }
            return true;
        }

        private string HtmlEncode(string text) {
            char[] chars = HttpUtility.HtmlEncode(text).ToCharArray();
            StringBuilder result = new(text.Length + (int)(text.Length * 0.1));          

            for (int i = 0; i < chars.Length; i++) {
                int value = Convert.ToInt32(chars[i]);
                if (value == 32) {
                    if (i > 0 && i < chars.Length - 1) {
                        int valuePrev = Convert.ToInt32(chars[i - 1]);
                        int valueNext = Convert.ToInt32(chars[i + 1]);
                        if (!IsVowel(valueNext) && !IsVowel(valuePrev)) {
                            result.Insert(0, $"&#160;");
                        }
                    }
                }
                if (value > 127)
                    result.Insert(0, $"&#{value};");
            }

            return result.ToString();
        }

        private string CSharpEncode(string text) {
            char[] chars = HttpUtility.HtmlEncode(text).ToCharArray();
            var result = new StringBuilder(text.Length + (int)(text.Length * 0.1));

            for (int i = 0; i < chars.Length; i++) {
                int value = Convert.ToInt32(chars[i]);
                if (value == 32) {
                    if (i > 0 && i < chars.Length - 1) {
                        int valuePrev = Convert.ToInt32(chars[i - 1]);
                        int valueNext = Convert.ToInt32(chars[i + 1]);
                        if (!IsVowel(valueNext) && !IsVowel(valuePrev)) {
                            result.Insert(0, '\u00A0');
                        }
                    }
                }
                if (value > 127) { result.Insert(0, chars[i]); }
            }

            return result.ToString();
        }

        private object[] RecognizeBibleTags(string text) {
            const string pattern = @"(?<book>([0-9]\s+)?[0-9\u0041-\u005A\u0061-\u007A\u00AA\u00B5\u00BA\u00C0-\u00D6\u00D8-\u00F6\u00F8-\u02C1\u02C6-\u02D1\u02E0-\u02E4\u02EC\u02EE\u0370-\u0374\u0376\u0377\u037A-\u037D\u0386\u0388-\u038A\u038C\u038E-\u03A1\u03A3-\u03F5\u03F7-\u0481\u048A-\u0527\u0531-\u0556\u0559\u0561-\u0587\u05D0-\u05EA\u05F0-\u05F2\u0620-\u064A\u066E\u066F\u0671-\u06D3\u06D5\u06E5\u06E6\u06EE\u06EF\u06FA-\u06FC\u06FF\u0710\u0712-\u072F\u074D-\u07A5\u07B1\u07CA-\u07EA\u07F4\u07F5\u07FA\u0800-\u0815\u081A\u0824\u0828\u0840-\u0858\u08A0\u08A2-\u08AC\u0904-\u0939\u093D\u0950\u0958-\u0961\u0971-\u0977\u0979-\u097F\u0985-\u098C\u098F\u0990\u0993-\u09A8\u09AA-\u09B0\u09B2\u09B6-\u09B9\u09BD\u09CE\u09DC\u09DD\u09DF-\u09E1\u09F0\u09F1\u0A05-\u0A0A\u0A0F\u0A10\u0A13-\u0A28\u0A2A-\u0A30\u0A32\u0A33\u0A35\u0A36\u0A38\u0A39\u0A59-\u0A5C\u0A5E\u0A72-\u0A74\u0A85-\u0A8D\u0A8F-\u0A91\u0A93-\u0AA8\u0AAA-\u0AB0\u0AB2\u0AB3\u0AB5-\u0AB9\u0ABD\u0AD0\u0AE0\u0AE1\u0B05-\u0B0C\u0B0F\u0B10\u0B13-\u0B28\u0B2A-\u0B30\u0B32\u0B33\u0B35-\u0B39\u0B3D\u0B5C\u0B5D\u0B5F-\u0B61\u0B71\u0B83\u0B85-\u0B8A\u0B8E-\u0B90\u0B92-\u0B95\u0B99\u0B9A\u0B9C\u0B9E\u0B9F\u0BA3\u0BA4\u0BA8-\u0BAA\u0BAE-\u0BB9\u0BD0\u0C05-\u0C0C\u0C0E-\u0C10\u0C12-\u0C28\u0C2A-\u0C33\u0C35-\u0C39\u0C3D\u0C58\u0C59\u0C60\u0C61\u0C85-\u0C8C\u0C8E-\u0C90\u0C92-\u0CA8\u0CAA-\u0CB3\u0CB5-\u0CB9\u0CBD\u0CDE\u0CE0\u0CE1\u0CF1\u0CF2\u0D05-\u0D0C\u0D0E-\u0D10\u0D12-\u0D3A\u0D3D\u0D4E\u0D60\u0D61\u0D7A-\u0D7F\u0D85-\u0D96\u0D9A-\u0DB1\u0DB3-\u0DBB\u0DBD\u0DC0-\u0DC6\u0E01-\u0E30\u0E32\u0E33\u0E40-\u0E46\u0E81\u0E82\u0E84\u0E87\u0E88\u0E8A\u0E8D\u0E94-\u0E97\u0E99-\u0E9F\u0EA1-\u0EA3\u0EA5\u0EA7\u0EAA\u0EAB\u0EAD-\u0EB0\u0EB2\u0EB3\u0EBD\u0EC0-\u0EC4\u0EC6\u0EDC-\u0EDF\u0F00\u0F40-\u0F47\u0F49-\u0F6C\u0F88-\u0F8C\u1000-\u102A\u103F\u1050-\u1055\u105A-\u105D\u1061\u1065\u1066\u106E-\u1070\u1075-\u1081\u108E\u10A0-\u10C5\u10C7\u10CD\u10D0-\u10FA\u10FC-\u1248\u124A-\u124D\u1250-\u1256\u1258\u125A-\u125D\u1260-\u1288\u128A-\u128D\u1290-\u12B0\u12B2-\u12B5\u12B8-\u12BE\u12C0\u12C2-\u12C5\u12C8-\u12D6\u12D8-\u1310\u1312-\u1315\u1318-\u135A\u1380-\u138F\u13A0-\u13F4\u1401-\u166C\u166F-\u167F\u1681-\u169A\u16A0-\u16EA\u1700-\u170C\u170E-\u1711\u1720-\u1731\u1740-\u1751\u1760-\u176C\u176E-\u1770\u1780-\u17B3\u17D7\u17DC\u1820-\u1877\u1880-\u18A8\u18AA\u18B0-\u18F5\u1900-\u191C\u1950-\u196D\u1970-\u1974\u1980-\u19AB\u19C1-\u19C7\u1A00-\u1A16\u1A20-\u1A54\u1AA7\u1B05-\u1B33\u1B45-\u1B4B\u1B83-\u1BA0\u1BAE\u1BAF\u1BBA-\u1BE5\u1C00-\u1C23\u1C4D-\u1C4F\u1C5A-\u1C7D\u1CE9-\u1CEC\u1CEE-\u1CF1\u1CF5\u1CF6\u1D00-\u1DBF\u1E00-\u1F15\u1F18-\u1F1D\u1F20-\u1F45\u1F48-\u1F4D\u1F50-\u1F57\u1F59\u1F5B\u1F5D\u1F5F-\u1F7D\u1F80-\u1FB4\u1FB6-\u1FBC\u1FBE\u1FC2-\u1FC4\u1FC6-\u1FCC\u1FD0-\u1FD3\u1FD6-\u1FDB\u1FE0-\u1FEC\u1FF2-\u1FF4\u1FF6-\u1FFC\u2071\u207F\u2090-\u209C\u2102\u2107\u210A-\u2113\u2115\u2119-\u211D\u2124\u2126\u2128\u212A-\u212D\u212F-\u2139\u213C-\u213F\u2145-\u2149\u214E\u2183\u2184\u2C00-\u2C2E\u2C30-\u2C5E\u2C60-\u2CE4\u2CEB-\u2CEE\u2CF2\u2CF3\u2D00-\u2D25\u2D27\u2D2D\u2D30-\u2D67\u2D6F\u2D80-\u2D96\u2DA0-\u2DA6\u2DA8-\u2DAE\u2DB0-\u2DB6\u2DB8-\u2DBE\u2DC0-\u2DC6\u2DC8-\u2DCE\u2DD0-\u2DD6\u2DD8-\u2DDE\u2E2F\u3005\u3006\u3031-\u3035\u303B\u303C\u3041-\u3096\u309D-\u309F\u30A1-\u30FA\u30FC-\u30FF\u3105-\u312D\u3131-\u318E\u31A0-\u31BA\u31F0-\u31FF\u3400-\u4DB5\u4E00-\u9FCC\uA000-\uA48C\uA4D0-\uA4FD\uA500-\uA60C\uA610-\uA61F\uA62A\uA62B\uA640-\uA66E\uA67F-\uA697\uA6A0-\uA6E5\uA717-\uA71F\uA722-\uA788\uA78B-\uA78E\uA790-\uA793\uA7A0-\uA7AA\uA7F8-\uA801\uA803-\uA805\uA807-\uA80A\uA80C-\uA822\uA840-\uA873\uA882-\uA8B3\uA8F2-\uA8F7\uA8FB\uA90A-\uA925\uA930-\uA946\uA960-\uA97C\uA984-\uA9B2\uA9CF\uAA00-\uAA28\uAA40-\uAA42\uAA44-\uAA4B\uAA60-\uAA76\uAA7A\uAA80-\uAAAF\uAAB1\uAAB5\uAAB6\uAAB9-\uAABD\uAAC0\uAAC2\uAADB-\uAADD\uAAE0-\uAAEA\uAAF2-\uAAF4\uAB01-\uAB06\uAB09-\uAB0E\uAB11-\uAB16\uAB20-\uAB26\uAB28-\uAB2E\uABC0-\uABE2\uAC00-\uD7A3\uD7B0-\uD7C6\uD7CB-\uD7FB\uF900-\uFA6D\uFA70-\uFAD9\uFB00-\uFB06\uFB13-\uFB17\uFB1D\uFB1F-\uFB28\uFB2A-\uFB36\uFB38-\uFB3C\uFB3E\uFB40\uFB41\uFB43\uFB44\uFB46-\uFBB1\uFBD3-\uFD3D\uFD50-\uFD8F\uFD92-\uFDC7\uFDF0-\uFDFB\uFE70-\uFE74\uFE76-\uFEFC\uFF21-\uFF3A\uFF41-\uFF5A\uFF66-\uFFBE\uFFC2-\uFFC7\uFFCA-\uFFCF\uFFD2-\uFFD7\uFFDA-\uFFDC]+)?\s+(?<chapter>[0-9]+)\:(?<verseStart>[0-9]+)(\-(?<verseEnd>[0-9]+))?(?<lit>\s+L[\.\s])?";
            if (text != null) {
                if (text.Trim().Length < 2) { return null; }
                if (!text.Any(Char.IsDigit)) { return null; }

                var matches = Regex.Matches(text, pattern);
                if (matches.Count > 0) {
                    var result = new List<object>();
                    var currentBook = "";
                    int currentIndex = 0;

                    foreach (Match match in matches) {
                        if (match.Success) {
                            if (match.Index > 0) {
                                var __text = text.Substring(currentIndex, match.Index - currentIndex);
                                RecognizeSequentialVerseNumbers(ref result, ref __text);
                                if (__text.IsNotNullOrEmpty()) result.Add(__text);
                            }
                            currentIndex = match.Index + match.Length;

                            var lit = match.Groups["lit"] != null && match.Groups["lit"].Success;
                            var book = match.Groups["book"] != null && match.Groups["book"].Success ? match.Groups["book"].Value : currentBook;
                            if (book.IsNotNullOrEmpty() && currentBook != book) { currentBook = book; }
                            var chapter = match.Groups["chapter"] != null && match.Groups["chapter"].Success ? match.Groups["chapter"].Value : null;
                            var verseStart = match.Groups["verseStart"] != null && match.Groups["verseStart"].Success ? match.Groups["verseStart"].Value : null;
                            var verseEnd = match.Groups["verseEnd"] != null && match.Groups["verseEnd"].Success ? match.Groups["verseEnd"].Value : null;

                            // tu trzeba zwalidować informację ... co udało się rozpoznać

                            NoteReferenceModel refModel = new(match.Value.Trim(), lit);
                            var refText = $"{GetShortcutFromEIBAbbreviation(currentBook)}.{chapter}";
                            if (verseStart != null) {
                                refText += $".{verseStart}";
                            }
                            if (verseEnd != null) {
                                refText += $"-{GetShortcutFromEIBAbbreviation(currentBook)}.{chapter}.{verseEnd}";
                            }
                            refModel.Ref = refText;
                            result.Add(refModel);
                        }
                    }
                    if (currentIndex < text.Length) {
                        var __text = text.Substring(currentIndex);
                        RecognizeSequentialVerseNumbers(ref result, ref __text);
                        result.Add(__text);
                    }

                    return result.Count > 0 ? result.ToArray() : null;
                }
            }
            return null;
        }

        private void RecognizeSequentialVerseNumbers(ref List<object> result, ref string __text) {
            if (result.Count > 0 && result.Last() is NoteReferenceModel && __text.IsNotNullOrWhiteSpace() && __text.Any(Char.IsDigit)) {
                var versesMatchesAdded = false;
                var versesPattern = @"^((\s+)?(,)?(\s+)?(?<verse>[0-9]+))+";
                var versesMatches = Regex.Matches(__text, versesPattern);
                if (versesMatches.Count > 0) {
                    var __last = result.Last() as NoteReferenceModel;
                    foreach (Match verseMatch in versesMatches) {
                        if (verseMatch.Success) {
                            var verseGroup = verseMatch.Groups["verse"];
                            if (verseGroup != null && verseGroup.Success) {
                                foreach (Capture capture in verseGroup.Captures) {
                                    var verseNumber = capture.Value;
                                    var __ref = new NoteReferenceModel(capture.Value) {
                                        Ref = $"{__last.Index.BookShortcut}.{__last.Index.ChapterNumber}.{verseNumber}"
                                    };
                                    result.Add(", ");
                                    result.Add(__ref);
                                    versesMatchesAdded = true;
                                }
                            }
                        }
                    }
                }
                if (versesMatchesAdded) {
                    __text = Regex.Replace(__text, versesPattern, new MatchEvaluator(delegate (Match m) { return String.Empty; }));
                }
            }
        }

        public BibleModel GetBibleModelFromFile(string filePath) {
            if (filePath != null && File.Exists(filePath)) {
                XmlSerializer serializer = new XmlSerializer(typeof(BibleModel));
                using (Stream reader = new FileStream(filePath, FileMode.Open)) {
                    return (BibleModel)serializer.Deserialize(reader);
                }

            }
            return default;
        }

        public void SaveBibleModelToFile(BibleModel model, string filePath) {
            if (model != null && Directory.Exists(Path.GetDirectoryName(filePath))) {
                XmlSerializer serializer = new XmlSerializer(typeof(BibleModel));

                using (Stream fs = new FileStream(filePath, FileMode.Create)) {

                    var settings = new XmlWriterSettings {
                        OmitXmlDeclaration = false,
                        Indent = true,
                        NewLineOnAttributes = false,
                        Encoding = Encoding.UTF8
                    };

                    using (XmlWriter writer = XmlWriter.Create(fs, settings)) {
                        serializer.Serialize(writer, model);
                    }
                }
            }
        }

        private string ReplaceEIBBookNames(string text) {
            var pattern = "(?<book>";
            foreach (var item in BookNames) {
                pattern += @$"({item})?";
            }
            pattern += @")\s+";

            var result = Regex.Replace(text, pattern, delegate (Match m) {
                return GetShortcutFromEIBAbbreviation(m.Groups["book"].Value) + " ";
            });
            return result;
        }
        public string[] BookNames = {
            "Rdz", "Wj", "Kpł", "Lb", "Pwt",
            "Joz", "Sdz", "Rt", "1Sm", "2Sm",
            "1Krl", "2Krl", "1Krn", "2Krn",
            "Ezd", "Ne", "Est", "Jb", "Ps",
            "Prz", "Kzn", "Pnp", "Iz", "Jr",
            "Tr", "Ez", "Dn", "Oz", "Jl",
            "Am", "Ab","Jon", "Mi","Na","Ha",
            "So", "Ag", "Za", "Ma", "Mt",
            "Mk", "Łk", "J","Dz","Rz", "1Kor",
            "2Kor", "Ga", "Ef", "Flp", "Kol",
            "1Ts", "2Ts", "1Tm", "2Tm", "Tt",
            "Flm", "Hbr", "Jk", "1P", "2P",
            "1J", "2J", "3J", "Jd", "Obj"};
        private string GetShortcutFromEIBAbbreviation(string nameOfBook) {
            if (nameOfBook.Contains("Rdz")) { return "Ge"; }
            if (nameOfBook.Contains("Wj")) { return "Ex"; }
            if (nameOfBook.Contains("Kpł")) { return "Le"; }
            if (nameOfBook.Contains("Lb")) { return "Nu"; }
            if (nameOfBook.Contains("Pwt")) { return "Dt"; }

            if (nameOfBook.Contains("Joz")) { return "Jos"; }
            if (nameOfBook.Contains("Sdz")) { return "Jdg"; }
            if (nameOfBook.Contains("Rt")) { return "Ru"; }

            if (nameOfBook.Contains("1Sm")) { return "1Sa"; }
            if (nameOfBook.Contains("2Sm")) { return "2Sa"; }
            if (nameOfBook.Contains("1Krl")) { return "1Ki"; }
            if (nameOfBook.Contains("2Krl")) { return "2Ki"; }
            if (nameOfBook.Contains("1Krn")) { return "1Ch"; }
            if (nameOfBook.Contains("2Krn")) { return "2Ch"; }


            if (nameOfBook.Contains("Ezd")) { return "Ezr"; }
            if (nameOfBook.Contains("Ne")) { return "Ne"; }
            if (nameOfBook.Contains("Est")) { return "Es"; }

            if (nameOfBook.Contains("Jb")) { return "Job"; }
            if (nameOfBook.Contains("Ps")) { return "Ps"; }
            if (nameOfBook.Contains("Prz")) { return "Pr"; }
            if (nameOfBook.Contains("Kzn")) { return "Ec"; }
            if (nameOfBook.Contains("Pnp")) { return "So"; }

            if (nameOfBook.Contains("Iz")) { return "Is"; }
            if (nameOfBook.Contains("Jr")) { return "Je"; }
            if (nameOfBook.Contains("Tr")) { return "La"; }
            if (nameOfBook.Contains("Ez")) { return "Eze"; }
            if (nameOfBook.Contains("Dn")) { return "Da"; }
            if (nameOfBook.Contains("Oz")) { return "Ho"; }
            if (nameOfBook.Contains("Jl")) { return "Joe"; }
            if (nameOfBook.Contains("Am")) { return "Am"; }
            if (nameOfBook.Contains("Ab")) { return "Ob"; }
            if (nameOfBook.Contains("Jon")) { return "Jon"; }
            if (nameOfBook.Contains("Mi")) { return "Mic"; }
            if (nameOfBook.Contains("Na")) { return "Na"; }
            if (nameOfBook.Contains("Ha")) { return "Hab"; }
            if (nameOfBook.Contains("So")) { return "Zep"; }
            if (nameOfBook.Contains("Ag")) { return "Hag"; }
            if (nameOfBook.Contains("Za")) { return "Zec"; }
            if (nameOfBook.Contains("Ma")) { return "Mal"; }

            if (nameOfBook.Contains("Mt")) { return "Mt"; }
            if (nameOfBook.Contains("Mk")) { return "Mk"; }
            if (nameOfBook.Contains("Łk")) { return "Lk"; }
            if (nameOfBook.Contains("1J")) { return "1Jn"; }
            if (nameOfBook.Contains("2J")) { return "2Jn"; }
            if (nameOfBook.Contains("3J")) { return "3Jn"; }
            if (nameOfBook=="J") { return "Jn"; }
            if (nameOfBook.Contains("Dz")) { return "Ac"; }
            if (nameOfBook.Contains("Rz")) { return "Ro"; }
            if (nameOfBook.Contains("1Kor")) { return "1Co"; }
            if (nameOfBook.Contains("2Kor")) { return "2Co"; }
            if (nameOfBook.Contains("Ga")) { return "Ga"; }
            if (nameOfBook.Contains("Ef")) { return "Eph"; }
            if (nameOfBook.Contains("Flp")) { return "Php"; }
            if (nameOfBook.Contains("Kol")) { return "Col"; }
            if (nameOfBook.Contains("1Ts")) { return "1Th"; }
            if (nameOfBook.Contains("2Ts")) { return "2Th"; }
            if (nameOfBook.Contains("1Tm")) { return "1Ti"; }
            if (nameOfBook.Contains("2Tm")) { return "2Ti"; }
            if (nameOfBook.Contains("Tt")) { return "Tt"; }
            if (nameOfBook.Contains("Flm")) { return "Phm"; }
            if (nameOfBook.Contains("Hbr")) { return "Heb"; }
            if (nameOfBook.Contains("Jk")) { return "Jas"; }
            if (nameOfBook.Contains("1P")) { return "1Pe"; }
            if (nameOfBook.Contains("2P")) { return "2Pe"; }
            //if (nameOfBook.Contains("1J")) { return "1Jn"; }
            //if (nameOfBook.Contains("2J")) { return "2Jn"; }
            //if (nameOfBook.Contains("3J")) { return "3Jn"; }
            if (nameOfBook.Contains("Jd")) { return "Jud"; }
            if (nameOfBook.Contains("Obj")) { return "Re"; }

            return default;
        }

        public void Dispose() { }
    }
}
