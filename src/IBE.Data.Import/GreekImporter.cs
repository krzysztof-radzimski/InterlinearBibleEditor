using DevExpress.Xpo;
using IBE.Data.Model;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace IBE.Data.Import {
    public class GreekImporter : BaseImporter {
        public override void Import(string filePath, UnitOfWork uow) {
            if (filePath != null && File.Exists(filePath)) {
                var pattern = "index_split_{0}.html";

                var bookNumber = 41;
                Book book;

                for (int i = 2; i < 32; i++) {
                    var fileName = string.Format(pattern, i.ToString().PadLeft(3, '0'));
                    if (FileExists(filePath, fileName)) {
                        XElement html = XElement.Load(new MemoryStream(GetFileData(filePath, fileName)));
                        if (html != null && html.HasElements) {
                            var body = html.Elements().Where(x => x.Name.LocalName == "body").FirstOrDefault();
                            if (body != null && body.HasElements) {

                                Chapter chapter;
                                Verse verse;


                                foreach (var item in body.Elements()) {
                                    if (item.HasAttributes && item.Attribute("id") != null && item.Attribute("id").Value.Contains("filepos") &&
                                        item.Attribute("class") != null && item.Attribute("class").Value == "calibre_6") {

                                        var gr = item.Elements().Where(x => x.Attribute("class") != null && x.Attribute("class").Value == "calibre5").FirstOrDefault();
                                        var pl = item.Elements().Where(x => x.Attribute("class") != null && x.Attribute("class").Value == "calibre5").LastOrDefault();

                                        book = new Book(uow) {
                                            NumberOfBook = bookNumber,
                                            GreekName = gr != null ? gr.Value.Trim() : String.Empty,
                                            BookName = pl != null ? pl.Value.Trim() : String.Empty
                                        };

                                        book.Save();

                                        bookNumber++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
