using ChurchServices.Data.Import.EIB.Model.Bible;
using HtmlAgilityPack;

namespace ChurchServices.Data.Import.BPK {
    public class BPKService {
        public BibleModel GetBibleModel(string folderPath) {
            if (Directory.Exists(folderPath)) {

               

                var bmodel = new BibleModel() {
                    Shortcut = "BPK",
                    Books = new List<BookModel>(),
                    Name = "Biblia Pierwszego Kościoła",
                    Type = TranslationType.Default,
                    DetailedInfo = GetDetailedInfo(),
                    Introduction = GetIntroduction()
                };


                BookModel book = null;

                foreach (var file in Directory.GetFiles(folderPath).OrderBy(_ => Path.GetFileName(_))) {
                    if (Path.GetExtension(file).Contains("xhtml")) {
                        HtmlDocument doc = new();
                        doc.Load(file);

                        var body = doc.DocumentNode.SelectSingleNode("//body");
                        if (body != null && body.HasClass("txt")) {
                            if (book == null && body.SelectNodes("//h2[@title and normalize-space(@title)!='']").Count == 0) {
                                // nie doszedłem jeszcze do pliku z księgą rodzaju
                                // pomijam ten plik
                                continue;
                            }

                            if (body.SelectNodes("//h2[@title and normalize-space(@title)!='']").Count > 0) {
                                // to powinna być księga
                                var h2 = body.SelectNodes("//h2[@title and normalize-space(@title)!='']").FirstOrDefault() ;
                                var bookName = h2.Attributes["title"].Value;
                                book = new BookModel() {
                                    BookName = bookName
                                };
                                
                            }
                        }
                    }
                }

                return bmodel;
            }
            return null;
        }

        private string GetBookShortcut(string bookName) {
            switch (bookName) {
                case "Księga Rodzaju": return "Rdz";
                case "Księga Wyjścia": return "Wj";
                case "Księga Kapłańska": return "Kpł";
                case "Księga Liczb": return "Lb";
                case "Księga Powtórzonego Prawa": return "Pwt";
                //case "Księga Rodzaju": return "Rdz";
                //case "Księga Rodzaju": return "Rdz";
                //case "Księga Rodzaju": return "Rdz";
                //case "Księga Rodzaju": return "Rdz";
                //case "Księga Rodzaju": return "Rdz";
                //case "Księga Rodzaju": return "Rdz";
                //case "Księga Rodzaju": return "Rdz";
                //case "Księga Rodzaju": return "Rdz";
                //case "Księga Rodzaju": return "Rdz";
            }
            return string.Empty;
        }

        private FormattedText GetIntroduction() {
            var info = new FormattedText();
            info.Items.Add(new Paragraph(@"PRZEDMOWA REDAKTORA NAUKOWEGO „PRYMASOWSKIEJ SERII BIBLIJNEJ”") { Style = "text-align: center; font-style: bold;" });
            info.Items.Add(new Paragraph(@"„Prymasowska Seria Biblijna” powiększa się o kolejny ważny tom. Składa się on z połączenia przekładu Septuaginty, czyli Biblii Greckiej Starego Testamentu, oraz przekładu Nowego Testamentu na język polski. Tego monumentalnego dzieła dokonał ks. prof. Remigiusz Popowski SDB i stanowi ono jego opus vitae. Przekład Starego Testamentu ukazał się w 2013 r. jako 37. tom „Prymasowskiej Serii Biblijnej” i obejmował również apokryfy, które w tym wydaniu zostały pominięte. Wznowiony w 2014 r. i zaopatrzony w onomastykon, jako 41. tom tejże serii, spotkał się z ogromnym zainteresowaniem, jest to bowiem pierwsze tłumaczenie całej Septuaginty na język polski. Przekład Nowego Testamentu ukazał się znacznie wcześniej, w Wielkim Jubileuszu Roku 2000 jako 15. tom „Prymasowskiej Serii Biblijnej”. Połączenie obydwu części Biblii, czyli Starego i Nowego Testamentu, jest nie tylko w pełni uzasadnione, lecz i bardzo potrzebne. Z jednej strony pozwala zapoznać się z całością translatorskiego dorobku ks. prof. R. Popowskiego SDB dotyczącego Biblii, a z drugiej ułatwia porównanie polskiego tłumaczenia jej wersji greckiej z tłumaczeniami na język polski dokonanymi z języków oryginalnych, czyli hebrajskiego, aramejskiego i greckiego."));
            info.Items.Add(new Paragraph(@"Jednak najważniejsza okoliczność uzasadniająca opracowanie i wydanie tego tomu została podana w jego tytule: Biblia pierwszego Kościoła. Właśnie Septuaginta, a nie Biblia Hebrajska, oraz jej kanon ksiąg świętych, znacznie bardziej pojemny niż zredukowany przez rabinów w konfrontacji z chrześcijaństwem kanon Biblii Hebrajskiej, stanowiła Biblię autorów Nowego Testamentu oraz Kościoła apostolskiego i pisarzy wczesnochrześcijańskich. Dominacja Septuaginty w „pierwszym Kościele” była niewątpliwa i powszechna. W Kościele Zachodnim trwała do czasu, gdy św. Hieronim dokonał przekładu Biblii na język łaciński, który przyjął się jako Wulgata, natomiast w Kościele Wschodnim trwa do dzisiaj. Septuaginta stworzyła grunt, na którym rodziła się i dynamicznie rozwijała ewangelizacja starożytnego świata, zintensyfikowana przez św. Pawła i wielu innych głosicieli Ewangelii. Ponieważ księgi święte biblijnego Izraela istniały w języku greckim, czytane i rozważane na przełomie ery przedchrześcijańskiej i chrześcijańskiej w żydowskich synagogach, zatem orędzie Jezusa Chrystusa i Ewangelia zostały zapisane właśnie w języku greckim. Autorzy czterech Ewangelii kanonicznych i pozostałych pism Nowego Testamentu chętnie i często odwoływali się do Septuaginty, cytując ją wprost i nawiązując do niej na wiele rozmaitych sposobów."));
            info.Items.Add(new Paragraph(@"Septuaginta nie jest jedynie przekładem hebrajskich ksiąg świętych, obejmuje bowiem księgi napisane lub zachowane w języku greckim. Co więcej, odzwierciedla postęp teologiczny, którego znaczenie polega na tym, że został usankcjonowany w Nowym Testamencie. Z tego względu Septuaginta była od początku traktowana w chrześcijaństwie jako praeparatio evangelica, czyli „przygotowanie do Ewangelii”. Ten fakt spowodował odrzucenie jej przez judaizm rabiniczny, co tym bardziej nasuwa pytanie o jej genezę, treść i oddziaływanie. Specjalistyczne studia nad Biblią polegają między innymi na wnikliwym porównywaniu tekstu hebrajskiego i aramejskiego z jego wersją grecką, rozpoznawaniu i analizowaniu zauważonych różnic oraz określaniu kierunków interpretacyjnych i aspektów teologicznych, które znalazły wyraz w Biblii Greckiej. Ten tom umożliwia swoiście równoległą refleksję, której sedno stanowi analogiczne porównywanie tekstów Biblii Hebrajskiej i Biblii Greckiej przetłumaczonych na język polski, z dodaniem polskiego tłumaczenia Nowego Testamentu. "));
            info.Items.Add(new Paragraph(@"Poprzednie tomy „Prymasowskiej Serii Biblijnej” opracowane przez ks. prof. R. Popowskiego SDB przyczyniły się wydatnie do ponownego dowartościowania Septuaginty, jej miejsca w Kościele oraz w wierze i teologii chrześcijańskiej, tym bardziej że obejmowały również Onomastykon Septuaginty (PSB 38, 41) Należy wyrazić nadzieję, że ten tom zaowocuje dalszymi badaniami nie tylko nad Septuagintą, lecz również nad początkami Kościoła oraz najwcześniejszymi uwarunkowaniami i okolicznościami głoszenia Ewangelii."));
            info.Items.Add(new Paragraph(@"Ks. prof. Waldemar Chrostowski") { Style = "text-align: right;" });
            info.Items.Add(new Paragraph(@"Dyrektor Instytutu Nauk Biblijnych") { Style = "text-align: right;" });
            info.Items.Add(new Paragraph(@"Uniwersytet Kardynała Stefana Wyszyńskiego w Warszawie") { Style = "text-align: right;" });
            info.Items.Add(new Paragraph(@"Warszawa, 29 czerwca 2016 roku") { Style = "text-align: right;" });
            return info;
        }

        private FormattedText GetDetailedInfo() {
            var info = new FormattedText();
            info.Items.Add(new Paragraph(@"OD WYDAWCY") { Style = "text-align: center; font-style: bold;" });
            info.Items.Add(new Paragraph(@"W trakcie prac redakcyjnych nad dziełem, które Państwo właśnie otrzymali, wiele osób zadawało mi pytanie: dlaczego zdecydowaliście się na publikację Pisma świętego w przekładzie z języka greckiego? Jeśli chodzi o Nowy Testament, to sprawa jest oczywista. W tym właśnie języku został on zapisany i jest to najlepsze źródło tekstu oryginalnego. Część badaczy snuje domysły, iż być może pierwsze manuskrypty Nowego Testamentu zostały spisane w języku aramejskim. Jednak kłopot, z jakim zderza się ta teoria, wiąże się z faktem, że nie znamy ani jednego rękopisu, który potwierdzałby tego typu założenia. Nieco inaczej przedstawia się sprawa Starego Testamentu, którego oryginał został spisany w znakomitej większości w języku hebrajskim. Czy w związku z tym, że język hebrajski był oryginalnym dla tekstu natchnionego Starego Testamentu, to współczesne tłumaczenia z tego języka są lepsze, wierniejsze lub bliższe w swym zrozumieniu przekazu czasom biblijnym niż ich starożytny przekład na język grecki, który został dokonany w okresie od III do I w. przed Chrystusem? Odpowiedź na to pytanie wcale nie jest prosta ani jednoznaczna. Badając to zagadnienie, należałoby przeanalizować dwie ważne kwestie."));
            info.Items.Add(new Paragraph(@"Po pierwsze Septuaginta (tak nazywany jest przekład Starego Testamentu na język grecki) zawiera zarówno tłumaczenia Biblii Hebrajskiej uznawanej przez Żydów za świętą, jak i zbiór ksiąg spisanych po grecku (lub przełożonych na grecki z zaginionych oryginałów hebrajskich), które katolicy i prawosławni uznają za święte. W przypadku ksiąg uznawanych przez Żydów za święte musimy wiedzieć, że Septuaginta stanowi przekład bardzo starożytnej ich wersji, tzn. sprzed powstania tzw. tekstu masoreckiego. Musimy też pamiętać, że wszystkie współczesne przekłady Starego Testamentu z języka hebrajskiego są dokonywane z tekstu masoreckiego, który jest późniejszy w stosunku do Septuaginty o ok. 1000 lat. Ten fakt otwiera zupełnie nowe horyzonty dla badań i egzegezy pism starotestamentalnych, szczególnie po odkryciach w Qumran. "));
            info.Items.Add(new Paragraph(@"Drugą kwestią, być może ważniejszą dla ludzi wierzących, jest znaczenie, jakie Septuaginta miała dla pierwszego Kościoła, który zaczął formować się po zesłaniu Ducha Świętego, tzn. wówczas gdy Dobra Nowina o zbawieniu w Chrystusie zaczęła się rozchodzić na cały świat. Kiedy nawracający się poganie (Grecy, Rzymianie, mieszkańcy Azji Mniejszej) chcieli zapoznać się z historią zbawienia, sięgali właśnie do greckiej Septuaginty (jako że nie znali języka hebrajskiego, który bardzo często nie był zrozumiały nawet dla rodowitych Hebrajczyków). Ta obecna w pierwotnym Kościele tendencja jest wyraźnie widoczna w spisanych w I w. Ewangeliach, w których znakomita większość cytatów ze Starego Testamentu pochodzi w swym brzmieniu właśnie z Septuaginty, a nie z tekstów hebrajskich."));
            info.Items.Add(new Paragraph(@"Szczególnym argumentem świadczącym o roli, jaką odgrywała Septuaginta w pierwszym Kościele jest fakt, że w obliczu narastającego odrzucenia wierzących w Jezusa Chrystusa przez Żydów, gdy chrześcijanie przyjęli tekst Septuaginty za swoją Biblię, wyznawcy judaizmu natychmiast ją odrzucili, chociaż do czasu powstania chrześcijaństwa uważali tę księgę za szanowany i powszechnie akceptowany przekład pism świętych. Kościół prawosławny do dziś uznaje Septuagintę za natchniony tekst Starego Testamentu i w ogóle nie tłumaczy tekstów biblijnych z języka hebrajskiego. Warto więc zapoznać się z księgą, która wywarła tak ogromny wpływ na wiarę i życie rodzącego się chrześcijaństwa i do dziś stanowi jedno z najważniejszych i najwcześniejszych świadectw relacjonujących historię zbawienia."));
            info.Items.Add(new Paragraph(@"Oficyna Wydawnicza VOCATIO, wprowadzając w życie przedśmiertną wolę ks. Remigiusza Popowskiego SDB, najlepszego polskiego tłumacza greki starożytnej oraz koine, który dzięki swej tytanicznej pracy samodzielnie dokonał przekładu na język polski zarówno Septuaginty, jak i Nowego Testamentu, zdecydowała się na przygotowanie specjalnej edycji Pisma św. opatrzonej tytułem: Biblia pierwszego Kościoła (BPK). Dla wygody czytelnika w publikacji tej została zachowana kolejność, struktura ksiąg i rozdziałów według klasycznego układu Biblii, jakim powszechnie posługują się chrześcijanie (w Septuagincie była ona nieco inna), jak również zastosowano powszechnie przyjęte w Polsce brzmienie imion i nazw własnych znanych z wielu opracowań słownikowych i leksykograficznych publikowanych przez VOCATIO w ramach „Prymasowskiej Serii Biblijnej”, a także m.in. z Biblii Tysiąclecia. W publikacji zrezygnowano ze wstępów do poszczególnych ksiąg z uwagi na fakt, że ks. prof. Popowski prosił, aby jemu osobiście pozostawić ten przywilej. Niestety, śmierć nie pozwoliła Profesorowi na realizację tego zadania."));
            info.Items.Add(new Paragraph(@"Mamy nadzieję, że Biblia pierwszego Kościoła będzie dla każdego z czytelników głęboką inspiracją do osobistego zanurzenia się w przekaz Bożego Słowa, które – niezależnie od języka, w jakim go odczytujemy – ma bez wątpienia siłę i moc przemieniania ludzkiego myślenia i życia. I pamiętajmy, że Jezus powiedział:"));
            info.Items.Add(new Paragraph(@"Przeszukujecie Pisma, bo uważacie, że macie w nich życie wieczne, a one właśnie dają świadectwo o mnie. (J 5,39 BPK)"));
            info.Items.Add(new Paragraph(@"Nie pozwólmy więc na to, aby cokolwiek powstrzymało nas przed zobaczeniem i&#160;zrozumieniem tego, co jest zasadniczym sensem i głównym przekazem całej Biblii, a&#160;mianowicie: że życie wieczne (a w zasadzie odwieczne i nieskończone Życie – ta wspaniała wiekuista rzeczywistość Boga) zostało udostępnione ludziom w sposób pełny tylko w Osobie Jezusa Chrystusa. Boże odwieczne Życie jest złożone wyłącznie w Nim i z Nim związane w sposób absolutnie nierozerwalny, dlatego tylko ten, kto wierzy w Syna, ma życie wieczne; a kto Synowi nie wierzy, nie ujrzy życia, lecz gniew Boga wisi nad nim. (J 3,36 BPK)"));
            info.Items.Add(new Paragraph(@"Piotr Wacławik") { Style = "text-align: right;" });
            info.Items.Add(new Paragraph(@"Wydawca") { Style = "text-align: right;" });
            return info;
        }
    }

    public class BPKXHtmlFileService {
    }
}
