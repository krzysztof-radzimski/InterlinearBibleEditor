/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.Data.Export {
    public class DefaultExporter : BaseDefaultExporter {
        private int footNoteIndex = 1;
        private DefaultExporter() : base() { }
        public DefaultExporter(byte[] asposeLicense, string host) : base(asposeLicense, host) { }

        protected override void ExportVerse(Verse verse, ref Paragraph par, DocumentBuilder builder) {
            var footNotes = new Dictionary<int, string>();

            var book = verse.ParentChapter.ParentBook;
            if (verse.ParentChapter.Subtitles.Count > 0) {
                var subtitles = verse.ParentChapter.Subtitles.Where(x => x.BeforeVerseNumber == verse.NumberOfVerse).OrderBy(x => x.Level);
                if (subtitles.Any()) {
                    var _par = builder.InsertParagraph();
                    _par.ParagraphFormat.Style = builder.Document.Styles["Nagłówek 3"];
                    _par.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                    _par.ParagraphFormat.KeepWithNext = true;

                    foreach (var subtitle in subtitles) {
                        var storyText = subtitle.Text;
                        // <x>230 1-41</x>
                        if (storyText.Contains("<x>")) {
                            var pattern = @"\<x\>(?<book>[0-9]+)\s(?<num>[0-9]+\-[0-9]+)\<\/x\>";
                            var pattern2 = @"\<x\>(?<book>[0-9]+)\s(?<num>[0-9]+(\s)?\:(\s)?[0-9]+\-[0-9]+)\<\/x\>";

                            storyText = Regex.Replace(storyText, pattern, delegate (Match m) {
                                return $"({verse.ParentTranslation.Books.Where(x => x.NumberOfBook == Convert.ToInt32(m.Groups["book"].Value)).First().BookName} {m.Groups["num"].Value})";
                            }, RegexOptions.IgnoreCase);
                            storyText = Regex.Replace(storyText, pattern2, delegate (Match m) {
                                return $"({verse.ParentTranslation.Books.Where(x => x.NumberOfBook == Convert.ToInt32(m.Groups["book"].Value)).First().BookName} {m.Groups["num"].Value})";
                            }, RegexOptions.IgnoreCase);
                        }

                        if (book.BaseBook.Status.BiblePart == BiblePart.OldTestament) {
                            storyText = Regex.Replace(storyText, @"\sPAN(A)?(EM)?(U)?(IE)?", delegate (Match m) {
                                return " JAHWE";
                            });
                        }

                        var run = new Run(builder.Document) {
                            Text = storyText
                        };
                        run.Font.Bold = true;

                        _par.AppendChild(run);
                    }

                    par = builder.InsertParagraph();
                    par.ParagraphFormat.Style = builder.Document.Styles["Normal"];
                    par.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                    par.ParagraphFormat.KeepWithNext = false;
                    builder.MoveTo(par);
                }
            }


            var text = " " + verse.Text;
            if (text.Contains("<n>") && text.Contains("*")) {
                var footNoteTextPatternFragment = @"\w\s\.\=\""\,\;\:\-\(\)\<\>\„\”\/\!";
                var f1 = $@"\[\*\s?(?<f1>[{footNoteTextPatternFragment}]+)\]";
                var f2 = $@"\[\*\*\s?(?<f2>[{footNoteTextPatternFragment}]+)\]";
                var f3 = $@"\[\*\*\*\s?(?<f3>[{footNoteTextPatternFragment}]+)\]";
                var f4 = $@"\[\*\*\*\*\s?(?<f4>[{footNoteTextPatternFragment}]+)\]";
                var f5 = $@"\[\*\*\*\*\*\s?(?<f4>[{footNoteTextPatternFragment}]+)\]";
                var footNoteTextPattern = $@"\<n\>{f1}(\s+)?({f2})?(\s+)?({f3})?(\s+)?({f4})?(\s+)?({f5})?\</n\>";

                text = Regex.Replace(text, footNoteTextPattern, delegate (Match m) {
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
                }, RegexOptions.IgnoreCase);
            }

            // Słowa Jezusa
            text = text.Replace("<J>", @"<span style=""color: darkred;"">").Replace("</J>", "</span>");

            // Elementy dodane
            text = text.Replace("<n>", @"<span style=""color: darkgray;"">").Replace("</n>", "</span>");

            text = text.Replace("<pb/>", "").Replace("<t>", "").Replace("</t>", "").Replace("<e>", "").Replace("</e>", "");

            // zamiana na imię Boże
            if (book.BaseBook.Status.BiblePart == BiblePart.OldTestament) {
                text = Regex.Replace(text, @"(?<prefix>[\s\”\""\„ʼ])(?<name>PAN(A)?(EM)?(U)?(IE)?)[\s\,\.\:\""\'\”ʼ]", delegate (Match m) {
                    var prefix = m.Groups["prefix"].Value;
                    return $"{prefix}JAHWE{m.Value.Last()}";
                });
            }
            if (book.BaseBook.Status.BiblePart == BiblePart.OldTestament) {
                text = Regex.Replace(text, @"(?<prefix>[\s\”\""\„ʼ])(?<name>JHWH)[\s\,\.\:\""\'\”ʼ]", delegate (Match m) {
                    var prefix = m.Groups["prefix"].Value;
                    return $"{prefix}JAHWE{m.Value.Last()}";
                });
            }
            if (book.BaseBook.Status.BiblePart == BiblePart.OldTestament) {
                text = Regex.Replace(text, @"(?<prefix>[\s\”\""\„ʼ])(?<name>Jehow(a)?(y)?(ie)?(ę)?(o)?)[\s\,\.\:\""\'\”ʼ]", delegate (Match m) {
                    var prefix = m.Groups["prefix"].Value;
                    return $"{prefix}JAHWE{m.Value.Last()}";
                });
            }
            if (book.BaseBook.Status.BiblePart == BiblePart.NewTestament) {
                text = Regex.Replace(text, @"(?<prefix>[\s\”\""\„ʼ])(?<name>Jehow(?<ending>(a)?(y)?(ie)?(ę)?(o)?))[\s\,\.\:\""\'\”ʼ]", delegate (Match m) {
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
            text = Regex.Replace(text, @"[\s\(\,\;][a,i,o,w,z]\s", delegate (Match m) {
                return " " + m.Value.Trim() + "&nbsp;";
            }, RegexOptions.IgnoreCase);

            // usuwam puste przypisy
            text = Regex.Replace(text, @"\[[0-9]+\]", delegate (Match m) {
                return String.Empty;
            }, RegexOptions.IgnoreCase);


            builder.CurrentParagraph.ParagraphFormat.Alignment = ParagraphAlignment.Justify;
            builder.CurrentParagraph.ParagraphFormat.LineSpacingRule = LineSpacingRule.Multiple;
            builder.CurrentParagraph.ParagraphFormat.LineSpacing = 18;

            builder.InsertHtml($"<b>{verse.NumberOfVerse}</b>.&nbsp;");

            builder.InsertHtml($"{text}");
            if (footNotes.Count > 0) {
                foreach (var item in footNotes) {
                    builder.InsertFootnote(FootnoteType.Footnote, item.Value, $"{item.Key})");
                }
            }
            builder.Write(" ");
        }
    }
}
