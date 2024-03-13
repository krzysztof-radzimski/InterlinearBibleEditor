namespace ChurchServices.Data.Export {
    public class ArticleExporter : BaseExporter {
        public ArticleExporter() : base() { }
        public ArticleExporter(byte[] asposeLicense, string host) : base(asposeLicense, host) { }
        public byte[] Export(Article article, ExportSaveFormat saveFormat) {
            if (article != null) {
                var builder = GetDocumentBuilder();

                var normalStyle = builder.Document.Styles[StyleIdentifier.Normal];
                if (normalStyle.IsNotNull()) {
                    normalStyle.Font.Size = 14;
                }

                builder.ParagraphFormat.Style = builder.Document.Styles["Nagłówek 1"];
                builder.Writeln($"{article.Subject}");
                builder.ParagraphFormat.ClearFormatting();
                builder.InsertHtml($"Autor: {article.AuthorName}<br />Data: {article.Date.GetDatePl()}");

                var text = article.Text;

                // usuwam sierotki
                //text = Regex.Replace(text, @"[\s\(\,\;][a,i,o,w,z]\s", delegate (Match m) {
                //    return " " + m.Value.Trim() + "&nbsp;";
                //}, RegexOptions.IgnoreCase);

                text = text.Replace("class=\"mt-3 quote\"", "class=\"mt-3 quote\" style=\"margin-left: 1cm; text-align: justify;\"");
                text = text.Replace("class=\"fs-5 mt-3\"", "class=\"fs-5 mt-3\" style=\"text-align: justify;\"");
                builder.InsertHtml(text);

                var patterns = new string[] {
                    @"\s([aiouwzAIOUWZ])\s",
                    @"\s(nie)\s",
                    @"\s[0-9]+\s"
                };
                var patternsPrev = new string[] {
                    @"\s([0-9]+\s)+(zł|km)\s",
                };
                var patternsInner = new string[] {
                   @"[0-9]+\s(zł|km)",
                   @"Rdz\s[0-9]+",
                   @"Wj\s[0-9]+",
                   @"Kpł\s[0-9]+",
                   @"Lb\s[0-9]+",
                   @"Pwt\s[0-9]+",
                   @"Joz\s[0-9]+",
                   @"Sdz\s[0-9]+",
                   @"1Sm\s[0-9]+",
                   @"2Sm\s[0-9]+",
                   @"1Krl\s[0-9]+",
                   @"2Krl\s[0-9]+",
                   @"1Krn\s[0-9]+",
                   @"2Krn\s[0-9]+",
                   @"Ezd\s[0-9]+",
                   @"Neh\s[0-9]+",
                   @"Rt\s[0-9]+",
                   @"Est\s[0-9]+",
                   @"Hi\s[0-9]+",
                   @"Job\s[0-9]+",
                   @"Ps\s[0-9]+",
                   @"Prz\s[0-9]+",
                   @"Kaz\s[0-9]+",
                   @"Pnp\s[0-9]+",
                   @"Iz\s[0-9]+",
                   @"Jr\s[0-9]+",
                   @"Lm\s[0-9]+",
                   @"Ez\s[0-9]+",
                   @"Dn\s[0-9]+",
                   @"Oz\s[0-9]+",
                   @"Jl\s[0-9]+",
                   @"Am\s[0-9]+",
                   @"Ab\s[0-9]+",
                   @"Jo\s[0-9]+",
                   @"Mi\s[0-9]+",
                   @"Na\s[0-9]+",
                   @"Ha\s[0-9]+",
                   @"So\s[0-9]+",
                   @"Ag\s[0-9]+",
                   @"Za\s[0-9]+",
                   @"Ml\s[0-9]+",

                   @"Mt\s[0-9]+",
                   @"Mk\s[0-9]+",
                   @"Łk\s[0-9]+",
                   @"J\s[0-9]+",
                   @"Dz\s[0-9]+",
                   @"Rz\s[0-9]+",
                   @"1Kor\s[0-9]+",
                   @"2Kor\s[0-9]+",
                   @"Ga\s[0-9]+",
                   @"Ef\s[0-9]+",
                   @"Kol\s[0-9]+",
                   @"Flp\s[0-9]+",
                   @"1Tes\s[0-9]+",
                   @"2Tes\s[0-9]+",
                   @"1Tm\s[0-9]+",
                   @"2Tm\s[0-9]+",
                   @"Flm\s[0-9]+",
                   @"Tt\s[0-9]+",
                   @"Hbr\s[0-9]+",
                   @"Heb\s[0-9]+",
                   @"Jk\s[0-9]+",
                   @"1P\s[0-9]+",
                   @"2P\s[0-9]+",
                   @"1J\s[0-9]+",
                   @"2J\s[0-9]+",
                   @"3J\s[0-9]+",
                   @"Jud\s[0-9]+",
                   @"Obj\s[0-9]+",
                };

                var h = false;
                Style style = null;
                foreach (Paragraph par in builder.Document.Sections[0].Body.Paragraphs) {
                    if (par.ParagraphFormat.StyleName.StartsWith("Heading")) {
                        if (par.ParagraphFormat.StyleName.StartsWith("Heading 1")) {
                            par.ParagraphFormat.Style = builder.Document.Styles["Nagłówek 1"];
                        }
                        if (par.ParagraphFormat.StyleName.StartsWith("Heading 2")) {
                            par.ParagraphFormat.Style = builder.Document.Styles["Nagłówek 2"];
                        }                        
                        if (!par.ParagraphFormat.KeepWithNext) par.ParagraphFormat.KeepWithNext = true;
                    }

                    style = par.ParagraphFormat.Style;

                    foreach (Run run in par.Runs) {
                        if (run.Font.Size != style.Font.Size) {
                            run.Font.Size = style.Font.Size;
                        }
                        if (h) {
                            h = false;
                            run.Font.Superscript = true;
                            run.Font.Color = Color.Black;
                            run.Font.Underline = Underline.None;
                        }
                        if (run.Text != null && run.Text.TrimStart().StartsWith("HYPERLINK")) {
                            run.Text = "";
                            h = true;
                        }
                    }
                }

                foreach (Paragraph par in builder.Document.Sections[0].Body.Paragraphs) {
                    var firstRun = par.Runs.FirstOrDefault() as Run;
                    if (firstRun != null) {
                        foreach (Run run in par.Runs) {
                            if (firstRun == run) { continue; }
                            if (run.Font.Bold == firstRun.Font.Bold && run.Font.Italic == firstRun.Font.Italic && run.Font.Superscript == firstRun.Font.Superscript) {
                                firstRun.Text += run.Text;
                                run.Text = String.Empty;
                            }
                            else {
                                firstRun = run;
                            }
                        }
                    }
                }

                foreach (Paragraph par in builder.Document.Sections[0].Body.Paragraphs) {
                    foreach (Run run in par.Runs) {
                        var s = run.Text;
                        if (s.IsNotNullOrEmpty()) {
                            foreach (var pattern in patterns) {
                                s = Regex.Replace(s, pattern, orphansMatchEvaluator);
                            }
                            foreach (var pattern in patternsPrev) {
                                s = Regex.Replace(s, pattern, orphansMatchEvaluatorPrev);
                            }
                            foreach (var pattern in patternsInner) {
                                s = Regex.Replace(s, pattern, orphansMatchEvaluatorInner);
                            }

                            if (s.Contains(HARD_SPACE_REPLACEMENT)) {
                                run.Text = s.Replace(HARD_SPACE_REPLACEMENT, HARD_SPACE);
                            }
                        }
                    }
                }

                builder.InsertParagraph();
                builder.Font.Size = 10;
                builder.Writeln($"{DateTime.Now.Year} - Kościół Chrześcijan Baptystów w Nowym Dworze Mazowieckim");
                builder.Writeln("ul. Sukienna 52, 05-100 Nowy Dwór Mazowiecki");
                builder.Writeln("https://kosciol-jezusa.pl http://ndm.baptysci.pl");

                return SaveBuilder(saveFormat, builder);
            }
            return default;
        }
        public const char HARD_SPACE = ' ';
        public const char HARD_SPACE_REPLACEMENT = '^';
        string orphansMatchEvaluatorInner(Match m) {
            return Regex.Replace(m.ToString(), @"\s", HARD_SPACE_REPLACEMENT.ToString());
        }
        string orphansMatchEvaluator(Match m) {
            return " " + m.ToString().Trim() + HARD_SPACE_REPLACEMENT.ToString();
        }
        string orphansMatchEvaluatorPrev(Match m) {
            return HARD_SPACE_REPLACEMENT.ToString() + m.ToString().Trim() + " ";
        }

    }
}
