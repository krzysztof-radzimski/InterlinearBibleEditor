using ChurchServices.Extensions;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ChurchServices.Data.Import.EIB.Model.Osis {
    public class OsisModelService : IDisposable {
        const string OLD_ID = "oldID";
        const string OSIS_ID = "osisID";
        const string START_ID = "sID";
        const string END_ID = "eID";
        const string REF_ID = "osisRef";
        const string NOTE = "note";
        const string CHAPTER = "chapter";
        const string VERSE = "verse";
        const string DIV = "div";
        const string SECTION = "section";
        const string TITLE = "title";

        public OsisModelService() { }
        public OsisModel GetModelFromFile(string filePath, bool renumerateForLogos = false) {
            if (filePath != null && File.Exists(filePath)) {
                if (renumerateForLogos) {
                    filePath = ProcessForLogos(filePath);
                }
                XmlSerializer serializer = new(typeof(OsisModel));
                using (Stream reader = new FileStream(filePath, FileMode.Open)) {
                    return (OsisModel)serializer.Deserialize(reader);
                }

            }
            return default;
        }

        private string ProcessForLogos(string filePath) {
            var result = Path.Combine(Path.GetDirectoryName(filePath), $"{Path.GetFileNameWithoutExtension(filePath)}.logos{Path.GetExtension(filePath)}");
            var xml = XElement.Load(filePath);
            if (xml != null && xml.Name.LocalName == "osis") {
                var book = "Num";
                // Nu 16 i 17
                var num = (xml.FirstNode as XElement).Elements().Where(x => x.Name.LocalName == DIV && x.Attribute(OSIS_ID).Value == "Num").FirstOrDefault();
                if (num != null) {
                    // remove end of chapter 16
                    num.Descendants().Where(x => x.Name.LocalName == CHAPTER && x.Attribute(END_ID) != null && x.Attribute(END_ID).Value == "Num.16").Remove();
                    // remove start of chapter 17
                    num.Descendants().Where(x => x.Name.LocalName == CHAPTER && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == "Num.17").Remove();
                    // expand chapter 16
                    XElement lastItem = null;
                    var vn = 36;
                    for (int i = 1; i < 16; i++) {
                        var items = num.Descendants().Where(x => (x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"Num.17.{i}") || (x.Name.LocalName == NOTE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value.StartsWith($"Num.17.{i}!")));

                        foreach (var item in items) {
                            if (item.Name.LocalName == VERSE && item.Attribute(OSIS_ID) != null) { item.Attribute(OSIS_ID).Value = $"Num.16.{vn}"; }
                            else if (item.Name.LocalName == NOTE && item.Attribute(OSIS_ID) != null) { item.Attribute(OSIS_ID).Value = $"Num.16.{vn}!note.{item.Attribute("n").Value}"; }
                            if (item.Attribute(START_ID) != null) { item.Attribute(START_ID).Value = $"Num.16.{vn}"; }
                            if (item.Attribute(END_ID) != null) { item.Attribute(END_ID).Value = $"Num.16.{vn}"; }
                            if (item.Name.LocalName == NOTE && item.Attribute(REF_ID) != null) { item.Attribute(REF_ID).Value = $"Num.16.{vn}"; }
                            lastItem = item;
                        }
                        vn++;
                    }
                    lastItem.AddAfterSelf(new XElement(XName.Get(CHAPTER, lastItem.Name.NamespaceName), new XAttribute(END_ID, "Num.16"), new XAttribute(OSIS_ID, "Num.16")));

                    // repair chapter 17
                    var on = 16;
                    var startItem = num.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == $"Num.17.{on}").FirstOrDefault();
                    if (startItem != null) {
                        startItem.AddBeforeSelf(new XElement(XName.Get(CHAPTER, lastItem.Name.NamespaceName), new XAttribute(START_ID, "Num.17"), new XAttribute(OSIS_ID, "Num.17")));
                        vn = 1;
                        for (int i = on; i < 29; i++) {
                            var items = num.Descendants().Where(x => (x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"Num.17.{i}") || (x.Name.LocalName == NOTE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value.StartsWith($"Num.17.{i}!")));
                            foreach (var item in items) {
                                if (item.Name.LocalName == VERSE && item.Attribute(OSIS_ID) != null) { item.Attribute(OSIS_ID).Value = $"Num.17.{vn}"; }
                                else if (item.Name.LocalName == NOTE && item.Attribute(OSIS_ID) != null) { item.Attribute(OSIS_ID).Value = $"Num.17.{vn}!note.{item.Attribute("n").Value}"; }
                                if (item.Attribute(START_ID) != null) { item.Attribute(START_ID).Value = $"Num.17.{vn}"; }
                                if (item.Attribute(END_ID) != null) { item.Attribute(END_ID).Value = $"Num.17.{vn}"; }
                                if (item.Name.LocalName == NOTE && item.Attribute(REF_ID) != null) { item.Attribute(REF_ID).Value = $"Num.17.{vn}"; }
                            }
                            vn++;
                        }
                    }


                    // Nu 25:19
                    var verse = num.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == "Num.25.19").FirstOrDefault();
                    if (verse != null) {
                        var verse2 = num.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == "Num.26.1").FirstOrDefault();
                        if (verse2 != null) {
                            verse2.AddAfterSelf(verse.NextNode);
                            while (verse.NextNode != null) {
                                if (verse.NextNode is XElement && (verse.NextNode as XElement).Name.LocalName == CHAPTER) { break; }
                                verse.NextNode.Remove();
                            }
                            verse.Remove();
                        }
                    }

                    // Nu 30:17
                    verse = num.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == "Num.30.17").FirstOrDefault();
                    if (verse != null) {
                        var verse2 = num.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(END_ID) != null && x.Attribute(END_ID).Value == "Num.30.16").FirstOrDefault();
                        if (verse2 != null) {
                            verse2.AddBeforeSelf(new XText(" "));
                            verse2.AddBeforeSelf(verse.NextNode);
                            while (verse.NextNode != null) {
                                if (verse.NextNode is XElement && (verse.NextNode as XElement).Name.LocalName == CHAPTER) { break; }
                                verse.NextNode.Remove();
                            }
                            verse.Remove();
                        }
                    }
                }

                book = "Deut";
                // Deut 13:1 --> 12:32
                var deut = (xml.FirstNode as XElement).Elements().Where(x => x.Name.LocalName == DIV && x.Attribute(OSIS_ID).Value == book).FirstOrDefault();
                if (deut != null) {
                    var lastverse = deut.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(END_ID) != null && x.Attribute(END_ID).Value == $"{book}.12.31").FirstOrDefault();
                    if (lastverse != null) {
                        lastverse.AddAfterSelf(new XElement(XName.Get(VERSE, lastverse.Name.NamespaceName), new XAttribute(START_ID, $"{book}.12.32"), new XAttribute(OSIS_ID, $"{book}.12.32")));
                    }
                    var verse = deut.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == $"{book}.13.1").FirstOrDefault();
                    if (verse != null) {
                        var objects = new List<object>();
                        while (verse.NextNode != null) {
                            if (verse.NextNode is XElement && (verse.NextNode as XElement).Name.LocalName == VERSE) { verse.NextNode.Remove(); break; }
                            objects.Add(verse.NextNode);
                            verse.NextNode.Remove();
                        }
                        objects.Add(new XElement(XName.Get(VERSE, lastverse.Name.NamespaceName), new XAttribute(END_ID, $"{book}.12.32"), new XAttribute(OSIS_ID, $"{book}.12.32")));
                        lastverse = deut.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == $"{book}.12.32").FirstOrDefault();
                        lastverse.AddAfterSelf(objects.ToArray());
                    }
                    verse.Remove();

                    var ch = 13;
                    var on = 2;
                    var startItem = deut.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == $"{book}.{ch}.{on}").FirstOrDefault();
                    if (startItem != null) {
                        var vn = 1;
                        for (int i = on; i < 20; i++) {
                            var items = deut.Descendants().Where(x => (x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"{book}.{ch}.{i}") || (x.Name.LocalName == NOTE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value.StartsWith($"{book}.{ch}.{i}!")));
                            foreach (var item in items) {
                                if (item.Attribute(OSIS_ID) != null) {
                                    item.Add(new XAttribute(OLD_ID, item.Attribute(OSIS_ID).Value));
                                }
                                if (item.Name.LocalName == VERSE && item.Attribute(OSIS_ID) != null) {
                                    item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}";
                                }
                                else if (item.Name.LocalName == NOTE && item.Attribute(OSIS_ID) != null) {
                                    item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}!note.{item.Attribute("n").Value}";
                                }
                                if (item.Attribute(START_ID) != null) { item.Attribute(START_ID).Value = $"{book}.{ch}.{vn}"; }
                                if (item.Attribute(END_ID) != null) { item.Attribute(END_ID).Value = $"{book}.{ch}.{vn}"; }
                                if (item.Name.LocalName == NOTE && item.Attribute(REF_ID) != null) { item.Attribute(REF_ID).Value = $"{book}.{ch}.{vn}"; }
                            }
                            vn++;
                        }
                    }

                    // Dt 23:26 (23:1 -> 22:30)
                    ch = 22;
                    deut.Descendants().Where(x => x.Name.LocalName == CHAPTER && x.Attribute(END_ID) != null && x.Attribute(END_ID).Value == $"{book}.{ch}").Remove();
                    deut.Descendants().Where(x => x.Name.LocalName == CHAPTER && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == $"{book}.23").Remove();
                    var section23_1 = deut.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == $"{book}.23.1").FirstOrDefault().Parent;
                    if (section23_1 != null) {
                        foreach (var item in section23_1.Elements()) {
                            if (item.Attribute(OSIS_ID) != null) {
                                item.Add(new XAttribute(OLD_ID, item.Attribute(OSIS_ID).Value));
                            }
                            if (item.Name.LocalName == VERSE) {
                                item.Attribute(OSIS_ID).Value = $"{book}.{ch}.30";
                                if (item.Attribute(END_ID) != null) { item.Attribute(END_ID).Value = $"{book}.{ch}.30"; }
                                if (item.Attribute(START_ID) != null) { item.Attribute(START_ID).Value = $"{book}.{ch}.30"; }
                            }
                            else if (item.Name.LocalName == NOTE) {
                                item.Attribute(OSIS_ID).Value = item.Attribute(OSIS_ID).Value.Replace($"{book}.23.1", $"{book}.{ch}.30");
                                item.Attribute(REF_ID).Value = $"{book}.{ch}.30";
                            }
                        }
                        section23_1.Add(new XElement(XName.Get(CHAPTER, section23_1.Name.NamespaceName),
                            new XAttribute(END_ID, $"{book}.{ch}"),
                            new XAttribute(OSIS_ID, $"{book}.{ch}")));
                    }
                    ch = 23;
                    on = 2;
                    startItem = deut.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == $"{book}.{ch}.{on}").FirstOrDefault();
                    if (startItem != null) {
                        startItem.AddBeforeSelf(new XElement(XName.Get(CHAPTER, startItem.Name.NamespaceName),
                            new XAttribute(START_ID, $"{book}.{ch}"),
                            new XAttribute(OSIS_ID, $"{book}.{ch}")));
                        var vn = 1;
                        for (int i = on; i < 27; i++) {
                            var items = deut.Descendants().Where(x => (x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"{book}.{ch}.{i}") || (x.Name.LocalName == NOTE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value.StartsWith($"{book}.{ch}.{i}!")));
                            foreach (var item in items) {
                                if (item.Attribute(OSIS_ID) != null) {
                                    item.Add(new XAttribute(OLD_ID, item.Attribute(OSIS_ID).Value));
                                }
                                if (item.Name.LocalName == VERSE && item.Attribute(OSIS_ID) != null) {
                                    item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}";
                                }
                                else if (item.Name.LocalName == NOTE && item.Attribute(OSIS_ID) != null) {
                                    item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}!note.{item.Attribute("n").Value}";
                                }
                                if (item.Attribute(START_ID) != null) { item.Attribute(START_ID).Value = $"{book}.{ch}.{vn}"; }
                                if (item.Attribute(END_ID) != null) { item.Attribute(END_ID).Value = $"{book}.{ch}.{vn}"; }
                                if (item.Name.LocalName == NOTE && item.Attribute(REF_ID) != null) { item.Attribute(REF_ID).Value = $"{book}.{ch}.{vn}"; }
                            }
                            vn++;
                        }
                    }


                    // Dt 28:69
                    ch = 28;
                    on = 69;
                    {
                        var objects = new List<object>();
                        startItem = deut.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == $"{book}.{ch}.{on}").FirstOrDefault();
                        if (startItem != null) {
                            objects.Add(startItem);
                            while (startItem.NextNode != null) {
                                if (startItem.NextNode is XElement && (startItem.NextNode as XElement).Name.LocalName == CHAPTER) { break; }
                                objects.Add(startItem.NextNode);
                                startItem.NextNode.Remove();
                            }
                            startItem.Remove();
                        }
                        ch = 29;
                        var chapterDt29 = deut.Descendants().Where(x => x.Name.LocalName == CHAPTER && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == $"{book}.{ch}").FirstOrDefault();
                        if (chapterDt29 != null) {
                            foreach (var o in objects) {
                                if (o is XElement) {
                                    var item = o as XElement;
                                    if (item.Attribute(OSIS_ID) != null) {
                                        item.Add(new XAttribute(OLD_ID, item.Attribute(OSIS_ID).Value));
                                    }
                                    if (item.Name.LocalName == VERSE && item.Attribute(OSIS_ID) != null) {
                                        item.Attribute(OSIS_ID).Value = $"{book}.{ch}.0";
                                    }
                                    else if (item.Name.LocalName == NOTE && item.Attribute(OSIS_ID) != null) {
                                        item.Attribute(OSIS_ID).Value = $"{book}.{ch}.0!note.{item.Attribute("n").Value}";
                                    }
                                    if (item.Attribute(START_ID) != null) { item.Attribute(START_ID).Value = $"{book}.{ch}.0"; }
                                    if (item.Attribute(END_ID) != null) { item.Attribute(END_ID).Value = $"{book}.{ch}.0"; }
                                    if (item.Name.LocalName == NOTE && item.Attribute(REF_ID) != null) { item.Attribute(REF_ID).Value = $"{book}.{ch}.0"; }
                                }
                            }
                            chapterDt29.AddAfterSelf(objects);

                            var vn = 29;
                            on = 0;
                            for (int i = 28; i >= 0; i--) {
                                var items = deut.Descendants().Where(x => (x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"{book}.{ch}.{i}") || (x.Name.LocalName == NOTE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value.StartsWith($"{book}.{ch}.{i}!")));
                                foreach (var item in items) {
                                    if (i != 0 && item.Attribute(OSIS_ID) != null) {
                                        item.Add(new XAttribute(OLD_ID, item.Attribute(OSIS_ID).Value));
                                    }
                                    if (item.Name.LocalName == VERSE && item.Attribute(OSIS_ID) != null) {
                                        item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}";
                                    }
                                    else if (item.Name.LocalName == NOTE && item.Attribute(OSIS_ID) != null) {
                                        item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}!note.{item.Attribute("n").Value}";
                                    }
                                    if (item.Attribute(START_ID) != null) { item.Attribute(START_ID).Value = $"{book}.{ch}.{vn}"; }
                                    if (item.Attribute(END_ID) != null) { item.Attribute(END_ID).Value = $"{book}.{ch}.{vn}"; }
                                    if (item.Name.LocalName == NOTE && item.Attribute(REF_ID) != null) { item.Attribute(REF_ID).Value = $"{book}.{ch}.{vn}"; }
                                }
                                vn--;
                            }
                        }
                    }


                } // end of Deut

                book = "1Sam";
                var ISam = (xml.FirstNode as XElement).Elements().Where(x => x.Name.LocalName == DIV && x.Attribute(OSIS_ID).Value == book).FirstOrDefault();
                if (ISam != null) {
                    // 21.1
                    var previousItem = ISam.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(END_ID) != null && x.Attribute(END_ID).Value == $"{book}.20.42").FirstOrDefault();
                    var startItem = ISam.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(END_ID) != null && x.Attribute(END_ID).Value == $"{book}.21.1").FirstOrDefault();
                    if (startItem != null && previousItem != null) {
                        previousItem.AddBeforeSelf(new XText(" "));
                        while (startItem.PreviousNode != null) {
                            if (startItem.PreviousNode is XElement && (startItem.PreviousNode as XElement).Name.LocalName == VERSE) { break; }
                            previousItem.AddBeforeSelf(startItem.PreviousNode);
                            startItem.PreviousNode.Remove();
                        }
                        ISam.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"{book}.21.1").Remove();
                        ISam.Descendants().Where(x => x.Name.LocalName == CHAPTER && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == $"{book}.21").Remove();
                    }

                    var ch = 21;
                    var on = 2;
                    startItem = ISam.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == $"{book}.{ch}.{on}").FirstOrDefault();
                    if (startItem != null) {
                        startItem.AddBeforeSelf(new XElement(XName.Get(CHAPTER, startItem.Name.NamespaceName),
                            new XAttribute(START_ID, $"{book}.{ch}"),
                            new XAttribute(OSIS_ID, $"{book}.{ch}")));
                        var vn = 1;
                        for (int i = on; i < 17; i++) {
                            var items = ISam.Descendants().Where(x => (x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"{book}.{ch}.{i}") || (x.Name.LocalName == NOTE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value.StartsWith($"{book}.{ch}.{i}!")));
                            foreach (var item in items) {
                                if (item.Attribute(OSIS_ID) != null) {
                                    item.Add(new XAttribute(OLD_ID, item.Attribute(OSIS_ID).Value));
                                }
                                if (item.Name.LocalName == VERSE && item.Attribute(OSIS_ID) != null) {
                                    item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}";
                                }
                                else if (item.Name.LocalName == NOTE && item.Attribute(OSIS_ID) != null) {
                                    item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}!note.{item.Attribute("n").Value}";
                                }
                                if (item.Attribute(START_ID) != null) { item.Attribute(START_ID).Value = $"{book}.{ch}.{vn}"; }
                                if (item.Attribute(END_ID) != null) { item.Attribute(END_ID).Value = $"{book}.{ch}.{vn}"; }
                                if (item.Name.LocalName == NOTE && item.Attribute(REF_ID) != null) { item.Attribute(REF_ID).Value = $"{book}.{ch}.{vn}"; }
                            }
                            vn++;
                        }
                    }

                    // 24.1
                    previousItem = ISam.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(END_ID) != null && x.Attribute(END_ID).Value == $"{book}.23.28").FirstOrDefault();
                    startItem = ISam.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(END_ID) != null && x.Attribute(END_ID).Value == $"{book}.24.1").FirstOrDefault();
                    if (startItem != null && previousItem != null) {
                        previousItem.AddBeforeSelf(new XText(" "));
                        while (startItem.PreviousNode != null) {
                            if (startItem.PreviousNode is XElement && (startItem.PreviousNode as XElement).Name.LocalName == VERSE) { break; }
                            if (startItem.PreviousNode is XElement && (startItem.PreviousNode as XElement).Name.LocalName == NOTE) {
                                var note = startItem.PreviousNode as XElement;
                                if (note != null) {
                                    note.Attribute(OSIS_ID).Value = note.Attribute(OSIS_ID).Value.Replace("1Sam.24.1", "1Sam.23.28");
                                    note.Attribute(REF_ID).Value = note.Attribute(REF_ID).Value.Replace("1Sam.24.1", "1Sam.23.28");
                                }
                            }
                            previousItem.AddBeforeSelf(startItem.PreviousNode);
                            startItem.PreviousNode.Remove();
                        }
                        ISam.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"{book}.24.1").Remove();
                        ISam.Descendants().Where(x => x.Name.LocalName == CHAPTER && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == $"{book}.24").Remove();
                    }

                    ch = 24;
                    on = 2;
                    startItem = ISam.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == $"{book}.{ch}.{on}").FirstOrDefault();
                    if (startItem != null) {
                        startItem.AddBeforeSelf(new XElement(XName.Get(CHAPTER, startItem.Name.NamespaceName),
                            new XAttribute(START_ID, $"{book}.{ch}"),
                            new XAttribute(OSIS_ID, $"{book}.{ch}")));
                        var vn = 1;
                        for (int i = on; i < 24; i++) {
                            var items = ISam.Descendants().Where(x => (x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"{book}.{ch}.{i}") || (x.Name.LocalName == NOTE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value.StartsWith($"{book}.{ch}.{i}!")));
                            foreach (var item in items) {
                                if (item.Attribute(OSIS_ID) != null) {
                                    item.Add(new XAttribute(OLD_ID, item.Attribute(OSIS_ID).Value));
                                }
                                if (item.Name.LocalName == VERSE && item.Attribute(OSIS_ID) != null) {
                                    item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}";
                                }
                                else if (item.Name.LocalName == NOTE && item.Attribute(OSIS_ID) != null) {
                                    item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}!note.{item.Attribute("n").Value}";
                                }
                                if (item.Attribute(START_ID) != null) { item.Attribute(START_ID).Value = $"{book}.{ch}.{vn}"; }
                                if (item.Attribute(END_ID) != null) { item.Attribute(END_ID).Value = $"{book}.{ch}.{vn}"; }
                                if (item.Name.LocalName == NOTE && item.Attribute(REF_ID) != null) { item.Attribute(REF_ID).Value = $"{book}.{ch}.{vn}"; }
                            }
                            vn++;
                        }
                    }
                }

                book = "1Kgs";
                var IKgs = (xml.FirstNode as XElement).Elements().Where(x => x.Name.LocalName == DIV && x.Attribute(OSIS_ID).Value == book).FirstOrDefault();
                if (IKgs != null) {
                    var ch = 22;
                    var on = 44;
                    var previousItem = IKgs.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(END_ID) != null && x.Attribute(END_ID).Value == $"{book}.{ch}.43").FirstOrDefault();
                    var startItem = IKgs.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == $"{book}.{ch}.{on}").FirstOrDefault();
                    if (startItem != null && previousItem != null) {
                        if (startItem.NextNode != null) {
                            previousItem.AddBeforeSelf(" ");
                            previousItem.AddBeforeSelf(startItem.NextNode);
                            startItem.NextNode.Remove();
                            IKgs.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"{book}.{ch}.{on}").Remove();
                        }
                    }

                    var vn = 44;
                    for (int i = 45; i < 55; i++) {
                        var items = IKgs.Descendants().Where(x => (x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"{book}.{ch}.{i}") || (x.Name.LocalName == NOTE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value.StartsWith($"{book}.{ch}.{i}!")));
                        foreach (var item in items) {
                            if (item.Attribute(OSIS_ID) != null) {
                                item.Add(new XAttribute(OLD_ID, item.Attribute(OSIS_ID).Value));
                            }
                            if (item.Name.LocalName == VERSE && item.Attribute(OSIS_ID) != null) {
                                item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}";
                            }
                            else if (item.Name.LocalName == NOTE && item.Attribute(OSIS_ID) != null) {
                                item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}!note.{item.Attribute("n").Value}";
                            }
                            if (item.Attribute(START_ID) != null) { item.Attribute(START_ID).Value = $"{book}.{ch}.{vn}"; }
                            if (item.Attribute(END_ID) != null) { item.Attribute(END_ID).Value = $"{book}.{ch}.{vn}"; }
                            if (item.Name.LocalName == NOTE && item.Attribute(REF_ID) != null) { item.Attribute(REF_ID).Value = $"{book}.{ch}.{vn}"; }

                        }
                        vn++;
                    }
                }

                book = "2Kgs";
                var IIKgs = (xml.FirstNode as XElement).Elements().Where(x => x.Name.LocalName == DIV && x.Attribute(OSIS_ID).Value == book).FirstOrDefault();
                if (IIKgs != null) {
                    var ch = 12;
                    var on = 1;
                    var startItem = IIKgs.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == $"{book}.{ch}.{on}").FirstOrDefault();
                    if (startItem != null) {
                        var objects = new List<object>();
                        while (startItem.NextNode != null) {
                            if (startItem.NextNode is XElement && (startItem.NextNode as XElement).Name.LocalName == VERSE) { break; }
                            objects.Add(startItem.NextNode);
                            startItem.NextNode.Remove();
                        }
                        IIKgs.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"{book}.{ch}.{on}").Remove();
                        objects.Insert(0, new XElement(XName.Get(VERSE, startItem.Name.NamespaceName),
                            new XAttribute(START_ID, $"{book}.11.21"),
                            new XAttribute(OSIS_ID, $"{book}.11.21")
                        ));
                        objects.Add(new XElement(XName.Get(VERSE, startItem.Name.NamespaceName),
                           new XAttribute(END_ID, $"{book}.11.21"),
                           new XAttribute(OSIS_ID, $"{book}.11.21")
                        ));

                        var previousItem = IIKgs.Descendants().Where(x => x.Name.LocalName == CHAPTER && x.Attribute(END_ID) != null && x.Attribute(END_ID).Value == $"{book}.11").FirstOrDefault();
                        previousItem.AddBeforeSelf(objects);
                    }
                    var vn = 1;
                    for (int i = 2; i < 23; i++) {
                        var items = IIKgs.Descendants().Where(x => (x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"{book}.{ch}.{i}") || (x.Name.LocalName == NOTE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value.StartsWith($"{book}.{ch}.{i}!")));
                        foreach (var item in items) {
                            if (item.Attribute(OSIS_ID) != null) {
                                item.Add(new XAttribute(OLD_ID, item.Attribute(OSIS_ID).Value));
                            }
                            if (item.Name.LocalName == VERSE && item.Attribute(OSIS_ID) != null) {
                                item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}";
                            }
                            else if (item.Name.LocalName == NOTE && item.Attribute(OSIS_ID) != null) {
                                item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}!note.{item.Attribute("n").Value}";
                            }
                            if (item.Attribute(START_ID) != null) { item.Attribute(START_ID).Value = $"{book}.{ch}.{vn}"; }
                            if (item.Attribute(END_ID) != null) { item.Attribute(END_ID).Value = $"{book}.{ch}.{vn}"; }
                            if (item.Name.LocalName == NOTE && item.Attribute(REF_ID) != null) { item.Attribute(REF_ID).Value = $"{book}.{ch}.{vn}"; }

                        }
                        vn++;
                    }
                }

                book = "1Chr";
                var IChr = (xml.FirstNode as XElement).Elements().Where(x => x.Name.LocalName == DIV && x.Attribute(OSIS_ID).Value == book).FirstOrDefault();
                if (IChr != null) {
                    var prevItem = IChr.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(END_ID) != null && x.Attribute(END_ID).Value == $"{book}.5.26").FirstOrDefault();
                    var ch5 = IChr.Descendants().Where(x => x.Name.LocalName == CHAPTER && x.Attribute(END_ID) != null && x.Attribute(END_ID).Value == $"{book}.5").FirstOrDefault();
                    if (ch5 != null && prevItem != null) {
                        var section = ch5.Parent;
                        var vn = 81;
                        var ch = 6;
                        var on = 66;

                        {
                            for (int i = on; i > 0; i--) {
                                var items = IChr.Descendants().Where(x => (x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"{book}.{ch}.{i}") || (x.Name.LocalName == NOTE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value.StartsWith($"{book}.{ch}.{i}!")));
                                foreach (var item in items) {
                                    if (item.Attribute(OSIS_ID) != null) {
                                        item.Add(new XAttribute(OLD_ID, item.Attribute(OSIS_ID).Value));
                                    }
                                    if (item.Name.LocalName == VERSE && item.Attribute(OSIS_ID) != null) {
                                        item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}";
                                    }
                                    else if (item.Name.LocalName == NOTE && item.Attribute(OSIS_ID) != null) {
                                        item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}!note.{item.Attribute("n").Value}";
                                    }
                                    if (item.Attribute(START_ID) != null) { item.Attribute(START_ID).Value = $"{book}.{ch}.{vn}"; }
                                    if (item.Attribute(END_ID) != null) { item.Attribute(END_ID).Value = $"{book}.{ch}.{vn}"; }
                                    if (item.Name.LocalName == NOTE && item.Attribute(REF_ID) != null) { item.Attribute(REF_ID).Value = $"{book}.{ch}.{vn}"; }

                                }
                                vn--;
                            }
                        }

                        vn = 1;
                        on = 27;

                        if (section != null) {
                            for (int i = on; i < 42; i++) {
                                var items = section.Descendants().Where(x => (x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"{book}.5.{i}") || (x.Name.LocalName == NOTE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value.StartsWith($"{book}.5.{i}!")));
                                foreach (var item in items) {
                                    if (item.Attribute(OSIS_ID) != null) {
                                        item.Add(new XAttribute(OLD_ID, item.Attribute(OSIS_ID).Value));
                                    }
                                    if (item.Name.LocalName == VERSE && item.Attribute(OSIS_ID) != null) {
                                        item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}";
                                    }
                                    else if (item.Name.LocalName == NOTE && item.Attribute(OSIS_ID) != null) {
                                        item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}!note.{item.Attribute("n").Value}";
                                    }
                                    if (item.Attribute(START_ID) != null) { item.Attribute(START_ID).Value = $"{book}.{ch}.{vn}"; }
                                    if (item.Attribute(END_ID) != null) { item.Attribute(END_ID).Value = $"{book}.{ch}.{vn}"; }
                                    if (item.Name.LocalName == NOTE && item.Attribute(REF_ID) != null) { item.Attribute(REF_ID).Value = $"{book}.{ch}.{vn}"; }

                                }
                                vn++;
                            }
                        }
                        prevItem.AddAfterSelf(ch5);
                        ch5.Remove();

                        var ch6 = IChr.Descendants().Where(x => x.Name.LocalName == CHAPTER && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == $"{book}.{ch}").FirstOrDefault();
                        prevItem = IChr.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == $"{book}.{ch}.1").FirstOrDefault();
                        if (ch6 != null && prevItem != null) {
                            prevItem.AddBeforeSelf(ch6);
                            ch6.Remove();
                        }
                    }

                    {
                        var ch = 12;
                        var on = 5;
                        var vn = 5;
                        var startItem = IChr.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == $"{book}.{ch}.{on}").FirstOrDefault();
                        prevItem = IChr.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(END_ID) != null && x.Attribute(END_ID).Value == $"{book}.{ch}.{on - 1}").FirstOrDefault();
                        if (startItem != null && prevItem != null) {
                            prevItem.AddBeforeSelf(" ");
                            prevItem.AddBeforeSelf(startItem.NextNode);
                            startItem.NextNode.Remove();
                            IChr.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"{book}.{ch}.{on}").Remove();
                            on = 6;
                            for (int i = on; i < 42; i++) {
                                var items = IChr.Descendants().Where(x => (x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"{book}.{ch}.{i}") || (x.Name.LocalName == NOTE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value.StartsWith($"{book}.{ch}.{i}!")));
                                foreach (var item in items) {
                                    if (item.Attribute(OSIS_ID) != null) {
                                        item.Add(new XAttribute(OLD_ID, item.Attribute(OSIS_ID).Value));
                                    }
                                    if (item.Name.LocalName == VERSE && item.Attribute(OSIS_ID) != null) {
                                        item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}";
                                    }
                                    else if (item.Name.LocalName == NOTE && item.Attribute(OSIS_ID) != null) {
                                        item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}!note.{item.Attribute("n").Value}";
                                    }
                                    if (item.Attribute(START_ID) != null) { item.Attribute(START_ID).Value = $"{book}.{ch}.{vn}"; }
                                    if (item.Attribute(END_ID) != null) { item.Attribute(END_ID).Value = $"{book}.{ch}.{vn}"; }
                                    if (item.Name.LocalName == NOTE && item.Attribute(REF_ID) != null) { item.Attribute(REF_ID).Value = $"{book}.{ch}.{vn}"; }

                                }
                                vn++;
                            }
                        }
                    }
                }

                book = "2Chr";
                var IIChr = (xml.FirstNode as XElement).Elements().Where(x => x.Name.LocalName == DIV && x.Attribute(OSIS_ID).Value == book).FirstOrDefault();
                if (IIChr != null) {
                    var ch = 2;
                    var vn = 18;
                    {
                        for (int i = 17; i > 0; i--) {
                            var items = IIChr.Descendants().Where(x => (x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"{book}.{ch}.{i}") || (x.Name.LocalName == NOTE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value.StartsWith($"{book}.{ch}.{i}!")));
                            foreach (var item in items) {
                                if (item.Attribute(OSIS_ID) != null) {
                                    item.Add(new XAttribute(OLD_ID, item.Attribute(OSIS_ID).Value));
                                }
                                if (item.Name.LocalName == VERSE && item.Attribute(OSIS_ID) != null) {
                                    item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}";
                                }
                                else if (item.Name.LocalName == NOTE && item.Attribute(OSIS_ID) != null) {
                                    item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}!note.{item.Attribute("n").Value}";
                                }
                                if (item.Attribute(START_ID) != null) { item.Attribute(START_ID).Value = $"{book}.{ch}.{vn}"; }
                                if (item.Attribute(END_ID) != null) { item.Attribute(END_ID).Value = $"{book}.{ch}.{vn}"; }
                                if (item.Name.LocalName == NOTE && item.Attribute(REF_ID) != null) { item.Attribute(REF_ID).Value = $"{book}.{ch}.{vn}"; }

                            }
                            vn--;
                        }

                        var verseItem = IIChr.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(END_ID) != null && x.Attribute(END_ID).Value == $"{book}.1.18").FirstOrDefault();
                        var objects = new List<object> {
                        new XElement(XName.Get(VERSE, verseItem.Name.NamespaceName),
                           new XAttribute(START_ID, $"{book}.{ch}.1"),
                           new XAttribute(OSIS_ID, $"{book}.{ch}.1")
                        ),
                        verseItem.PreviousNode,
                        new XElement(XName.Get(VERSE, verseItem.Name.NamespaceName),
                           new XAttribute(END_ID, $"{book}.{ch}.1"),
                           new XAttribute(OSIS_ID, $"{book}.{ch}.1")
                    )
                    };
                        verseItem.PreviousNode.Remove();
                        var prevItem = IIChr.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == $"{book}.{ch}.2").FirstOrDefault();
                        prevItem.AddBeforeSelf(objects);
                        IIChr.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"{book}.1.18").Remove();
                    }

                    ch = 14;
                    vn = 15;
                    {
                        for (int i = 14; i > 0; i--) {
                            var items = IIChr.Descendants().Where(x => (x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"{book}.{ch}.{i}") || (x.Name.LocalName == NOTE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value.StartsWith($"{book}.{ch}.{i}!")));
                            foreach (var item in items) {
                                if (item.Attribute(OSIS_ID) != null) {
                                    item.Add(new XAttribute(OLD_ID, item.Attribute(OSIS_ID).Value));
                                }
                                if (item.Name.LocalName == VERSE && item.Attribute(OSIS_ID) != null) {
                                    item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}";
                                }
                                else if (item.Name.LocalName == NOTE && item.Attribute(OSIS_ID) != null) {
                                    item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}!note.{item.Attribute("n").Value}";
                                }
                                if (item.Attribute(START_ID) != null) { item.Attribute(START_ID).Value = $"{book}.{ch}.{vn}"; }
                                if (item.Attribute(END_ID) != null) { item.Attribute(END_ID).Value = $"{book}.{ch}.{vn}"; }
                                if (item.Name.LocalName == NOTE && item.Attribute(REF_ID) != null) { item.Attribute(REF_ID).Value = $"{book}.{ch}.{vn}"; }

                            }
                            vn--;
                        }

                        var verseItem = IIChr.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == $"{book}.13.23").FirstOrDefault();
                        var objects = new List<object> {
                        new XElement(XName.Get(VERSE, verseItem.Name.NamespaceName),
                           new XAttribute(START_ID, $"{book}.{ch}.1"),
                           new XAttribute(OSIS_ID, $"{book}.{ch}.1")
                        )};
                        while (verseItem.NextNode != null) {
                            if (verseItem.NextNode is XElement && (verseItem.NextNode as XElement).Name.LocalName == VERSE) { break; }
                            if (verseItem.NextNode is XElement && (verseItem.NextNode as XElement).Name.LocalName == NOTE) {
                                (verseItem.NextNode as XElement).Attribute(OSIS_ID).Value = (verseItem.NextNode as XElement).Attribute(OSIS_ID).Value.Replace($"{book}.13.23", $"{book}.{ch}.1");
                                (verseItem.NextNode as XElement).Attribute(REF_ID).Value = (verseItem.NextNode as XElement).Attribute(REF_ID).Value.Replace($"{book}.13.23", $"{book}.{ch}.1");
                            }
                            objects.Add(verseItem.NextNode);
                            verseItem.NextNode.Remove();
                        }
                        objects.Add(new XElement(XName.Get(VERSE, verseItem.Name.NamespaceName),
                           new XAttribute(END_ID, $"{book}.{ch}.1"),
                           new XAttribute(OSIS_ID, $"{book}.{ch}.1")
                        ));
                        var prevItem = IIChr.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == $"{book}.{ch}.2").FirstOrDefault();
                        prevItem.AddBeforeSelf(objects);
                        IIChr.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"{book}.13.23").Remove();
                    }
                }

                book = "Neh";
                var Neh = (xml.FirstNode as XElement).Elements().Where(x => x.Name.LocalName == DIV && x.Attribute(OSIS_ID).Value == book).FirstOrDefault();
                if (Neh != null) {
                    {
                        var prevItem = Neh.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(END_ID) != null && x.Attribute(END_ID).Value == $"{book}.3.32").FirstOrDefault();
                        var chapterItem = Neh.Descendants().Where(x => x.Name.LocalName == CHAPTER && x.Attribute(END_ID) != null && x.Attribute(END_ID).Value == $"{book}.3").FirstOrDefault();
                        prevItem.AddAfterSelf(chapterItem);
                        chapterItem.Remove();
                        var ch = 4;
                        var vn = 23;
                        for (int i = 17; i > 0; i--) {
                            var items = Neh.Descendants().Where(x => (x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"{book}.{ch}.{i}") || (x.Name.LocalName == NOTE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value.StartsWith($"{book}.{ch}.{i}!")));
                            foreach (var item in items) {
                                if (item.Attribute(OSIS_ID) != null) {
                                    item.Add(new XAttribute(OLD_ID, item.Attribute(OSIS_ID).Value));
                                }
                                if (item.Name.LocalName == VERSE && item.Attribute(OSIS_ID) != null) {
                                    item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}";
                                }
                                else if (item.Name.LocalName == NOTE && item.Attribute(OSIS_ID) != null) {
                                    item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}!note.{item.Attribute("n").Value}";
                                }
                                if (item.Attribute(START_ID) != null) { item.Attribute(START_ID).Value = $"{book}.{ch}.{vn}"; }
                                if (item.Attribute(END_ID) != null) { item.Attribute(END_ID).Value = $"{book}.{ch}.{vn}"; }
                                if (item.Name.LocalName == NOTE && item.Attribute(REF_ID) != null) { item.Attribute(REF_ID).Value = $"{book}.{ch}.{vn}"; }

                            }
                            vn--;
                        }

                        vn = 1;
                        for (int i = 33; i < 39; i++) {
                            var items = Neh.Descendants().Where(x => (x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"{book}.3.{i}") || (x.Name.LocalName == NOTE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value.StartsWith($"{book}.3.{i}!")));
                            foreach (var item in items) {
                                if (item.Attribute(OSIS_ID) != null) {
                                    item.Add(new XAttribute(OLD_ID, item.Attribute(OSIS_ID).Value));
                                }
                                if (item.Name.LocalName == VERSE && item.Attribute(OSIS_ID) != null) {
                                    item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}";
                                }
                                else if (item.Name.LocalName == NOTE && item.Attribute(OSIS_ID) != null) {
                                    item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}!note.{item.Attribute("n").Value}";
                                }
                                if (item.Attribute(START_ID) != null) { item.Attribute(START_ID).Value = $"{book}.{ch}.{vn}"; }
                                if (item.Attribute(END_ID) != null) { item.Attribute(END_ID).Value = $"{book}.{ch}.{vn}"; }
                                if (item.Name.LocalName == NOTE && item.Attribute(REF_ID) != null) { item.Attribute(REF_ID).Value = $"{book}.{ch}.{vn}"; }

                            }
                            vn++;
                        }

                        prevItem = Neh.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(END_ID) != null && x.Attribute(END_ID).Value == $"{book}.{ch}.1").FirstOrDefault();
                        chapterItem = Neh.Descendants().Where(x => x.Name.LocalName == CHAPTER && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == $"{book}.4").FirstOrDefault();
                        prevItem.AddBeforeSelf(chapterItem);
                        chapterItem.Remove();
                    }

                    {
                        var ch = 10;
                        var verseItem = Neh.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == $"{book}.{ch}.1").FirstOrDefault();
                        var objects = new List<object> {
                            new XElement(XName.Get(VERSE, verseItem.Name.NamespaceName),
                                new XAttribute(START_ID, $"{book}.9.38"),
                                new XAttribute(OSIS_ID, $"{book}.9.38"))
                        };
                        while (verseItem.NextNode != null) {
                            if (verseItem.NextNode is XElement && (verseItem.NextNode as XElement).Name.LocalName == VERSE) { break; }
                            if (verseItem.NextNode is XElement && (verseItem.NextNode as XElement).Name.LocalName == NOTE) {
                                (verseItem.NextNode as XElement).Attribute(OSIS_ID).Value = (verseItem.NextNode as XElement).Attribute(OSIS_ID).Value.Replace($"{book}.{ch}.1", $"{book}.9.38");
                                (verseItem.NextNode as XElement).Attribute(REF_ID).Value = (verseItem.NextNode as XElement).Attribute(REF_ID).Value.Replace($"{book}.{ch}.1", $"{book}.9.38");
                            }
                            objects.Add(verseItem.NextNode);
                            verseItem.NextNode.Remove();
                        }
                        objects.Add(new XElement(XName.Get(VERSE, verseItem.Name.NamespaceName),
                                new XAttribute(END_ID, $"{book}.9.38"),
                                new XAttribute(OSIS_ID, $"{book}.9.38")));

                        var prevItem = Neh.Descendants().Where(x => x.Name.LocalName == CHAPTER && x.Attribute(END_ID) != null && x.Attribute(END_ID).Value == $"{book}.9").FirstOrDefault();
                        prevItem.AddBeforeSelf(objects);
                        Neh.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"{book}.{ch}.1").Remove();

                        var vn = 1;
                        for (int i = 2; i < 41; i++) {
                            var items = Neh.Descendants().Where(x => (x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"{book}.{ch}.{i}") || (x.Name.LocalName == NOTE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value.StartsWith($"{book}.{ch}.{i}!")));
                            foreach (var item in items) {
                                if (item.Attribute(OSIS_ID) != null) {
                                    item.Add(new XAttribute(OLD_ID, item.Attribute(OSIS_ID).Value));
                                }
                                if (item.Name.LocalName == VERSE && item.Attribute(OSIS_ID) != null) {
                                    item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}";
                                }
                                else if (item.Name.LocalName == NOTE && item.Attribute(OSIS_ID) != null) {
                                    item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}!note.{item.Attribute("n").Value}";
                                }
                                if (item.Attribute(START_ID) != null) { item.Attribute(START_ID).Value = $"{book}.{ch}.{vn}"; }
                                if (item.Attribute(END_ID) != null) { item.Attribute(END_ID).Value = $"{book}.{ch}.{vn}"; }
                                if (item.Name.LocalName == NOTE && item.Attribute(REF_ID) != null) { item.Attribute(REF_ID).Value = $"{book}.{ch}.{vn}"; }

                            }
                            vn++;
                        }
                    }
                }

                book = "Job";
                var Job = (xml.FirstNode as XElement).Elements().Where(x => x.Name.LocalName == DIV && x.Attribute(OSIS_ID).Value == book).FirstOrDefault();
                if (Job != null) {
                    var ch = 41;
                    var vn = 34;
                    for (int i = 26; i > 0; i--) {
                        var items = Job.Descendants().Where(x => (x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"{book}.{ch}.{i}") || (x.Name.LocalName == NOTE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value.StartsWith($"{book}.{ch}.{i}!")));
                        foreach (var item in items) {
                            if (item.Attribute(OSIS_ID) != null) {
                                item.Add(new XAttribute(OLD_ID, item.Attribute(OSIS_ID).Value));
                            }
                            if (item.Name.LocalName == VERSE && item.Attribute(OSIS_ID) != null) {
                                item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}";
                            }
                            else if (item.Name.LocalName == NOTE && item.Attribute(OSIS_ID) != null) {
                                item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}!note.{item.Attribute("n").Value}";
                            }
                            if (item.Attribute(START_ID) != null) { item.Attribute(START_ID).Value = $"{book}.{ch}.{vn}"; }
                            if (item.Attribute(END_ID) != null) { item.Attribute(END_ID).Value = $"{book}.{ch}.{vn}"; }
                            if (item.Name.LocalName == NOTE && item.Attribute(REF_ID) != null) { item.Attribute(REF_ID).Value = $"{book}.{ch}.{vn}"; }

                        }
                        vn--;
                    }


                    vn = 1;
                    for (int i = 25; i < 33; i++) {
                        var items = Job.Descendants().Where(x => (x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"{book}.40.{i}") || (x.Name.LocalName == NOTE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value.StartsWith($"{book}.40.{i}!")));
                        foreach (var item in items) {
                            if (item.Attribute(OSIS_ID) != null) {
                                item.Add(new XAttribute(OLD_ID, item.Attribute(OSIS_ID).Value));
                            }
                            if (item.Name.LocalName == VERSE && item.Attribute(OSIS_ID) != null) {
                                item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}";
                            }
                            else if (item.Name.LocalName == NOTE && item.Attribute(OSIS_ID) != null) {
                                item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}!note.{item.Attribute("n").Value}";
                            }
                            if (item.Attribute(START_ID) != null) { item.Attribute(START_ID).Value = $"{book}.{ch}.{vn}"; }
                            if (item.Attribute(END_ID) != null) { item.Attribute(END_ID).Value = $"{book}.{ch}.{vn}"; }
                            if (item.Name.LocalName == NOTE && item.Attribute(REF_ID) != null) { item.Attribute(REF_ID).Value = $"{book}.{ch}.{vn}"; }

                        }
                        vn++;
                    }

                    var ch40 = Job.Descendants().Where(x => x.Name.LocalName == CHAPTER && x.Attribute(END_ID) != null && x.Attribute(END_ID).Value == $"{book}.40").FirstOrDefault();
                    var lstv = Job.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(END_ID) != null && x.Attribute(END_ID).Value == $"{book}.40.24").FirstOrDefault();
                    lstv.AddAfterSelf(ch40);
                    ch40.Remove();

                    var ch41 = Job.Descendants().Where(x => x.Name.LocalName == CHAPTER && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == $"{book}.41").FirstOrDefault();
                    lstv = Job.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == $"{book}.41.1").FirstOrDefault();
                    lstv.AddBeforeSelf(ch41);
                    ch41.Remove();
                }

                book = "Ps";
                var Ps = (xml.FirstNode as XElement).Elements().Where(x => x.Name.LocalName == DIV && x.Attribute(OSIS_ID).Value == book).FirstOrDefault();
                if (Ps != null) {
                    MoveUpOne(Ps, book, 3);
                    MoveUpOne(Ps, book, 4);
                    MoveUpOne(Ps, book, 5);
                    MoveUpOne(Ps, book, 6);
                    MoveUpOne(Ps, book, 7);
                    MoveUpOne(Ps, book, 8);
                    MoveUpOne(Ps, book, 9);
                    MoveUpOne(Ps, book, 12);
                    MoveUpOne(Ps, book, 18);
                    MoveUpOne(Ps, book, 19);
                    MoveUpOne(Ps, book, 20);
                    MoveUpOne(Ps, book, 21);
                    MoveUpOne(Ps, book, 22);
                    MoveUpOne(Ps, book, 30);
                    MoveUpOne(Ps, book, 31);
                    MoveUpOne(Ps, book, 34);
                    MoveUpOne(Ps, book, 36);
                    MoveUpOne(Ps, book, 38);
                    MoveUpOne(Ps, book, 39);
                    MoveUpOne(Ps, book, 40);
                    MoveUpOne(Ps, book, 41);
                    MoveUpOne(Ps, book, 42);
                    MoveUpOne(Ps, book, 44);
                    MoveUpOne(Ps, book, 45);
                    MoveUpOne(Ps, book, 46);
                    MoveUpOne(Ps, book, 47);
                    MoveUpOne(Ps, book, 48);
                    MoveUpOne(Ps, book, 49);
                    MoveUpOne(Ps, book, 51);
                    MoveUpOne(Ps, book, 51);
                    MoveUpOne(Ps, book, 52);
                    MoveUpOne(Ps, book, 52);
                    MoveUpOne(Ps, book, 53);
                    MoveUpOne(Ps, book, 54);
                    MoveUpOne(Ps, book, 54);
                    MoveUpOne(Ps, book, 55);
                    MoveUpOne(Ps, book, 56);
                    MoveUpOne(Ps, book, 57);
                    MoveUpOne(Ps, book, 58);
                    MoveUpOne(Ps, book, 59);
                    MoveUpOne(Ps, book, 60);
                    MoveUpOne(Ps, book, 60);
                    MoveUpOne(Ps, book, 61);
                    MoveUpOne(Ps, book, 62);
                    MoveUpOne(Ps, book, 63);
                    MoveUpOne(Ps, book, 64);
                    MoveUpOne(Ps, book, 65);
                    MoveUpOne(Ps, book, 67);
                    MoveUpOne(Ps, book, 68);
                    MoveUpOne(Ps, book, 69);
                    MoveUpOne(Ps, book, 70);
                    MoveUpOne(Ps, book, 75);
                    MoveUpOne(Ps, book, 76);
                    MoveUpOne(Ps, book, 77);
                    MoveUpOne(Ps, book, 80);
                    MoveUpOne(Ps, book, 81);
                    MoveUpOne(Ps, book, 83);
                    MoveUpOne(Ps, book, 84);
                    MoveUpOne(Ps, book, 85);
                    MoveUpOne(Ps, book, 88);
                    MoveUpOne(Ps, book, 89);
                    MoveUpOne(Ps, book, 92);
                    MoveUpOne(Ps, book, 102);
                    MoveUpOne(Ps, book, 108);
                    MoveUpOne(Ps, book, 140);
                    MoveUpOne(Ps, book, 142);
                }

                book = "Eccl";
                var Ecc = (xml.FirstNode as XElement).Elements().Where(x => x.Name.LocalName == DIV && x.Attribute(OSIS_ID).Value == book).FirstOrDefault();
                if (Ecc != null) {
                    // MoveLastVerseOfChapterToNext(Ecc, book, 4, 17, 5, 19);
                    MoveSectionToNextChapter(Ecc, book, 4, 17, 17, 5, 2);
                }

                book = "Isa";
                var Isa = (xml.FirstNode as XElement).Elements().Where(x => x.Name.LocalName == DIV && x.Attribute(OSIS_ID).Value == book).FirstOrDefault();
                if (Isa != null) {
                    MoveLastVerseOfChapterToNext(Isa, book, 8, 23, 9, 20);
                }

                book = "Jer";
                var Jer = (xml.FirstNode as XElement).Elements().Where(x => x.Name.LocalName == DIV && x.Attribute(OSIS_ID).Value == book).FirstOrDefault();
                if (Jer != null) {
                    MoveLastVerseOfChapterToNext(Jer, book, 8, 23, 9, 25);
                }

                book = "Ezek";
                var Ezek = (xml.FirstNode as XElement).Elements().Where(x => x.Name.LocalName == DIV && x.Attribute(OSIS_ID).Value == book).FirstOrDefault();
                if (Ezek != null) {
                    MoveSectionToPreviousChapter(Ezek, book, 21, 1, 5, 20, 44);
                }

                book = "Dan";
                var Dan = (xml.FirstNode as XElement).Elements().Where(x => x.Name.LocalName == DIV && x.Attribute(OSIS_ID).Value == book).FirstOrDefault();
                if (Dan != null) {
                    MoveSectionToNextChapter(Dan, book, 3, 31, 33, 4, 4);
                    MoveFirstVerseToPreviousChapter(Dan, book, 6, 5, 31, 29);
                }

                book = "Hos";
                var Hos = (xml.FirstNode as XElement).Elements().Where(x => x.Name.LocalName == DIV && x.Attribute(OSIS_ID).Value == book).FirstOrDefault();
                if (Hos != null) {
                    MoveFirstVerseToPreviousChapter(Hos, book, 2, 1, 10, 25);
                    MoveFirstVerseToPreviousChapter(Hos, book, 2, 1, 11, 24);
                    MoveFirstVerseToPreviousChapter(Hos, book, 12, 11, 12, 15);
                }

                book = "Joel";
                var Joel = (xml.FirstNode as XElement).Elements().Where(x => x.Name.LocalName == DIV && x.Attribute(OSIS_ID).Value == book).FirstOrDefault();
                if (Joel != null) {
                    MoveSectionToPreviousChapter(Joel, book, 3, 1, 5, 2, 27);
                    ChangeChapterNumber(Joel, book, 4, 3);
                }

                book = "Jonah";
                var Jon = (xml.FirstNode as XElement).Elements().Where(x => x.Name.LocalName == DIV && x.Attribute(OSIS_ID).Value == book).FirstOrDefault();
                if (Jon != null) {
                    MoveFirstVerseToPreviousChapter(Jon, book, 2, 1, 17, 11);
                }

                book = "Mic";
                var Mic = (xml.FirstNode as XElement).Elements().Where(x => x.Name.LocalName == DIV && x.Attribute(OSIS_ID).Value == book).FirstOrDefault();
                if (Mic != null) {
                    var ch = 5;
                    var vn = 15;
                    for (int i = vn - 1; i > 0; i--) {
                        var items = Mic.Descendants().Where(x => (x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"{book}.{ch}.{i}") || (x.Name.LocalName == NOTE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value.StartsWith($"{book}.{ch}.{i}!")));
                        foreach (var item in items) {
                            if (item.Attribute(OSIS_ID) != null) {
                                item.Add(new XAttribute(OLD_ID, item.Attribute(OSIS_ID).Value));
                            }
                            if (item.Name.LocalName == VERSE && item.Attribute(OSIS_ID) != null) {
                                item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}";
                            }
                            else if (item.Name.LocalName == NOTE && item.Attribute(OSIS_ID) != null) {
                                item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}!note.{item.Attribute("n").Value}";
                            }
                            if (item.Attribute(START_ID) != null) { item.Attribute(START_ID).Value = $"{book}.{ch}.{vn}"; }
                            if (item.Attribute(END_ID) != null) { item.Attribute(END_ID).Value = $"{book}.{ch}.{vn}"; }
                            if (item.Name.LocalName == NOTE && item.Attribute(REF_ID) != null) { item.Attribute(REF_ID).Value = $"{book}.{ch}.{vn}"; }

                        }
                        vn--;
                    }

                    var verseItem = Mic.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == $"{book}.4.14").FirstOrDefault();
                    var objects = new List<object>{
                            new XElement(XName.Get(VERSE, Mic.Name.NamespaceName),
                                new XAttribute(START_ID, $"{book}.{ch}.1"),
                                new XAttribute(OSIS_ID, $"{book}.{ch}.1")),
                            verseItem.NextNode,
                            new XElement(XName.Get(VERSE, verseItem.Name.NamespaceName),
                                new XAttribute(END_ID, $"{book}.{ch}.1"),
                                new XAttribute(OSIS_ID, $"{book}.{ch}.1"))
                        };
                    verseItem.NextNode.Remove();
                    Mic.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"{book}.4.14").Remove();
                    var ch5 = Mic.Descendants().Where(x => x.Name.LocalName == CHAPTER && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == $"{book}.5").FirstOrDefault();
                    ch5.AddAfterSelf(objects);
                }

                book = "Nah";
                var Nah = (xml.FirstNode as XElement).Elements().Where(x => x.Name.LocalName == DIV && x.Attribute(OSIS_ID).Value == book).FirstOrDefault();
                if (Nah != null) {
                    var ch = 2;
                    var objects = new List<object>{
                            new XElement(XName.Get(VERSE, Nah.Name.NamespaceName),
                                new XAttribute(START_ID, $"{book}.1.15"),
                                new XAttribute(OSIS_ID, $"{book}.1.15"))
                        };
                    var verseItem = Nah.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == $"{book}.{ch}.1").FirstOrDefault();
                    while (verseItem.NextNode != null) {
                        if (verseItem.NextNode is XElement && (verseItem.NextNode as XElement).Name.LocalName == VERSE) { break; }
                        if (verseItem.NextNode is XElement && (verseItem.NextNode as XElement).Name.LocalName == NOTE) {
                            (verseItem.NextNode as XElement).Attribute(OSIS_ID).Value = (verseItem.NextNode as XElement).Attribute(OSIS_ID).Value.Replace($"{book}.{ch}.1", $"{book}.1.15");
                            (verseItem.NextNode as XElement).Attribute(REF_ID).Value = (verseItem.NextNode as XElement).Attribute(REF_ID).Value.Replace($"{book}.{ch}.1", $"{book}.1.15");
                        }
                        objects.Add(verseItem.NextNode);
                        verseItem.NextNode.Remove();
                    }
                    objects.Add(new XElement(XName.Get(VERSE, verseItem.Name.NamespaceName),
                          new XAttribute(END_ID, $"{book}.1.15"),
                          new XAttribute(OSIS_ID, $"{book}.1.15")));

                    var prevItem = Nah.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(END_ID) != null && x.Attribute(END_ID).Value == $"{book}.1.14").FirstOrDefault();
                    prevItem.AddAfterSelf(objects);
                    Nah.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"{book}.{ch}.1").Remove();

                    var vn = 1;
                    for (int i = 2; i < 41; i++) {
                        var items = Nah.Descendants().Where(x => (x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"{book}.{ch}.{i}") || (x.Name.LocalName == NOTE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value.StartsWith($"{book}.{ch}.{i}!")));
                        foreach (var item in items) {
                            if (item.Attribute(OSIS_ID) != null) {
                                item.Add(new XAttribute(OLD_ID, item.Attribute(OSIS_ID).Value));
                            }
                            if (item.Name.LocalName == VERSE && item.Attribute(OSIS_ID) != null) {
                                item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}";
                            }
                            else if (item.Name.LocalName == NOTE && item.Attribute(OSIS_ID) != null) {
                                item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}!note.{item.Attribute("n").Value}";
                            }
                            if (item.Attribute(START_ID) != null) { item.Attribute(START_ID).Value = $"{book}.{ch}.{vn}"; }
                            if (item.Attribute(END_ID) != null) { item.Attribute(END_ID).Value = $"{book}.{ch}.{vn}"; }
                            if (item.Name.LocalName == NOTE && item.Attribute(REF_ID) != null) { item.Attribute(REF_ID).Value = $"{book}.{ch}.{vn}"; }

                        }
                        vn++;
                    }
                }

                book = "Zech";
                var Zech = (xml.FirstNode as XElement).Elements().Where(x => x.Name.LocalName == DIV && x.Attribute(OSIS_ID).Value == book).FirstOrDefault();
                if (Zech != null) {
                    var vn = 18;
                    var ch = 1;
                    for (int i = 1; i < 5; i++) {
                        var items = Zech.Descendants().Where(x => (x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"{book}.2.{i}") || (x.Name.LocalName == NOTE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value.StartsWith($"{book}.2.{i}!")));
                        foreach (var item in items) {
                            if (item.Attribute(OSIS_ID) != null) {
                                item.Add(new XAttribute(OLD_ID, item.Attribute(OSIS_ID).Value));
                            }
                            if (item.Name.LocalName == VERSE && item.Attribute(OSIS_ID) != null) {
                                item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}";
                            }
                            else if (item.Name.LocalName == NOTE && item.Attribute(OSIS_ID) != null) {
                                item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}!note.{item.Attribute("n").Value}";
                            }
                            if (item.Attribute(START_ID) != null) { item.Attribute(START_ID).Value = $"{book}.{ch}.{vn}"; }
                            if (item.Attribute(END_ID) != null) { item.Attribute(END_ID).Value = $"{book}.{ch}.{vn}"; }
                            if (item.Name.LocalName == NOTE && item.Attribute(REF_ID) != null) { item.Attribute(REF_ID).Value = $"{book}.{ch}.{vn}"; }

                        }
                        vn++;
                    }

                    ch = 2;
                    vn = 1;
                    for (int i = 5; i < 18; i++) {
                        var items = Zech.Descendants().Where(x => (x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"{book}.{ch}.{i}") || (x.Name.LocalName == NOTE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value.StartsWith($"{book}.{ch}.{i}!")));
                        foreach (var item in items) {
                            if (item.Attribute(OSIS_ID) != null) {
                                item.Add(new XAttribute(OLD_ID, item.Attribute(OSIS_ID).Value));
                            }
                            if (item.Name.LocalName == VERSE && item.Attribute(OSIS_ID) != null) {
                                item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}";
                            }
                            else if (item.Name.LocalName == NOTE && item.Attribute(OSIS_ID) != null) {
                                item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}!note.{item.Attribute("n").Value}";
                            }
                            if (item.Attribute(START_ID) != null) { item.Attribute(START_ID).Value = $"{book}.{ch}.{vn}"; }
                            if (item.Attribute(END_ID) != null) { item.Attribute(END_ID).Value = $"{book}.{ch}.{vn}"; }
                            if (item.Name.LocalName == NOTE && item.Attribute(REF_ID) != null) { item.Attribute(REF_ID).Value = $"{book}.{ch}.{vn}"; }

                        }
                        vn++;
                    }

                    var ch1 = Zech.Descendants().Where(x => x.Name.LocalName == CHAPTER && x.Attribute(END_ID) != null && x.Attribute(END_ID).Value == $"{book}.1").FirstOrDefault();
                    var lstv = Zech.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(END_ID) != null && x.Attribute(END_ID).Value == $"{book}.1.21").FirstOrDefault();
                    lstv.AddAfterSelf(ch1);
                    ch1.Remove();

                    var ch2 = Zech.Descendants().Where(x => x.Name.LocalName == CHAPTER && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == $"{book}.2").FirstOrDefault();
                    lstv = Zech.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == $"{book}.2.1").FirstOrDefault();
                    lstv.AddBeforeSelf(ch2);
                    ch2.Remove();
                }

                book = "Mal";
                var Mal = (xml.FirstNode as XElement).Elements().Where(x => x.Name.LocalName == DIV && x.Attribute(OSIS_ID).Value == book).FirstOrDefault();
                if (Mal != null) {
                    var vn = 1;
                    var ch = 4;
                    for (int i = 19; i < 25; i++) {
                        var items = Mal.Descendants().Where(x => (x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"{book}.3.{i}") || (x.Name.LocalName == NOTE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value.StartsWith($"{book}.3.{i}!")));
                        foreach (var item in items) {
                            if (item.Attribute(OSIS_ID) != null) {
                                item.Add(new XAttribute(OLD_ID, item.Attribute(OSIS_ID).Value));
                            }
                            if (item.Name.LocalName == VERSE && item.Attribute(OSIS_ID) != null) {
                                item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}";
                            }
                            else if (item.Name.LocalName == NOTE && item.Attribute(OSIS_ID) != null) {
                                item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}!note.{item.Attribute("n").Value}";
                            }
                            if (item.Attribute(START_ID) != null) { item.Attribute(START_ID).Value = $"{book}.{ch}.{vn}"; }
                            if (item.Attribute(END_ID) != null) { item.Attribute(END_ID).Value = $"{book}.{ch}.{vn}"; }
                            if (item.Name.LocalName == NOTE && item.Attribute(REF_ID) != null) { item.Attribute(REF_ID).Value = $"{book}.{ch}.{vn}"; }

                        }
                        vn++;
                    }
                    var ch3 = Mal.Descendants().Where(x => x.Name.LocalName == CHAPTER && x.Attribute(END_ID) != null && x.Attribute(END_ID).Value == $"{book}.3").FirstOrDefault();
                    var lstv = Mal.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(END_ID) != null && x.Attribute(END_ID).Value == $"{book}.3.18").FirstOrDefault();
                    lstv.AddAfterSelf(ch3);
                    ch3.Remove();

                    lstv = Mal.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == $"{book}.{ch}.1").FirstOrDefault();
                    lstv.AddBeforeSelf(new XElement(XName.Get(CHAPTER, lstv.Name.NamespaceName), new XAttribute(START_ID, $"{book}.{ch}"), new XAttribute(OSIS_ID, $"{book}.{ch}")));

                    lstv = Mal.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(END_ID) != null && x.Attribute(END_ID).Value == $"{book}.{ch}.6").FirstOrDefault();
                    lstv.AddAfterSelf(new XElement(XName.Get(CHAPTER, lstv.Name.NamespaceName), new XAttribute(END_ID, $"{book}.{ch}"), new XAttribute(OSIS_ID, $"{book}.{ch}")));

                }

                MoveTitleBeforeEndChapterToNextChapterBeginning(xml);

                xml.Save(result);
                return result;
            }
            return filePath;
        }

        void MoveTitleBeforeEndChapterToNextChapterBeginning(XElement e) {
            var titles = e.Descendants(XName.Get(TITLE, e.Name.NamespaceName))
                .Where(x => x.PreviousNode == null && 
                           x.NextNode != null && 
                           x.NextNode is XElement &&
                           (
                            (
                                (x.NextNode as XElement).Name.LocalName == CHAPTER && 
                                (x.NextNode as XElement).Attribute(END_ID) != null
                            ) ||
                            (
                                (x.NextNode as XElement).Name.LocalName == VERSE &&
                                (x.NextNode as XElement).Attribute(END_ID) != null &&
                                (x.NextNode.NextNode as XElement).Name.LocalName == CHAPTER &&
                                (x.NextNode.NextNode as XElement).Attribute(END_ID) != null
                            )
                           )
                     )
                .ToArray();   
            
            if (titles.Any()) {
                var length = titles.Length;
                for (int i = 0; i < length; i++) {
                    var title = titles[i];

                    var chapterEnd = title.ElementsAfterSelf(XName.Get(CHAPTER, title.Name.NamespaceName)).First() as XElement;
                    if (chapterEnd != null) {
                        var next = chapterEnd.NextNode as XElement;
                        if (next != null && next.Name.LocalName == CHAPTER) {
                            next.AddAfterSelf(title);
                            title.Remove();
                        }
                    }                    
                }
                
            }
        }

        void MoveLastVerseOfChapterToNext(XElement e, string book, int chFrom, int verseFrom, int ch, int vc) {
            var lastVerseStart = e.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == $"{book}.{chFrom}.{verseFrom}").FirstOrDefault();
            var objects = new List<object> {
                            new XElement(XName.Get(VERSE, e.Name.NamespaceName),
                                new XAttribute(START_ID, $"{book}.{ch}.1"),
                                new XAttribute(OSIS_ID, $"{book}.{ch}.1")),
            };

            while (lastVerseStart.NextNode != null) {
                if (lastVerseStart.NextNode is XElement && (lastVerseStart.NextNode as XElement).Name.LocalName == VERSE) { break; }
                if (lastVerseStart.NextNode is XElement && (lastVerseStart.NextNode as XElement).Name.LocalName == NOTE) {
                    (lastVerseStart.NextNode as XElement).Attribute(OSIS_ID).Value = (lastVerseStart.NextNode as XElement).Attribute(OSIS_ID).Value.Replace($"{book}.{chFrom}.{verseFrom}", $"{book}.{ch}.1");
                    (lastVerseStart.NextNode as XElement).Attribute(REF_ID).Value = (lastVerseStart.NextNode as XElement).Attribute(REF_ID).Value.Replace($"{book}.{chFrom}.{verseFrom}", $"{book}.{ch}.1");
                }
                objects.Add(lastVerseStart.NextNode);
                lastVerseStart.NextNode.Remove();
            }
            e.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"{book}.{chFrom}.{verseFrom}").Remove();


            objects.Add(new XElement(XName.Get(VERSE, e.Name.NamespaceName),
                                new XAttribute(END_ID, $"{book}.{ch}.1"),
                                new XAttribute(OSIS_ID, $"{book}.{ch}.1")));

            var vn = vc + 1;
            for (int i = vc; i > 0; i--) {
                var items = e.Descendants().Where(x => (x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"{book}.{ch}.{i}") || (x.Name.LocalName == NOTE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value.StartsWith($"{book}.{ch}.{i}!")));
                if (items.Count() == 0) { break; }
                foreach (var item in items) {
                    if (item.Attribute(OSIS_ID) != null) {
                        item.Add(new XAttribute(OLD_ID, item.Attribute(OSIS_ID).Value));
                    }
                    if (item.Name.LocalName == VERSE && item.Attribute(OSIS_ID) != null) {
                        item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}";
                    }
                    else if (item.Name.LocalName == NOTE && item.Attribute(OSIS_ID) != null) {
                        item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}!note.{item.Attribute("n").Value}";
                    }
                    if (item.Attribute(START_ID) != null) { item.Attribute(START_ID).Value = $"{book}.{ch}.{vn}"; }
                    if (item.Attribute(END_ID) != null) { item.Attribute(END_ID).Value = $"{book}.{ch}.{vn}"; }
                    if (item.Name.LocalName == NOTE && item.Attribute(REF_ID) != null) { item.Attribute(REF_ID).Value = $"{book}.{ch}.{vn}"; }

                }
                vn--;
            }

            var secondVerseStart = e.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == $"{book}.{ch}.2").FirstOrDefault();
            secondVerseStart.AddBeforeSelf(objects);
        }
        void MoveFirstVerseToPreviousChapter(XElement e, string book, int chFrom, int chTo, int verseTo, int vc) {
            var firstVerseStart = e.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == $"{book}.{chFrom}.1").FirstOrDefault();
            var objects = new List<object>{
                new XElement(XName.Get(VERSE, e.Name.NamespaceName),
                    new XAttribute(START_ID, $"{book}.{chTo}.{verseTo}"),
                    new XAttribute(OSIS_ID, $"{book}.{chTo}.{verseTo}"))
            };

            if (firstVerseStart != null) {
                while (firstVerseStart.NextNode != null) {
                    if (firstVerseStart.NextNode is XElement && (firstVerseStart.NextNode as XElement).Name.LocalName == VERSE) { break; }
                    if (firstVerseStart.NextNode is XElement && (firstVerseStart.NextNode as XElement).Name.LocalName == NOTE) {
                        (firstVerseStart.NextNode as XElement).Attribute(OSIS_ID).Value = (firstVerseStart.NextNode as XElement).Attribute(OSIS_ID).Value.Replace($"{book}.{chFrom}.1", $"{book}.{chTo}.{verseTo}");
                        (firstVerseStart.NextNode as XElement).Attribute(REF_ID).Value = (firstVerseStart.NextNode as XElement).Attribute(REF_ID).Value.Replace($"{book}.{chFrom}.1", $"{book}.{chTo}.{verseTo}");
                    }
                    objects.Add(firstVerseStart.NextNode);
                    firstVerseStart.NextNode.Remove();
                }
            }

            e.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"{book}.{chFrom}.1").Remove();

            objects.Add(new XElement(XName.Get(VERSE, e.Name.NamespaceName),
                    new XAttribute(END_ID, $"{book}.{chTo}.{verseTo}"),
                    new XAttribute(OSIS_ID, $"{book}.{chTo}.{verseTo}")));

            var lastVerseEnd = e.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(END_ID) != null && x.Attribute(END_ID).Value == $"{book}.{chTo}.{(verseTo - 1)}").FirstOrDefault();
            if (lastVerseEnd != null) {
                lastVerseEnd.AddAfterSelf(objects);
            }

            var vn = 1;
            for (int i = 2; i < vc + 1; i++) {
                var items = e.Descendants().Where(x => (x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"{book}.{chFrom}.{i}") || (x.Name.LocalName == NOTE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value.StartsWith($"{book}.{chFrom}.{i}!")));
                if (items.Count() == 0) { continue; }

                foreach (var item in items) {
                    if (item.Attribute(OSIS_ID) != null) {
                        if (item.Attribute(OLD_ID) == null) { item.Add(new XAttribute(OLD_ID, item.Attribute(OSIS_ID).Value)); }
                    }
                    if (item.Name.LocalName == VERSE && item.Attribute(OSIS_ID) != null) {
                        item.Attribute(OSIS_ID).Value = $"{book}.{chFrom}.{vn}";
                    }
                    else if (item.Name.LocalName == NOTE && item.Attribute(OSIS_ID) != null) {
                        item.Attribute(OSIS_ID).Value = $"{book}.{chFrom}.{vn}!note.{item.Attribute("n").Value}";
                    }
                    if (item.Attribute(START_ID) != null) { item.Attribute(START_ID).Value = $"{book}.{chFrom}.{vn}"; }
                    if (item.Attribute(END_ID) != null) { item.Attribute(END_ID).Value = $"{book}.{chFrom}.{vn}"; }
                    if (item.Name.LocalName == NOTE && item.Attribute(REF_ID) != null) { item.Attribute(REF_ID).Value = $"{book}.{chFrom}.{vn}"; }

                }
                vn++;
            }
        }
        XElement MoveSectionToPreviousChapter(XElement e, string book, int chFrom, int verseFrom, int verseTo, int ch, int vs) {
            var section = e.Descendants().Where(x => x.Name.LocalName == CHAPTER && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == $"{book}.{chFrom}").FirstOrDefault().Parent;
            if (section != null) {
                var previousSection = section.PreviousNode as XElement;
                if (previousSection != null) {
                    previousSection.Elements().Where(x => x.Name.LocalName == CHAPTER && x.Attribute(END_ID) != null && x.Attribute(END_ID).Value == $"{book}.{ch}").Remove();
                }
                section.Elements().Where(x => x.Name.LocalName == CHAPTER && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == $"{book}.{chFrom}").Remove();

                var n = 1;
                var vn = vs + 1;
                for (int i = verseFrom; i < verseTo + 1; i++) {
                    var items = section.Descendants().Where(x => (x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"{book}.{chFrom}.{i}") || (x.Name.LocalName == NOTE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value.StartsWith($"{book}.{chFrom}.{i}!")));
                    foreach (var item in items) {
                        if (item.Attribute(OSIS_ID) != null) {
                            item.Add(new XAttribute(OLD_ID, item.Attribute(OSIS_ID).Value));
                        }
                        if (item.Name.LocalName == VERSE && item.Attribute(OSIS_ID) != null) {
                            item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}";
                        }
                        else if (item.Name.LocalName == NOTE && item.Attribute(OSIS_ID) != null) {
                            item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}!note.{item.Attribute("n").Value}";
                        }
                        if (item.Attribute(START_ID) != null) { item.Attribute(START_ID).Value = $"{book}.{ch}.{vn}"; }
                        if (item.Attribute(END_ID) != null) { item.Attribute(END_ID).Value = $"{book}.{ch}.{vn}"; }
                        if (item.Name.LocalName == NOTE && item.Attribute(REF_ID) != null) { item.Attribute(REF_ID).Value = $"{book}.{ch}.{vn}"; }

                    }
                    vn++;
                    n++;
                }

                section.Add(new XElement(XName.Get(CHAPTER, e.Name.NamespaceName),
                    new XAttribute(END_ID, $"{book}.{ch}"),
                    new XAttribute(OSIS_ID, $"{book}.{ch}")
                    ));

                ch = chFrom;
                vn = 1;
                for (int i = n; i < 200; i++) {
                    var items = e.Descendants().Where(x => (x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"{book}.{ch}.{i}") || (x.Name.LocalName == NOTE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value.StartsWith($"{book}.{ch}.{i}!")));
                    if (items.Count() == 0) { break; }
                    foreach (var item in items) {
                        if (item.Attribute(OSIS_ID) != null) {
                            item.Add(new XAttribute(OLD_ID, item.Attribute(OSIS_ID).Value));
                        }
                        if (item.Name.LocalName == VERSE && item.Attribute(OSIS_ID) != null) {
                            item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}";
                        }
                        else if (item.Name.LocalName == NOTE && item.Attribute(OSIS_ID) != null) {
                            item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}!note.{item.Attribute("n").Value}";
                        }
                        if (item.Attribute(START_ID) != null) { item.Attribute(START_ID).Value = $"{book}.{ch}.{vn}"; }
                        if (item.Attribute(END_ID) != null) { item.Attribute(END_ID).Value = $"{book}.{ch}.{vn}"; }
                        if (item.Name.LocalName == NOTE && item.Attribute(REF_ID) != null) { item.Attribute(REF_ID).Value = $"{book}.{ch}.{vn}"; }

                    }
                    vn++;
                }

                var lstv = e.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == $"{book}.{ch}.1").FirstOrDefault();
                if (lstv != null) {
                    lstv.AddBeforeSelf(new XElement(XName.Get(CHAPTER, lstv.Name.NamespaceName), new XAttribute(START_ID, $"{book}.{ch}"), new XAttribute(OSIS_ID, $"{book}.{ch}")));
                }
                else {
                    e.Descendants().Where(x => x.Name.LocalName == CHAPTER && x.Attribute(END_ID) != null && x.Attribute(END_ID).Value == $"{book}.{ch}").Remove();
                }
            }

            return section;
        }
        void MoveSectionToNextChapter(XElement e, string book, int chFrom, int verseFrom, int verseTo, int ch, int vs) {

            e.Descendants().Where(x => x.Name.LocalName == CHAPTER && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == $"{book}.{ch}").Remove();
            var vn = vs;
            for (int i = 200; i > 0; i--) {
                var items = e.Descendants().Where(x => (x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"{book}.{ch}.{i}") || (x.Name.LocalName == NOTE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value.StartsWith($"{book}.{ch}.{i}!")));
                if (items.Count() == 0) { continue; }
                vn = i + vs - 1;
                foreach (var item in items) {
                    if (item.Attribute(OSIS_ID) != null) {
                        item.Add(new XAttribute(OLD_ID, item.Attribute(OSIS_ID).Value));
                    }
                    if (item.Name.LocalName == VERSE && item.Attribute(OSIS_ID) != null) {
                        item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}";
                    }
                    else if (item.Name.LocalName == NOTE && item.Attribute(OSIS_ID) != null) {
                        item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}!note.{item.Attribute("n").Value}";
                    }
                    if (item.Attribute(START_ID) != null) { item.Attribute(START_ID).Value = $"{book}.{ch}.{vn}"; }
                    if (item.Attribute(END_ID) != null) { item.Attribute(END_ID).Value = $"{book}.{ch}.{vn}"; }
                    if (item.Name.LocalName == NOTE && item.Attribute(REF_ID) != null) { item.Attribute(REF_ID).Value = $"{book}.{ch}.{vn}"; }

                }
                vn--;
            }

            var section = e.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == $"{book}.{chFrom}.{verseFrom}").FirstOrDefault().Parent;
            if (section != null) {

                var endChapter = section.Elements().Where(x => x.Name.LocalName == CHAPTER && x.Attribute(END_ID) != null && x.Attribute(END_ID).Value == $"{book}.{chFrom}").FirstOrDefault();
                if (endChapter != null) {
                    var previousSection = section.PreviousNode as XElement;
                    if (previousSection != null) {
                        previousSection.Add(endChapter);
                        endChapter.Remove();
                    }
                }

                vn = 1;
                for (int i = verseFrom; i < verseTo + 1; i++) {
                    var items = section.Descendants().Where(x => (x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"{book}.{chFrom}.{i}") || (x.Name.LocalName == NOTE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value.StartsWith($"{book}.{chFrom}.{i}!")));
                    foreach (var item in items) {
                        if (item.Attribute(OSIS_ID) != null) {
                            item.Add(new XAttribute(OLD_ID, item.Attribute(OSIS_ID).Value));
                        }
                        if (item.Name.LocalName == VERSE && item.Attribute(OSIS_ID) != null) {
                            item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}";
                        }
                        else if (item.Name.LocalName == NOTE && item.Attribute(OSIS_ID) != null) {
                            item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}!note.{item.Attribute("n").Value}";
                        }
                        if (item.Attribute(START_ID) != null) { item.Attribute(START_ID).Value = $"{book}.{ch}.{vn}"; }
                        if (item.Attribute(END_ID) != null) { item.Attribute(END_ID).Value = $"{book}.{ch}.{vn}"; }
                        if (item.Name.LocalName == NOTE && item.Attribute(REF_ID) != null) { item.Attribute(REF_ID).Value = $"{book}.{ch}.{vn}"; }

                    }
                    vn++;
                }

                var lstv = e.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == $"{book}.{ch}.1").FirstOrDefault();
                lstv.AddBeforeSelf(new XElement(XName.Get(CHAPTER, lstv.Name.NamespaceName), new XAttribute(START_ID, $"{book}.{ch}"), new XAttribute(OSIS_ID, $"{book}.{ch}")));

            }
        }
        void ChangeChapterNumber(XElement e, string book, int chFrom, int chTo) {
            var items = e.Descendants().Where(x => (x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value.StartsWith($"{book}.{chFrom}")) ||
                                                   (x.Name.LocalName == NOTE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value.StartsWith($"{book}.{chFrom}")));
            foreach (var item in items) {
                var vn = 0;
                if (item.Attribute(OSIS_ID) != null) {
                    var pattern = @"[a-zA-Z]\.[0-9]+\.(?<vn>[0-9]+)";
                    vn = Regex.Match(item.Attribute(OSIS_ID).Value, pattern).Groups["vn"].Value.ToInt();
                    item.Add(new XAttribute(OLD_ID, item.Attribute(OSIS_ID).Value));
                }
                if (item.Name.LocalName == VERSE && item.Attribute(OSIS_ID) != null) {
                    item.Attribute(OSIS_ID).Value = $"{book}.{chTo}.{vn}";
                }
                else if (item.Name.LocalName == NOTE && item.Attribute(OSIS_ID) != null) {
                    item.Attribute(OSIS_ID).Value = $"{book}.{chTo}.{vn}!note.{item.Attribute("n").Value}";
                }
                if (item.Attribute(START_ID) != null) { item.Attribute(START_ID).Value = $"{book}.{chTo}.{vn}"; }
                if (item.Attribute(END_ID) != null) { item.Attribute(END_ID).Value = $"{book}.{chTo}.{vn}"; }
                if (item.Name.LocalName == NOTE && item.Attribute(REF_ID) != null) { item.Attribute(REF_ID).Value = $"{book}.{chTo}.{vn}"; }
            }

            var chapters = e.Descendants().Where(x => x.Name.LocalName == CHAPTER && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"{book}.{chFrom}");
            foreach (var chapter in chapters) {
                chapter.Attribute(OSIS_ID).Value = $"{book}.{chTo}";
                if (chapter.Attribute(START_ID) != null) { chapter.Attribute(START_ID).Value = $"{book}.{chTo}"; }
                if (chapter.Attribute(END_ID) != null) { chapter.Attribute(END_ID).Value = $"{book}.{chTo}"; }
            }
        }
        void MoveUpOne(XElement e, string book, int ch) {
            var firstVerseEnd = e.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(END_ID) != null && x.Attribute(END_ID).Value == $"{book}.{ch}.1").FirstOrDefault();
            var objects = new List<object>();
            var secondVerseStart = e.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(START_ID) != null && x.Attribute(START_ID).Value == $"{book}.{ch}.2").FirstOrDefault();
            if (secondVerseStart != null) {
                while (secondVerseStart.NextNode != null) {
                    if (secondVerseStart.NextNode is XElement && (secondVerseStart.NextNode as XElement).Name.LocalName == VERSE) { break; }
                    if (secondVerseStart.NextNode is XElement && (secondVerseStart.NextNode as XElement).Name.LocalName == NOTE) {
                        (secondVerseStart.NextNode as XElement).Attribute(OSIS_ID).Value = (secondVerseStart.NextNode as XElement).Attribute(OSIS_ID).Value.Replace($"{book}.{ch}.2", $"{book}.{ch}.1");
                        (secondVerseStart.NextNode as XElement).Attribute(REF_ID).Value = (secondVerseStart.NextNode as XElement).Attribute(REF_ID).Value.Replace($"{book}.{ch}.2", $"{book}.{ch}.1");
                    }
                    objects.Add(secondVerseStart.NextNode);
                    secondVerseStart.NextNode.Remove();
                }
                e.Descendants().Where(x => x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"{book}.{ch}.2").Remove();

                firstVerseEnd.AddBeforeSelf(" ");
                firstVerseEnd.AddBeforeSelf(objects);

                var vn = 2;
                for (int i = 3; i < 200; i++) {
                    var items = e.Descendants().Where(x => (x.Name.LocalName == VERSE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value == $"{book}.{ch}.{i}") || (x.Name.LocalName == NOTE && x.Attribute(OSIS_ID) != null && x.Attribute(OSIS_ID).Value.StartsWith($"{book}.{ch}.{i}!")));
                    if (items.Count() == 0) { break; }
                    foreach (var item in items) {
                        if (item.Attribute(OSIS_ID) != null) {
                            if (item.Attribute(OLD_ID) == null) { item.Add(new XAttribute(OLD_ID, item.Attribute(OSIS_ID).Value)); }
                        }
                        if (item.Name.LocalName == VERSE && item.Attribute(OSIS_ID) != null) {
                            item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}";
                        }
                        else if (item.Name.LocalName == NOTE && item.Attribute(OSIS_ID) != null) {
                            item.Attribute(OSIS_ID).Value = $"{book}.{ch}.{vn}!note.{item.Attribute("n").Value}";
                        }
                        if (item.Attribute(START_ID) != null) { item.Attribute(START_ID).Value = $"{book}.{ch}.{vn}"; }
                        if (item.Attribute(END_ID) != null) { item.Attribute(END_ID).Value = $"{book}.{ch}.{vn}"; }
                        if (item.Name.LocalName == NOTE && item.Attribute(REF_ID) != null) { item.Attribute(REF_ID).Value = $"{book}.{ch}.{vn}"; }

                    }
                    vn++;
                }
            }
        }

        public static string[] BookNames = { "Gen", "Exod" , "Lev" , "Num", "Deut", "Josh",
                                      "Judg", "Ruth", "1Sam", "2Sam", "1Kgs", "2Kgs",
                                      "1Chr" , "2Chr", "Ezra", "Neh", "Esth", "Job",
                                      "Ps", "Prov", "Eccl", "Song", "Isa", "Jer",
                                      "Lam", "Ezek", "Dan", "Hos", "Joel", "Amos",
                                      "Obad", "Jonah", "Mic", "Nah", "Hab", "Zeph",
                                      "Hag", "Zech", "Mal", "Matt", "Mark", "Luke",
                                      "John", "Acts", "Rom", "1Cor", "2Col", "Gal",
                                      "Eph", "Phil", "Col", "1Thess", "2Thess",
                                      "1Tim", "2Tim", "Heb", "Jas", "1Pet", "2Pet",
                                      "1John", "2John", "3John", "Jude", "Rev"};

        public static int GetBookNumberFromOsisAbbreviation(string nameOfBook) {
            if (nameOfBook.Contains("Gen")) { return 10; }
            if (nameOfBook.Contains("Exod")) { return 20; }
            if (nameOfBook.Contains("Lev")) { return 30; }
            if (nameOfBook.Contains("Num")) { return 40; }
            if (nameOfBook.Contains("Deut")) { return 50; }

            if (nameOfBook.Contains("Josh")) { return 60; }
            if (nameOfBook.Contains("Judg")) { return 70; }
            if (nameOfBook.Contains("Ruth")) { return 80; }

            if (nameOfBook.Contains("1Sam")) { return 90; }
            if (nameOfBook.Contains("2Sam")) { return 100; }
            if (nameOfBook.Contains("1Kgs")) { return 110; }
            if (nameOfBook.Contains("2Kgs")) { return 120; }
            if (nameOfBook.Contains("1Chr")) { return 130; }
            if (nameOfBook.Contains("2Chr")) { return 140; }


            if (nameOfBook.Contains("Ezra")) { return 150; }
            if (nameOfBook.Contains("Neh")) { return 160; }
            if (nameOfBook.Contains("Esth")) { return 190; }

            if (nameOfBook.Contains("Job")) { return 220; }
            if (nameOfBook.Contains("Ps")) { return 230; }
            if (nameOfBook.Contains("Prov")) { return 240; }
            if (nameOfBook.Contains("Eccl")) { return 250; }
            if (nameOfBook.Contains("Song")) { return 260; }

            if (nameOfBook.Contains("Isa")) { return 290; }
            if (nameOfBook.Contains("Jer")) { return 300; }
            if (nameOfBook.Contains("Lam")) { return 310; }
            if (nameOfBook.Contains("Ezek")) { return 330; }
            if (nameOfBook.Contains("Dan")) { return 340; }
            if (nameOfBook.Contains("Hos")) { return 350; }
            if (nameOfBook.Contains("Joel")) { return 360; }
            if (nameOfBook.Contains("Amos")) { return 370; }
            if (nameOfBook.Contains("Obad")) { return 380; }
            if (nameOfBook.Contains("Jonah")) { return 390; }
            if (nameOfBook.Contains("Mic")) { return 400; }
            if (nameOfBook.Contains("Nah")) { return 410; }
            if (nameOfBook.Contains("Hab")) { return 420; }
            if (nameOfBook.Contains("Zeph")) { return 430; }
            if (nameOfBook.Contains("Hag")) { return 440; }
            if (nameOfBook.Contains("Zech")) { return 450; }
            if (nameOfBook.Contains("Mal")) { return 460; }

            if (nameOfBook.Contains("Matt")) { return 470; }
            if (nameOfBook.Contains("Mark")) { return 480; }
            if (nameOfBook.Contains("Luke")) { return 490; }
            if (nameOfBook.Contains("1John")) { return 690; }
            if (nameOfBook.Contains("2John")) { return 700; }
            if (nameOfBook.Contains("3John")) { return 710; }
            if (nameOfBook.Contains("John")) { return 500; }
            if (nameOfBook.Contains("Acts")) { return 510; }
            if (nameOfBook.Contains("Rom")) { return 520; }
            if (nameOfBook.Contains("1Cor")) { return 530; }
            if (nameOfBook.Contains("2Cor")) { return 540; }
            if (nameOfBook.Contains("Gal")) { return 550; }
            if (nameOfBook.Contains("Eph")) { return 560; }
            if (nameOfBook.Contains("Phil")) { return 570; }
            if (nameOfBook.Contains("Col")) { return 580; }
            if (nameOfBook.Contains("1Thess")) { return 590; }
            if (nameOfBook.Contains("2Thess")) { return 600; }
            if (nameOfBook.Contains("1Tim")) { return 610; }
            if (nameOfBook.Contains("2Tim")) { return 620; }
            if (nameOfBook.Contains("Titus")) { return 630; }
            if (nameOfBook.Contains("Phlm")) { return 640; }
            if (nameOfBook.Contains("Heb")) { return 650; }
            if (nameOfBook.Contains("Jas")) { return 660; }
            if (nameOfBook.Contains("1Pet")) { return 670; }
            if (nameOfBook.Contains("2Pet")) { return 680; }
            //if (nameOfBook.Contains("1John")) { return 690; }
            //if (nameOfBook.Contains("2John")) { return 700; }
            //if (nameOfBook.Contains("3John")) { return 710; }
            if (nameOfBook.Contains("Jude")) { return 720; }
            if (nameOfBook.Contains("Rev")) { return 730; }

            return default;
        }

        public static string GetShortcutFromOsisAbbreviation(string nameOfBook) {
            if (nameOfBook.Contains("Gen")) { return "Ge"; }
            if (nameOfBook.Contains("Exod")) { return "Ex"; }
            if (nameOfBook.Contains("Lev")) { return "Le"; }
            if (nameOfBook.Contains("Num")) { return "Nu"; }
            if (nameOfBook.Contains("Deut")) { return "Dt"; }

            if (nameOfBook.Contains("Josh")) { return "Jos"; }
            if (nameOfBook.Contains("Judg")) { return "Jdg"; }
            if (nameOfBook.Contains("Ruth")) { return "Ru"; }

            if (nameOfBook.Contains("1Sam")) { return "1Sa"; }
            if (nameOfBook.Contains("2Sam")) { return "2Sa"; }
            if (nameOfBook.Contains("1Kgs")) { return "1Ki"; }
            if (nameOfBook.Contains("2Kgs")) { return "2Ki"; }
            if (nameOfBook.Contains("1Chr")) { return "1Ch"; }
            if (nameOfBook.Contains("2Chr")) { return "2Ch"; }


            if (nameOfBook.Contains("Ezra")) { return "Ezr"; }
            if (nameOfBook.Contains("Neh")) { return "Ne"; }
            if (nameOfBook.Contains("Esth")) { return "Es"; }

            if (nameOfBook.Contains("Job")) { return "Job"; }
            if (nameOfBook.Contains("Ps")) { return "Ps"; }
            if (nameOfBook.Contains("Prov")) { return "Pr"; }
            if (nameOfBook.Contains("Eccl")) { return "Ec"; }
            if (nameOfBook.Contains("Song")) { return "So"; }

            if (nameOfBook.Contains("Isa")) { return "Is"; }
            if (nameOfBook.Contains("Jer")) { return "Je"; }
            if (nameOfBook.Contains("Lam")) { return "La"; }
            if (nameOfBook.Contains("Ezek")) { return "Eze"; }
            if (nameOfBook.Contains("Dan")) { return "Da"; }
            if (nameOfBook.Contains("Hos")) { return "Ho"; }
            if (nameOfBook.Contains("Joel")) { return "Joe"; }
            if (nameOfBook.Contains("Amos")) { return "Am"; }
            if (nameOfBook.Contains("Obad")) { return "Ob"; }
            if (nameOfBook.Contains("Jonah")) { return "Jon"; }
            if (nameOfBook.Contains("Mic")) { return "Mic"; }
            if (nameOfBook.Contains("Nah")) { return "Na"; }
            if (nameOfBook.Contains("Hab")) { return "Hab"; }
            if (nameOfBook.Contains("Zeph")) { return "Zep"; }
            if (nameOfBook.Contains("Hag")) { return "Hag"; }
            if (nameOfBook.Contains("Zech")) { return "Zec"; }
            if (nameOfBook.Contains("Mal")) { return "Mal"; }

            if (nameOfBook.Contains("Matt")) { return "Mt"; }
            if (nameOfBook.Contains("Mark")) { return "Mk"; }
            if (nameOfBook.Contains("Luke")) { return "Lk"; }
            if (nameOfBook.Contains("1John")) { return "1Jn"; }
            if (nameOfBook.Contains("2John")) { return "2Jn"; }
            if (nameOfBook.Contains("3John")) { return "3Jn"; }
            if (nameOfBook.Contains("John")) { return "Jn"; }
            if (nameOfBook.Contains("Acts")) { return "Ac"; }
            if (nameOfBook.Contains("Rom")) { return "Ro"; }
            if (nameOfBook.Contains("1Cor")) { return "1Co"; }
            if (nameOfBook.Contains("2Cor")) { return "2Co"; }
            if (nameOfBook.Contains("Gal")) { return "Ga"; }
            if (nameOfBook.Contains("Eph")) { return "Eph"; }
            if (nameOfBook.Contains("Phil")) { return "Php"; }
            if (nameOfBook.Contains("Col")) { return "Col"; }
            if (nameOfBook.Contains("1Thess")) { return "1Th"; }
            if (nameOfBook.Contains("2Thess")) { return "2Th"; }
            if (nameOfBook.Contains("1Tim")) { return "1Ti"; }
            if (nameOfBook.Contains("2Tim")) { return "2Ti"; }
            if (nameOfBook.Contains("Titus")) { return "Tt"; }
            if (nameOfBook.Contains("Phlm")) { return "Phm"; }
            if (nameOfBook.Contains("Heb")) { return "Heb"; }
            if (nameOfBook.Contains("Jas")) { return "Jas"; }
            if (nameOfBook.Contains("1Pet")) { return "1Pe"; }
            if (nameOfBook.Contains("2Pet")) { return "2Pe"; }
            //if (nameOfBook.Contains("1John")) { return "1Jn"; }
            //if (nameOfBook.Contains("2John")) { return "2Jn"; }
            //if (nameOfBook.Contains("3John")) { return "3Jn"; }
            if (nameOfBook.Contains("Jude")) { return "Jud"; }
            if (nameOfBook.Contains("Rev")) { return "Re"; }

            return default;
        }

        public static string GetBookName(string nameOfBook) {
            if (nameOfBook.Contains("Gen")) { return "Księga Rodzaju"; }
            if (nameOfBook.Contains("Exod")) { return "Księga Wyjścia"; }
            if (nameOfBook.Contains("Lev")) { return "Księga Kapłańska"; }
            if (nameOfBook.Contains("Num")) { return "Księga Liczb"; }
            if (nameOfBook.Contains("Deut")) { return "Księga Powtórzonego Prawa"; }

            if (nameOfBook.Contains("Josh")) { return "Księga Jozuego"; }
            if (nameOfBook.Contains("Judg")) { return "Księga Sędziów"; }
            if (nameOfBook.Contains("Ruth")) { return "Księga Rut"; }

            if (nameOfBook.Contains("1Sam")) { return "Pierwsza Księga Samuela"; }
            if (nameOfBook.Contains("2Sam")) { return "Druga Księga Samuela"; }
            if (nameOfBook.Contains("1Kgs")) { return "Pierwsza Księga Królewska"; }
            if (nameOfBook.Contains("2Kgs")) { return "Druga Księga Królewska"; }
            if (nameOfBook.Contains("1Chr")) { return "Pierwsza Księga Kronik"; }
            if (nameOfBook.Contains("2Chr")) { return "Druga Księga Kronik"; }


            if (nameOfBook.Contains("Ezra")) { return "Księga Ezdrasza"; }
            if (nameOfBook.Contains("Neh")) { return "Księga Nehemiasza"; }
            if (nameOfBook.Contains("Esth")) { return "Księga Estery"; }

            if (nameOfBook.Contains("Job")) { return "Księga Hioba"; }
            if (nameOfBook.Contains("Ps")) { return "Psalmy"; }
            if (nameOfBook.Contains("Prov")) { return "Przypowieści Salomona"; }
            if (nameOfBook.Contains("Eccl")) { return "Księga Kaznodziei"; }
            if (nameOfBook.Contains("Song")) { return "Pieśń nad pieśniami"; }

            if (nameOfBook.Contains("Isa")) { return "Księga Izajasza"; }
            if (nameOfBook.Contains("Jer")) { return "Księga Jeremiasza"; }
            if (nameOfBook.Contains("Lam")) { return "Treny"; }
            if (nameOfBook.Contains("Ezek")) { return "Księga Ezechiela"; }
            if (nameOfBook.Contains("Dan")) { return "Księga Daniela"; }
            if (nameOfBook.Contains("Hos")) { return "Księga Ozeasza"; }
            if (nameOfBook.Contains("Joel")) { return "Księga Joela"; }
            if (nameOfBook.Contains("Amos")) { return "Księga Amosa"; }
            if (nameOfBook.Contains("Obad")) { return "Księga Abdiasza"; }
            if (nameOfBook.Contains("Jonah")) { return "Księga Jonasza"; }
            if (nameOfBook.Contains("Mic")) { return "Księga Micheasza"; }
            if (nameOfBook.Contains("Nah")) { return "Księga Nahuma"; }
            if (nameOfBook.Contains("Hab")) { return "Księga Habakuka"; }
            if (nameOfBook.Contains("Zeph")) { return "Księga Sofoniasza"; }
            if (nameOfBook.Contains("Hag")) { return "Księga Aggeusza"; }
            if (nameOfBook.Contains("Zech")) { return "Księga Zachariasza"; }
            if (nameOfBook.Contains("Mal")) { return "Księga Malachiasza"; }

            if (nameOfBook.Contains("Matt")) { return "Ewangelia według św. Mateusza"; }
            if (nameOfBook.Contains("Mark")) { return "Ewangelia według św. Marka"; }
            if (nameOfBook.Contains("Luke")) { return "Ewangelia według św. Łukasza"; }
            if (nameOfBook.Contains("1John")) { return "Pierwszy List św. Jana"; }
            if (nameOfBook.Contains("2John")) { return "Drugi List św. Jana"; }
            if (nameOfBook.Contains("3John")) { return "Trzeci List św. Jana"; }
            if (nameOfBook.Contains("John")) { return "Ewangelia według św. Jana"; }
            if (nameOfBook.Contains("Acts")) { return "Dzieje Apostolskie"; }
            if (nameOfBook.Contains("Rom")) { return "List św. Pawła do Rzymian"; }
            if (nameOfBook.Contains("1Cor")) { return "Pierwszy List św. Pawła do Koryntian"; }
            if (nameOfBook.Contains("2Cor")) { return "Drugi List św. Pawła do Koryntian"; }
            if (nameOfBook.Contains("Gal")) { return "List św. Pawła do Galacjan"; }
            if (nameOfBook.Contains("Eph")) { return "List św. Pawła do Efezjan"; }
            if (nameOfBook.Contains("Phil")) { return "List św. Pawła do Filipian"; }
            if (nameOfBook.Contains("Col")) { return "List św. Pawła do Kolosan"; }
            if (nameOfBook.Contains("1Thess")) { return "Pierwszy List św. Pawła do Tesaloniczan"; }
            if (nameOfBook.Contains("2Thess")) { return "Drugi List św. Pawła do Tesaloniczan"; }
            if (nameOfBook.Contains("1Tim")) { return "Pierwszy List św. Pawła do Tymoteusza"; }
            if (nameOfBook.Contains("2Tim")) { return "Drugi List św. Pawła do Tymoteusza"; }
            if (nameOfBook.Contains("Titus")) { return "List św. Pawła do Tytusa"; }
            if (nameOfBook.Contains("Phlm")) { return "List św. Pawła do Filemona"; }
            if (nameOfBook.Contains("Heb")) { return "List do Hebrajczyków"; }
            if (nameOfBook.Contains("Jas")) { return "List Jakuba"; }
            if (nameOfBook.Contains("1Pet")) { return "Pierwszy List św. Piotra"; }
            if (nameOfBook.Contains("2Pet")) { return "Drugi List św. Piotra"; }
            //if (nameOfBook.Contains("1John")) { return "Pierwszy List św. Jana"; }
            //if (nameOfBook.Contains("2John")) { return "Drugi List św. Jana"; }
            //if (nameOfBook.Contains("3John")) { return "Trzeci List św. Jana"; }
            if (nameOfBook.Contains("Jude")) { return "List Judy"; }
            if (nameOfBook.Contains("Rev")) { return "Objawienie św. Jana"; }

            return default;
        }

        public static string GetBookColor(string nameOfBook) {
            if (nameOfBook.Contains("Gen")) { return "#ccccff"; }
            if (nameOfBook.Contains("Exod")) { return "#ccccff"; }
            if (nameOfBook.Contains("Lev")) { return "#ccccff"; }
            if (nameOfBook.Contains("Num")) { return "#ccccff"; }
            if (nameOfBook.Contains("Deut")) { return "#ccccff"; }

            if (nameOfBook.Contains("Josh")) { return "#ffcc99"; }
            if (nameOfBook.Contains("Judg")) { return "#ffcc99"; }
            if (nameOfBook.Contains("Ruth")) { return "#ffcc99"; }

            if (nameOfBook.Contains("1Sam")) { return "#ffcc99"; }
            if (nameOfBook.Contains("2Sam")) { return "#ffcc99"; }
            if (nameOfBook.Contains("1Kgs")) { return "#ffcc99"; }
            if (nameOfBook.Contains("2Kgs")) { return "#ffcc99"; }
            if (nameOfBook.Contains("1Chr")) { return "#ffcc99"; }
            if (nameOfBook.Contains("2Chr")) { return "#ffcc99"; }


            if (nameOfBook.Contains("Ezra")) { return "#ffcc99"; }
            if (nameOfBook.Contains("Neh")) { return "#ffcc99"; }
            if (nameOfBook.Contains("Esth")) { return "#ffcc99"; }

            if (nameOfBook.Contains("Job")) { return "#66ff99"; }
            if (nameOfBook.Contains("Ps")) { return "#66ff99"; }
            if (nameOfBook.Contains("Prov")) { return "#66ff99"; }
            if (nameOfBook.Contains("Eccl")) { return "#66ff99"; }
            if (nameOfBook.Contains("Song")) { return "#66ff99"; }

            if (nameOfBook.Contains("Isa")) { return "#ff9fb4"; }
            if (nameOfBook.Contains("Jer")) { return "#ff9fb4"; }
            if (nameOfBook.Contains("Lam")) { return "#ff9fb4"; }
            if (nameOfBook.Contains("Ezek")) { return "#ff9fb4"; }
            if (nameOfBook.Contains("Dan")) { return "#ff9fb4"; }
            if (nameOfBook.Contains("Hos")) { return "#ffff99"; }
            if (nameOfBook.Contains("Joel")) { return "#ffff99"; }
            if (nameOfBook.Contains("Amos")) { return "#ffff99"; }
            if (nameOfBook.Contains("Obad")) { return "#ffff99"; }
            if (nameOfBook.Contains("Jonah")) { return "#ffff99"; }
            if (nameOfBook.Contains("Mic")) { return "#ffff99"; }
            if (nameOfBook.Contains("Nah")) { return "#ffff99"; }
            if (nameOfBook.Contains("Hab")) { return "#ffff99"; }
            if (nameOfBook.Contains("Zeph")) { return "#ffff99"; }
            if (nameOfBook.Contains("Hag")) { return "#ffff99"; }
            if (nameOfBook.Contains("Zech")) { return "#ffff99"; }
            if (nameOfBook.Contains("Mal")) { return "#ffff99"; }

            if (nameOfBook.Contains("Matt")) { return "#ff6600"; }
            if (nameOfBook.Contains("Mark")) { return "#ff6600"; }
            if (nameOfBook.Contains("Luke")) { return "#ff6600"; }
            if (nameOfBook.Contains("1John")) { return "#00ff00"; }
            if (nameOfBook.Contains("2John")) { return "#00ff00"; }
            if (nameOfBook.Contains("3John")) { return "#00ff00"; }
            if (nameOfBook.Contains("John")) { return "#ff6600"; }

            if (nameOfBook.Contains("Acts")) { return "#00ffff"; }

            if (nameOfBook.Contains("Rom")) { return "#ffff00"; }
            if (nameOfBook.Contains("1Cor")) { return "#ffff00"; }
            if (nameOfBook.Contains("2Cor")) { return "#ffff00"; }
            if (nameOfBook.Contains("Gal")) { return "#ffff00"; }
            if (nameOfBook.Contains("Eph")) { return "#ffff00"; }
            if (nameOfBook.Contains("Phil")) { return "#ffff00"; }
            if (nameOfBook.Contains("Col")) { return "#ffff00"; }
            if (nameOfBook.Contains("1Thess")) { return "#ffff00"; }
            if (nameOfBook.Contains("2Thess")) { return "#ffff00"; }
            if (nameOfBook.Contains("1Tim")) { return "#ffff00"; }
            if (nameOfBook.Contains("2Tim")) { return "#ffff00"; }
            if (nameOfBook.Contains("Titus")) { return "#ffff00"; }
            if (nameOfBook.Contains("Phlm")) { return "#ffff00"; }
            if (nameOfBook.Contains("Heb")) { return "#ffff00"; }
            if (nameOfBook.Contains("Jas")) { return "#00ff00"; }
            if (nameOfBook.Contains("1Pet")) { return "#00ff00"; }
            if (nameOfBook.Contains("2Pet")) { return "#00ff00"; }
            //if (nameOfBook.Contains("1John")) { return "#00ff00"; }
            //if (nameOfBook.Contains("2John")) { return "#00ff00"; }
            //if (nameOfBook.Contains("3John")) { return "#00ff00"; }
            if (nameOfBook.Contains("Jude")) { return "#00ff00"; }
            if (nameOfBook.Contains("Rev")) { return "#ff7c80"; }

            return default;
        }

        public static string ReplaceBookNames(string text) {
            var pattern = "(?<book>";
            foreach (var item in BookNames) {
                pattern += @$"({item})?";
            }
            pattern += @")\.";

            var result = Regex.Replace(text, pattern, delegate (Match m) {
                return GetShortcutFromOsisAbbreviation(m.Groups["book"].Value) + ".";
            });
            return result;
        }

        public void Dispose() {

        }
    }
}
