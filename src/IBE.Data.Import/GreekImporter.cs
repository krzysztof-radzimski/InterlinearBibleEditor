using DevExpress.Xpo;
using IBE.Data.Model;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace IBE.Data.Import {
    /// <summary>
    /// Importer bazując na pliku 28th-Novum-Testamentum-Graece-Nestle-Aland.epub
    ///  zawierającym tekst nowego testamentu w wersji Nestle-Aland co ważne z akcentami
    ///  uzupełnia tabele z listą ksiąg, rozdziałów, wersetó i śródtytułów w języku polskim.
    /// </summary>
    public class GreekImporter : BaseImporter {
        public override void Import(string filePath, UnitOfWork uow) {
            if (filePath != null && File.Exists(filePath)) {
                var pattern = "index_split_{0}.html";

                var bookNumber = 40;
                int verseNumber = 1;
                Book book = null;
                Chapter chapter = null; // mogą dzielić się pomiędzy plikami
                var contentStart = false;

                var status = new BookStatus(uow) {
                    StatusName = "Kanoniczna",
                    CanonType = CanonType.Canon,
                    BiblePart = BiblePart.NewTestament
                };

                status.Save();

                for (int i = 2; i < 32; i++) {
                    var fileName = string.Format(pattern, i.ToString().PadLeft(3, '0'));
                    if (FileExists(filePath, fileName)) {
                        var htmlText = Encoding.UTF8.GetString(GetFileData(filePath, fileName));
                        htmlText = htmlText.Replace("&nbsp;", HARD_SPACE.ToString()).Trim();

                        XElement html = XElement.Parse(htmlText);
                        if (html != null && html.HasElements) {
                            var body = html.Elements().Where(x => x.Name.LocalName == "body").FirstOrDefault();
                            if (body != null && body.HasElements) {
                                foreach (var item in body.Elements()) {
                                    //Book
                                    if (IsBook(item)) {
                                        var gr = item.Elements().Where(x => x.Attribute("class") != null && x.Attribute("class").Value == "calibre5").FirstOrDefault();
                                        var pl = item.Elements().Where(x => x.Attribute("class") != null && x.Attribute("class").Value == "calibre5").LastOrDefault();

                                        if (chapter != null && chapter.NumberOfVerses == 0) { chapter.NumberOfVerses = chapter.Verses.Count; chapter.Save(); }
                                        if (book != null && book.NumberOfChapters == 0) { book.NumberOfChapters = book.Chapters.Count; book.Save(); }

                                        book = new Book(uow) {
                                            NumberOfBook = bookNumber,
                                            GreekName = gr != null ? gr.Value.Trim() : String.Empty,
                                            BookName = pl != null ? pl.Value.Replace("(", "").Replace(")", "").Trim() : String.Empty,
                                            Status = status
                                        };

                                        book.Save();
                                        contentStart = false;
                                        chapter = null; // jeżeli nowa księga to nowy rozdział
                                        bookNumber++;
                                    }
                                    // Chapter
                                    else if (IsChapter(item)) {
                                        var chapterText = item.Value.Trim().Split(' ');
                                        if (chapterText.Length == 3) {
                                            chapterText = new string[] { $"{chapterText[0]} {chapterText[1]}", chapterText[2] };
                                        }
                                        if (chapterText.Length == 2) {
                                            if (book != null && book.BookShortcut == null) {
                                                book.BookShortcut = chapterText[0];
                                                book.Save();
                                            }

                                            if (chapter != null && chapter.NumberOfVerses == 0) { chapter.NumberOfVerses = chapter.Verses.Count; chapter.Save(); }

                                            chapter = new Chapter(uow) {
                                                NumberOfChapter = Convert.ToInt32(chapterText[1]),
                                                ParentBook = book
                                            };

                                            chapter.Save();
                                            contentStart = false;
                                            verseNumber = 1;
                                        }
                                    }
                                    // title
                                    else if (IsTitle(item)) {
                                        if (chapter == null) { chapter = CreateFirstChapter(book); }
                                        var title = new Subtitle(uow) {
                                            BeforeVerseNumber = verseNumber,
                                            Level = 1,
                                            ParentChapter = chapter,
                                            Text = item.Value.Trim()
                                        };

                                        title.Save();
                                    }
                                    // subtitle
                                    else if (IsSubtitle(item)) {
                                        if (chapter == null) { chapter = CreateFirstChapter(book); }
                                        var title = new Subtitle(uow) {
                                            BeforeVerseNumber = verseNumber,
                                            Level = 2,
                                            ParentChapter = chapter,
                                            Text = item.Value.Trim()
                                        };

                                        title.Save();
                                    }
                                    // verse
                                    else if (IsVerse(item)) {
                                        contentStart = true;
                                        if (chapter == null) { chapter = CreateFirstChapter(book); }
                                        verseNumber = AnalizeVerseContent(uow, item, chapter);
                                    }
                                    // content
                                    else {
                                        if (contentStart) {
                                            if (chapter == null) { chapter = CreateFirstChapter(book); }
                                            verseNumber = AnalizeVerseContent(uow, item, chapter);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // Apokalipsa :)
                if (book != null && book.NumberOfChapters == 0) { book.NumberOfChapters = book.Chapters.Count; book.Save(); }

            }
        }

        private Chapter CreateFirstChapter(Book parentBook) {
            var chapter = new Chapter(parentBook.Session) {
                NumberOfChapter = 1,
                ParentBook = parentBook
            };
            chapter.Save();
            return chapter;
        }

        private int AnalizeVerseContent(Session session, XElement item, Chapter chapter) {
            Verse verse = null;
            var citation = item.Name.LocalName == "blockquote";
            var nodes = item.Nodes().ToArray();
            for (int i = 0; i < nodes.Length; i++) {
                if (nodes[i].NodeType == System.Xml.XmlNodeType.Element) {
                    if (IsVerse(nodes[i] as XElement, out var verseNumber)) {
                        verse = new Verse(session) {
                            NumberOfVerse = verseNumber,
                            ParentChapter = chapter
                        };
                        verse.Save();
                    }
                    else {
                        AnalizeVerseContent(session, nodes[i] as XElement, chapter);
                    }
                }
                else if (nodes[i].NodeType == System.Xml.XmlNodeType.Text) {
                    var xtext = nodes[i] as XText;
                    if (!String.IsNullOrWhiteSpace(xtext.Value)) {
                        var words = xtext.Value.Trim().Split(' ');
                        var index = 1;

                        if (verse == null) {
                            verse = chapter.Verses.LastOrDefault();
                        }
                        foreach (var word in words) {
                            var verseWord = new VerseWord(session) {
                                Citation = citation,
                                NumberOfVerseWord = index,
                                ParentVerse = verse,
                                SourceWord = word
                            };

                            verseWord.Save();

                            index++;
                        }
                    }
                }
            }

            if (chapter != null) {
                chapter.NumberOfVerses = chapter.Verses.Count;
                chapter.Save();
                return chapter.Verses.Select(x => x.NumberOfVerse).Max();
            }
            if (verse != null) { return verse.NumberOfVerse; }
            return 1;
        }

        private bool IsVerse(XElement item) {
            var result = item.HasAttributes &&
               item.Attribute("class") != null &&
               (item.Attribute("class").Value == "calibre_7" || item.Attribute("class").Value == "calibre_1" || item.Attribute("class").Value == "calibre_11" || item.Attribute("class").Value == "calibre_12") &&
               item.Elements().FirstOrDefault() != null &&
               item.Elements().FirstOrDefault().Attribute("class") != null &&
               item.Elements().FirstOrDefault().Attribute("class").Value == "calibre6" &&
               item.Elements().FirstOrDefault().Elements().FirstOrDefault() != null &&
               item.Elements().FirstOrDefault().Elements().FirstOrDefault().Attribute("class") != null &&
               item.Elements().FirstOrDefault().Elements().FirstOrDefault().Attribute("class").Value == "calibre_10";
            if (result) {
                var number = Convert.ToInt32(item.Elements().FirstOrDefault().Elements().FirstOrDefault().Value.Trim());
                return result && number > 0;
            }
            return default;
        }

        private bool IsVerse(XElement item, out int number) {
            number = 0;
            var result = item.HasAttributes &&
               item.Attribute("class") != null &&
               item.Attribute("class").Value == "calibre6" &&
               item.Elements().FirstOrDefault() != null &&
               item.Elements().FirstOrDefault().Attribute("class") != null &&
               item.Elements().FirstOrDefault().Attribute("class").Value == "calibre_10";
            if (result) {
                number = Convert.ToInt32(item.Elements().FirstOrDefault().Value.Trim());
                return result && number > 0;
            }
            return default;
        }

        private bool IsSubtitle(XElement item) {
            return item.HasAttributes &&
                item.Attribute("id") == null &&
                item.Attribute("class") != null &&
                item.Attribute("class").Value == "calibre_7" &&
                item.Elements().FirstOrDefault() != null &&
                item.Elements().FirstOrDefault().Attribute("class") != null &&
                item.Elements().FirstOrDefault().Attribute("class").Value == "bold" &&
                item.Elements().FirstOrDefault().Elements().FirstOrDefault() != null &&
                item.Elements().FirstOrDefault().Elements().FirstOrDefault().Attribute("class") != null &&
                item.Elements().FirstOrDefault().Elements().FirstOrDefault().Attribute("class").Value == "calibre_9";
        }

        private bool IsTitle(XElement item) {
            return item.HasAttributes &&
                   item.Attribute("class") != null &&
                   item.Attribute("class").Value == "calibre_8" &&

                   item.Elements().FirstOrDefault() != null &&
                   item.Elements().FirstOrDefault().Attribute("class") != null &&
                   item.Elements().FirstOrDefault().Attribute("class").Value == "calibre3" &&

                   item.Elements().FirstOrDefault().Elements().FirstOrDefault() != null &&
                   item.Elements().FirstOrDefault().Elements().FirstOrDefault().Attribute("class") != null &&
                   item.Elements().FirstOrDefault().Elements().FirstOrDefault().Attribute("class").Value == "bold" &&

                   item.Elements().FirstOrDefault().Elements().FirstOrDefault().Elements().FirstOrDefault() != null &&
                   item.Elements().FirstOrDefault().Elements().FirstOrDefault().Elements().FirstOrDefault().Attribute("class") != null &&
                   item.Elements().FirstOrDefault().Elements().FirstOrDefault().Elements().FirstOrDefault().Attribute("class").Value == "calibre_9";
        }

        private bool IsChapter(XElement item) {
            var result = false;
            if (item.HasAttributes &&
                item.Attribute("id") != null &&
                item.Attribute("id").Value.Contains("filepos") &&
                item.Attribute("class") != null &&
                item.Attribute("class").Value == "calibre_7") {
                if (item.Elements().FirstOrDefault() != null &&
                    item.Elements().FirstOrDefault().Attribute("class") != null &&
                    item.Elements().FirstOrDefault().Attribute("class").Value == "calibre3") {
                    if (item.Elements().FirstOrDefault().Elements().FirstOrDefault() != null &&
                        item.Elements().FirstOrDefault().Elements().FirstOrDefault().Attribute("class") != null &&
                        item.Elements().FirstOrDefault().Elements().FirstOrDefault().Attribute("class").Value == "bold") {
                        result = true;
                    }
                }
            }



            return result;
        }

        private bool IsBook(XElement item) {
            return item.HasAttributes &&
                   item.Attribute("id") != null &&
                   item.Attribute("id").Value.Contains("filepos") &&
                   item.Attribute("class") != null &&
                   item.Attribute("class").Value == "calibre_6";
        }
    }
}
