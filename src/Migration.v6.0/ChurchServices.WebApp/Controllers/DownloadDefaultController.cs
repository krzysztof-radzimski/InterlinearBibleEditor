/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2025 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using ChurchServices.WebApp.Filters;
using HtmlAgilityPack;
using System.IO;
using System.Xml.Serialization;

namespace ChurchServices.WebApp.Controllers {
    [BlockGoogleBots]
    public abstract class DownloadDefaultController : Controller {
        private string DownloadFileName = null;
        protected readonly IConfiguration Configuration;
        protected readonly IWebHostEnvironment WebHostEnvironment;
        protected abstract ExportSaveFormat Format { get; }
        public DownloadDefaultController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment) {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Get() {
            var qs = Request.QueryString;

            if (qs.IsNotNull() && qs.Value.IsNotNullOrEmpty() && qs.Value.Length >= 5) {
                DownloadFileName = Format.GetCategory();
                var queryString = HttpUtility.UrlDecode(qs.Value).RemoveAny("?q=");
                Stream stream;
                try {
                    stream = await CreateStream(queryString);
                }
                catch (AuthException) {
                    return new RedirectResult("/Account/Index?ReturnUrl=" + Request.Path + HttpUtility.UrlDecode(Request.QueryString.Value));
                }
                if (stream.IsNull()) { return NotFound(); }

                return File(stream, Format.GetDescription(), DownloadFileName);
            }

            return NotFound();
        }

        private async Task<Stream> CreateStream(string queryString) {
            Book book;
            var paramsTable = queryString.Split(',');
            if (paramsTable.Length == 3) {
                Chapter chapter;

                var translationName = paramsTable[0];
                var bookNumber = paramsTable[1].ToInt();
                var chapterNumber = paramsTable[2].ToInt();

                var uow = new UnitOfWork();

                var trans = new XPQuery<Translation>(uow).Where(x => !x.Hidden && x.Name == translationName).FirstOrDefault();
                if (trans.IsNull() && translationName.EndsWith(" ")) {
                    translationName = translationName.Trim() + "+";
                    trans = new XPQuery<Translation>(uow).Where(x => x.Name == translationName).FirstOrDefault();
                }
                if (trans.IsNull()) { return default; }

                if (!trans.OpenAccess && !User.Identity.IsAuthenticated) {
                    throw new AuthException();
                }

                book = trans.Books.Where(x => x.NumberOfBook == bookNumber).FirstOrDefault();
                chapter = book.Chapters.Where(x => x.NumberOfChapter == chapterNumber).FirstOrDefault();

                byte[] licData = await GetLicData();
                var host = (this.Request.IsHttps ? "https://" : "http://") + this.Request.Host;
                var result = new DefaultExporter(licData, host).Export(chapter, Format);

                return new MemoryStream(result);
            }
            else if (paramsTable.Length == 2) {
                var translationName = paramsTable[0];
                var bookNumber = paramsTable[1].ToInt();

                var uow = new UnitOfWork();

                var trans = new XPQuery<Translation>(uow).Where(x => !x.Hidden && x.Name == translationName).FirstOrDefault();
                if (trans.IsNull() && translationName.EndsWith(" ")) {
                    translationName = translationName.Trim() + "+";
                    trans = new XPQuery<Translation>(uow).Where(x => x.Name == translationName).FirstOrDefault();
                }
                if (trans.IsNull()) { return default; }
                if (!trans.OpenAccess && !User.Identity.IsAuthenticated) {
                    throw new AuthException();
                }

                book = trans.Books.Where(x => x.NumberOfBook == bookNumber).FirstOrDefault();

                byte[] licData = await GetLicData();
                var host = (this.Request.IsHttps ? "https://" : "http://") + this.Request.Host;
                var result = new DefaultExporter(licData, host).Export(book, Format);

                return new MemoryStream(result);
            }
            else if (queryString.IsNotNullOrWhiteSpace() && paramsTable.Length == 1) {
                var translationName = queryString;

                var fileName = $"{translationName.Replace("+", "").Trim()}.{Format.ToString().ToLower()}";
                DownloadFileName = fileName;
                var filePath = Path.Combine(WebHostEnvironment.WebRootPath, $"download\\bible\\{fileName}");
                if (System.IO.File.Exists(filePath)) {
                    var filePathData = await System.IO.File.ReadAllBytesAsync(filePath);
                    return new MemoryStream(filePathData);
                }

                var uow = new UnitOfWork();

                var trans = new XPQuery<Translation>(uow).Where(x => !x.Hidden && x.Name == translationName).FirstOrDefault();
                if (trans.IsNull() && translationName.EndsWith(" ")) {
                    translationName = translationName.Trim() + "+";
                    trans = new XPQuery<Translation>(uow).Where(x => x.Name == translationName).FirstOrDefault();
                }
                if (trans.IsNull()) { return default; }
                if (!trans.OpenAccess && !User.Identity.IsAuthenticated) {
                    throw new AuthException();
                }

                byte[] licData = await GetLicData();
                var host = (this.Request.IsHttps ? "https://" : "http://") + this.Request.Host;
                var result = new DefaultExporter(licData, host).Export(trans, Format);

                if (!System.IO.File.Exists(filePath) && result != null) {
                    await System.IO.File.WriteAllBytesAsync(filePath, result);
                }

                return new MemoryStream(result);
            }


            return default;
        }

        private async Task<byte[]> GetLicData() {
            var licPath = Configuration["AsposeLic"];
            var licInfo = new System.IO.FileInfo(licPath);

            if (licInfo.Exists) {
                return await System.IO.File.ReadAllBytesAsync(licPath);
            }
            return default;
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class DownloadDefaultDocxController : DownloadDefaultController {
        protected override ExportSaveFormat Format => ExportSaveFormat.Docx;
        public DownloadDefaultDocxController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment) : base(configuration, webHostEnvironment) { }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class DownloadDefaultPdfController : DownloadDefaultController {
        protected override ExportSaveFormat Format => ExportSaveFormat.Pdf;
        public DownloadDefaultPdfController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment) : base(configuration, webHostEnvironment) { }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class DownloadZefaniaXmlController : Controller {
        private string DownloadFileName = null;
        protected readonly IConfiguration Configuration;
        protected readonly IWebHostEnvironment WebHostEnvironment;
        protected readonly IBibleTagController BibleTag;

        public DownloadZefaniaXmlController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment, IBibleTagController bibleTag) {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
            BibleTag = bibleTag;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string translationName) {
            if (translationName != null) {
                DownloadFileName = $"{translationName}.xml";
                var filePath = Path.Combine(WebHostEnvironment.WebRootPath, $"download\\bible\\{DownloadFileName}");
                if (System.IO.File.Exists(filePath)) {
                    var filePathData = await System.IO.File.ReadAllBytesAsync(filePath);
                    return File(new MemoryStream(filePathData), "application/xml", DownloadFileName);
                }


                var uow = new UnitOfWork();

                var trans = await new XPQuery<Translation>(uow).Where(x => !x.Hidden && x.Name == translationName).FirstOrDefaultAsync();
                if (trans.IsNull() && translationName.EndsWith(" ")) {
                    translationName = translationName.Trim() + "+";
                    trans = await new XPQuery<Translation>(uow).Where(x => x.Name == translationName).FirstOrDefaultAsync();
                }
                if (trans.IsNotNull()) {
                    var root = new XmlBible() {
                        Version = "1.0.0.0",
                        Status = "v",
                        BibleName = trans.Description,
                        Revision = 1,
                        Type = "x-bible",
                        BibleBooks = new List<XmlBibleBook>()
                    };
                    var detailedInfo = String.Empty;
                    if (trans.DetailedInfo.IsNotNullOrWhiteSpace()) {
                        var htmlDoc = new HtmlDocument();
                        htmlDoc.LoadHtml(trans.DetailedInfo);
                        // Pobranie tekstu bez znaczników
                        detailedInfo = htmlDoc.DocumentNode.InnerText;
                        // Usunięcie zbędnych białych znaków i wyświetlenie tekstu
                        detailedInfo = HtmlEntity.DeEntitize(detailedInfo).Trim();
                    }

                    var information = new XmlBibleInformation() {
                        Title = trans.Description,
                        Creator = String.Empty,
                        Subject = "Pismo Święte",
                        Description = detailedInfo,
                        Publisher = String.Empty,
                        Contributors = String.Empty,
                        Date = "2025-01-01",
                        Type = "Bible",
                        Format = "Zefania XML Bible Markup Language",
                        Identifier = translationName,
                        Source = String.Empty,
                        Language = "pl-PL",
                        Coverage = "provide the Bible to the nations of the world",
                        Rights = String.Empty,
                    };

                    root.Information = information;
                    var bookNr = 1;
                    foreach (var book in trans.Books.OrderBy(_ => _.NumberOfBook)) {
                        var bibleBook = new XmlBibleBook() {
                            BookNumber = bookNr,
                            BookName = book.BookName,
                            BookShortName = book.BookShortcut,
                            Chapters = new List<XmlBibleChapter>()
                        };

                        foreach (var chapter in book.Chapters.OrderBy(_ => _.NumberOfChapter)) {
                            var bibleChapter = new XmlBibleChapter() {
                                ChapterNumber = chapter.NumberOfChapter,
                                Verses = new List<XmlBibleVerse>()
                            };

                            foreach (var verse in chapter.Verses.OrderBy(_ => _.NumberOfVerse)) {
                                var bibleVerse = new XmlBibleVerse() {
                                    VerseNumber = verse.NumberOfVerse,
                                    Text = BibleTag.CleanVerseText(verse.Text, true)
                                                    .Replace("<br/>", " ")
                                                    .Replace("<br>", " ")
                                                    .Replace("   ", " ")
                                                    .Replace("  ", " ")
                                                    .Replace("  ", " ")
                                                    .Trim()
                                };

                                bibleChapter.Verses.Add(bibleVerse);
                            }

                            bibleBook.Chapters.Add(bibleChapter);
                        }

                        root.BibleBooks.Add(bibleBook);

                        bookNr++;
                    }


                    using (StreamWriter writer = new StreamWriter(filePath)) {
                        var serializer = new XmlSerializer(typeof(XmlBible));
                        serializer.Serialize(writer, root);
                    }

                    var filePathData = await System.IO.File.ReadAllBytesAsync(filePath);
                    return File(new MemoryStream(filePathData), "application/xml", DownloadFileName);
                }
            }
            return NotFound();
        }

        [XmlRoot("XMLBIBLE")]
        public class XmlBible {
            [XmlAttribute("version")]
            public string Version { get; set; }

            [XmlAttribute("status")]
            public string Status { get; set; }

            [XmlAttribute("biblename")]
            public string BibleName { get; set; }

            [XmlAttribute("revision")]
            public int Revision { get; set; }

            [XmlAttribute("type")]
            public string Type { get; set; }

            [XmlElement("INFORMATION")]
            public XmlBibleInformation Information { get; set; }

            [XmlElement("BIBLEBOOK")]
            public List<XmlBibleBook> BibleBooks { get; set; }
        }

        public class XmlBibleInformation {
            [XmlElement("title")]
            public string Title { get; set; }

            [XmlElement("creator")]
            public string Creator { get; set; }

            [XmlElement("subject")]
            public string Subject { get; set; }

            [XmlElement("description")]
            public string Description { get; set; }

            [XmlElement("publisher")]
            public string Publisher { get; set; }

            [XmlElement("contributors")]
            public string Contributors { get; set; }

            [XmlElement("date")]
            public string Date { get; set; }

            [XmlElement("type")]
            public string Type { get; set; }

            [XmlElement("format")]
            public string Format { get; set; }

            [XmlElement("identifier")]
            public string Identifier { get; set; }

            [XmlElement("source")]
            public string Source { get; set; }

            [XmlElement("language")]
            public string Language { get; set; }

            [XmlElement("coverage")]
            public string Coverage { get; set; }

            [XmlElement("rights")]
            public string Rights { get; set; }
        }

        public class XmlBibleBook {
            [XmlAttribute("bnumber")]
            public int BookNumber { get; set; }

            [XmlAttribute("bname")]
            public string BookName { get; set; }

            [XmlAttribute("bsname")]
            public string BookShortName { get; set; }

            [XmlElement("CHAPTER")]
            public List<XmlBibleChapter> Chapters { get; set; }
        }

        public class XmlBibleChapter {
            [XmlAttribute("cnumber")]
            public int ChapterNumber { get; set; }

            [XmlElement("VERS")]
            public List<XmlBibleVerse> Verses { get; set; }
        }

        public class XmlBibleVerse {
            [XmlAttribute("vnumber")]
            public int VerseNumber { get; set; }

            [XmlText]
            public string Text { get; set; }
        }

    }
}
