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
                        while (verseItem.NextNode!=null) {
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

                xml.Save(result);
                return result;
            }
            return filePath;
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
            if (nameOfBook.Contains("Nah")) { return "Księga Hahuma"; }
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
