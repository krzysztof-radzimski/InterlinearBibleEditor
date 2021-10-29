using Aspose.Words;
using Aspose.Words.Notes;
using IBE.Common.Extensions;
using IBE.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBE.Data.Export {
    public class CompareVersesExporter : BaseExporter {
        private int footNoteIndex = 1;
        private CompareVersesExporter() : base() { }
        public CompareVersesExporter(byte[] asposeLicense, string host) : base(asposeLicense, host) { }

        public byte[] Export(CompareVerseModel model, ExportSaveFormat saveFormat) {
            if (model.IsNull()) { throw new ArgumentNullException("model"); }
            var builder = GetDocumentBuilder();

            var first = model.Verses.First();
            var title = $"Porównanie tłumaczeń {first.GetBookName()} {first.ParentChapter.NumberOfChapter}:{first.NumberOfVerse}";
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
                var index = item.GetVerseIndex();
                if (index.IsNull()) { continue; }
                var translationName = item.ParentTranslation.Name;
                var translationDesc = item.ParentTranslation.Description;
                var translationType = item.ParentTranslation.Type.GetDescription();
                Dictionary<int, string> footNotes = null;
                var itemHtml = GetVerseHtml(item, out footNotes);

                builder.InsertCell();
                builder.Writeln(translationName);
                builder.InsertCell();
                builder.Writeln(translationType);
                builder.InsertCell();
                builder.Writeln(translationDesc);
                builder.InsertCell();

                builder.InsertHtml($"{itemHtml}");
                if (footNotes.IsNotNull() && footNotes.Count > 0) {
                    foreach (var footNote in footNotes) {
                        builder.InsertFootnote(FootnoteType.Footnote, footNote.Value, $"{footNote.Key})");
                    }
                }

                builder.EndRow();
            }

            table.AutoFit(Aspose.Words.Tables.AutoFitBehavior.AutoFitToWindow);
            builder.EndTable();

            return SaveBuilder(saveFormat, builder);
        }

        private string GetVerseHtml(Verse verse, out Dictionary<int, string> notes) {
            notes = null;
            var footNotes = new Dictionary<int, string>();
            var book = verse.ParentChapter.ParentBook;

            var text = " " + verse.Text;
            if (text.Contains("<n>") && text.Contains("*")) {
                var footNoteTextPatternFragment = @"\w\s\.\=\""\,\;\:\-\(\)\<\>\„\”\/\!";
                var f1 = $@"\[\*\s?(?<f1>[{footNoteTextPatternFragment}]+)\]";
                var f2 = $@"\[\*\*\s?(?<f2>[{footNoteTextPatternFragment}]+)\]";
                var f3 = $@"\[\*\*\*\s?(?<f3>[{footNoteTextPatternFragment}]+)\]";
                var f4 = $@"\[\*\*\*\*\s?(?<f4>[{footNoteTextPatternFragment}]+)\]";
                var f5 = $@"\[\*\*\*\*\*\s?(?<f4>[{footNoteTextPatternFragment}]+)\]";
                var footNoteTextPattern = $@"\<n\>{f1}(\s+)?({f2})?(\s+)?({f3})?(\s+)?({f4})?(\s+)?({f5})?\</n\>";

                text = System.Text.RegularExpressions.Regex.Replace(text, footNoteTextPattern, delegate (System.Text.RegularExpressions.Match m) {
                    if (m.Groups != null && m.Groups.Count > 0) {
                        if (m.Groups["f1"] != null && m.Groups["f1"].Success) {
                            footNotes.Add(footNoteIndex, $@"{m.Groups["f1"].Value}");
                            footNoteIndex++;
                        }
                        if (m.Groups["f2"] != null && m.Groups["f2"].Success) {
                            footNotes.Add(footNoteIndex, $@"{m.Groups["f2"].Value}");
                            footNoteIndex++;
                        }
                        if (m.Groups["f3"] != null && m.Groups["f3"].Success) {
                            footNotes.Add(footNoteIndex, $@"{m.Groups["f3"].Value}");
                            footNoteIndex++;
                        }
                        if (m.Groups["f4"] != null && m.Groups["f4"].Success) {
                            footNotes.Add(footNoteIndex, $@"{m.Groups["f4"].Value}");
                            footNoteIndex++;
                        }
                        if (m.Groups["f5"] != null && m.Groups["f5"].Success) {
                            footNotes.Add(footNoteIndex, $@"{m.Groups["f5"].Value}</p>");
                            footNoteIndex++;
                        }
                    }

                    var result = String.Empty;
                    return result;
                }, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            }

            // Słowa Jezusa
            text = text.Replace("<J>", @"<span style=""color: darkred;"">").Replace("</J>", "</span>");

            // Elementy dodane
            text = text.Replace("<n>", @"<span style=""color: darkgray;"">").Replace("</n>", "</span>");

            text = text.Replace("<pb/>", "").Replace("<t>", "").Replace("</t>", "").Replace("<e>", "").Replace("</e>", "");

            // zamiana na imię Boże
            if (book.BaseBook.Status.BiblePart == IBE.Data.Model.BiblePart.OldTestament) {
                text = System.Text.RegularExpressions.Regex.Replace(text, @"(?<prefix>[\s\”\""\„ʼ])(?<name>PAN(A)?(EM)?(U)?(IE)?)[\s\,\.\:\""\'\”ʼ]", delegate (System.Text.RegularExpressions.Match m) {
                    var prefix = m.Groups["prefix"].Value;
                    return $"{prefix}JAHWE{m.Value.Last()}";
                });
            }
            if (book.BaseBook.Status.BiblePart == IBE.Data.Model.BiblePart.OldTestament) {
                text = System.Text.RegularExpressions.Regex.Replace(text, @"(?<prefix>[\s\”\""\„ʼ])(?<name>JHWH)[\s\,\.\:\""\'\”ʼ]", delegate (System.Text.RegularExpressions.Match m) {
                    var prefix = m.Groups["prefix"].Value;
                    return $"{prefix}JAHWE{m.Value.Last()}";
                });
            }
            if (book.BaseBook.Status.BiblePart == IBE.Data.Model.BiblePart.OldTestament) {
                text = System.Text.RegularExpressions.Regex.Replace(text, @"(?<prefix>[\s\”\""\„ʼ])(?<name>Jehow(a)?(y)?(ie)?(ę)?(o)?)[\s\,\.\:\""\'\”ʼ]", delegate (System.Text.RegularExpressions.Match m) {
                    var prefix = m.Groups["prefix"].Value;
                    return $"{prefix}JAHWE{m.Value.Last()}";
                });
            }
            if (book.BaseBook.Status.BiblePart == IBE.Data.Model.BiblePart.NewTestament) {
                text = System.Text.RegularExpressions.Regex.Replace(text, @"(?<prefix>[\s\”\""\„ʼ])(?<name>Jehow(?<ending>(a)?(y)?(ie)?(ę)?(o)?))[\s\,\.\:\""\'\”ʼ]", delegate (System.Text.RegularExpressions.Match m) {
                    var prefix = m.Groups["prefix"].Value;
                    var ending = m.Groups["ending"].Value;
                    var root = "Pan";
                    if (ending == "ie") { root += "u"; }
                    if (ending == "o") { root += "ie"; }
                    if (ending == "y" || ending == "ę") { root += "a"; }
                    return $"{prefix}{root}{m.Value.Last()}";
                });
            }

            // usuwam sierotki
            text = System.Text.RegularExpressions.Regex.Replace(text, @"[\s\(\,\;][a,i,o,w,z]\s", delegate (System.Text.RegularExpressions.Match m) {
                return " " + m.Value.Trim() + "&nbsp;";
            }, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            // usuwam puste przypisy
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\[[0-9]+\]", delegate (System.Text.RegularExpressions.Match m) {
                return String.Empty;
            }, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            notes = footNotes;
            return text;
        }
    }
}
