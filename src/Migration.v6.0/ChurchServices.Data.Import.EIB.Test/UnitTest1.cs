using ChurchServices.Data.Import.EIB.Model;
using ChurchServices.Data.Import.EIB.Model.Bible;
using ChurchServices.Data.Import.EIB.Model.Osis;
using ChurchServices.Data.Import.Hebrew;
using ChurchServices.Data.Model;
using ChurchServices.Extensions;
using DevExpress.Xpo;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.Primitives;
using System.Text;
using System.Text.RegularExpressions;
using static DevExpress.Data.Helpers.ExpressiveSortInfo;

namespace ChurchServices.Data.Import.EIB.Test {
    [TestClass]
    public class UnitTest1 {

        [TestInitialize]
        public void Init() {
            new ConnectionHelper().Connect(
                connectionString: @"XpoProvider=SQLite;data source=..\..\..\..\..\..\db\IBE.SQLite3");
        }

        [TestMethod]
        public void ImportBHS() {
            var bhs = new BHSInterlinearBuilder();
            var uow = new UnitOfWork();
            bhs.Build(uow, @"C:\Users\krzysztof.radzimski\Downloads\BHSEk+\BHSEk+.SQLite3");
        }

        [TestMethod]
        public void ImportLHB() {
            var bhs = new LhbBuilder();
            var uow = new UnitOfWork();
            bhs.Build(uow, @"C:\Users\krzysztof.radzimski\Downloads\LHBc\LHBc.SQLite3");
        }

        [TestMethod]
        public void ImportHSB() {
            var bhs = new HBSTransliterationBuilder();
            var uow = new UnitOfWork();
            bhs.Build(uow, @"C:\Users\krzysztof.radzimski\Downloads\HSB+\HSB+.SQLite3");
        }

        [TestMethod]
        public void RepairVerseTextInHSB() {
            var bhs = new HBSTransliterationBuilder();
            var uow = new UnitOfWork();
            bhs.RepairVerseText(uow);
        }

        [TestMethod]
        public void TestOsisModel() {
            var service = new OsisModelService();
            var model = service.GetModelFromFile(@"D:\OneDrive\WBST\2020\Fakultety\Biblistyka\EIB\SNP_BibleEngine\SNP_BibleEngine\SNPD\snpd.osis.v3.xml");
            if (model != null) {
                Assert.IsTrue(model.Text != null && model.Text.Divisions != null && model.Text.Divisions.Count > 0);
            }
            else {
                Assert.Fail();
            }
        }

        private void SetBookInfo(BookModel book, BibleModel model) {
            var uow = new UnitOfWork();
            var dbBook = new XPQuery<ChurchServices.Data.Model.BookBase>(uow).Where(x => x.NumberOfBook == book.NumberOfBook).FirstOrDefault();
            if (dbBook != null) {
                book.PlaceWhereBookWasWritten = RecognizeBookInfo(dbBook.PlaceWhereBookWasWritten, model, uow);
                book.AuthorName = RecognizeBookInfo(dbBook.AuthorName, model, uow);
                book.TimeOfWriting = RecognizeBookInfo(dbBook.TimeOfWriting, model, uow);
                book.Purpose = RecognizeBookInfo(dbBook.Purpose, model, uow);
                book.Subject = RecognizeBookInfo(dbBook.Subject, model, uow);
                if (book.BookName.IsNullOrEmpty()) { book.BookName = dbBook.BookTitle; }
                if (book.Color.IsNullOrEmpty()) { book.Color = dbBook.Color; }
            }
        }

        private string GetSNPLIntroduction() {
            var html = @"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//PL"">
<html>
<head>
<title></title>
</head>
<body>
<p>&nbsp;</p>
<p>&nbsp;</p>
<p>&nbsp;</p>
<p>&nbsp;</p>
<p style=""text-align: center; font-size: 40pt;"">BIBLIA<br/>to jest<br/>Pismo Święte<br/>Starego i&nbsp;Nowego<br/>Przymierza</p>
<p>&nbsp;</p>
<p>&nbsp;</p>
<p style=""text-align: center; font-size: 14pt;"">Przekład literacki z&nbsp;języka hebrajskiego,<br/>aramejskiego i&nbsp;greckiego,<br/>z przypisami</p>
<p>&nbsp;</p>
<p>&nbsp;</p>
<p>&nbsp;</p>
<p>&nbsp;</p>

<p><b>Biblia, to jest Pismo Święte Starego i&nbsp;Nowego Przymierza<br/>Przekład literacki z&nbsp;języka hebrajskiego, aramejskiego i&nbsp;greckiego, z&nbsp;przypisami</b></p>
<span>Wydanie pierwsze</span></h2>
<p>Original edition © 2016 Ewangeliczny Instytut Biblijny, www.feib.pl</p>
<p>Ebook edition © 2016 LOGOS MEDIA, www.logosmedia.pl</p>
<p><b>Tłumaczenie</b>: Piotr Zaremba (z&nbsp;Anną Haning: Mt-J, Rz, Hbr)</p>
<p><b>Uaktualnienia związane z&nbsp;tekstem greckim NA28</b>: Piotr Zaremba</p>
<p><b>Uaktualnienia językowe i&nbsp;stylistyczne</b>: Karolina J. Zaremba, Piotr Zaremba</p>
<p><b>Odsyłacze</b>: Adam Ciorga, Karolina J. Zaremba, Krystyna W. Wierszyłowska-Zaremba, Piotr Zaremba</p>
<p><b>Części wstępne</b>: <i>Ważniejsze uwagi o&nbsp;przekładzie</i>, tabele informacyjne, <i>Miary i&nbsp;wagi</i>, uwagi na temat not wstępnych i&nbsp;podziału Księgi Psalmów: Piotr Zaremba</p>
<p><b>Przekład części wstępnych na język angielsk</b>i: Karolina J. Zaremba</p>
<p><b>Redakcja naukowa</b>: Dariusz Banicki, Adam Ciorga, Robert Merecz, Piotr Muchowski, Andrzej Zaborski (†), Karolina J. Zaremba</p>
<p><b>Konsultacja polonistyczna</b>: Ewa Sawicka (†), Ewa i&nbsp;Andrzej Seweryn, Marta Tylenda-Wodniczak, Karolina J. Zaremba</p>
<p><b>Sponsor przekładu</b>: In Touch Mission International</p>
<p><b>Redakcja wydania elektronicznego</b>: Jarosław Jankowski</p>
<p><b>Konwersja publikacji i&nbsp;redakcja techniczn</b>a: Zbigniew Szalbot</p>
<p><b>Projekt okładki</b>: Marcin Wiśniewski</p>
<p>Publikacja powstała we współpracy z&nbsp;Ewangelicznym Instytutem Biblijnym.</p>
<p><b>Wersja ebooka</b>: 1.0</p>
<p>All Rights Reserved. Wszelkie prawa zastrzeżone. Przedruk, odtwarzanie lub przetwarzanie całości lub fragmentów książki w&nbsp;mediach każdego rodzaju wymaga pisemnego zezwolenia Ewangelicznego Instytutu Biblijnego.</p>
<p>&nbsp;</p>
<p>&nbsp;</p> 
<p><b>Wydawca</b>: LOGOS MEDIA</p>
<p><b>Przygotowanie wersji dla <i>Logos Study Bible 10</i></b>: Krzysztof Radzimski</p>
<p>EPUB: ISBN  978-83-63837-88-4<br/>MOBI: ISBN 978-83-63837-89-1</p>
<p><b>Patronat</b>:</p>
<p><a href=""http://www.jezus.pl"">jezus.pl</a></p>
<p>&nbsp;</p>
<p>&nbsp;</p>

<p style=""text-align: center;""><b>Słowo zaproszenia</b></p>
<p>Trzymasz w&nbsp;ręce nie tyle Księgę, co bibliotekę sześćdziesięciu sześciu Ksiąg Starego i&nbsp;Nowego Przymierza. Uwiecznione w&nbsp;nich słowa wkraczały w&nbsp;ludzką rzeczywistość stopniowo, na przestrzeni bodaj tysiąca czterystu lat, aby w&nbsp;końcu znaleźć potwierdzenie i&nbsp;pełny wyraz w&nbsp;Słowie, które stało się ciałem, w&nbsp;Jezusie Chrystusie.</p>
<p>Księgi tego zbioru nie przestają służyć wielu za podręcznik definiowania racji. Tymczasem mają one służyć pogłębianiu relacji. Obserwując wzmożone wysiłki współczesnych sobie uczonych, Pan Jezus Chrystus podsumował: <b>„Zagłębiacie się w&nbsp;Pisma, ponieważ sądzicie, że macie w&nbsp;nich życie wieczne, podczas gdy one składają świadectwo o&nbsp;Mnie. A jednak nie chcecie przyjść do Mnie, aby zyskać życie.”</b></p>
<p>Trudno o&nbsp;lepsze ujęcie sprawy. Jeśli na kartach tych Pism zamierzasz szukać przede wszystkim racji, to zapewne ją znajdziesz, jak wielu innych – na własną miarę, jednym na pociechę, drugim na przekór. Może tej racji zechcesz bronić, a&nbsp;nawet za nią umrzeć. Jeśli jednak dzięki tej Księdze nie nawiążesz relacji z&nbsp;Osobą, z&nbsp;Panem wszechrzeczy, Jezusem Chrystusem, jeśli On sam nie stanie się rdzeniem Twojego życia, to uzbrojony w&nbsp;prawdziwe racje, możesz rozminąć się z&nbsp;prawdziwym życiem.</p>
<p>Księga ta nie należy do łatwych. I&nbsp;dobrze. Szczerzej przyjdą Ci słowa: <b>„Panie, otwórz mi oczy na to, co na tych kartach czytam, a&nbsp;to, co zrozumiem, zastosuję w&nbsp;życiu.”</b> Przy takiej postawie doznasz czegoś szczególnego: zauważysz, że Pismo Święte jest Księgą, którą czyta się w&nbsp;obecności jej Autora. Na tym polega wyjątkowość tej Księgi – i&nbsp;przygoda, do której Cię zapraszamy.</p>
<p style=""text-align: right;""><a href=""https://feib.pl/"">Ewangeliczny Instytut Biblijny</a></p>
<p>&nbsp;</p>
<p>&nbsp;</p>

<h2 style=""text-align: center;"">Wykaz skrótów i oznaczeń</h2>
<p><b>Skróty ksiąg biblijnych:</b></p>
<p>Stare Przymierze: Rdz, Wj, Kpł, Lb, Pwt, Joz, Sdz, Rt, 1-2Sm, 1-2Krl, 1-2Krn, Ezd, Ne, Est, Jb, Ps, Prz, Kzn, Pnp, Iz, Jr, Tr, Ez, Dn, Oz, Jl, Am, Ab, Jo, Mi, Na, Ha, So, Ag, Za, Ml</p>
<p>Nowe Przymierze: Mt, Mk, Łk, J , Dz, Rz, 1-2Kor, Ga, Ef, Flp, Kol, 1-2Ts, 1-2Tm, Tt, Flm, Hbr, Jk, 1-2P, 1-3J, Jd, Obj</p>
<p>&nbsp;</p>
<p><b>Nazwy znaków alfabetu hebrajskiego:</b></p>
<p><span lang=""he"" dir=""rtl"">א</span><span dir=""ltr"">&nbsp;alef</span>, <span lang=""he"" dir=""rtl"">ב</span><span dir=""ltr"">&nbsp;bet</span>, <span lang=""he"" dir=""rtl"">ג</span><span dir=""ltr"">&nbsp;gimel</span>, <span lang=""he"" dir=""rtl"">ד</span><span dir=""ltr"">&nbsp;dalet</span>, <span lang=""he"" dir=""rtl"">ה</span><span dir=""ltr"">&nbsp;he</span>, <span lang=""he"" dir=""rtl"">ו</span><span dir=""ltr"">&nbsp;waw</span>, <span lang=""he"" dir=""rtl"">ז</span><span dir=""ltr"">&nbsp;zain</span>, <span lang=""he"" dir=""rtl"">ח</span><span dir=""ltr"">&nbsp;chet</span>, <span lang=""he"" dir=""rtl"">ט</span><span dir=""ltr"">&nbsp;tet</span>, <span lang=""he"" dir=""rtl"">י</span><span dir=""ltr"">&nbsp;jod</span>, <span lang=""he"" dir=""rtl"">כ</span><span dir=""ltr"">&nbsp;kaw</span>, <span lang=""he"" dir=""rtl"">ל</span><span dir=""ltr"">&nbsp;lamed</span>, <span lang=""he"" dir=""rtl"">מ</span><span dir=""ltr"">&nbsp;mem</span>, <span lang=""he"" dir=""rtl"">נ</span><span dir=""ltr"">&nbsp;nun</span>, <span lang=""he"" dir=""rtl"">ס</span><span dir=""ltr"">&nbsp;samek</span>, <span lang=""he"" dir=""rtl"">ע</span><span dir=""ltr"">&nbsp;ain</span>, <span lang=""he"" dir=""rtl"">פ</span><span dir=""ltr"">&nbsp;pe</span>, <span lang=""he"" dir=""rtl"">צ</span><span dir=""ltr"">&nbsp;tsade</span>, <span lang=""he"" dir=""rtl"">ק</span><span dir=""ltr"">&nbsp;kof</span>, <span lang=""he"" dir=""rtl"">ר</span><span dir=""ltr"">&nbsp;resz</span>, <span lang=""he"" dir=""rtl"">ש</span><span dir=""ltr"">&nbsp;szin</span>, <span lang=""he"" dir=""rtl"">ת</span><span dir=""ltr"">&nbsp;taw</span></p>
<p>&nbsp;</p>

<p><b>Inne skróty i oznaczenia</b></p>
<p style=""text-align: left;"">
abc – kursywą zaznaczono cytaty ze Starego Przymierza za NA27 i NA28. W samym Starym Przymierzu zaznaczono w&nbsp;ten sposób nie tłumaczone wyrazy obcego pochodzenia, jak: Sela i pur<br />
A – Kodeks Aleksandryjski = GA w&nbsp;SP, V w. po Chr.<br />
abs. – absolutus<br />
acc. – acusativus<br />
ak. – akkadyjski<br />
aleks. – aleksandryjski<br />
amor. – amorycki<br />
aor. – aoryst<br />
Ar – wersja arabska<br />
arab. – arabski<br />
aram. – aramejski<br />
as. – asyryjski<br />
B – Kodeks Watykański = GB w&nbsp;SP, IV w. po Chr.<br />
Ba – Baruch<br />
bab. – babiloński<br />
bezok – bezokolicznik<br />
bliskozn. – bliskoznaczny, -a, -e<br />
bo – wersja bohairyczna<br />
BHQ – Biblia Hebraica Quinta. 2010. Schenker. Stuttgart: Deutsche Bibelgesellschaft<br />
BHS – Biblia Hebraica Stuttgartensia. 1997. K. Elliger, W. Rudolf, red., wyd. 5. Stuttgart: Deutsche Bibelgesellschaft<br />
C – Kodeks Efrema, V w. po Chr.<br />
chald. – chaldejski<br />
con. – coniunctivus<br />
cz – czasownik, -niki<br />
D – Kodeks Bezy, V w. po Chr.<br />
dat. – dativus<br />
dat. instr. – dativus instrumentalis<br />
dat. loc. – dativus locativus<br />
defek. – defektywna (forma)<br />
det. – determinatyw<br />
Did – Didache<br />
dit. – dittografia<br />
dk – dokonane<br />
dł. – długość<br />
dod. – dodatek, dodaje, -ją, -wać<br />
dos. – dosłowny, -a, -e, -nie<br />
dot. – dotyczy, - czący<br />
du – dualis<br />
duch. – duchowy, -a, -e, -o<br />
EA – Tel-el-Amarna<br />
ed(d) – edycje Ms(s)<br />
egip. – egipski<br />
elam. – elamicki, -e<br />
em. – emendacja, -dowane<br />
emf. – emfatyczne<br />
Ew. – Ewangelia<br />
etiop. — etiopski<br />
etym. – etymologia, -cznie<br />
euf. – eufemizm<br />
f. – funkcja, funkcjonować<br />
fen. – fenicki<br />
filist. – filistyński<br />
frg. frgy. – fragment, –y<br />
fryg. – frygijski, -a,-e<br />
fut. – futurum, futuryczny, -a, -e<br />
G* – G, tekst gr. pierwotny<br />
LXX, G, Gmss – Septuaginta, niektóre manuskrypty Septuaginty<br />
GA – Kodeks Aleksandryjski<br />
GB – Kodeks Watykański<br />
GBal – Kodeks Watykański wtóry<br />
GE – Göttingen Exodus<br />
gen. – genetivus<br />
GK – fragmenty z&nbsp;Genizy Kairskiej<br />
GL – G, recenzja Lucjana<br />
Gmin – G, mss minuskułowe<br />
GN – kodeks Basiliano-Vaticanus połączony z&nbsp;kodeksem Veneto<br />
GO – G, recenzja Orygenesa<br />
GS – zasada Granville’a Sharpa<br />
GS – Kodeks Synaiticus<br />
GV – Kodeks Venetus<br />
G82 – G ms 82<br />
G127 – G ms 127<br />
głęb. – głębokość<br />
godz. – godzina<br />
gr. – grecki<br />
gram. – gramaty-ka, -cznie, -czny<br />
grub. – grubość, -ści<br />
H – przekład Hieronima<br />
h – h wygłosowe w&nbsp;transkrypcji wyrażeń<br />
hbr., które przy wymowie pozostaje nieme<br />
h. – homonim, homonim(icznie)(iczny)<br />
hap. – haplografia<br />
hbr. – hebrajski<br />
hebr. – hebraizm<br />
Hen – Henoch, księgi Henocha<br />
hend. – hendiadys<br />
het. – hetycki, -s<br />
Hex – źródła heksaplaryczne<br />
hi – hifil<br />
hier. egip. – hieroglificzny egipski<br />
hist. – historyczny, -a, -e, -nie<br />
hitp – hitpael<br />
hitpalp – hitpalpel<br />
hitpo – hitpolel<br />
HM – hebrajski misznaicki<br />
HN – hebrajski nebatejski<br />
hof – hofal<br />
hl – hapax legomenon, słowo użyte tylko raz w&nbsp;tekście Biblii<br />
hl2,3 – słowo użyte w&nbsp;tekście Biblii, odpowiednio, dwa lub trzy razy<br />
hom. – homonim<br />
hur. – hurycki<br />
Id, If, Ih, Ip – interpretacje: duchowa, futuryczna, historycza i preteryczna<br />
ill. – illyryjski<br />
imp. – imperativus<br />
impf. – imperfectum<br />
ind. – indicativus<br />
inf. – infinitivus<br />
ins. – inskrypcja, -cje<br />
int. – interpretacja, -cje<br />
J – źródło jahwistyczne<br />
J. – jezioro<br />
jedn. – jednostka<br />
JHWH – Imię własne Boga, Jahwe<br />
Jub – Księga Jubileuszy<br />
Jud – Księga Judyty<br />
juss. – jussivus<br />
kan. – kananejski, -a<br />
KA – Konstytucje Apostolskie<br />
KD – Kodeks Damasceński<br />
KH – Kodeks Hammurabiego<br />
KP – Kodeks Petersburski, B 19A<br />
ketiw – wyrażenie zaświadczone w&nbsp;tekście głównym MT, dla którego sugeruje się właściwy odczyt, czyli qere<br />
kj,w,z – kryterium, odpowiednio, językowe, większego wglądu w&nbsp;tekst wyjściowy i&nbsp;zrozumiałości.<br />
klk, klkn, klkd, klks – kilka, -naście, -dziesiąt, -set.<br />
kod. – kodeks, -y<br />
konstr. – konstrukcja<br />
kont. — kontekst, -owy<br />
kor. – korekta<br />
L – Kodeks angelicus, IX w. po Chr.<br />
l. – lub<br />
la b d e o – lectio, odpowiednio: ante, brevior, difficilior, facilus explicavit, lingua originali<br />
licz – liczebnik<br />
lit. – literatura<br />
lp, lm – liczba pojedyncza, liczba mnoga<br />
LR – Leviticus Rabba<br />
łac. – łaciński<br />
mat. – materiał(y)<br />
M. – morze<br />
Mch – Machabejskie, księgi<br />
Mdr – Księga Mądrości<br />
med. – medyczny<br />
met. – metonimia, -nimicznie<br />
metaf. – metafora, metaforycz(ny)(nie)<br />
metat. – metateza<br />
mez. – mezopotamski, -a<br />
mg – margines, -owy, -owa<br />
min. – minuta<br />
min – w&nbsp;indeksie górnym: minuskuł(a)(owy)<br />
m.in. – między innymi<br />
mn. – mniejszy, -a<br />
monet. – monetarny<br />
ms, mss – manuskrypt(y)<br />
Ms – kodeks hbr. średniowieczny<br />
MT, MTmss – tekst masorecki, manuskrypty tekstu masoreckiego<br />
MTMs, MTMss – manuskrypt, manuskrypty hebrajskie średniowieczne<br />
NA27, NA28 – Novum Testamentum Graece. Nestle-Aland. 1993, 2012. Stuttgart: Deutsche Bibelgesellschaft<br />
NB – Niewola Babilońska<br />
ndk – niedokonane<br />
neoas. – noeasyryjski, -a, -e<br />
ni – nifal<br />
niereg. – nieregularny, -a, -e<br />
nom. – nominativus<br />
NP – Nowe Przymierze (Nowy Testament)<br />
n.p.m. – nad poziomem morza<br />
O – recenzja Orygenesa<br />
OG – wersje starogreckie<br />
okr. – okres<br />
OL – wersje starołacińskie<br />
oryg. – oryginalny (tekst l. wariant)<br />
ozn. – oznacza<br />
P – źródło kapłańskie<br />
palp – palpel<br />
pap. – papirus(y)<br />
par. – paralelne, paralelizm<br />
pas. – passivus, strona bierna<br />
pd – południe<br />
pd wsch – południowy wschód<br />
pd zach – południowy zachód<br />
pers. – perski<br />
pf. – perfectum<br />
pi – piel<br />
pilp – pilpel<br />
PA – Pouczenia Amenemope<br />
PL – Pardes Lauder<br />
pl – pluralis, lm<br />
pn – północ<br />
pn wsch – północny wchód<br />
pn zach – północny zachód<br />
poch. – pochodzenie<br />
pod. – podobnie, podobieństwo<br />
poj. – pojęcie<br />
pojem. – pojemność<br />
polit. – polityczny, -a, -e<br />
polp – polpal<br />
por. – porównaj<br />
pow. – powierzchnia<br />
późn. – późniejszy, -e<br />
p.p.m. – poniżej poziomu morza<br />
praes. – praesens, czas teraźniejszy<br />
pret. – preteryczny, -a, -e, -nie<br />
prek. – prekatywny, -a, -e, -nie<br />
profet. – profetyczny/e<br />
prop. – proponowany, -a, -e, -zycja, -uje<br />
przedr – przedrostek<br />
przen. – przenośnia, -e<br />
przetłum. – przetłumaczony -a, -e<br />
przyd – przydawka<br />
przyim – przyimek<br />
przym – przymiotnik, -kowy<br />
przyp. – przypadek<br />
przys – przysłówek<br />
PN – Papirus Nasha<br />
PS – Pięcioksiąg Samarytański<br />
PsSal – Psalmy Salomona<br />
ptc. – participium, imiesłów<br />
pu – pual<br />
przyp. – przypis, -y<br />
przys – przysłówek<br />
pyt. – pytanie<br />
q – kal<br />
qere – wyrażenie sugerowane jako właściwy odczyt wyrażenia zaświadczonego w&nbsp;tekście głównym MT, czyli ketiw<br />
red. – redakcja, -cyjny<br />
rel. – religijny, -a, -e<br />
rewok. – rewokalizacja<br />
rm – rodzaj męski<br />
rodz. – rodzajnik<br />
r. p. Chr. – rok przed Chrystusem<br />
r. po Chr. – rok po Chrystusie<br />
rz – rzeczownik<br />
rzym. – rzymski<br />
rż – rodzaj żeński<br />
S – tekst syryjski<br />
sbab. – starobabiloński<br />
sc – status constructus<br />
scp – scriptio plena, pisownia pełna<br />
scd – scriptio defectiva, pisownia ułomna<br />
szer. – szerokość<br />
SG – Salkinson-Ginsburg. 1886. Hebrew New Testament, wyd. popr. 1999 w&nbsp;kierunku zgodności z&nbsp;Tekstem przyjętym greckiego NT<br />
sg – singularis, lp<br />
skr. – skrót, skrócony, -ne<br />
SP – Stare Przymierze (Stary Testament)<br />
spers. – staroperski<br />
spój – spójnik<br />
st. – stopień<br />
str. – strona, -y<br />
suf – sufiks<br />
sym. – symbol, -icznie, -iczny<br />
syn. – synonim, synonimiczny, -nie<br />
synek. – synekdocha, -chicznie<br />
Syr – Mądrość Syracha<br />
syr. – syryjski<br />
T – Talmud<br />
tamud. – tamudyjski<br />
TB – Talmud Babiloński<br />
Tb – Księga Tobiasza<br />
Tg – targum<br />
TgJo – Targum Jonatana<br />
TgN – Targum Neofiti<br />
TgPsJ – Targum Pseudo-Jonatana<br />
TgO – Targum Onkelosa<br />
TgMs/Mss – kodeks, kodeksy Targumów<br />
Th – grecki tekst Teodocjona<br />
tiq – tiqqune soferim<br />
TL – Testament Lewiego<br />
tłum. – tłumaczenie, -one<br />
TP – Tekst przyjęty<br />
trad. – tradycja, -cyjny, -cyjnie<br />
trans. – transliteracja<br />
Trt – Tertulian<br />
TW – Tekst większościowy<br />
tys. – tysiąclecie<br />
tyt. – tytuł<br />
ugar. – ugarycki<br />
vid – wariant widoczny, lecz niepewny<br />
Vg – Wulgata<br />
W – Kodeks Waszyngtoński, IV/V w. po Chr.<br />
w., ww. – werset, wersety<br />
wd,l,o,p,s,t – wzgląd, odpowiednio, duchowego dziedzictwa, logiczności, objętości, pierwszeństwa NP, starożytności i&nbsp;trudności wariantu<br />
war. – warunek, - owy, -owa<br />
WMoj – Wniebowstąpienie Mojżesza<br />
wok. – wokalizacja<br />
wsch – wschód<br />
wsp. – współczesne<br />
wyr. – wyrażenie<br />
wys. – wysokość<br />
wzgl. – względnie<br />
zach – zachód<br />
zaim – zaimek<br />
zal. – zależne, zależność, -ści<br />
zał. – założenie<br />
zak. – zakończenie<br />
zap. – zapożyczenie<br />
ZMM – Zwoje znad Morza Martwego<br />
zm. – zmarł<br />
zn. – znaczenie<br />
zob. – zobacz<br />
zwok. – zwokalizowane<br />
zwr. – zwrotny, -a, -e<br />
I, II... – homonimy (pierwszy, drugi...) ’‘ – transkrypcja odpowiednio (alef) i&nbsp;(ain)<br />
α’ – przekład Akwili<br />
ε’ – Quinta, warianty piątej kolumny Heksapli εβρ’ – późniejsze przekłady tekstu na język grecki hebrajskiego, lecz bliżej nieokreślonego pochodzenia<br />
P – papirus<br />
Θ Ψ m P75 892txt 070 1241 f 1.13 – powszechnie stosowane określenia świadectw tekstowych Nowego Przymierza. W&nbsp;niniejszym przekładzie przytaczane za Novum Testamentum Graece (jak wyżej)<br />
θ’ – przekład Teodocjona<br />
σ’ – przekład Symmacha<br />
Ψ – Athous Lavrensis, IX/X w. po Chr.<br />
1Hen – Pierwsza Księga Henocha<br />
1Kl – Pierwszy List Klemensa<br />
1Mch, 2Mch, 4Mch – Pierwsza, Druga, Czwarta Księga Machabejska<br />
<span lang=""he"" dir=""rtl"">א</span> – Kodeks synajski, III w. po Chr.<br />
[<span lang=""he"" dir=""rtl"">ה</span>] – znak lub znaki w&nbsp;nawiasie kwadratowym oznaczają, że nie zachowały się one w&nbsp;manuskryptach<br />
<span lang=""he"" dir=""rtl"">מֿהל֯ש̇</span> — brak znaku nad literą oznacza literę pewną, kropka prawdopodobną, kółko możliwą, kreska zaś niepewność co do identyfikacji liter podobnych<br />
[...] – nawias kwadratowy zamyka wyrazy lub wyrażenia, których w&nbsp;oryginale wyraźnie brak, a które w&nbsp;odczuciu tłumaczy wyjaśniają lub stanowią opcję wyjaśnienia znaczenia tekstu<br />
* – oryginalny odczyt ms<br />
1,2... – pierwsza, druga ... grupa korekt<br />
099 – Kodeks majuskułowy, VII w. po Chr.<br />
597 – Kodeks minuskułowy, XIII w. po Chr.
</p>

<p>&nbsp;</p>
<p>&nbsp;</p> 
<p>&nbsp;</p>
<p>&nbsp;</p> 
</body>
</html>";
            return html;
        }
        private string GetSNPDIntroduction() {
            var html = @"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//PL"">
<html>
<head>
<title></title>
</head>
<body>
<p>&nbsp;</p>
<p>&nbsp;</p>
<p>&nbsp;</p>
<p>&nbsp;</p>
<p style=""text-align: center; font-size: 40pt;"">BIBLIA<br/>to jest<br/>Pismo Święte<br/>Starego i&nbsp;Nowego<br/>Przymierza</p>
<p>&nbsp;</p>
<p>&nbsp;</p>
<p style=""text-align: center; font-size: 14pt;"">Przekład dosłowny z&nbsp;języka hebrajskiego,<br/>aramejskiego i&nbsp;greckiego,<br/>z przypisami</p>
<p>&nbsp;</p>
<p>&nbsp;</p>
<p>&nbsp;</p>
<p>&nbsp;</p>

<p><b>Biblia, to jest Pismo Święte Starego i&nbsp;Nowego Przymierza<br/>Przekład dosłowny z&nbsp;języka hebrajskiego, aramejskiego i&nbsp;greckiego, z&nbsp;przypisami</b></p>
<p>Wydanie piąte (cyfrowe)</p>
<p>© 2021 Ewangeliczny Instytut Biblijny, https://feib.pl</p>
<p>&nbsp;</p>
<p>&nbsp;</p>

<p><b>Tłumaczenie</b>: Piotr Zaremba</p>
<p><b>Odsyłacze</b>: Adam Ciorga, Karolina J. Zaremba, Krystyna W. Wierszyłowska-Zaremba, Piotr Zaremba</p>
<p><b>Przypisy i&nbsp;części wstępne</b>: Wstęp, Jak korzystać z&nbsp;przekładu dosłownego, tabele informacyjne, Miary i&nbsp;wagi, uwagi na temat not wstępnych i&nbsp;podziału Księgi Psalmów: Piotr Zaremba</p>
<p><b>Uwaga</b>: Ważnym uzupełnieniem przypisów i&nbsp;części wstępnych niniejszego dzieła jest Literatura i&nbsp;uwagi do wersetów Pisma Świętego w&nbsp;przekładzie dosłownym EIB oraz Literatura do przekładu Biblii, to jest Pisma Świętego Starego i&nbsp;Nowego Przymierza. Jeśli brak ich w&nbsp;tym dziele, to stanowią osobne publikacje EIB.</p>
<p><b>Przekład części wstępnych na język angielski</b>: Karolina J. Zaremba</p>
<p><b>Redakcja naukowa</b>: Dariusz Banicki, Robert Merecz, Andrzej Zaborski (†), Karolina J. Zaremba</p>
<p><b>Konsultacja polonistyczna</b>: Ewa i&nbsp;Andrzej Seweryn, Karolina J. Zaremba </p>
<p><b>Konsultacja czytelnicza</b>: Karol Czarnowski, Wojciech Leszczyński</p>
<p><b>Obróbka elektroniczna</b>: Łukasz Czarniecki</p>
<p><b>Skład i&nbsp;projekt okładki</b>: Łukasz Wodniczak</p>
<p><b>Mapy</b>: Paweł Kozłowski, Robert Mieniok, Piotr Zaremba</p>
<p><b>Ilustracje</b>: Maciej Duda, Emilia Maciejewska, Piotr Zaremba</p>
<p><b>Przygotowanie wersji dla <i>Logos Study Bible 10</i></b>: Krzysztof Radzimski</p>
<p><b>Sponsor przekładu</b>: In Touch Mission International</p>
<p>All Rights Reserved. Wszelkie prawa zastrzeżone. Przedruk, odtwarzanie lub przetwarzanie całości lub fragmentów książki w&nbsp;mediach każdego rodzaju wymaga pisemnego zezwolenia Ewangelicznego Instytutu Biblijnego.</p>
<p><b>ISBN</b>: 978-83-62242-07-8</p>
<p>&nbsp;</p>
<p>&nbsp;</p>

<p style=""text-align: center;""><b>Słowo zaproszenia</b></p>
<p>Trzymasz w&nbsp;ręce nie tyle Księgę, co bibliotekę sześćdziesięciu sześciu Ksiąg Starego i&nbsp;Nowego Przymierza. Uwiecznione w&nbsp;nich słowa wkraczały w&nbsp;ludzką rzeczywistość stopniowo, na przestrzeni bodaj tysiąca czterystu lat, aby w&nbsp;końcu znaleźć potwierdzenie i&nbsp;pełny wyraz w&nbsp;Słowie, które stało się ciałem, w&nbsp;Jezusie Chrystusie.</p>
<p>Księgi tego zbioru nie przestają służyć wielu za podręcznik definiowania racji. Tymczasem mają one służyć pogłębianiu relacji. Obserwując wzmożone wysiłki współczesnych sobie uczonych, Pan Jezus Chrystus podsumował: <b>„Zagłębiacie się w&nbsp;Pisma, ponieważ sądzicie, że macie w&nbsp;nich życie wieczne, podczas gdy one składają świadectwo o&nbsp;Mnie. A jednak nie chcecie przyjść do Mnie, aby zyskać życie.”</b></p>
<p>Trudno o&nbsp;lepsze ujęcie sprawy. Jeśli na kartach tych Pism zamierzasz szukać przede wszystkim racji, to zapewne ją znajdziesz, jak wielu innych – na własną miarę, jednym na pociechę, drugim na przekór. Może tej racji zechcesz bronić, a&nbsp;nawet za nią umrzeć. Jeśli jednak dzięki tej Księdze nie nawiążesz relacji z&nbsp;Osobą, z&nbsp;Panem wszechrzeczy, Jezusem Chrystusem, jeśli On sam nie stanie się rdzeniem Twojego życia, to uzbrojony w&nbsp;prawdziwe racje, możesz rozminąć się z&nbsp;prawdziwym życiem.</p>
<p>Księga ta nie należy do łatwych. I&nbsp;dobrze. Szczerzej przyjdą Ci słowa: <b>„Panie, otwórz mi oczy na to, co na tych kartach czytam, a&nbsp;to, co zrozumiem, zastosuję w&nbsp;życiu.”</b> Przy takiej postawie doznasz czegoś szczególnego: zauważysz, że Pismo Święte jest Księgą, którą czyta się w&nbsp;obecności jej Autora. Na tym polega wyjątkowość tej Księgi – i&nbsp;przygoda, do której Cię zapraszamy.</p>
<p style=""text-align: right;""><a href=""https://feib.pl/"">Ewangeliczny Instytut Biblijny</a></p>
<p>&nbsp;</p>
<p>&nbsp;</p>

<h1 style=""text-align: center;"">Wstęp</h1>
<h2>Plan Wstępu</h2>
<p>Niniejszy Wstęp dzieli się na cztery części:</p>
<p>&nbsp;1.&nbsp;Część 1 to informacje o&nbsp;Biblii, to jest Piśmie Świętym Starego i&nbsp;Nowego Przymierza, przekład dosłowny z&nbsp;języka hebrajskiego, aramejskiego i&nbsp;greckiego, z&nbsp;przypisami, jako publikacji.</p>
<p>&nbsp;2.&nbsp;Część 2 to dane o&nbsp;tekście źródłowym i&nbsp;kryteriach doboru wariantów tekstowych.</p>
<p>&nbsp;3.&nbsp;Część 3 to dane o&nbsp;cechach szczególnych przekładu.</p>
<p>&nbsp;4.&nbsp;Część 4 to informacje o&nbsp;układzie treści i&nbsp;szczegółach redakcji technicznej.</p>
<p>&nbsp;</p>
<p>Taki plan powinien pomóc Czytelnikom:</p>
<p>&nbsp;1.&nbsp;uświadomić sobie wyjątkowość dzieła;</p>
<p>&nbsp;2.&nbsp;powiązać niniejszą publikację z&nbsp;innymi publikacjami Ewangelicznego Instytutu Biblijnego (EIB);</p>
<p>&nbsp;3.&nbsp;dowiedzieć się o&nbsp;jej celach, przeznaczeniu i&nbsp;wartościach;</p>
<p>&nbsp;4.&nbsp;poznać źródła tekstów wyjściowych i&nbsp;kryteria doboru ich wariantów;</p>
<p>&nbsp;5.&nbsp;uchwycić cechy szczególne przekładu;</p>
<p>&nbsp;6.&nbsp;ułatwić korzystanie z&nbsp;publikacji.</p>
<p>&nbsp;</p>
<p>&nbsp;</p>

<h2 style=""text-align: left;"">Część&nbsp;1:<br/><i>Biblia, to jest Pismo Święte Starego i&nbsp;Nowego Przymierza, przekład dosłowny z&nbsp;języka hebrajskiego, aramejskiego i&nbsp;greckiego, z&nbsp;przypisami (SNPD)</i>, jako publikacja</h2>
<p>&nbsp;</p>

<h3>Wyjątkowość publikacji</h3>
<p>O wyjątkowości niniejszej publikacji stanowi to, że:</p>

<p>1.&nbsp;SNPD jest owocem pracy translatorskiej uwzględniającej najnowszy stan badań nad tekstem 66 protokanonicznych ksiąg Pisma Świętego.</p>
<p>2.&nbsp;Wraz z&nbsp;wydanym wcześniej przekładem literackim (SNP) SNPD tworzy „dwuprzekład” wykorzystywany z&nbsp;jednej strony w&nbsp;wydaniach Pisma Świętego dla dzieci i&nbsp;młodzieży, a&nbsp;z drugiej na zajęciach z&nbsp;translatoryki biblijnej.</p>
<p>3.&nbsp;SNPD można wykorzystywać jako dzieło jednotomowe, ale jednocześnie, przez jego ścisłe powiązanie z&nbsp;przekładem literackim EIB, jako podręcznik zapewniający głębsze zrozumienie tekstu biblijnego.</p>
<p>4.&nbsp;SNPD zawiera 20.006 przypisów – 15.549 w&nbsp;Starym i&nbsp;4.456 w&nbsp;Nowym Przymierzu – co do swojej treści językoznawcze, literaturoznawcze, historyczne, geograficzne, kulturowe i&nbsp;egzegetyczne umożliwiające szersze spojrzenie na dzieje tekstu biblijnego i&nbsp;jego znaczenie.</p>
<p>5.&nbsp;SNPD zawiera 78.502 odsyłacze; łączą one fragmenty biblijne paralelne, fragmenty przekazujące podobną myśl, wykazujące podobną konstrukcję morfologiczną, składniową lub logiczną, oraz odsyłające do uwag i&nbsp;literatury.</p>
<p>6.&nbsp;SNPD zawiera 937 odniesień do idiomów hebrajskich lub greckich oraz 96 wskazań na hebraizmy w&nbsp;grece Nowego Przymierza.</p>
<p>7.&nbsp;SNPD w&nbsp;sposób wyjątkowy i&nbsp;jak dotychczas niepowtarzalny na gruncie polskim podaje kryteria doboru wariantów tekstowych do tekstu głównego przekładu.</p>
<p>8.&nbsp;SNPD nazywa hebrajskie i&nbsp;greckie figury stylistyczne mogące w&nbsp;sposób zasadniczy wpływać na zrozumienie przesłania zawartego w&nbsp;tekście.</p>
<p>9.&nbsp;SNPD, w&nbsp;części Dodatki, podaje informacje umożliwiające precyzyjne powiązanie faktów i&nbsp;realiów kultury biblijnej z&nbsp;kulturą dnia dzisiejszego.</p>
<p>10.&nbsp;SNPD zawiera kolorowe mapy zapewniające identyfikację geograficzną wzmiankowanych w&nbsp;tekstach biblijnych miejsc.</p>
<p>11.&nbsp;SNPD zawiera zweryfikowane od strony inżynierskiej ilustracje budowli i&nbsp;sprzętów trudnych do wyobrażenia sobie na podstawie samej lektury tekstu.</p>
<p>12.&nbsp;SNPD, choć, jak wspomniano, może być wykorzystywane jako dzieło jednotomowe, dużo większe możliwości poznawcze oferuje w&nbsp;powiązaniu z&nbsp;częścią Literatura i&nbsp;uwagi do wersetów Pisma Świętego (LiU), zawierającą tysiące dodatkowych informacji dotyczących dokumentów biblijnych, literatury przedmiotu, a&nbsp;także informacji niosących odpowiedź na najczęściej zadawane przez Czytelników pytania.</p>
<p>&nbsp;</p>
<p>Ponad to, co zostało wyszczególnione powyżej, wersje cyfrowe SNPD, wydane przez EIB i&nbsp;nie odbiegające treścią od wydań drukowanych, oferują:</p>
<p>1.&nbsp;interaktywność spisu treści oraz odsyłaczy do innych fragmentów biblijnych, map, dodatkowych informacji oraz części Literatura i&nbsp;uwagi do wersetów Pisma Świętego;</p>
<p>2.&nbsp;wektorowe mapy gwarantujące najwyższą jakość obrazu niezależnie od powiększenia, co jest szczególnie ważne dla wykładowców, duszpasterzy i&nbsp;nauczycieli, i&nbsp;z myślą o&nbsp;nich zostało tak przygotowane;</p>
<p>3.&nbsp;kolorowy druk ułatwiający śledzenie treści przypisów należących do jednego wątku myślowego (dotyczy to na obecnym etapie przypisów w&nbsp;Księdze Objawienia);</p>
<p>4.&nbsp;elastyczność w&nbsp;kwestii objętości dzieła; czytelnik ma możliwość zaopatrzenia się w&nbsp;wersję podstawową, rozszerzoną i&nbsp;pełną, a&nbsp;także, osobno, w&nbsp;część lub części stanowiące dodatek do głównej części dzieła zawierającej przekład tekstu biblijnego.</p>
<p>Wszystkie zasygnalizowane wyżej sprawy znajdują rozwinięcie w&nbsp;dalszych częściach <i>Wstępu</i>.</p>
<p>&nbsp;</p>

<h3>Przekład dosłowny i&nbsp;literacki w&nbsp;rozumieniu zastosowanym w&nbsp;SNPD</h3>
<p>Określenia <i>przekład dosłowny i&nbsp;przekład literacki</i> można rozumieć na różne sposoby, ze względu na długą historię przekładów Pisma Świętego na świecie oraz dzięki postępom w&nbsp;dziedzinie przekładoznawstwa (zob. np. MEP: 50-56; 185-191.), stąd, w&nbsp;imię jasności, już na tym etapie <i>Wstępu</i> warto sprecyzować, że w&nbsp;przypadku prac prowadzonych w&nbsp;ramach EIB:</p>
<p>Przekład dosłowny to tłumaczenie tekstu biblijnego z&nbsp;języka wyjściowego na polski przy zachowaniu odpowiedniości części mowy, z&nbsp;uwzględnieniem wymogów składni, frazeologii i&nbsp;kolokacji.</p>
<p>Przekład literacki to tłumaczenie tekstu biblijnego z&nbsp;języka wyjściowego na polski podporządkowujące określone wyżej rygory przekładu dosłownego wytycznym piękna mowy ojczystej.</p>
<p>Obrazowo można powiedzieć, że o&nbsp;ile w&nbsp;ramach przekładu dosłownego tłumacz stara się przenieść odbiorcę tłumaczenia w&nbsp;czasy i&nbsp;środowisko, w&nbsp;których powstawał tekst wyjściowy, o&nbsp;tyle w&nbsp;ramach przekładu literackiego tłumacz stara się „pomóc” oryginałowi „odnaleźć się” w&nbsp;czasach odbiorcy tłumaczenia.</p>
<p>W dalszych częściach <i>Wstępu</i> jeszcze niejednokrotnie mowa będzie o&nbsp;przekładach dosłownym i&nbsp;literackim, w&nbsp;ustalonym wyżej rozumieniu, w&nbsp;różnych kontekstach.</p>
<p>&nbsp;</p>

<h3>Związek niniejszego wydania SNPD z&nbsp;wydaniami wcześniejszymi i&nbsp;z&nbsp;przekładem literackim EIB</h3>
<p>Niniejsze wydanie SNPD jest wydaniem piątym cyfrowym. Wyprzedza ono co do treści wcześniejsze wydania drukowane i&nbsp;cyfrowe opublikowane w&nbsp;ramach EIB i&nbsp;LB. Dokonane zmiany wyszczególniono w&nbsp;końcowej części LiU.</p>
<p>SNPD jest drugim powstałym w&nbsp;ramach polskiej części chrześcijaństwa ewangelicznego dziełem przekładu Pisma Świętego z&nbsp;języków oryginalnych. Przekład ten pozostaje w&nbsp;ścisłym związku z&nbsp;przekładem literackim, dokonanym w&nbsp;ramach EIB, noszącym podobny tytuł (Biblia, to jest Pismo Święte Starego i&nbsp;Nowego Przymierza. 2016-2020. Poznań: Ewangeliczny Instytut Biblijny, Liga Biblijna w&nbsp;Polsce). Oba przekłady powstawały jednocześnie. Wydany wcześniej przekład literacki powstawał na bazie niniejszego przekładu dosłownego, a&nbsp;jednocześnie spostrzeżenia poczynione w&nbsp;trakcie tworzenia przekładu literackiego i&nbsp;po jego wydaniu przyczyniały się do ulepszania kolejnych wydań przekładu dosłownego. Opisany związek obu przekładów będzie utrzymywany.</p>
<p>&nbsp;</p>

<h3>Przypisy w&nbsp;SNPD i&nbsp;ich uzupełnienie</h3>
<p>Przekład dosłowny wzbogacony został przypisami. Ważnym uzupełnieniem tych przypisów oraz części wstępnych dzieła jest Literatura i&nbsp;uwagi do wersetów Pisma Świętego w&nbsp;przekładzie dosłownym EIB oraz Literatura do przekładu Biblii, to jest Pisma Świętego Starego i&nbsp;Nowego Przymierza. Z&nbsp;uwagi na objętość całego dzieła i&nbsp;związane z&nbsp;tym względy praktyczne, wymienione części uzupełniające drukowane są w&nbsp;ramach odrębnych publikacji. Publikacje te można traktować jako kolejne tomy głównego dzieła zawierającego przekład ksiąg biblijnych.</p>
<p>W wersji cyfrowej części, o&nbsp;których mowa wyżej, publikowane są łącznie z&nbsp;tekstem przekładu, tak by użytkownicy mogli przenosić się do tych części po naciśnięciu odsyłacza z&nbsp;indeksem górnym „L”. W&nbsp;wersjach cyfrowych zachowany pozostaje kolorowy druk tam, gdzie został on użyty.</p>
<p>&nbsp;</p>

<h3>Ważniejsze zmiany w&nbsp;niniejszym wydaniu SNPD</h3>
<p>Jak wspomniano, niniejsze, piąte cyfrowe wydanie SNPD wyprzedza wcześniejsze wydania drukowane i&nbsp;cyfrowe opublikowane w&nbsp;ramach EIB i&nbsp;LB. Dotyczy to tylko wydań w&nbsp;formacie PDF, nie zaś modułów do programów biblijnych w&nbsp;innych formatach ani do modułów tworzonych przez inne podmioty wydawnicze. Ważniejsze zmiany w&nbsp;stosunku do wydań wcześniejszych to:</p>
<p>1.&nbsp;Zmieniony w&nbsp;kierunku większej dosłowności przekład [[Dn&nbsp;11:31>>Dn 11:31]]; [[12:11>>Dn 12:11]] i&nbsp;[[1Ts&nbsp;1:1>>1Th 1:1]]. Zdecydowano się na te zmiany z&nbsp;powodu przychylnego przyjęcia SNP, czyli przekładu literackiego, wydawanego przez EIB od roku 2016, ze strony Czytelników. Intencją tłumacza było mocniejsze odróżnienie przekładu literackiego od dosłownego, tak aby to zróżnicowanie zapewniło Czytelnikom szersze spojrzenie na znaczenie przesłania całego Starego i&nbsp;Nowego Przymierza.</p>
<p>2.&nbsp;Nieznaczny wzrost liczby przypisów; jest ich na obecnym etapie 20.006. Jednak w&nbsp;72 przypadkach przeredagowano treść przypisów. Wzrost ten wynika z&nbsp;ciągłej, wzmożonej pracy nad tekstem biblijnym oraz z&nbsp;szerokiej konsultacji społecznej. Ta konsultaca pozwala określić nowe kierunki oraz nowe obszary zainteresowań Czytelników tekstem biblijnym. Niesie ona także nowe pytania domagające się odpowiedzi.</p>
<p>3.&nbsp;Całkowite przeredagowanie przypisów informujących o&nbsp;zawartości treściowej wariantów tekstowych pism Nowego Przymierza. W&nbsp;sposób niespotykany jak dotychczas w&nbsp;polskich przekładach zostały one poddane kryteriom opisanym w&nbsp;części Tekst źródłowy i&nbsp;kryteria doboru wariantów tekstowych niniejszego Wstępu. Jednocześnie pełniejszą informację na ten temat przeniesiono do części Literatura i&nbsp;uwagi do wersetów Pisma Świętego w&nbsp;przekładzie dosłownym EIB (LiU), w&nbsp;której to części wariantom tym jest poświęcona jeszcze wnikliwsza uwaga.</p>
<p>4.&nbsp;Powiększenie o&nbsp;ponad jedną trzecią w&nbsp;stosunku do wydania wcześniejszego, cyfrowego, bazy informacyjnej zawartej w&nbsp;części Literatura i&nbsp;uwagi do wersetów Pisma Świętego w&nbsp;przekładzie dosłownym EIB. </p>
<p>&nbsp;</p>

<h3>Cele publikacji przekładu dosłownego</h3>
<p>Publikacja przekładu dosłownego spełnia następujące cele:</p>
<p>1.&nbsp;dostarcza tłumaczenia, które jest równie wierne przesłaniu oryginału jak każdy dobry przekład, lecz mniej niż przekład literacki odbiega od tekstu wyjściowego w&nbsp;zakresie zachowania odpowiedniości części mowy przy jednoczesnym zachowaniu wymogów składni, frazeologii, a&nbsp;tam, gdzie to mogło być istotne, nawet kolokacji;</p>
<p>2.&nbsp;dostarcza tłumaczenia, które zwięźle przedstawia bogactwo wariantów tekstu wyjściowego wraz z&nbsp;ich znaczeniem w&nbsp;języku polskim;</p>
<p>3.&nbsp;daje dostęp do dzieła, które może służyć jako zwięzły przekład, a&nbsp;zarazem komentarz lingwistyczno-tekstowy i&nbsp;literaturoznawczy przydatny w&nbsp;pracach nad tekstem biblijnym podejmowanych w&nbsp;pojedynkę, na przykład w&nbsp;trakcie przygotowywania kazania, lub grupowo, na przykład w&nbsp;gronie rodziny, przyjaciół czy uczestników życia wspólnoty wiernych;</p>
<p>4.&nbsp;daje dostęp do dzieła, które jako przekład eklektyczny, omówiony co do istoty w&nbsp;dalszej części Wstępu, unaocznia złożoność dziejów tekstu biblijnego i&nbsp;przez to pozwala Pismom Świętym wyraźniej składać świadectwo o&nbsp;sobie samych;</p>
<p>5.&nbsp;daje dostęp do dzieła, które ma intrygować, wywoływać twórczy niepokój i&nbsp;przez to budzić nieodparte pragnienie przedsiębrania tego, co trudne, dociekania tego, co zawiłe, mierzenia się z&nbsp;pytaniami wciąż pozostającymi bez odpowiedzi, stawania w&nbsp;bezsile, upokorzeniu, a&nbsp;nawet rozpaczy wobec Tego, za którego sprawą te Pisma zostały spisane, były przekazywane i&nbsp;redagowane i&nbsp;którego można by w&nbsp;najściślejszym sensie uznać za Autora tych Pism, Nauczyciela ich prawd i&nbsp;Sprawcę egzystencjalnej przemiany tych wszystkich, dla których On stał się Perłą Najdroższą – Pana Jezusa Chrystusa. </p>
<p>&nbsp;</p>

<h3>Adresaci przekładu dosłownego EIB</h3>
<p>Przekład dosłowny podany w&nbsp;niniejszej publikacji, choć powstał głównie w&nbsp;ramach polskiej części chrześcijaństwa ewangelicznego, nie został stworzony z&nbsp;zamiarem dostarczenia przekładu wyznaniowego. Więcej, ponieważ ta część chrześcijaństwa zdaje się być najmniej uwarunkowana dogmatycznie, niniejszy przekład może okazać się bardziej niż inne współczesne przekłady „akademicki” w&nbsp;sensie swojej niezależności naukowej. Jako głos tej części chrześcijaństwa adresowany on jest do całego społeczeństwa, szczególnie jednak do osób:</p>
<p>1.&nbsp;zainteresowanych głębszym poznaniem Pisma Świętego oraz kultury czasów, które je wydały;</p>
<p>2.&nbsp;zajmujących się, w&nbsp;każdej formie i&nbsp;zakresie, nauczaniem, wychowaniem, kaznodziejstwem i&nbsp;duszpasterstwem;</p>
<p>3.&nbsp;studiujących w&nbsp;każdym trybie i&nbsp;zakresie;</p>
<p>4.&nbsp;zainteresowanych powszechnie sprawdzalnymi sądami na temat Pisma Świętego i&nbsp;innych zabytków literackich z&nbsp;okresu jego powstawania.</p> 
<p>&nbsp;</p>

<h3>Wartość przekładu SNPD</h3>
<p>Wartość niniejszego przekładu nie ogranicza się do walorów tekstu samego tłumaczenia zawartego w&nbsp;kolumnach, jako dzieła z&nbsp;zakresu translatoryki. Obejmuje ona również, w&nbsp;pierwszym rzędzie, przypisy, których jest bez mała 20 tysięcy, a&nbsp;następnie odsyłacze do innych fragmentów biblijnych, których jest niemal 79 tysięcy.</p>
<p>Ważne znaczenie dla zrozumienia Pisma Świętego mają też wiadomości wstępne, podawane na początku każdej księgi, oraz mapy i&nbsp;inne dodatki zamieszczone na końcu publikacji. Tylko pozornie łączą się one z&nbsp;przetłumaczonym tekstem w&nbsp;mniejszym stopniu. Przeciwnie, umieszczają one dokumenty składające się na Pismo Święte w&nbsp;czasie i&nbsp;kulturze. Od zrozumienia tych czasów i&nbsp;kultury zależne jest zrozumienie jego samego.</p>
<p>Części: Literatura i&nbsp;uwagi do wersetów Pisma Świętego... oraz Literatura do przekładu Biblii, to jest Pisma Świętego Starego i&nbsp;Nowego Przymierza, omówione zostały wnikliwiej w&nbsp;części Jak korzystać z&nbsp;przekładu dosłownego. Każdy użytkownik SNPD może czuć się do nich zaproszony.</p>
<p>Trudno przesadzić w&nbsp;zachęcie, by z&nbsp;niniejszego przekładu, SNPD, korzystać wraz z&nbsp;przekładem literackim SNP. Przekłady te uzupełniają się wzajemnie i&nbsp;wzbogacają. Warto też, co oczywiste, jak najczęściej i&nbsp;w jak najszerszym zakresie odwoływać się do tekstu greckiego, hebrajskiego i&nbsp;aramejskiego Pisma Świętego oraz do wszelkich źródeł mogących przyczynić się do lepszego zrozumienia słownictwa, składni i&nbsp;kultury, których Pismo Święte jest owocem. Jest to szczególnie ważne, gdy weźmie się pod uwagę, że powstawało ono na przestrzeni dziejów porównywalnych czasowo z&nbsp;historią literatury polskiej. </p>
<p>&nbsp;</p>

<h2 style=""text-align: left;"">Część&nbsp;2:<br/>Tekst źródłowy i&nbsp;kryteria doboru wariantów tekstowych</h2>
<p>&nbsp;</p>

<h3>Tekst źródłowy Nowego Przymierza</h3>
<p>Podstawą prac przekładowych ksiąg Nowego Przymierza był tekst i&nbsp;aparat krytyczny zawarty zarówno w&nbsp;dwudziestym siódmym, jak i&nbsp;w dwudziestym ósmym wydaniu Novum Testamentum Graece Nestlego -Alanda (NA27 i&nbsp;NA28, 1993 i&nbsp;2012, Stuttgart: Deutsche Bibelgesellschaft).</p>
<p>Tekst Listów Powszechnych zweryfikowano w&nbsp;oparciu o&nbsp;NA28, jako że tylko w&nbsp;obrębie tych Listów wydawcy wspomnianego tekstu greckiego dokonali zmian w&nbsp;oparciu o&nbsp;nowo opracowaną wytyczną, tzw. metodę genealogii spójnych wariantów (ang. Coherence-Based Genealogical Method, w&nbsp;skrócie CBGM). Zgodnie z&nbsp;tą nazwą opiera się ona na statystycznej spójności pomiędzy wariantami potencjalnego poprzednika danego świadectwa tekstowego a&nbsp;wariantami jego niekoniecznie bezpośredniego następnika (Wasserman T., Gurry P. J. 2017. A New Approach to Textual Criticism. An Introduction to the Coherence-Based Genealogical Method. Atlanta: SBL Press, Stuttgart: Deutsche Bibelgesellschaft: 3-35). Metoda ta przywraca w&nbsp;tekście głównym NA28 miejsce tym wariantom tekstowym, które – jako późniejsze – znajdowały się w&nbsp;aparacie krytycznym NA27.</p>
<p>W&nbsp;ten sposób tekst grecki najnowszego wydania Novum Testamentum Graece w&nbsp;obrębie Listów Powszechnych zbliżył się do Tekstu większościowego (Wasserman T., Gurry P. J. 2017: 10).</p> 
<p>&nbsp;</p>

<h3>Tekst źródłowy Starego Przymierza</h3>
<p>Tekstem źródłowym dla przekładu ksiąg Starego Przymierza były: Biblia Hebraica Stuttgartensia (BHS - Biblia Hebraica Stuttgartensia. 1997. Stuttgart: Deutsche Bibelgesellschaft), Biblia Hebraica Quinta (BHQ - Biblia Hebraica Quinta. 2004-2011. Stuttgart: Deutsche Bibelgesellschaft). W&nbsp;zakresie ksiąg opublikowanych, to jest Księgi Powtórzonego Prawa, Księgi Sędziów, Ksiąg Pięciu Megilot, Ksiąg Ezdrasza i&nbsp;Nehemiasza oraz Proroków Mniejszych), Pięcioksiąg Samarytański (PS - Samaritan Pentateuch Edited to MS 6 of the Shekhem Synagogue. 1994; 2015, Accordance 11), tekst zwojów znad Morza Martwego (ZMM - Discoveries from the Judean Desert. 1955-2002. Oxford: Clarendon Press) w&nbsp;zakresie ksiąg biblijnych oraz tekst Septuaginty (G) w&nbsp;zakresie hebrajskiego Tanach (Septuaginta: (1)&nbsp;Rahlfs, A. 1979. Stuttgart: Deutsche Bibelgesellschaft; (2)&nbsp;Ziegler J. 1931-2006. Göttingen: Vanderhoeck & Ruprecht).</p>
<p>Przekład dosłowny EIB, jak większość współczesnych przekładów, jest przekładem eklektycznym, to jest opartym na wielu źródłach tekstu hebrajskiego, z&nbsp;mniejszymi fragmentami aramejskimi, i&nbsp;tekstu greckiego. Ten rodzaj przekładu narzuca potrzebę ustalenia kryteriów doboru wariantów tekstowych zaświadczonych w&nbsp;rozlicznych źródłach.</p>
<p>Przy wyborze wariantów tekstowych do tekstu głównego niniejszego przekładu kierowano się trzema ogólniejszymi kryteriami i&nbsp;sześcioma węższymi względami. Trzy kryteria to:</p>

<p>1.&nbsp;kryterium większego wglądu w&nbsp;złożoność tekstu biblijnego (kw);</p>
<p>2.&nbsp;kryterium większej zrozumiałości tekstu dla odbiorcy przekładu (kz);</p>
<p>3.&nbsp;kryterium językowe (kj).</p>
<p>&nbsp;</p>
<p>Kryterium pierwsze i&nbsp;drugie opiera się na obserwacji, że wszystkie świadectwa tekstowe Pisma Świętego są ważne, składają się na skarb kościoła i&nbsp;stanowią przyczynek do pełniejszego zrozumienia jego dziejów i&nbsp;przesłania. Trafność tej obserwacji znajduje uzasadnienie w&nbsp;obecnym stanie badań nad tekstem dostępnych nam świadectw. Naturalną konsekwencją takiego stanu rzeczy jest dawanie w&nbsp;tekście głównym miejsca wariantom zapewniającym szersze spojrzenie na różnorodność świadectw tekstowych, a&nbsp;jednocześnie skutkującym bardziej zrozumiałym przekładem. Kryterium trzecie odzwierciedla oczywistą zasadę tłumaczenia z&nbsp;oryginału, a&nbsp;nie z&nbsp;jego przekładu. Stąd w&nbsp;przypadku Starego Przymierza pierwszeństwo dawano pismom oryginalnie hebrajskim, z&nbsp;ich aramejskimi fragmentami, należącym do hebrajskiego Tanach. Jednak ze względu na stan i&nbsp;naturę tekstu spółgłoskowego Starego Przymierza w&nbsp;możliwie jak największym zakresie starano się uwzględnić odpowiadające mu pisma Septuaginty.</p>
<p>W przypadku Nowego Przymierza pierwszeństwo dawano pismom greckim, nie umniejszając znaczenia jego starożytnych przekładów. W&nbsp;przypisach zwracano też uwagę na możliwe wyniki tłumaczenia tekstu greckiego na język hebrajski, kierując się przy tym istniejącymi już przekładami Nowego Przymierza na ten język, co każdorazowo zaznaczano w&nbsp;przypisach.</p>
<p>W ramach wymienionych kryteriów kierowano się dodatkowo sześcioma względami, odpowiadającymi, poza piątym i&nbsp;szóstym, kryteriom przyjmowanym w&nbsp;krytyce tekstu i&nbsp;odzwierciedlonym we współczesnych przekładach Pisma Świętego na języki narodowe. Sześć względów to:</p>

<p>1.&nbsp;wzgląd starożytności wariantu (ws), korespondujący z&nbsp;omówionym dalej lectio ante potior (la);</p>
<p>2.&nbsp;wzgląd objętości wariantu (wo), korespondujący z&nbsp;omówionym dalej lectio brevior potior (lb);</p>
<p>3.&nbsp;wzgląd trudności wariantu (wt), korespondujący z&nbsp;omówionym dalej lectio difficilior potior (ld);</p>
<p>4.&nbsp;wzgląd logiczności wariantu (wl), dający pierwszeństwo tym wariantom, w&nbsp;których użyte słowa lub konstrukcje są bardziej logiczne;</p>
<p>5.&nbsp;wzgląd pierwszeństwa myśli Nowego Przymierza przed Starym (wp), w&nbsp;myśl którego tam, gdzie to miało znaczenie, trzymano się zasady wyjaśniania Starego Przymierza Nowym;</p>
<p>6.&nbsp;wzgląd duchowego dziedzictwa (wd), zgodnie z&nbsp;którym w&nbsp;tekście głównym dano miejsce wariantom z&nbsp;reguły nie uwzględnianym w&nbsp;tekście głównym przekładów stosujących kryteria krytycznych opracowań tekstów wyjściowych. Za daniem miejsca tym wariantom przemawia spostrzeżenie, że ich umieszczanie w&nbsp;przypisach powoduje całkowite ich zniknięcie z&nbsp;wydań Pisma Świętego, które przypisów nie publikują i&nbsp;w ten sposób odcinają Czytelników od duchowego dziedzictwa kościoła.</p>

<p>Zastosowanie przedstawionych kryteriów i&nbsp;względów jest, za pomocą podanych skrótów, zaznaczone we wszystkich przypisach Nowego Przymierza i&nbsp;w&nbsp;uznanych za kluczowe przypisach Starego.</p>
<p>Podane wyżej rozstrzygnięcia wymagają dodatkowego komentarza. Tekst biblijny podany w&nbsp;wyszczególnionych w&nbsp;niniejszym Wstępie źródłach został przygotowany w&nbsp;oparciu o&nbsp;założenia i&nbsp;kryteria przyjęte przez stojące za nimi grona biblistów, mających jednak na uwadze teksty wyjściowe, a&nbsp;nie teksty docelowe. Założeniom tym i&nbsp;kryteriom można podporządkować także proces tłumaczenia, ale można je zmodyfikować lub ustalić inne. W&nbsp;zależności od przyjętych kryteriów można na przykład otrzymać przekłady homogeniczne, to znaczy oparte na jednym źródle, albo eklektyczne, oparte na twórczym wykorzystaniu wielu źródeł. W&nbsp;pierwszym przypadku pełniejszy wgląd w&nbsp;bogactwo świadectw tekstowych byłby sprawą tysięcy przekładów na język polski. Ich czytelnicy mogliby je sobie porównywać i&nbsp;zestawiać we własnym zakresie. W&nbsp;drugim przypadku pełniejszy wgląd mógłby być sprawą jednego dzieła, z&nbsp;tekstem głównym wyrażającym określone założenia i&nbsp;kryteria oraz z&nbsp;bardziej lub mniej rozbudowanym aparatem krytycznym. Niniejszy przekład jest właśnie wyrazem tej drugiej możliwości.</p>
<p>Przekład dwudziestu czterech ksiąg hebrajskiego Tanach, czyli trzydziestu dziewięciu ksiąg Starego Przymierza, jako eklektyczny, nie jest konsekwentnym tłumaczeniem tekstu masoreckiego (MT) podanego w&nbsp;BHS. Znalazły w&nbsp;nim odzwierciedlenie również warianty tekstowe BHQ, PS, ZMM i&nbsp;G.</p>
<p>Takie podejście uzasadnione jest trzema względami: (1)&nbsp;Odkrycie zwojów znad Morza Martwego rzuciło nowe światło na tekst masorecki, a&nbsp;przez to na Septuagintę, która dzięki temu zyskała na znaczeniu. (2)&nbsp;Sama Septuaginta, pojmowana w&nbsp;tych rozważaniach jako grecki przekład ksiąg Biblii hebrajskiej, podobnie jak niniejszy przekład nie jest konsekwentnym tłumaczeniem tekstu masoreckiego w&nbsp;takiej formie, w&nbsp;jakiej podaje go BHS. Tekst Septuaginty jest w&nbsp;niektórych przypadkach bardziej zgodny z&nbsp;tekstem manuskryptów znad Morza Martwego, a&nbsp;w co najmniej dwóch przypadkach, być może z&nbsp;tekstem dokumentów dziś już nieznanych.</p>
<p>Jest to o&nbsp;tyle ważne, że z&nbsp;Septuaginty korzystali autorzy Nowego Przymierza. (3)&nbsp;Konsekwentne przekładanie tekstu masoreckiego czasem niepotrzebnie wprowadza do Pisma Świętego Starego i&nbsp;Nowego Przymierza rozbieżności znaczeniowe, co jest szczególnie ważne w&nbsp;przypadku cytatów tego tekstu w&nbsp;Nowym Przymierzu.</p>
<p>Przekład dwudziestu siedmiu ksiąg Nowego Przymierza jest również eklektyczny, podobnie jak tekst podany w&nbsp;NA27, NA28, w&nbsp;Tekście Przyjętym czy Większościowym, lecz odzwierciedla inne niż w&nbsp;przypadku tych dzieł kryteria doboru tekstu głównego – wyżej podane.</p>
<p>Ogólnie rzecz biorąc, kryteria rządzące doborem tekstu głównego mogą kształtować się od najprostszych, na przykład: tekst główny jest przekładem takiego a&nbsp;takiego manuskryptu, do bardziej złożonych, na przykład dwunastu, przyjętych przez biblistów stojących za tekstem greckim NA27 i&nbsp;NA28 i&nbsp;omówionych w&nbsp;publikacji The Text of the New Testament (Aland K. i&nbsp;B. 1995. The Text of the New Testament. An Introduction to the Critical Editions and to the Theory and Practice of Moder Textual Criticism. Grand Rapids: Eerdmans, 280-281) oraz na stronach wstępnych NA28.</p>
<p>Cokolwiek upraszczając, da się stwierdzić, że kryteria, których wyrazem są przekłady Pisma Świętego ostatnich dziesięcioleci, można sprowadzić do pięciu, z&nbsp;którymi wiąże się cechę obiektywności: (1)&nbsp;wiek wiariantu: im starszy, tym lepszy (lectio ante potior, la); (2)&nbsp;język wariantu: hebrajski i&nbsp;aramejski w&nbsp;przypadku pism Starego Przymierza, grecki w&nbsp;przypadku pism Nowego Przymierza (lingua originali potior, lo); (3)&nbsp;długość wariantu: im krótszy, tym lepszy (lectio brevior potior, lb); (4)&nbsp;zrozumiałość wariantu: im trudniejszy, tym lepszy (lectio difficilior potior, ld); (5)&nbsp;wyjaśnialność wariantu: za lepszy może uznany być ten, który da się bardziej przekonywująco wyjaśnić przez inne warianty, oczywiście przy zachowaniu zgodności z&nbsp;kontekstem i&nbsp;świadectwami zewnętrznymi (lectio facilius explicavit potior, le). Z&nbsp;wymienionych pięciu kryteriów tylko trzy pierwsze można by uznać za obiektywne, ale nawet ich nie można stosować – i&nbsp;w praktyce nie stosuje się – bez zastrzeżeń; (Liczne tego dowody w: Metzger B.M. 1994. A textual commentary on the Greek New Testament, second edition a&nbsp;companion volume to the United Bible Societies’ Greek New Testament (4th rev. ed.). London, New York: United Bible Societies) zatem i&nbsp;one są cokolwiek subiektywne.</p>
<p>Kryteria te skutkują tekstem trudniejszym do zrozumienia. Nie stanowi to większego problemu dla biblistów i&nbsp;wszystkich, którym właśnie o&nbsp;to chodzi. Utrudnia natomiast zrozumienie tekstu Czytelnikom mniej obeznanym z&nbsp;przedmiotem. Skoro bardziej zainteresowani spośród nich i&nbsp;tak, w&nbsp;poszukiwaniu zrozumienia, sięgają do przypisów i&nbsp;komentarzy, dlaczego nie uławić życia im i&nbsp;wszystkim innym, podając w&nbsp;tekście głównym tekst jaśniejszy, a&nbsp;w przypisach kwestie trudniejsze – dla tych, którzy gustują w&nbsp;rzeczach zawiłych i&nbsp;z zasady odwołują się do przypisów i&nbsp;komentarzy? Takie ułatwienie uznano za zasadne i&nbsp;dlatego niniejszy przekład, ze swoimi kryteriami i&nbsp;względami, jest jego wyrazem.</p>
<p>Kilku uwag wymaga także kryterium językowe (kj), to jest udzielenie pierwszeństwa, w&nbsp;przypadku Starego Przymierza, pismom oryginalnie hebrajskim, z&nbsp;fragmentami aramejskimi, należącym do hebrajskiego Tanach. Kryterium to z&nbsp;jednej strony nie budzi zastrzeżeń – tłumaczy się z&nbsp;tekstów oryginalnych, a&nbsp;nie z&nbsp;ich przekładów. Z&nbsp;drugiej jednak strony cieniem rzuca się na nie fakt, że autorzy Nowego Przymierza w&nbsp;większości przypadków cytowali z&nbsp;Septuaginty, poświadczając tym samym, że ich „oryginałem” był tekst grecki, nie hebrajski. W&nbsp;niniejszym przekładzie tekst Septuaginty jest poważnie wzięty pod uwagę, choć przyszłość może pokazać, że wciąż w&nbsp;zbyt małym zakresie.</p>
<p>Przy tej okazji warto dotknąć sprawy składu, lub kanonu, Starego Przymierza. Pismo Święte nigdzie bezpośrednio nie podaje składu własnych ksiąg. Choćby za wytyczną uznać słowa z&nbsp;[[Mt&nbsp;23:35>>Mt 23:35]] ([[Łk&nbsp;11:51>>Lk 11:51]]), wskazujące na Tanach, to i&nbsp;tak samo przywiązanie autorów Nowego Przymierza do Septuaginty i&nbsp;ich obeznanie z&nbsp;księgami niekanonicznymi przemawiają za tym, że sprawa składu ksiąg Starego Przymierza była dla nich kwestią otwartą na tyle, że współczesny czytelnik powinien również być jak najlepiej zaznajomiony z&nbsp;Septuagintą, z&nbsp;księgami niekanonicznymi (zob. np. [[2Tm&nbsp;2:19>>2Tim 2:19]]; [[3:8>>2Tim 3:8]]; [[Hbr&nbsp;11:35>>Heb 11:35]]) oraz, ogólnie, otwarty na kulturę (zob. np. [[Prz&nbsp;22:17>>Pr 22:17]]; [[Dz 17:28>>Ac 17:28]]). Przemawia za tym dodatkowo historia kształtowania się kanonu. W&nbsp;Nowym Przymierzu istnieją wyraźnie przesłanki, by objawienie się Boga w&nbsp;Chrystusie Jezusie traktować jako najważniejsze, jako klucz do zrozumienia Starego Przymierza. By się o&nbsp;tym przekonać, wystarczy odwołać się choćby do takich fragmentów jak: [[J 1:18>>Jn 1:18]]; [[14:5-17>>Jn 14:5-17]]; [[1Kor 15:1-15>>1Cor 15:1-15]] i&nbsp;[[Hbr 1:2>>Heb 1:2]]. W&nbsp;związku z&nbsp;tym, w&nbsp;tych przypadkach, gdzie miało to znaczenie, trzymano się zasady wyjaśniania Starego Przymierza Nowym (wp). Jednym z&nbsp;tego przykładów jest przetłumaczenie [[Ha 2:4>>Hab 2:4]] zgodnie z&nbsp;myślą apostoła Pawła zawartą w&nbsp;Liście do Rzymian ([[Rz 1:16-17>>Ro 1:16-17]]), tj. sprawiedliwy z&nbsp;wiary żyć będzie, a&nbsp;nie zgodnie z&nbsp;myślą, powiedzmy, Peszeru do Księgi Habakuka (Nitzan, B. 1986. Megillat Pesher Habakuk, Jerusalem: Mosad Bialik), tj. sprawiedliwy z&nbsp;wierności żyć będzie, choć takie tłumaczenie też byłoby poprawne i&nbsp;uzasadnione.</p>
<p>&nbsp;</p>

<h2 style=""text-align: left;"">Część 3:<br/>Cechy szczególne przekładu</h2>
<p>&nbsp;</p>

<h3>Przekład dosłowny – jak to rozumieć?</h3>
<p>Przekład dosłowny w&nbsp;odniesieniu do niniejszej publikacji należy rozumieć jako dzieło wyrażające troskę o&nbsp;to, by tam, gdzie to możliwe, jedno słowo w&nbsp;języku źródłowym tłumaczyć jednym słowem w&nbsp;języku polskim, przy zachowaniu odpowiedniości części mowy oraz uwzględnieniu zasad składni.</p>
<p>W powyższym zdaniu warto skomentować zastrzeżenie: tam, gdzie to możliwe, bo nie wszędzie jest to możliwe. Na przykład, nie da się przetłumaczyć determinatywów języka hebrajskiego ani rodzajników języka greckiego. W&nbsp;języku polskim nie mają one odpowiednika. Czasem obecność tej części mowy w&nbsp;tekście wyjściowym można zaznaczyć w&nbsp;inny sposób w&nbsp;przekładzie, ale nie zawsze jest to możliwe bez skutków dla znaczenia.</p>
<p>Poza tym istnieją przypadki, w&nbsp;których trzymanie się podanej zasady równoważności mogłoby razić lub wprowadzać w&nbsp;błąd. Dotyczy to na przykład terminów lub form zaliczanych w&nbsp;języku polskim do przestarzałych. Choćby były one przydatne, jak na przykład czas zaprzeszły czy imiesłowy uprzednie, w&nbsp;niektórych przypadkach sprawiałyby wrażenie zbyt staroświeckich, a&nbsp;nawet mogłyby zostać niewłaściwie zrozumiane.</p>
<p>W końcu, w&nbsp;kontekście rozważanego zastrzeżenia, należy powiedzieć o&nbsp;wprowadzaniu do przekładu słów nie mających odpowiednika w&nbsp;tekście źródłowym. Bywa to czasem konieczne. W&nbsp;tych przypadkach koniecznych starano się to wprowadzenie zaznaczać, ujmując dodane słowa w&nbsp;nawiasy kwadratowe. Jednak dla uniknięcia rażąco częstego wprowadzania ich do tekstu nie czyniono tego w&nbsp;każdym przypadku. Nie stosowano zatem nawiasów kwadratowych w&nbsp;przypadku metonimii. W&nbsp;ramach tej figury stylistycznej jednym i&nbsp;tym samym słowem określona bywa cała sfera pojęć należących do określanej dziedziny. Grzechem, na przykład, nazwana jest też ofiara za grzech. Ściśle rzecz biorąc, należałoby więc w&nbsp;nawiasach kwadratowych umieszczać słowa: ofiara za. Język hebrajski pełen jest też zdań nominalnych. Chcąc być skrupulatnym, należałoby w&nbsp;nawiasach kwadratowych umieszczać każdy wymagany w&nbsp;języku polskim i&nbsp;warunkowany kontekstem czasownik. Nawiasów zatem mogłoby być „niepokojąco” wiele.</p>
<p>Inną, choć związaną z&nbsp;nawiasami sprawą, są przypadki, w&nbsp;których dla jednego słowa w&nbsp;języku wyjściowym brak odpowiadającego mu jednego słowa w&nbsp;języku polskim. Wybór, które z&nbsp;dodatkowych słów umieścić w&nbsp;nawiasie kwadratowym, byłby czysto umowny. Można by w&nbsp;takich przypadkach wszystkie słowa w&nbsp;iązać łącznikami, ale na gruncie polskim to również mogłoby razić (W języku angielskim istnieje w&nbsp;tym względzie większa tolerancja, zob. The Amplified Bible, 1965, Michigan: Zondervan, np. long-promised ([[Ef&nbsp;1:13>>Eph 1:13]]), native-born ([[Ef&nbsp;5:8>>Eph 5:8]])), lub – co poważniejsze – czynić przekład mniej czytelnym.</p>
<p>Ostatnim szczególnym zagadnieniem, w&nbsp;obrębie którego stosowanie nawiasów kwadratowych byłoby sporne, są idiomy. Tłumaczeniu idiomów poświęcony jest jednak odrębny wywód o&nbsp;tym tytule w&nbsp;czwartej części Wstępu.</p> 
<p>Na koniec należy podkreślić, że przekład dosłowny nie jest przekładem interlinearnym, w&nbsp;którym słowa w&nbsp;języku wyjściowym, umieszczonym w&nbsp;jednej linii, podpisane są słowami w&nbsp;języku polskim, umieszczonym w&nbsp;następnej linii. Takie przekłady nie są w&nbsp;stanie spełnić wymogów składni i&nbsp;powinny być raczej uznawane za, skądinąd przydatne, uproszczone słowniki, a&nbsp;nie przekłady właściwe, to znaczy takie, które poza leksykonem uwzględniają również kontekst i&nbsp;składnię.</p>
<p>&nbsp;</p>

<h3>Przekład dosłowny a&nbsp;przekład literacki</h3>
<p>Scharakteryzowany wyżej przekład dosłowny tym różni się od przekładu literackiego zaproponowanego przez EIB, że  w&nbsp;przypadku tego drugiego w&nbsp;procesie tłumaczenia starano się język greckiego, hebrajskiego i&nbsp;aramejskiego oryginału w&nbsp;większym stopniu podporządkować wymogom języka polskiego. Na przykład tylko w&nbsp;wyjątkowych przypadkach, jak [[Rz&nbsp;1:18>>Ro 1:18]] czy [[1J&nbsp;5:16>>1Jn 5:16]], zastosowano nawiasy kwadratowe dla zaznaczenia dodania słów nie występujących w&nbsp;tekście oryginalnym. Ściślej też rugowano formy przestarzałe i&nbsp;książkowe. Nie zawsze, choć bez jakiegokolwiek zmieniania znaczenia tekstu, trzymano się zasady tłumaczenia jednego słowa w&nbsp;tekście wyjściowym jednym słowem w&nbsp;języku polskim. Różnica między oboma przekładami jest zatem raczej ilościowa niż jakościowa.</p>
<p>&nbsp;</p>

<h3>Dosłowność, literackość a&nbsp;wierność przekładu</h3>
<p>Przy rozważaniach nad dosłownością i&nbsp;literackością może pojawić się pytanie o&nbsp;wierność przekładu, to znaczy: który z&nbsp;przekładów jest wierniejszy, dosłowny czy literacki? Odpowiedź na to pytanie zależy od przedmiotu wierności. Jeśli za przedmiot wierności uznać na przykład zachowanie odpowiedniości w&nbsp;sferze części mowy, to przekład dosłowny wypadnie jako wierniejszy. Ale jeśli przedmiotem wierności uczynić odpowiedniość znaczenia formułowanych w&nbsp;obu językach myśli, odpowiedniość reakcji na takie ich sformułowanie lub odpowiedniość w&nbsp;odniesieniu do ich idiomatyczności, to wierniejszym może okazać się odpowiednio przekład literacki, dynamiczny lub idiomatyczny (Rozliczne definicje przekładów, zob. MEP; Beekman 1974). Właśnie dlatego ten, komu zależy na dogłębnym zrozumieniu Pisma Świętego, a&nbsp;nie jest w&nbsp;stanie uczyć się języków biblijnych, powinien korzystać ze wszystkich przekładów, mając na uwadze swoistość każdego z&nbsp;nich.</p>
<p>Jeśli chodzi o&nbsp;wymienione wyżej przekłady EIB, to oba są wierne w&nbsp;obrębie własnych cech. Literackość przekładu EIB odnosi się głównie: (1)&nbsp;do realiów życia codziennego; (2)&nbsp;do narracji historycznych i&nbsp;geograficznych (jak na przykład w&nbsp;Dziejach Apostolskich); (3)&nbsp;do stosowania zaimków zamiast powtarzania tych samych słów lub do wtrącania imion autorów wypowiedzi tam, gdzie brak tego mógłby zamazywać znaczenie tekstu; (4)&nbsp;do przekładu idiomów (choć kulturowe ich brzmienia zostały podane w&nbsp;przypisach); (5)&nbsp;do podziału czasem bardzo długich i&nbsp;trudnych do zrozumienia zdań na krótsze, bez szkody dla sensu wypowiedzi. </p>
<p>&nbsp;</p>

<h3>Dosłowność a&nbsp;konsekwencja w&nbsp;przekładzie tych samych słów</h3>
<p>Wydawałoby się, że w&nbsp;przekładzie dosłownym słowa języka wyjściowego powinny być tłumaczone na język polski zawsze w&nbsp;ten sam sposób. Taka praktyka byłaby jednak błędna – i&nbsp;słusznie jest za taką uważana. Powszechnie wiadomo, że w&nbsp;każdym przypadku tłumaczeniem słowa rządzi kontekst. Te same słowa w&nbsp;różnym kontekście mogą mieć różne znaczenie. Przykładów tego można by podać setki. Być może najbardziej wymownym jest greckie słowo pistis, które w&nbsp;zależności od kontekstu może oznaczać wiarę, ufność i&nbsp;wierność. </p> 
<p>&nbsp;</p>

<h3>Oznajmujący charakter etyki</h3>
<p>Daje się zauważyć, że zalecenia etyczne Nowego Przymierza mają charakter oznajmujący w&nbsp;przeciwieństwie do rozkazującego. Wskazuje się nam na to, co powinno leżeć w&nbsp;granicach naszych możliwości, co możemy jako ludzie w&nbsp;Chrystusie. Daje to o&nbsp;sobie znać szczególnie w&nbsp;pismach apostoła Pawła, ale nie tylko. Czynią to również Ewangeliści, akcentując, że Pan Jezus wzywa swych uczniów do postępowania szczególnego, niepospolitego, będącego rezultatem wolnej decyzji. Ten oznajmujący, zachęcający, wręcz estetyczny w&nbsp;ymiar z&nbsp;aleceń etycznych starano się oddawać słowami, które na taki wymiar wskazują, np. przymiotnikami: piękny, szlachetny, wspaniały zamiast dobry, który w&nbsp;języku polskim częściej wyraża zgodność z&nbsp;normą prawną.</p> 
<p>Z&nbsp;oznajmującym wymiarem etyki łączy się słownictwo apostoła Pawła wyrażające wymiar rzeczywistości w&nbsp;Chrystusie. Słownictwo to tłumaczono bez wyjaśniających uproszczeń nawet w&nbsp;przekładzie literackim, założono bowiem, że po przeczytaniu wszystkich pism apostoła jego myśl stanie się dla Czytelnika jasna.</p> 
<p>W&nbsp;tym kontekście należy zaznaczyć, że rozróżniono wyrażenie wiara w&nbsp;Chrystusa (gr. pistis eis Christon) od wyrażenia wiara Chrystusa (gr. pistis Christou). To drugie tłumaczono jako zawierzenie lub zaufanie Chrystusowi. Takie tłumaczenie lepiej uwzględnia to, że wyrażenie wiara Jezusa może odnosić się do sfery przeżywania lub doświadczenia.</p>
<p>&nbsp;</p>

<h3>Trudniejsza terminologia</h3>
<p>Biorąc pod uwagę związek przekładu dosłownego z&nbsp;literackim oraz zakładając korzystanie przez Czytelników z&nbsp;obu, starano się, aby tam, gdzie ma to znaczenie dla szerszego spojrzenia na sprawę, w&nbsp;obu przekładach występowały różne określenia tych samych urzędów, ról, postaw i&nbsp;miejsc. W&nbsp;przekładzie dosłownym zarezerwowano miejsce dla określeń trudniejszych, takich na przykład jak: biskup, prezbiter, diakon, bojaźń, Abys, Gehenna, Hades, Szeol, Tartar. Określenia te są wyjaśnione, ale pełniejsze zapoznanie się z&nbsp;nimi wymaga sięgnięcia do leksykonów, słowników i&nbsp;podręczników. </p>
<p><span dir=""ltr"">Jeśli chodzi o&nbsp;termin Słowo, hbr.</span><span lang=""he"" dir=""rtl"">דָּברָ</span><span dir=""ltr""> (dawar), gr. </span><span lang=""el"">λόγος</span> (logos), tam, gdzie termin ten określa wolę Bożą wyrażoną w&nbsp;Prawie, obietnicy lub proroctwie, pisano go wielką literą; gr. <span lang=""el"">ῥῆμα</span> (rhema), mające nieraz także znaczenie słowa, i&nbsp;synonimiczne względem gr. <span lang=""el"">λόγος</span>, pisano małą literą.</p>
<p>&nbsp;</p>

<h2 style=""text-align: left;"">Część 4:<br/>Układ treści i&nbsp;szczegóły formalne</h2>
<p>&nbsp;</p>

<h3>Przypisy</h3>
<p>Przypisy stanowią ważną część niniejszej publikacji. Jest ich około dwudziestu tysięcy. Wartość tej publikacji łączy się w&nbsp;ogromnej mierze z&nbsp;przypisami,o&nbsp;których mówią również inne części niniejszego Wstępu. Treścią przypisów są:</p>
<p>1.&nbsp;Warianty tekstowe wraz z&nbsp;ich tłumaczeniem na język polski, chyba że charakter wariantu tekstowego nie wymagał takiego tłumaczenia, np. gdy przedmiotem wariantu był determinatyw lub synonim nie mający w&nbsp;języku polskim osobnego odpowiednika.</p>
<p>2.&nbsp;Inne możliwe tłumaczenia tych samych słów lub wyrażeń.</p>
<p>3.&nbsp;Wiadomości z&nbsp;dziedziny historii, kultury&nbsp;i języka, ze stosownymi omówieniami.</p>
<p>4.&nbsp;Uwagi mające znaczenie dla rozwoju życia w&nbsp;Chrystusie i&nbsp;dla pogłębienia więzi z&nbsp;Nim — szczególnie w&nbsp;Nowym Przymierzu.</p>
<p>W przypisach podawano wyraźnie tylko skróty nazw źródeł tekstu wyjściowego, źródłowego, np. BHS, PS, G. Innych not bibliograficznych nie podawano, lecz zaznaczono ich obecność za pomocą odsyłacza z&nbsp;indeksem „L”, np. [[J&nbsp;3:16L>>Jn 3:16]]. Odsyłacz z&nbsp;tym indeksem kieruje do odrębnej publikacji, zatytułowanej <i>Literatura i&nbsp;uwagi do wersetów Pisma Świętego</i>. Uwagi na jej temat zostały zawarte poniżej, w&nbsp;części Literatura pomocnicza przekładu.</p>
<p>Jeśli chodzio&nbsp;zakres odwoływania się do wariantów tekstowych wymienionych wyżej, w&nbsp;punkcie&nbsp;1, to nie jest on jeszcze identyczny w&nbsp;odniesieniu do poszczególnych ksiąg. Pozostaje to sprawą przyszłych wydań. Uwzględniono jednak niemal wszystkie warianty PS, a&nbsp;w&nbsp;przypadku Księgi Psalmów, Izajasza, Jeremiasza i&nbsp;Proroków Mniejszych niemal wszystkie warianty ZMM. W&nbsp;ogromnym, szczegółowo zaznaczonym zakresie uwzględniono warianty G, a&nbsp;jeśli nie uwzględniono ich w&nbsp;pełni, to tylko dlatego, że w&nbsp;przypadku niektórych ksiąg ich pełne uwzględnienie musiałoby poskutkować osobną publikacją.</p>
<p>Co do uwag mających znaczenie dla rozwoju życia w&nbsp;Chrystusie i&nbsp;dla pogłębienia więzi z&nbsp;Nim, wspomnianych powyżej, w&nbsp;punkcie&nbsp;4, starano się uwzględniać te, które mogą być wysnute z&nbsp;prostego znaczenia tekstu biblijnego. Zdawano sobie przy tym sprawę z&nbsp;płynności granicy między tym, co biblijne, a&nbsp;tym, co teologiczne lub wyznaniowe, oraz z&nbsp;nieostrości w&nbsp;kwestii tego, co można zaliczyć do spraw pogłębiających życie w&nbsp;Chrystusie, a&nbsp;co już nie.</p>
<p>&nbsp;</p>

<h3>Literatura pomocnicza przekładu</h3>
<p>Literaturę pomocniczą przekładu stanowią dwie części ściśle związane z&nbsp;częścią główną, zawierającą tekst tłumaczenia: Literatura i&nbsp;uwagi do wersetów Pisma Świętego w&nbsp;przekładzie EIB (LiU) oraz Literatura przekładu Biblii, to jest Pisma Świętego Starego i&nbsp;Nowego Przymierza. W&nbsp;zależności od sposobu wydania całego dzieła, mogą one być po prostu połączone w&nbsp;jeden moduł, jak w&nbsp;przypadku wydań cyfowych albo wydawane jako odrębny tom lub broszury.</p>
<p>Pierwsza z&nbsp;wymienionych publikacji spełnia dwa cele: (a)&nbsp;zmniejsza objętość przypisów pod tekstem biblijnym dzięki przeniesieniu do niej skróconych not bibliograficznych i&nbsp;bardziej szczegółowych uwag; (b)&nbsp;pozwala na częstsze uaktualnianie not bibliograficznych i&nbsp;uwag bez potrzeby równie częstego i&nbsp;nad wyraz skomplikowanego uaktualniania składu samego tekstu biblijnego.</p>
<p>Druga z&nbsp;wymienionych publikacji zawiera pełne noty bibliograficzne dzieł i&nbsp;artykułów użytych w&nbsp;czasie przekładu Pisma Świętego.</p>
<p>Obie publikacje będą się z&nbsp;biegiem czasu i&nbsp;postępem pracy nad tekstem Pisma Świętego rozrastały. Informacje o&nbsp;kolejnych wydaniach lub aktualizacjach obu wymienionych publikacji będą podawane do publicznej wiadomości zarówno przez EIB, jak i&nbsp;przez wszystkie współpracujące z&nbsp;tym podmiotem instytucje.</p>
<p>&nbsp;</p>

<h3>Skróty</h3>
<p>Pojawiające się w&nbsp;przypisach skróty zostały wyjaśnione w&nbsp;części Wykaz skrótów i&nbsp;oznaczeń. Skróty zastosowane w&nbsp;części Literatura i&nbsp;uwagi do wersetów Pisma Świętego w&nbsp;przekładzie EIB wyjaśnione są na końcu tejże części. Starano się, aby skróty nie odbiegały od form, które upowszechniły się dzięki wcześniejszym publikacjom z&nbsp;dziedziny biblistyki – również internetowym. Szczególnie zainteresowani z&nbsp;pewnością skorzystają, zaopatrując się w&nbsp;publikacje wyszczególnione w&nbsp;części Tekst źródłowy Nowego Przymierza i&nbsp;Tekst źródłowy Starego Przymierza, czy to w&nbsp;formie książkowej, czy cyfrowej.</p>
<p>&nbsp;</p>

<h3>Kursywa</h3>
<p>Kursywą zaznaczono:</p>
<p>1.&nbsp;W&nbsp;tekście głównym Nowego Przymierza cytaty ze Starego Przymierza; uczyniono to za NA27 i&nbsp;NA28.</p>
<p>2.&nbsp;W&nbsp;tekście głównym Starego Przymierza nietłumaczone wyrazy obcego pochodzenia, jak: Sela i&nbsp;pur.</p>
<p>3.&nbsp;W&nbsp;przypisach słowa cytowane z&nbsp;tekstu głównego oraz znaczenia słów i&nbsp;wyrażeń obcojęzycznych, o&nbsp;ile traktowane są one jako nazwy lub są tłumaczeniem treści wariantu.</p>
<p>&nbsp;</p>

<h3>Transkrypcja</h3>
<p>Pojawiające się w&nbsp;przypisach wyrażenia hebrajskie transkrybowano na język polski w&nbsp;sposób uproszczony, z&nbsp;wyjątkiem miejsc, w&nbsp;których transkrypcja pokrywa się z&nbsp;brzmieniem polskiego odpowiednika lub w&nbsp;których, ze względu na osoby zaznajomione z&nbsp;językiem hebrajskim i&nbsp;dla ich wygody, podano dłuższe cytaty z&nbsp;tekstu hebrajskiego.</p>
<p>Transkrypcja ta nie uwzględnia znaków akcentowych. Dla zaznaczenia obecności wszystkich spółgłosek hebrajskich w&nbsp;słowach transkrybowanych uwzględniono również alef i&nbsp;ain, odpowiednio znakami: (’) oraz (‘), które „czyta się” bez wydawania dźwięku, po prostu zwarciem głośni. Hebrajskie he w&nbsp;wygłosie (h), przy czytaniu pozostaje nieme.</p> 
<p>Wyrażenia greckie z&nbsp;reguły nie były transkrybowane na język polski, ponieważ litery greckie, przynajmniej częściowo, są powszechnie znane. Transkrybowano tylko te słowa, które w&nbsp;formie zapożyczeń używane są w&nbsp;języku polskim. Transkrypcja ta nie uwzględnia znaków akcentowych, ale przy czytaniu słów transkrybowanych akcent należy stawiać tam, gdzie znaki akcentowe umieszczone są nad literami słów greckich.</p>
<p>Wyrażenia pisane za pomocą znaków hebrajskich należy uważać za hebrajskie,o&nbsp;ile nie zaznaczono inaczej, np. „aram.” wskazuje na słowa pochodzenia aramejskiego. Podobnie wyrażenia pisane znakami greckimi należy uważać za greckie.</p>
<p>&nbsp;</p> 

<h3>Tetragram JHWH</h3>
<p>Występujące powszechnie w&nbsp;Starym Przymierzu imię własne Boga JHWH, oddawano w&nbsp;formie JHWH równoważnej określeniu PAN w&nbsp;przekładzie literackim EIB. Tetragram ten zwokalizowany brzmi: Jahwe. W&nbsp;czasie lektury Czytelnik powinien odczytywać Tetragram jako „Pan”, chyba że wyjątkowe względy narzucają odczyt „Jahwe”.</p>
<p>W cytowaniu niniejszej publikacji zaleca się też, ze względu na wrażliwość wyznaniową lub religijną niektórych społeczności ludzkich, zamienianie Tetragramu, jeśli występuje w&nbsp;cytacie, na „PAN”, szczególnie jeśli cytaty te mają być wystawiane na widok publiczny lub odczytywane w&nbsp;miejscach publicznych.</p>
<p>&nbsp;</p>

<h3>Onomastykon</h3>
<p>Co do imion i&nbsp;nazw własnych, w&nbsp;przekładzie dosłownym podobnie jak w&nbsp;literackim kierowano się zasadami podanymi w&nbsp;Onomastykonie Biblii Hebrajskiej i&nbsp;Nowego Testamentu autorstwa Krzysztofa Sielickiego (2010, Warszawa: Vocatio), jednak nie bez odstępstw uznanych za konieczne. Odstępstwa te dotyczą głównie: (1)&nbsp;imion teoforycznych; (2)&nbsp;imion i&nbsp;nazw własnych z&nbsp;ustaloną w&nbsp;języku polskim pisownią; (3)&nbsp;imion i&nbsp;nazw własnych trudniejszych do odnalezienia, w&nbsp;podanej w&nbsp;Onomastykonie formie, w&nbsp;literaturze anglojęzycznej oraz na stronach internetowych; (4)&nbsp;imion i&nbsp;nazw własnych, których podana w&nbsp;Onomastykonie forma rodzi w&nbsp;języku polskim na przykład humorystyczne skojarzenia.</p>
<p>&nbsp;</p>

<h3>Idiomy</h3>
<p>Do idiomów zaliczono słowa, zwroty i&nbsp;wyrażenia charakterystyczne tylko dla hebrajskiego i&nbsp;greckiego sposobu opisywania rzeczywistości, ale także antropomorfizmy, personifikacje przedmiotów nieożywionych oraz zwroty możliwe do zaklasyfikowania w&nbsp;kategorii wyrażeń symbolicznych lub metafor. Umieszczano je w&nbsp;przypisach, tłumacząc w&nbsp;tekście głównym ich znaczenie, ale też – odwrotnie – umieszczano w&nbsp;tekście głównym, podając w&nbsp;przypisach ich znaczenie. O wyborze miejsca umieszczenia idiomu decydował w&nbsp;każdym przypadku kontekst, względy poznawcze lub takie, które mieszczą się w&nbsp;kategoriach literackiego smaku lub estetyki.</p>
<p>W kwestii terminów duch, dusza, serce, nerki i&nbsp;wątroba, tam, gdzie słowa te opisują niematerialną sferę życia człowieka, pierwsze trzy umieszczano w&nbsp;tekście głównym, natomiast występowanie dwóch pozostałych zaznaczano w&nbsp;przypisach, w&nbsp;kategorii idiomów.</p>
<p>&nbsp;</p>

<h3>Tabele informacyjne na początku ksiąg</h3>
<p>Na początku każdej księgi zamieszczono tabele informacyjne. Każda składa się z&nbsp;pięciu wierszy: autor, czas, miejsce, cel i&nbsp;temat. Zwięzłość zawartych w&nbsp;nich informacji ułatwia szybkie umiejscowienie każdej z&nbsp;ksiąg w&nbsp;perspektywie dziejowej, jednak stopień ogólności tych informacji wymaga kilku ważnych komentarzy:</p>
<p>1.&nbsp;W&nbsp;przypadku ksiąg Nowego Przymierza stosunkowo nietrudno podać autora księgi oraz czas i&nbsp;miejsce jej napisania. W&nbsp;przypadku ksiąg Starego Przymierza rzecz ma się inaczej. W&nbsp;większości przypadków autor jest nieznany. Łączone z&nbsp;księgami imiona nie muszą oznaczać autorstwa. Trudny do zidentyfikowania wkład redakcyjny na przestrzeni dziejów, czyni te księgi czymś w&nbsp;rodzaju dzieł zbiorowych. W&nbsp;wierszu Czas w&nbsp;wielu przypadkach odróżniono czas opisywanych wydarzeń od niełatwego do uchwycenia okresu lub okresów redakcji księgi. W&nbsp;wierszu Miejsce chodzio&nbsp;miejsce akcji, chyba że wskazano inaczej.</p>
<p>2.&nbsp;W&nbsp;przypadku ksiąg Starego Przymierza tam, gdzie istniały dwie opcje datowania, trzymano się jednej, uznanej za bardziej spójną z&nbsp;tekstem biblijnym, bez rozważania opcji drugiej. Uznano, że lepszym miejscem dla takich, w&nbsp;większości ciekawych, rozważań są komentarze lub publikacje wyraźnie temu poświęcone.</p>
<p>3. Poczucie niepewności związane z&nbsp;informacjami zawartymi w&nbsp;tabelach nie powinno podważać wiarygodności samych ksiąg. Byłoby to niedorzecznością. Księgi te nie powstały, by zaspokajać współczesne normy. Z&nbsp;własnymi normami, nie przestają one być dla nas pożyteczne i&nbsp;w twórczy sposób wyzywające.</p>
<p>&nbsp;</p> 

<h3>Przyszłe wydania SNPD</h3>
<p>Pierwszą troską przy tworzeniu „dwuprzekładu”, czyli SNP i&nbsp;niniejszego SNPD, była publikacja tych dzieł jako obejmujących wszystkie księgi protokanoniczne. W&nbsp;tym procesie SNPD okazało się przedsięwzięciem dużo bardziej pracochłonnym, przedsięwzięciem, które mogłoby przerodzić się w&nbsp;niekończącą się historię. Zostaje ono przekazane do druku jako kompletne, ale jednak w&nbsp;taki sposób, że osoby z&nbsp;wyczulonym słuchem już słyszą wołanieo&nbsp;następne wydanie. Jeśli do takiego wydania dojdzie, to powinno ono przynieść, między innymi:</p>
<p>1.&nbsp;pełniejsze opracowanie wariantów tekstowych;</p>
<p>2.&nbsp;pełniejsze wyjaśnienia miejsc trudnych i&nbsp;zagadkowych;</p>
<p>3.&nbsp;pełniejsze ukazanie różnic między tekstem hebrajskim a&nbsp;greckim tekstem Septuaginty, jeśli chodzio&nbsp;części prorockie;</p>
<p>4.&nbsp;pełniejsze odniesienie do literatury mogącej przyczynić się do lepszego zrozumienia przesłania Pisma Świętego;</p> 
<p>5.&nbsp;przekład ksiąg deuterokanonicznych.</p>
<p>&nbsp;</p>

<h3>Popularyzatorski walor SNPD</h3>
<p>Wydawany od 2016&nbsp;r. przekład literacki SNP posłużył za podstawę wydań wymienionych w&nbsp;końcowej części Literatury do przekładu Biblii, to jest: trzech wydań Pisma Świętego dla dzieci, czterech wydań dla młodzieży, jednego wydania dla grupy zawodowej (kierowców), dwóch modułów do programów naukowych (Accordance i&nbsp;BibleWorks), czterech modułów do programów popularnonaukowych, dwóch nagrań dźwiękowych (Nowe Przymierze oraz Księga Psalmów) i&nbsp;stał się pierwszym rekomendowanym przekładem polskim w&nbsp;ramach międzynarodowej platformy YouVersion. Wydania te wzbudziły u&nbsp;użytkowników zapotrzebowanie, któremu doskonale wychodzi naprzeciw SNPD, które, jak wynika z&nbsp;korespondencji tłumacza, już służy pomocą studentom mierzącym się z&nbsp;wyzwaniami translatoryki biblijnej. Nadzieją Wydawców pozostaje to, że ich dzieło z&nbsp;każdym kolejnym wydaniem coraz wyraźniej włączać się będzie w&nbsp;nurt zleconego kościołowi Wielkiego Posłannictwa.</p>
<p>&nbsp;</p> 

<h3>Podziękowania</h3>
<p>Naturalną rzeczą są wyrazy uznania i&nbsp;wdzięczności, choć jest oczywiste, że te najważniejsze pozostają w&nbsp;sferze rzeczywistości nadchodzącej.</p>
<p>Jesteśmy wdzięczni wszystkim – małym i&nbsp;wielkim – za wszelkie fachowe, duchowe i&nbsp;materialne wsparcie – małe i&nbsp;wielkie. Nie sposób wymieniać na tych stronicach setek osób w&nbsp;różnym stopniu zaangażowanych, lecz tak samo ważnych. Ich imiona i&nbsp;nazwiska zamieścimy w&nbsp;osobnej publikacji.</p>
<p style=""text-align: right"">Piotr Zaremba</p>
<p style=""text-align: right"">Ewangeliczny Instytut Biblijny</p>
<p>&nbsp;</p> 
<p>&nbsp;</p> 

<h2 style=""text-align: center;"">Jak korzystać z&nbsp;przekładu dosłownego</h2>
<h3>Uwagi ogólne</h3>
<p>1.&nbsp;Każdy korzystający z&nbsp;niniejszego dzieła uczyni właściwie, jeśli zapozna się z&nbsp;całym Wstępem.</p>
<p>2.&nbsp;Biorąc do ręki jedną księgę, Biblię, to jest Pismo Święte Starego i&nbsp;Nowego Przymierza, Czytelnik ma przed sobą dzieło, którego tekst nie pochodzi z&nbsp;jednej księgi, ale z&nbsp;kilku tysięcy manuskryptów pieczołowicie przygotowanych, ocenionych i&nbsp;podanych do przetłumaczenia – trzyma zatem całą bibliotekę dokumentów sprzed setek i&nbsp;tysięcy lat. Dokumenty te różnią się między sobą, a&nbsp;różnice te zostały odnotowane w&nbsp;przypisach, z&nbsp;którymi warto się zapoznawać i&nbsp;które stanowią o&nbsp;wartości niniejszego dzieła.</p>
<p>3.&nbsp;Żaden przekład nie zastąpi oryginału, a&nbsp;Biblia, to jest Pismo Święte Starego i&nbsp;Nowego Przymierza jest tylko przekładem. Zaletą tego przekładu jest to, że zaznajamia osoby nie znające języków biblijnych z&nbsp;treścią dokumentów oryginalnych, ale wadą między innymi to, że język tego przekładu, dobieranyz taką starannością, wprowadza w&nbsp;pewne złudzenie. Otóż Czytelnikowi może wydawać się, że język oryginału, podobnie jak język przekładu, też pochodzi z&nbsp;jednego, dość krótkiego okresu. Tak jednak nie jest. Księgi biblioteki zwanej Biblią pochodzą z&nbsp;różnego okresu, powstawały na przestrzeni kilkunastu wieków i&nbsp;gdyby chcieć dać temu wyraz w&nbsp;tłumaczeniu, niektóre części musiałyby być przełożone np. polszczyzną Mikołaja Reja, a&nbsp;niektóre polszczyzną ostatnich lat. Warto zatem zapoznawać się z&nbsp;tabelami informacyjnymi zamieszczonymi na początku każdej księgi, podającymi, w&nbsp;pewnym przybliżeniu, daty ich powstania. </p>
<p>4.&nbsp;Mnogość ksiąg i&nbsp;wariantów tekstowych, różny okres ich powstawania i&nbsp;redakcji, mozaika kultur, wpływów, języków i&nbsp;okoliczności, w&nbsp;kontekście których powstały, powinna uświadamiać Czytelnikowi z&nbsp;całą wyrazistością, że Pisma Święte nie są systematycznym wykładem teologii, historii, kosmogonii czy eschatologii, lecz zbiorem pism pochodzących od wielu autorów, napisanych w&nbsp;różnym stylu i&nbsp;celu. Na podstawie tego zbioru próbuje się co najwyżej tworzyć różne „systematyki”, obciążone skądinąd bagażem doświadczeń własnych twórców i&nbsp;zbyt pochopnie brane za równoznaczne ze Słowem Bożym. Pisma Święte jako zbiór, na obecne czasy taki a&nbsp;nie inny, świadczy sam o&nbsp;sobie rzetelniej niż głoszone na jego temat poglądy, wypływające bardziej z&nbsp;przesłanek ideologicznych niż egzegetycznych. W&nbsp;kontekście spostrzeżeń wyrażonych w&nbsp;tym punkcie Czytelnik musi pamiętać, że powiedzenie „Pismo tłumaczy Pismo” należy przyjmować z&nbsp;bardzo daleko posuniętą ostrożnością, uwzględniającą całą złożoność historyczną, kulturową i&nbsp;językową.</p>
<p>5.&nbsp;Przy czytaniu Pisma Świętego należy brać pod uwagę, że podział tekstu biblijnego na rozdziały i&nbsp;wersety jest sprawą wtórną. Sprawą wtórną jest też podział rozdziałów na części tematyczne. Dziełem tłumaczy lub wydawców są tytuły tych części. Ponadto w&nbsp;tekście hebrajskim rzeczą wtórną, dziełem masoretów, są samogłoski i&nbsp;znaki akcentowe. Prace nad tą tzw. wokalizacją ukończono dopiero w&nbsp;XI w. Co do interpunkcji, sprawa jest bardziej złożona. W&nbsp;tekście hebrajskim nie ma znaków przestankowych jako takich. Ich rolę jednak spełniają: składnia, zaimki pytajne, określenia stałe, końcowe formy niektórych liter, a&nbsp;w poezji paralelizm członów. Ogromną pomocą, choć jednocześnie elementem interpretacyjnym, jest w&nbsp;tym względzie późniejsza wokalizacja. W&nbsp;tekście greckim rzecz przedstawia się podobnie, z&nbsp;tą różnicą, że język grecki posiada samogłoski i&nbsp;nigdy nie miał swoich „masoretów”.</p>
<p>6.&nbsp;Wielu, jeśli nie większość, szuka w&nbsp;Pismach Świętych recept na własne życie. I&nbsp;słusznie. Ci, którzy to czynią, powinni jednak pamiętać, że ogólne zasady to nie to samo, co szczególne przypadki, z&nbsp;których te zasady można dopiero wyłuskiwać.</p>
<p>7.&nbsp;Z lekturą Pism Świętych jest jak z&nbsp;lekturą książki kucharskiej. Czytanie przepisów nie syci. Sycą posiłki przygotowane według przepisów. Kto zechce „przyrządzić” swe życie według przepisów ewangelii, dla tego Jezus Chrystus stanie się satysfakcją na wieki.</p>


<h3>Uwagi szczególne</h3>
<p>1.&nbsp;Kto chce uskutecznić w&nbsp;swoim życiu cel, dla którego, za sprawą Boga, doszło do powstania Pisma Świętego, powinien uczynić jego lekturę częścią swoich spotkań z&nbsp;Bogiem, wyrażając swoją otwartość do dialogu z&nbsp;Nim choćby słowami: Boże, pozwól mi zrozumieć to, co czytam, a&nbsp;to, co zrozumiem, wprowadzę w&nbsp;życie.</p>
<p>2.&nbsp;Lekturę przekładu dosłownego można rozpocząć w&nbsp;dowolny sposób, na przykład: (a)&nbsp;możemy zaglądać do przekładu dosłownego, gdy zastanowi nas coś przy lekturze przekładu literackiego lub przy lekturze jakiegokolwiek innego przekładu czy publikacji. Innymi słowy, możemy odwoływać się do przekładu dosłownego niczym do słownika lub przewodnika. (b)&nbsp;Możemy czytać przekład dosłowny w&nbsp;sposób systematyczny, księga po księdze, rozdział po rozdziale; w&nbsp;takim przypadku uważniejsza lektura całości może nam zająć od trzech do pięciu lat. Nie musimy przy tym czytać „od deski do deski”. Jednego dnia możemy czytać rozdział z&nbsp;Prawa, drugiego dnia fragment Proroctw lub Pism, trzeciego dnia urywek Nowego Przymierza. Sposobów jest tyle, ilu jest Czytelników.</p>
<p>3.&nbsp;Zawsze należy mieć w&nbsp;świadomości to, co jest treścią tabeli zamieszczonej na początku każdej księgi. Ta perspektywa pomaga umieścić daną księgę wraz z&nbsp;tym, co stanowi jej treść, w&nbsp;czasie – po to, by różnice czasowe, a&nbsp;przez to językowe i&nbsp;kulturowe, brać pod uwagę przy kojarzeniu różnych fragmentów i&nbsp;wyłuskiwaniu z&nbsp;nich zasad ogólniejszych.</p>
<p>4.&nbsp;Należy zapoznawać się z&nbsp;przypisami. Wszystkie są ważne. Te, które informują o&nbsp;wariantach tekstowych, ukazują bezpośrednio: (a)&nbsp;który wariant został wybrany do tekstu głównego, z&nbsp;podaniem miejsca, w&nbsp;którym został zaświadczony i&nbsp;wieku świadectwa tekstowego, np. 𝔓46 (200) oznacza, że wariant został zaświadczony w&nbsp;papirusie o&nbsp;numerze 46 pochodzącym z&nbsp;200 r. po Chr.; (b)&nbsp;dodatkowe świadectwa tekstowe; (c)&nbsp;inne istniejące warianty, z&nbsp;identyfikacją miejsca i&nbsp;ich wiekiem; (d)&nbsp;powód wyboru danego wariantu, np. kw wl oznacza, że został on wybrany z&nbsp;uwagi na kryterium większego wglądu w&nbsp;złożoność tekstu biblijnego i&nbsp;ze względów logicznych. Przypisy informujące o&nbsp;wariantach tekstowych zawierają odsyłacze z&nbsp;indeksem „L”. Odsyła on do części Literatura i&nbsp;uwagi do wersetów Pisma Świętego. Zawarte w&nbsp;tej części informacje pozwalają nam wnioskować o&nbsp;takich szczegółach, jak: (a)&nbsp;liczba świadectw tekstowych danej księgi; (b)&nbsp;stan zachowanego tekstu; (c)&nbsp;stopień zróżnicowania tekstu; (d)&nbsp;popularność tekstu wśród jego użytkowników; (e)&nbsp;prawdopodobne brzmienie najwcześniejszej zachowanej wersji; (f)&nbsp;zakres i&nbsp;przybliżoną liczbę jego redakcji; (g)&nbsp;możliwe znaczenia tekstu, a&nbsp;w&nbsp;konsekwencji jego przydatność do formułowania jednoznacznych wniosków lub zasad.</p>
<p>Przypisy, które informują o&nbsp;innych możliwych znaczeniach tekstu w&nbsp;obrębie jednego wariantu, pogłębiają kryjące się w&nbsp;nim bogactwo treści, a&nbsp;przez to chronią przed nieuzasadnioną jednostronnością w&nbsp;jego rozumieniu. Nie do przecenienia są przytaczane warianty Septuaginty (G). Ukazują one m.in.: (a)&nbsp;jak rozumiano tekst hebrajski w&nbsp;czasie, gdy go tłumaczono; (b)&nbsp;jak my współcześnie moglibyśmy rozumieć tekst hebrajski tam, gdzie współczesny jego odczyt różni się od odczytu tłumaczy starożytnych; (c)&nbsp;jak mógł brzmieć tekst hebrajski innych, zaginionych lub dotąd nie odkrytych źródeł. Informacja o&nbsp;wariantach tekstowych i&nbsp;ich znaczeniu jest bardzo ważna. Nie zawsze bowiem jasno dostrzegany jest fakt, że oryginały ksiąg biblijnych w&nbsp;sensie pierwotnych dokumentów, zwanych autografami, nie zachowały się do naszych czasów. Możemy wprawdzie wiele na ich temat powiedzieć, są jednak przypadki, w&nbsp;których szczegóły ich treści i&nbsp;kompozycji pozostają sprawą niejasną. Budowanie na niecałkowicie pewnych fundamentach całkowicie pewnych systemów jest zatem co najmniej ryzykowne. Przypisy podające wiadomości z&nbsp;dziedziny historii, kultury i&nbsp;języka są wręcz kopalnią wniosków. Na przykład różnice w&nbsp;datowaniu mogą zdradzać różne sposoby liczenia lat w&nbsp;zależności od kultury i&nbsp;jej kalendarza, stopień zainteresowania historią oraz jej funkcję w&nbsp;społeczeństwie.</p>
<p>Przypisy podające znaczenia imion i&nbsp;nazw mogą informować o&nbsp;przenikaniu się kultur i&nbsp;religii, stopniu życzliwości między ich przedstawicielami, a&nbsp;nawet wskazywać po średnio na czas powstania danej jednostki literackiej.</p>
<p>W końcu przypisy mające znaczenie dla rozwoju życia w&nbsp;Chrystusie i&nbsp;dla pogłębienia więzi z&nbsp;Nim — pojawiające się szczególnie w&nbsp;Nowym Przymierzu – dotykają najważniejszego celu Pisma Świętego, którym jest pogłębienie naszej więzi z&nbsp;Jezusem Chrystusem.</p>
<p>Niektóre przypisy nie mieszczą się w&nbsp;powyższych opisach. Odwołują się do ważnych spostrzeżeń natury etycznej lub do czegoś zachęcają. Przypisy te są wyrazem zadumy lub zachwytu nad tekstem Pism Świętych ze strony ich autora. Przypisy zredagowane są tak, by dostarczać korzystającym skrótowych lub ogólnych, łatwo dostępnych informacji. Ich zwięzłość wynika ze świadomej chęci dostarczenia Czytelnikom dzieła jednotomowego. O&nbsp;źródle dalszych wiadomości informuje czwarta część Wstępu, tj. Literaturze pomocniczej.</p>
<p>5.&nbsp;Równie pouczające jak przypisy są odsyłacze zamieszczone pod tytułami niektórych cześci tematycznych oraz przy kolumnach tekstu głównego. Te pierwsze odsyłają do miejsc paralelnych, te drugie do fragmentów zawierających podobne lub przeciwstawne słowa, terminy, wiadomości, myśli, a&nbsp;także ciekawsze w&nbsp;odczuciu wydawców konstrukcje gramatyczne. Część odsyłaczy ma charakter wzajemny, na przykład jeden z&nbsp;odsyłaczy [[J&nbsp;3:16>>Jn 3:16]] kieruje do [[Rz&nbsp;5:8>>Ro 5:8]], a&nbsp;jeden z&nbsp;odsyłaczy [[Rz&nbsp;5:8>>Ro 5:8]] do [[J&nbsp;3:16>>Jn 3:16]]. Celem odsyłaczy jest ułatwienie poruszania się po tekście biblijnym i&nbsp;pogłębienie zrozumienia myśli w&nbsp;nim wyrażonej. Odsyłacze, mimo swej ogromnej liczby, zachowują charakter pomocniczy. Pomagają one wyjaśniać Pismo Święte Pismem Świętym, ale nie zastępują leksykonów, słowników ani komentarzy. Nie odsyłają one także do źródeł pozabiblijnych ani, w&nbsp;tym wydaniu, do ksiąg niekanonicznch. Do tych ostatnich jednak odsyłają – tam, gdzie to ma znaczenie – przypisy.</p>
<p>Ogromną pomocą w&nbsp;posługiwaniu się odsyłaczami jest wersja cyfrowa niniejszej publikacji w&nbsp;formacie PDF. Odsyłacze są w&nbsp;niej interaktywne, co oznacza, że po dotknięciu odsyłacza na ekranach dotykowych lub kliknięciu lewym klawiszem myszki program otwiera opisane odsyłaczem miejsce, śledzi historię dotknięć lub kliknięć i&nbsp;pozwala na powrót do wersetu lub fragmentu wyjściowego. </p>
<p>Niektóre odsyłacze w&nbsp;przypisach zaopatrzone są w&nbsp;indeks L (L). Odsyłacze te kierują do odrębnej publikacji omówionej w&nbsp;czwartej części Wstępu, w&nbsp;Literaturze pomocniczej.</p>
<p>6.&nbsp;Miary i&nbsp;wagi. Szczegółowe informacje i&nbsp;objaśnienia na ich temat zamieszczono w&nbsp;Dodatkach, w&nbsp;części zatytułowanej Miary i&nbsp;wagi. Współczesne równoważniki biblijnych miar i&nbsp;wag.</p>
<p>7.&nbsp;Osoba czytająca Księgę Psalmów natrafia na powtarzające się w&nbsp;niej noty wstępne, np. Do złotej myśli. Z pewnością łatwiej byłoby znajdować ich wyjaśnienie w&nbsp;przypisach, to jednak powiększałoby ich objętość. Dlatego zdecydowano się zebrać wszystkie wyjaśnienia w&nbsp;Dodatkach, w&nbsp;części zatytułowanej <i>Informacje do Księgi Psalmów</i>.</p>
<p>8.&nbsp;Warte kilku uwag są Mapy i&nbsp;ilustracje zamieszczone na końcowych stronach. Mapy mają charakter przybliżony. Starano się, by były jak najdokładniejsze, jednak na obecnym etapie wydawniczym nie dorównują one mapom publikowanym szczególnie w&nbsp;nowszych atlasach. Niemniej korzystający z&nbsp;Biblii, natrafiając na opisy geograficzne albo spisy miast, ma – dzięki zamieszczonym mapom – możliwość unaocznienia sobie ich położenia.</p>
<p>Ilustracje również mają charakter przybliżony. W&nbsp;tym przypadku jednak przyczyną tego jest głównie niedokładność opisów biblijnych. Dobrym tego przykładem jest ilustracja świecznika, którego wymiarów nie znajdziemy na kartach Biblii. Podobnie kadź. W&nbsp;Księdze Wyjścia nie jest ona opisana wcale. O jej wyglądzie trzeba wnioskować, odwołując się do celów, dla których została zbudowana, do danych pozabiblijnych oraz do zdrowego rozsądku. </p>
<p>Przedstawione ilustracje charakteryzują się ważną zaletą. Zostały utworzone w&nbsp;programie komputerowym. Dzięki temu można je drukować w&nbsp;dowolnym rzucie dwuwymiarowym lub trójwymiarowym (na drukarkach 3D), a&nbsp;także można nimi, na ekranie, w&nbsp;dowolny sposób manipulować. Nie trzeba wielkiej wyobraźni, aby uświadomić sobie przydatność takiego rozwiązania na użytek osobisty i&nbsp;edukacyjny.</p>
<p>&nbsp;</p>
<h3>Uwagi dla nauczycieli</h3>
<p>Nauczyciele, to jest również wykładowcy i&nbsp;kaznodziejowie, jako fachowcy wiedzą, jak korzystać z&nbsp;pomocy dydaktycznych. Rodzice, którzy są nauczycielami szczególnego rodzaju, zwykle nie mają fachowej wiedzy, ale ich intuicja i&nbsp;wyobraźnia wynikająca z&nbsp;miłości czasem przewyższa fachowość naznaczoną stereotypami i&nbsp;zachowawczością. Poniższeuwagi zatem nie muszą być odkrywcze,ale mogą być inspirujące.</p>
<p>1.&nbsp;Jeśli wyjaśniamy znaczenie Pisma Świętego, posługując się przekładem literackimEIB lub jakimś innym, możemy odwoływać się do przypisów przekładu dosłownego wybiórczo, aby na ich podstawie budować ciekawe, a&nbsp;jednocześnie rzetelne wyjaśnienia. W&nbsp;przypadkach, kiedy przypisy wzbudziły w&nbsp;nas szczególne zainteresowanie, możemy je pogłębić, odwołując się do Literatury i&nbsp;uwag do wersetów Pisma Świętego lub kierując się bezpośrednio do EIB. Podawane w&nbsp;przypisach alternatywne znaczenia tekstu biblijnego nie muszą być jedynymi możliwymi. Nie trzeba ich też traktować w&nbsp;kategoriach rozłącznych (albo takie znaczenie – albo inne), ale łącznych (jedno i&nbsp;drugie znaczenie może być właściwe i&nbsp;oba pogłębiają zrozumienie tekstu).</p>
<p>2.&nbsp;Każdy, kto wykłada Pismo Święte, stoi przed ogromnym wyzwaniem oddzielenia tego, co mówi samo Pismo Święte, od tego, co jest już jego własnym wnioskiem lub wnioskiem reprezentowanego przez niego systemu teologicznego. Nauczyciele nieświadomi tej różnicy, często głoszą własne lub przejęte poglądy, dla których szukają uzasadnienia w&nbsp;Piśmie Świętym. Prowadzi to do niepotrzebnych sporów doktrynalnych i&nbsp;rozgrywek między „-izmami”, które nawet jeśli jakoś usprawiedliwiane, nie mają uzasadnienia w&nbsp;Piśmie Świętym.</p>
<p>3.&nbsp;Można odnieść wrażenie, że zbyt często za kryterium prawowierności uznawane jest nie Słowo, którym jest Chrystus w&nbsp;nas, nadzieja chwały, lecz zgodność z&nbsp;literą Pisma Świętego. Kryterium postawione tak, jak w&nbsp;tym drugim przypadku, rozgrzewa spory o&nbsp;najbardziej wiarygodne manuskrypty lub warianty tekstowe. Żywi się nadzieję, że tak wyłoniona litera pozwoli dotrzeć do jedynej prawdziwej normy życia. Praktyka ta wydaje się bardzo stara. Badacie Pisma, bo sądzicie, że w&nbsp;nich macie życie wieczne – to przecież Słowa naszego Mistrza. Trafnie pouczył On badaczy, że Pisma świadczą o&nbsp;Nim. Brane we właściwej perspektywie, wszelkie badania nad literą Pisma Świętego mają ogromną wartość. Jeśli jednak nie skutkują tym, że życie współczesnych przeradza się – dzięki Pismu Świętemu – w&nbsp;świadectwo o&nbsp;Słowie, to badania takie, choćby najbardziej wartościowe, nie mają większego znaczenia ani dla doczesności, ani dla wieczności – pomnażają głównie zasoby biblioteczne.</p>
<p>4.&nbsp;Słusznie Pismo Święte przyjmowane jest za normę. Normy jednak bywają różnego rodzaju. Bez wdawania się w&nbsp;szczegóły można powiedzieć, że badania nad Pismem Świętym prowadzą do wniosku, że nie jest ono normą prawną, ale normą twórczą. Jeśliby porównać je do klocków, to nie są to klocki, z&nbsp;których może powstać jedyna właściwa, normatywna budowla. Są to jedyne właściwe klocki, z&nbsp;których ludzie Jezusa Chrystusa, wolni, twórczy i&nbsp;misyjni, są w&nbsp;stanie budować budowle na miarę czasów, dla zadziwienia Go własną wiarą.</p>
<p style=""text-align: right;"">Piotr Zaremba</p>
<p style=""text-align: right;"">Ewangeliczny Instytut Biblijny</p>
<p>&nbsp;</p>


<h2 style=""text-align: center;"">Wykaz skrótów i oznaczeń</h2>
<p><b>Skróty ksiąg biblijnych:</b></p>
<p>Stare Przymierze: Rdz, Wj, Kpł, Lb, Pwt, Joz, Sdz, Rt, 1-2Sm, 1-2Krl, 1-2Krn, Ezd, Ne, Est, Jb, Ps, Prz, Kzn, Pnp, Iz, Jr, Tr, Ez, Dn, Oz, Jl, Am, Ab, Jo, Mi, Na, Ha, So, Ag, Za, Ml</p>
<p>Nowe Przymierze: Mt, Mk, Łk, J , Dz, Rz, 1-2Kor, Ga, Ef, Flp, Kol, 1-2Ts, 1-2Tm, Tt, Flm, Hbr, Jk, 1-2P, 1-3J, Jd, Obj</p>
<p>&nbsp;</p>
<p><b>Nazwy znaków alfabetu hebrajskiego:</b></p>
<p><span lang=""he"" dir=""rtl"">א</span><span dir=""ltr"">&nbsp;alef</span>, <span lang=""he"" dir=""rtl"">ב</span><span dir=""ltr"">&nbsp;bet</span>, <span lang=""he"" dir=""rtl"">ג</span><span dir=""ltr"">&nbsp;gimel</span>, <span lang=""he"" dir=""rtl"">ד</span><span dir=""ltr"">&nbsp;dalet</span>, <span lang=""he"" dir=""rtl"">ה</span><span dir=""ltr"">&nbsp;he</span>, <span lang=""he"" dir=""rtl"">ו</span><span dir=""ltr"">&nbsp;waw</span>, <span lang=""he"" dir=""rtl"">ז</span><span dir=""ltr"">&nbsp;zain</span>, <span lang=""he"" dir=""rtl"">ח</span><span dir=""ltr"">&nbsp;chet</span>, <span lang=""he"" dir=""rtl"">ט</span><span dir=""ltr"">&nbsp;tet</span>, <span lang=""he"" dir=""rtl"">י</span><span dir=""ltr"">&nbsp;jod</span>, <span lang=""he"" dir=""rtl"">כ</span><span dir=""ltr"">&nbsp;kaw</span>, <span lang=""he"" dir=""rtl"">ל</span><span dir=""ltr"">&nbsp;lamed</span>, <span lang=""he"" dir=""rtl"">מ</span><span dir=""ltr"">&nbsp;mem</span>, <span lang=""he"" dir=""rtl"">נ</span><span dir=""ltr"">&nbsp;nun</span>, <span lang=""he"" dir=""rtl"">ס</span><span dir=""ltr"">&nbsp;samek</span>, <span lang=""he"" dir=""rtl"">ע</span><span dir=""ltr"">&nbsp;ain</span>, <span lang=""he"" dir=""rtl"">פ</span><span dir=""ltr"">&nbsp;pe</span>, <span lang=""he"" dir=""rtl"">צ</span><span dir=""ltr"">&nbsp;tsade</span>, <span lang=""he"" dir=""rtl"">ק</span><span dir=""ltr"">&nbsp;kof</span>, <span lang=""he"" dir=""rtl"">ר</span><span dir=""ltr"">&nbsp;resz</span>, <span lang=""he"" dir=""rtl"">ש</span><span dir=""ltr"">&nbsp;szin</span>, <span lang=""he"" dir=""rtl"">ת</span><span dir=""ltr"">&nbsp;taw</span></p>
<p>&nbsp;</p>

<p><b>Inne skróty i oznaczenia</b></p>
<p style=""text-align: left;"">
abc – kursywą zaznaczono cytaty ze Starego Przymierza za NA27 i NA28. W samym Starym Przymierzu zaznaczono w&nbsp;ten sposób nie tłumaczone wyrazy obcego pochodzenia, jak: Sela i pur<br />
A – Kodeks Aleksandryjski = GA w&nbsp;SP, V w. po Chr.<br />
abs. – absolutus<br />
acc. – acusativus<br />
ak. – akkadyjski<br />
aleks. – aleksandryjski<br />
amor. – amorycki<br />
aor. – aoryst<br />
Ar – wersja arabska<br />
arab. – arabski<br />
aram. – aramejski<br />
as. – asyryjski<br />
B – Kodeks Watykański = GB w&nbsp;SP, IV w. po Chr.<br />
Ba – Baruch<br />
bab. – babiloński<br />
bezok – bezokolicznik<br />
bliskozn. – bliskoznaczny, -a, -e<br />
bo – wersja bohairyczna<br />
BHQ – Biblia Hebraica Quinta. 2010. Schenker. Stuttgart: Deutsche Bibelgesellschaft<br />
BHS – Biblia Hebraica Stuttgartensia. 1997. K. Elliger, W. Rudolf, red., wyd. 5. Stuttgart: Deutsche Bibelgesellschaft<br />
C – Kodeks Efrema, V w. po Chr.<br />
chald. – chaldejski<br />
con. – coniunctivus<br />
cz – czasownik, -niki<br />
D – Kodeks Bezy, V w. po Chr.<br />
dat. – dativus<br />
dat. instr. – dativus instrumentalis<br />
dat. loc. – dativus locativus<br />
defek. – defektywna (forma)<br />
det. – determinatyw<br />
Did – Didache<br />
dit. – dittografia<br />
dk – dokonane<br />
dł. – długość<br />
dod. – dodatek, dodaje, -ją, -wać<br />
dos. – dosłowny, -a, -e, -nie<br />
dot. – dotyczy, - czący<br />
du – dualis<br />
duch. – duchowy, -a, -e, -o<br />
EA – Tel-el-Amarna<br />
ed(d) – edycje Ms(s)<br />
egip. – egipski<br />
elam. – elamicki, -e<br />
em. – emendacja, -dowane<br />
emf. – emfatyczne<br />
Ew. – Ewangelia<br />
etiop. — etiopski<br />
etym. – etymologia, -cznie<br />
euf. – eufemizm<br />
f. – funkcja, funkcjonować<br />
fen. – fenicki<br />
filist. – filistyński<br />
frg. frgy. – fragment, –y<br />
fryg. – frygijski, -a,-e<br />
fut. – futurum, futuryczny, -a, -e<br />
G* – G, tekst gr. pierwotny<br />
LXX, G, Gmss – Septuaginta, niektóre manuskrypty Septuaginty<br />
GA – Kodeks Aleksandryjski<br />
GB – Kodeks Watykański<br />
GBal – Kodeks Watykański wtóry<br />
GE – Göttingen Exodus<br />
gen. – genetivus<br />
GK – fragmenty z&nbsp;Genizy Kairskiej<br />
GL – G, recenzja Lucjana<br />
Gmin – G, mss minuskułowe<br />
GN – kodeks Basiliano-Vaticanus połączony z&nbsp;kodeksem Veneto<br />
GO – G, recenzja Orygenesa<br />
GS – zasada Granville’a Sharpa<br />
GS – Kodeks Synaiticus<br />
GV – Kodeks Venetus<br />
G82 – G ms 82<br />
G127 – G ms 127<br />
głęb. – głębokość<br />
godz. – godzina<br />
gr. – grecki<br />
gram. – gramaty-ka, -cznie, -czny<br />
grub. – grubość, -ści<br />
H – przekład Hieronima<br />
h – h wygłosowe w&nbsp;transkrypcji wyrażeń<br />
hbr., które przy wymowie pozostaje nieme<br />
h. – homonim, homonim(icznie)(iczny)<br />
hap. – haplografia<br />
hbr. – hebrajski<br />
hebr. – hebraizm<br />
Hen – Henoch, księgi Henocha<br />
hend. – hendiadys<br />
het. – hetycki, -s<br />
Hex – źródła heksaplaryczne<br />
hi – hifil<br />
hier. egip. – hieroglificzny egipski<br />
hist. – historyczny, -a, -e, -nie<br />
hitp – hitpael<br />
hitpalp – hitpalpel<br />
hitpo – hitpolel<br />
HM – hebrajski misznaicki<br />
HN – hebrajski nebatejski<br />
hof – hofal<br />
hl – hapax legomenon, słowo użyte tylko raz w&nbsp;tekście Biblii<br />
hl2,3 – słowo użyte w&nbsp;tekście Biblii, odpowiednio, dwa lub trzy razy<br />
hom. – homonim<br />
hur. – hurycki<br />
Id, If, Ih, Ip – interpretacje: duchowa, futuryczna, historycza i preteryczna<br />
ill. – illyryjski<br />
imp. – imperativus<br />
impf. – imperfectum<br />
ind. – indicativus<br />
inf. – infinitivus<br />
ins. – inskrypcja, -cje<br />
int. – interpretacja, -cje<br />
J – źródło jahwistyczne<br />
J. – jezioro<br />
jedn. – jednostka<br />
JHWH – Imię własne Boga, Jahwe<br />
Jub – Księga Jubileuszy<br />
Jud – Księga Judyty<br />
juss. – jussivus<br />
kan. – kananejski, -a<br />
KA – Konstytucje Apostolskie<br />
KD – Kodeks Damasceński<br />
KH – Kodeks Hammurabiego<br />
KP – Kodeks Petersburski, B 19A<br />
ketiw – wyrażenie zaświadczone w&nbsp;tekście głównym MT, dla którego sugeruje się właściwy odczyt, czyli qere<br />
kj,w,z – kryterium, odpowiednio, językowe, większego wglądu w&nbsp;tekst wyjściowy i&nbsp;zrozumiałości.<br />
klk, klkn, klkd, klks – kilka, -naście, -dziesiąt, -set.<br />
kod. – kodeks, -y<br />
konstr. – konstrukcja<br />
kont. — kontekst, -owy<br />
kor. – korekta<br />
L – Kodeks angelicus, IX w. po Chr.<br />
l. – lub<br />
la b d e o – lectio, odpowiednio: ante, brevior, difficilior, facilus explicavit, lingua originali<br />
licz – liczebnik<br />
lit. – literatura<br />
lp, lm – liczba pojedyncza, liczba mnoga<br />
LR – Leviticus Rabba<br />
łac. – łaciński<br />
mat. – materiał(y)<br />
M. – morze<br />
Mch – Machabejskie, księgi<br />
Mdr – Księga Mądrości<br />
med. – medyczny<br />
met. – metonimia, -nimicznie<br />
metaf. – metafora, metaforycz(ny)(nie)<br />
metat. – metateza<br />
mez. – mezopotamski, -a<br />
mg – margines, -owy, -owa<br />
min. – minuta<br />
min – w&nbsp;indeksie górnym: minuskuł(a)(owy)<br />
m.in. – między innymi<br />
mn. – mniejszy, -a<br />
monet. – monetarny<br />
ms, mss – manuskrypt(y)<br />
Ms – kodeks hbr. średniowieczny<br />
MT, MTmss – tekst masorecki, manuskrypty tekstu masoreckiego<br />
MTMs, MTMss – manuskrypt, manuskrypty hebrajskie średniowieczne<br />
NA27, NA28 – Novum Testamentum Graece. Nestle-Aland. 1993, 2012. Stuttgart: Deutsche Bibelgesellschaft<br />
NB – Niewola Babilońska<br />
ndk – niedokonane<br />
neoas. – noeasyryjski, -a, -e<br />
ni – nifal<br />
niereg. – nieregularny, -a, -e<br />
nom. – nominativus<br />
NP – Nowe Przymierze (Nowy Testament)<br />
n.p.m. – nad poziomem morza<br />
O – recenzja Orygenesa<br />
OG – wersje starogreckie<br />
okr. – okres<br />
OL – wersje starołacińskie<br />
oryg. – oryginalny (tekst l. wariant)<br />
ozn. – oznacza<br />
P – źródło kapłańskie<br />
palp – palpel<br />
pap. – papirus(y)<br />
par. – paralelne, paralelizm<br />
pas. – passivus, strona bierna<br />
pd – południe<br />
pd wsch – południowy wschód<br />
pd zach – południowy zachód<br />
pers. – perski<br />
pf. – perfectum<br />
pi – piel<br />
pilp – pilpel<br />
PA – Pouczenia Amenemope<br />
PL – Pardes Lauder<br />
pl – pluralis, lm<br />
pn – północ<br />
pn wsch – północny wchód<br />
pn zach – północny zachód<br />
poch. – pochodzenie<br />
pod. – podobnie, podobieństwo<br />
poj. – pojęcie<br />
pojem. – pojemność<br />
polit. – polityczny, -a, -e<br />
polp – polpal<br />
por. – porównaj<br />
pow. – powierzchnia<br />
późn. – późniejszy, -e<br />
p.p.m. – poniżej poziomu morza<br />
praes. – praesens, czas teraźniejszy<br />
pret. – preteryczny, -a, -e, -nie<br />
prek. – prekatywny, -a, -e, -nie<br />
profet. – profetyczny/e<br />
prop. – proponowany, -a, -e, -zycja, -uje<br />
przedr – przedrostek<br />
przen. – przenośnia, -e<br />
przetłum. – przetłumaczony -a, -e<br />
przyd – przydawka<br />
przyim – przyimek<br />
przym – przymiotnik, -kowy<br />
przyp. – przypadek<br />
przys – przysłówek<br />
PN – Papirus Nasha<br />
PS – Pięcioksiąg Samarytański<br />
PsSal – Psalmy Salomona<br />
ptc. – participium, imiesłów<br />
pu – pual<br />
przyp. – przypis, -y<br />
przys – przysłówek<br />
pyt. – pytanie<br />
q – kal<br />
qere – wyrażenie sugerowane jako właściwy odczyt wyrażenia zaświadczonego w&nbsp;tekście głównym MT, czyli ketiw<br />
red. – redakcja, -cyjny<br />
rel. – religijny, -a, -e<br />
rewok. – rewokalizacja<br />
rm – rodzaj męski<br />
rodz. – rodzajnik<br />
r. p. Chr. – rok przed Chrystusem<br />
r. po Chr. – rok po Chrystusie<br />
rz – rzeczownik<br />
rzym. – rzymski<br />
rż – rodzaj żeński<br />
S – tekst syryjski<br />
sbab. – starobabiloński<br />
sc – status constructus<br />
scp – scriptio plena, pisownia pełna<br />
scd – scriptio defectiva, pisownia ułomna<br />
szer. – szerokość<br />
SG – Salkinson-Ginsburg. 1886. Hebrew New Testament, wyd. popr. 1999 w&nbsp;kierunku zgodności z&nbsp;Tekstem przyjętym greckiego NT<br />
sg – singularis, lp<br />
skr. – skrót, skrócony, -ne<br />
SP – Stare Przymierze (Stary Testament)<br />
spers. – staroperski<br />
spój – spójnik<br />
st. – stopień<br />
str. – strona, -y<br />
suf – sufiks<br />
sym. – symbol, -icznie, -iczny<br />
syn. – synonim, synonimiczny, -nie<br />
synek. – synekdocha, -chicznie<br />
Syr – Mądrość Syracha<br />
syr. – syryjski<br />
T – Talmud<br />
tamud. – tamudyjski<br />
TB – Talmud Babiloński<br />
Tb – Księga Tobiasza<br />
Tg – targum<br />
TgJo – Targum Jonatana<br />
TgN – Targum Neofiti<br />
TgPsJ – Targum Pseudo-Jonatana<br />
TgO – Targum Onkelosa<br />
TgMs/Mss – kodeks, kodeksy Targumów<br />
Th – grecki tekst Teodocjona<br />
tiq – tiqqune soferim<br />
TL – Testament Lewiego<br />
tłum. – tłumaczenie, -one<br />
TP – Tekst przyjęty<br />
trad. – tradycja, -cyjny, -cyjnie<br />
trans. – transliteracja<br />
Trt – Tertulian<br />
TW – Tekst większościowy<br />
tys. – tysiąclecie<br />
tyt. – tytuł<br />
ugar. – ugarycki<br />
vid – wariant widoczny, lecz niepewny<br />
Vg – Wulgata<br />
W – Kodeks Waszyngtoński, IV/V w. po Chr.<br />
w., ww. – werset, wersety<br />
wd,l,o,p,s,t – wzgląd, odpowiednio, duchowego dziedzictwa, logiczności, objętości, pierwszeństwa NP, starożytności i&nbsp;trudności wariantu<br />
war. – warunek, - owy, -owa<br />
WMoj – Wniebowstąpienie Mojżesza<br />
wok. – wokalizacja<br />
wsch – wschód<br />
wsp. – współczesne<br />
wyr. – wyrażenie<br />
wys. – wysokość<br />
wzgl. – względnie<br />
zach – zachód<br />
zaim – zaimek<br />
zal. – zależne, zależność, -ści<br />
zał. – założenie<br />
zak. – zakończenie<br />
zap. – zapożyczenie<br />
ZMM – Zwoje znad Morza Martwego<br />
zm. – zmarł<br />
zn. – znaczenie<br />
zob. – zobacz<br />
zwok. – zwokalizowane<br />
zwr. – zwrotny, -a, -e<br />
I, II... – homonimy (pierwszy, drugi...) ’‘ – transkrypcja odpowiednio (alef) i&nbsp;(ain)<br />
α’ – przekład Akwili<br />
ε’ – Quinta, warianty piątej kolumny Heksapli εβρ’ – późniejsze przekłady tekstu na język grecki hebrajskiego, lecz bliżej nieokreślonego pochodzenia<br />
P – papirus<br />
Θ Ψ m P75 892txt 070 1241 f 1.13 – powszechnie stosowane określenia świadectw tekstowych Nowego Przymierza. W&nbsp;niniejszym przekładzie przytaczane za Novum Testamentum Graece (jak wyżej)<br />
θ’ – przekład Teodocjona<br />
σ’ – przekład Symmacha<br />
Ψ – Athous Lavrensis, IX/X w. po Chr.<br />
1Hen – Pierwsza Księga Henocha<br />
1Kl – Pierwszy List Klemensa<br />
1Mch, 2Mch, 4Mch – Pierwsza, Druga, Czwarta Księga Machabejska<br />
<span lang=""he"" dir=""rtl"">א</span> – Kodeks synajski, III w. po Chr.<br />
[<span lang=""he"" dir=""rtl"">ה</span>] – znak lub znaki w&nbsp;nawiasie kwadratowym oznaczają, że nie zachowały się one w&nbsp;manuskryptach<br />
<span lang=""he"" dir=""rtl"">מֿהל֯ש̇</span> — brak znaku nad literą oznacza literę pewną, kropka prawdopodobną, kółko możliwą, kreska zaś niepewność co do identyfikacji liter podobnych<br />
[...] – nawias kwadratowy zamyka wyrazy lub wyrażenia, których w&nbsp;oryginale wyraźnie brak, a które w&nbsp;odczuciu tłumaczy wyjaśniają lub stanowią opcję wyjaśnienia znaczenia tekstu<br />
* – oryginalny odczyt ms<br />
1,2... – pierwsza, druga ... grupa korekt<br />
099 – Kodeks majuskułowy, VII w. po Chr.<br />
597 – Kodeks minuskułowy, XIII w. po Chr.
</p>

<p>&nbsp;</p>
<p>&nbsp;</p> 
<p>&nbsp;</p>
<p>&nbsp;</p> 
</body>
</html>";
            return html;
        }

        private string RemoveOrphans(string text) {
            var pattern = @"((?<text1>\s[a-zA-Z])(?<space1>\s)(?<text2>[a-zA-Z])(?<space2>\s))|((?<text3>\s[a-zA-Z])(?<space3>\s))|((?<text4>\s[0-9]+)(?<space4>\s))|(^(?<text5>[a-zA-Z])(?<space5>\s))";
            return Regex.Replace(text, pattern, delegate (Match m) {
                if (m.Groups["text1"] != null && m.Groups["text1"].Value.IsNotNullOrEmpty()) {
                    return $"{m.Groups["text1"].Value}\u00A0{m.Groups["text2"].Value}\u00A0";
                }
                if (m.Groups["text3"] != null && m.Groups["text3"].Value.IsNotNullOrEmpty()) {
                    return $"{m.Groups["text3"].Value}\u00A0";
                }
                if (m.Groups["text4"] != null && m.Groups["text4"].Value.IsNotNullOrEmpty()) {
                    return $"{m.Groups["text4"].Value}\u00A0";
                }
                if (m.Groups["text5"] != null && m.Groups["text5"].Value.IsNotNullOrEmpty()) {
                    return $"{m.Groups["text5"].Value}\u00A0";
                }
                return m.Value; // nic nie zmieniam
            });
        }

        private FormattedText RecognizeBookInfo(string text, BibleModel model, UnitOfWork uow) {
            text = text.Replace("&nbsp;", "\u00A0");
            var result = text.Contains("<x>") ? new FormattedText() : new FormattedText(RemoveOrphans(text));
            if (result.Items == null) {
                result.Items = new List<object>();

                var pattern = @"(?<prefix>[\w\s\(\)\d\,\;\.\:]+)?(\<x\>(?<book>[0-9]+)\s(?<chapterStart>[0-9]+)(\s)?\:(\s)?(?<verseStart>[0-9]+)(\-(?<verseEnd>[0-9]+))?\<\/x\>)(?<postfix>[\w\s\(\)\d\,\;\.\:]+)?";
                var matches = Regex.Matches(text, pattern);
                foreach (Match match in matches) {
                    if (match.Groups["prefix"] != null && match.Groups["prefix"].Value.IsNotNullOrEmpty()) {
                        result.Items.Add(RemoveOrphans(match.Groups["prefix"].Value));
                    }
                    var sb = new StringBuilder();
                    var sb2 = new StringBuilder();
                    if (match.Groups["book"] != null && match.Groups["book"].Value.IsNotNullOrEmpty()) {
                        var bn = match.Groups["book"].Value.ToInt();
                        var dbBook = new XPQuery<ChurchServices.Data.Model.BookBase>(uow).Where(x => x.NumberOfBook == bn).FirstOrDefault();
                        var bm = model.Books.Where(x => x.NumberOfBook == bn).FirstOrDefault();
                        sb2.Append($"{dbBook.BookShortcut}\u00A0");
                        sb.Append($"{bm.BookShortcut}\u00A0");
                    }
                    if (match.Groups["chapterStart"] != null && match.Groups["chapterStart"].Value.IsNotNullOrEmpty()) {
                        sb2.Append(match.Groups["chapterStart"].Value);
                        sb.Append(match.Groups["chapterStart"].Value);
                    }
                    if (match.Groups["verseStart"] != null && match.Groups["verseStart"].Value.IsNotNullOrEmpty()) {
                        sb2.Append($":{match.Groups["verseStart"].Value}");
                        sb.Append($":{match.Groups["verseStart"].Value}");
                    }
                    if (match.Groups["verseEnd"] != null && match.Groups["verseEnd"].Value.IsNotNullOrEmpty()) {
                        sb2.Append($"-{match.Groups["verseEnd"].Value}");
                        sb.Append($"-{match.Groups["verseEnd"].Value}");
                    }
                    if (sb.Length > 0) {
                        var sb3 = $"[[{sb2}>>{sb}]]";
                        result.Items.Add(sb3);
                    }

                    if (match.Groups["postfix"] != null && match.Groups["postfix"].Value.IsNotNullOrEmpty()) {
                        result.Items.Add(RemoveOrphans(match.Groups["postfix"].Value));
                    }
                }

            }
            return result;
        }

        [TestMethod]
        public void GetSNPLFromXHtmlFiles() {
            var dir = @"D:\OneDrive\WBST\2020\Fakultety\Biblistyka\EIB\SNP_BibleEngine\SNP_BibleEngine\SNPL\XHTML";
            var bservice = new BibleModelService();
            var bmodel = new BibleModel() {
                Shortcut = "SNPL",
                Books = new List<BookModel>(),
                Name = "Stare i Nowe Przymierze - Przekład Literacki",
                Type = Model.Bible.TranslationType.Default
            };
            var service = new XhtmlModelService();
            for (int i = 1; i <= 66; i++) {
                var filePath = System.IO.Path.Combine(dir, $"{i.ToString().PadLeft(2, '0')}.xhtml");
                if (File.Exists(filePath)) {
                    var book = service.GetBook(filePath);
                    if (book != null) { bmodel.Books.Add(book); }
                }
            }

            Assert.IsTrue(bmodel.Books.Count > 0);

            foreach (var book in bmodel.Books) {
                SetBookInfo(book, bmodel);
            }

            var xmlInputFile = @"D:\OneDrive\WBST\2020\Fakultety\Biblistyka\EIB\SNP_BibleEngine\SNP_BibleEngine\SNPL\snpl.eib.xml";
            bservice.SaveBibleModelToFile(bmodel, xmlInputFile);

            if (File.Exists(xmlInputFile)) {
                using (var service2 = new LogosBibleModelService()) {
                    var bibleModelOutFilePath = @"D:\OneDrive\WBST\2020\Fakultety\Biblistyka\EIB\SNP_BibleEngine\SNP_BibleEngine\SNPL\snpl.eib.docx";
                    service2.Export(xmlInputFile, bibleModelOutFilePath, introductionHtml: GetSNPLIntroduction(), addTitle: false, repairModel: true);
                }
            }
            else {
                Assert.Fail();
            }
        }


        [TestMethod]
        public void ReplaceSNPDInDatabaseFromEibModel() {
            const string TRANSLATION = "PBD";
            var bservice = new BibleModelService();
            var bmodel = bservice.GetBibleModelFromFile(@"D:\OneDrive\WBST\2020\Fakultety\Biblistyka\EIB\SNP_BibleEngine\SNP_BibleEngine\SNPD\snpd.eib.xml");
            if (bmodel != null) {
                var uow = new UnitOfWork();

                var subTitles = new XPQuery<Subtitle>(uow).Where(x => x.ParentChapter.Index.StartsWith($"{TRANSLATION}.")).ToList();
                if (subTitles.Count > 0) {
                    uow.Delete(subTitles);
                    uow.CommitChanges();
                }
                foreach (var book in bmodel.Books) {                    
                    foreach (var chapter in book.Chapters) {
                        var title = "";
                        foreach (var item in chapter.Items) {
                            if (item is FormattedText) {
                                title = (item as FormattedText).ToString();
                            }
                            else if (item is VerseModel) {
                                var verse = item as VerseModel;
                                if (title.IsNotNullOrEmpty()) {
                                    var dbTitle = new Subtitle(uow) {
                                        BeforeVerseNumber = verse.NumberOfVerse,
                                        Level = 2,
                                        ParentChapter = new XPQuery<Chapter>(uow).Where(x=>x.Index == $"{TRANSLATION}.{book.NumberOfBook}.{chapter.NumberOfChapter}").FirstOrDefault(),
                                        Text = title
                                    };
                                    dbTitle.Save();
                                    title = "";
                                }
                            }
                        }
                    }
                }

                uow.CommitChanges();
                uow.PurgeDeletedObjects();
            }
        }

        [TestMethod]
        public void ReplaceSNPDInDatabaseFromOsisModel() {
            const string TRANSLATION = "PBD";
            var service = new OsisModelService();
            var model = service.GetModelFromFile(@"D:\OneDrive\WBST\2020\Fakultety\Biblistyka\EIB\SNP_BibleEngine\SNP_BibleEngine\SNPD\snpd.osis.v3.xml");
            if (model != null) {
                var bservice = new BibleModelService();
                var bmodel = bservice.GetBibleModelFromOsisModel(model);
                if (bmodel != null) {
                    var uow = new UnitOfWork();
                    var translation = new XPQuery<Translation>(uow).Where(x => x.Name == TRANSLATION).FirstOrDefault();

                    var subTitles = new XPQuery<Subtitle>(uow).Where(x => x.ParentChapter.ParentBook.ParentTranslation == translation).ToList();
                    if (subTitles.Count > 0) {
                        uow.Delete(subTitles);
                        uow.CommitChanges();
                    }

                    foreach (var book in bmodel.Books) {
                        var baseBook = new XPQuery<BookBase>(uow).Where(x => x.NumberOfBook == book.NumberOfBook).FirstOrDefault();
                        var dbBook = new XPQuery<Book>(uow).Where(x => x.ParentTranslation == translation && x.NumberOfBook == book.NumberOfBook).FirstOrDefault();
                        if (dbBook == null) {
                            dbBook = new Book(uow) {
                                NumberOfBook = book.NumberOfBook,
                                BaseBook = baseBook,
                                BookShortcut = baseBook.BookShortcut,
                                BookName = book.BookName,
                                NumberOfChapters = book.Chapters.Count,
                                Color = book.Color,
                                ParentTranslation = translation
                            };
                            dbBook.Save();
                        }
                        else {
                            dbBook.NumberOfChapters = book.Chapters.Count;
                            dbBook.BookName = book.BookName;
                            dbBook.BookShortcut = baseBook.BookShortcut;
                        }

                        foreach (var chapter in book.Chapters) {
                            var dbChapter = new XPQuery<Chapter>(uow).Where(x => x.ParentBook == dbBook && x.NumberOfChapter == chapter.NumberOfChapter).FirstOrDefault();
                            if (dbChapter == null) {
                                dbChapter = new Chapter(uow) {
                                    NumberOfChapter = chapter.NumberOfChapter,
                                    ParentBook = dbBook,
                                    NumberOfVerses = chapter.Verses().Count(),
                                    Index = $"{TRANSLATION}.{book.NumberOfBook}.{chapter.NumberOfChapter}"
                                };
                                dbChapter.Save();
                            }
                            else {
                                dbChapter.NumberOfVerses = chapter.Verses().Count();
                            }

                            var vn = 1;
                            VerseModel verse = null;
                            foreach (var item in chapter.Items) {
                                if (item is VerseModel) {
                                    verse = item as VerseModel;
                                    vn = verse.NumberOfVerse;
                                    var dbVerse = new XPQuery<Verse>(uow).Where(x => x.ParentChapter == dbChapter && x.NumberOfVerse == vn).FirstOrDefault();
                                    if (dbVerse == null) {
                                        dbVerse = new Verse(uow) {
                                            ParentChapter = dbChapter,
                                            NumberOfVerse = verse.NumberOfVerse,
                                            Text = GetVerseText(verse),
                                            StartFromNewLine = false,
                                            Index = $"{TRANSLATION}.{book.NumberOfBook}.{chapter.NumberOfChapter}.{verse.NumberOfVerse}"
                                        };
                                    }
                                    else {
                                        dbVerse.Text = GetVerseText(verse);
                                    }
                                    dbVerse.Save();
                                }
                                else if (item is FormattedText) {
                                    var title = item as FormattedText;
                                    if (title != null) {
                                        var dbTitle = new XPQuery<Subtitle>(uow).Where(x => x.ParentChapter == dbChapter && x.BeforeVerseNumber == vn).FirstOrDefault();
                                        if (dbTitle == null) {
                                            dbTitle = new Subtitle(uow) {
                                                BeforeVerseNumber = vn,
                                                Level = 2,
                                                ParentChapter = dbChapter,
                                                Text = title.ToString()
                                            };
                                            dbTitle.Save();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    uow.CommitChanges();
                    uow.PurgeDeletedObjects();
                }
            }
        }

        private string GetVerseText(VerseModel verse) {
            if (verse != null) {
                var sbfm = String.Empty;
                var sb = new StringBuilder();
                var sbf = new StringBuilder();
                foreach (var item in verse.Items) {
                    if (item is string) {
                        sb.Append(RemoveBrakets(item));
                    }
                    else if (item is NoteModel) {
                        sbfm = $"{sbfm}*";
                        if (sb.ToString().EndsWith("*")) { sb.Append(" "); }
                        sb.Append(sbfm);
                        sbf.Append($"[{sbfm}");
                        var nbp = String.Empty;
                        foreach (var nitem in (item as NoteModel).Items) {
                            if (nitem is NoteReferenceModel) {
                                if (nitem.ToString().Contains(":")) {
                                    var nref = nitem as NoteReferenceModel;
                                    var abbr = nref.Ref.Substring(0, nref.Ref.IndexOf(".")).Replace(".", "");
                                    if (abbr.IsNotNullOrWhiteSpace()) { nbp = abbr; }
                                    var idx = nref.Text.IndexOf(" ");
                                    var nn = "";
                                    if (idx != -1) {
                                        nn = nref.Text.Substring(nref.Text.IndexOf(" ")).Trim();
                                    }
                                    else {
                                        nn = nref.Text;
                                    }

                                    if (abbr == "" && nbp != "" && idx == -1) {
                                        abbr = nbp;
                                    }

                                    if (abbr != "") {
                                        var bn = BibleModelService.GetBookNumberFromLogosAbbreviation(abbr);
                                        if (bn != 0) {
                                            var reff = $"<x>{bn} {nn.Replace("L.", "").Replace("L", "").Trim()}</x>";
                                            if (nn.Contains("L")) {
                                                reff += "L.";
                                            }
                                            sbf.Append(reff);
                                        }
                                        else {
                                            sbf.Append(RemoveBrakets(nitem));
                                        }
                                    }
                                    else {
                                        sbf.Append(RemoveBrakets(nitem));
                                    }
                                }
                                else {
                                    sbf.Append(RemoveBrakets(nitem));
                                }
                            }
                            else {
                                sbf.Append(RemoveBrakets(nitem));
                            }
                        }
                        sbf.Append($"]");
                    }
                    else if (item is WordOfGodModel) {
                        sb.Append($"<J>{RemoveBrakets(item)}</J>");
                    }
                    else if (item is SpanModel) {
                        sb.Append(RemoveBrakets(item));
                    }
                }
                return $"{sb}<n>{sbf}</n>";
            }
            return null;
        }

        private string RemoveBrakets(object o) {
            var s = o.ToString();
            return s.Replace("[", "(").Replace("]", ")").Replace("*", String.Empty);
        }

        [TestMethod]
        public void TestConvertOsisModelToBibleModel() {
            var service = new OsisModelService();
            var model = service.GetModelFromFile(@"D:\OneDrive\WBST\2020\Fakultety\Biblistyka\EIB\SNP_BibleEngine\SNP_BibleEngine\SNPD\snpd.osis.v3.xml", true);
            if (model != null) {
                var bservice = new BibleModelService();
                var bmodel = bservice.GetBibleModelFromOsisModel(model);
                if (bmodel != null) {
                    Assert.IsTrue(bmodel.Books.Count > 0);

                    foreach (var book in bmodel.Books) {
                        SetBookInfo(book, bmodel);
                    }

                    var xmlInputFile = @"D:\OneDrive\WBST\2020\Fakultety\Biblistyka\EIB\SNP_BibleEngine\SNP_BibleEngine\SNPD\snpd.eib.xml";
                    bservice.SaveBibleModelToFile(bmodel, xmlInputFile);

                    if (File.Exists(xmlInputFile)) {
                        using (var service2 = new LogosBibleModelService()) {
                            var bibleModelOutFilePath = @"D:\OneDrive\WBST\2020\Fakultety\Biblistyka\EIB\SNP_BibleEngine\SNP_BibleEngine\SNPD\snpd.eib.docx";
                            service2.Export(xmlInputFile, bibleModelOutFilePath, introductionHtml: GetSNPDIntroduction(), addTitle: false);
                        }
                    }
                    else {
                        Assert.Fail();
                    }
                }
                else {
                    Assert.Fail();
                }
            }
            else {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void BuildLogosWordFromBibleModel() {
            var bibleModelFilePath = @"D:\OneDrive\WBST\2020\Fakultety\Biblistyka\EIB\SNP_BibleEngine\SNP_BibleEngine\SNPD\snpd.eib.xml";
            var bibleModelOutFilePath = @"D:\OneDrive\WBST\2020\Fakultety\Biblistyka\EIB\SNP_BibleEngine\SNP_BibleEngine\SNPD\snpd.eib.docx";
            using (var service = new LogosBibleModelService()) {
                service.Export(bibleModelFilePath, bibleModelOutFilePath, introductionHtml: GetSNPDIntroduction());
            }
        }

        [TestMethod]
        public void ExportDbBibleBWToLogosFile() { ExportDbBibleToLogosFile("BW"); }

        [TestMethod]
        public void ExportDbBibleEkuToLogosFile() { ExportDbBibleToLogosFile("EKU'18"); }

        [TestMethod]
        public void ExportDbBibleBTToLogosFile() { ExportDbBibleToLogosFile("BT'99"); }

        private void ExportDbBibleToLogosFile(string name) {
            var fn = 1;
            if (name != null) {
                var srv = new BibleModelService();
                var uow = new UnitOfWork();
                var dbTranslation = new XPQuery<ChurchServices.Data.Model.Translation>(uow).Where(x => x.Name == name).FirstOrDefault();
                if (dbTranslation != null) {
                    var model = new BibleModel() {
                        Shortcut = dbTranslation.Name,
                        Name = dbTranslation.Description,
                        Type = Model.Bible.TranslationType.Default,
                        Books = new List<BookModel>()
                    };
                    foreach (var dbBook in dbTranslation.Books) {

                        var dbBaseBook = new XPQuery<ChurchServices.Data.Model.BookBase>(uow).Where(x => x.NumberOfBook == dbBook.NumberOfBook).FirstOrDefault();

                        var book = new BookModel() {
                            BookShortcut = srv.GetShortcutFromEIBAbbreviation(dbBaseBook.BookShortcut),
                            NumberOfBook = dbBook.NumberOfBook,
                            BookName = dbBook.BookName,
                            Color = dbBook.Color,
                            Chapters = new List<ChapterModel>()
                        };

                        model.Books.Add(book);

                        foreach (var dbChapter in dbBook.Chapters.OrderBy(x => x.NumberOfChapter)) {
                            var chapter = new ChapterModel() {
                                NumberOfChapter = dbChapter.NumberOfChapter,
                                Items = new List<object>()
                            };
                            book.Chapters.Add(chapter);

                            foreach (var dbVerse in dbChapter.Verses.OrderBy(x => x.NumberOfVerse)) {

                                var dbSubtitles = new XPQuery<ChurchServices.Data.Model.Subtitle>(uow).Where(x => x.ParentChapter.Oid == dbChapter.Oid && x.BeforeVerseNumber == dbVerse.NumberOfVerse).OrderBy(x => x.Level).ToList();
                                if (dbSubtitles.Count > 0) {
                                    foreach (var dbSubtitle in dbSubtitles) {
                                        var stext = dbSubtitle.Text.Replace("<", "").Replace(">", "");
                                        chapter.Items.Add(
                                            new FormattedText(RemoveOrphans(stext))
                                        );
                                    }
                                }
                                var verse = new VerseModel() {
                                    NumberOfVerse = dbVerse.NumberOfVerse,
                                    Style = VerseStyle.Default,
                                    StartFromNewLine = dbVerse.StartFromNewLine,
                                    Items = new List<object>()
                                };

                                // add content
                                var text = dbVerse.Text
                                    .Replace("<J>", "{{field-on:words-of-christ}}")
                                    .Replace("</J>", "{{field-off:words-of-christ}}")
                                    .Replace("<e>", "{{field-on:ot-quote}}")
                                    .Replace("</e>", "{{field-off:ot-quote}}")
                                    .Replace("<t>", "")
                                    .Replace("</t>", "")
                                    .Replace("<i>", "")
                                    .Replace("</i>", "")
                                    .Replace("<u>", "")
                                    .Replace("</u>", "")
                                    .Replace("<b>", "")
                                    .Replace("</b>", "")
                                    .Replace("<pb>", "")
                                    .Replace("<pb/>", "")
                                    .Replace("<br/>", "")
                                    .Replace("*", "");

                                text = Regex.Replace(text, @"\<f\>\[[0-9]+\]\<\/f\>", delegate (Match e) {
                                    return "";
                                });
                                text = Regex.Replace(text, @"\!{2,3}", delegate (Match e) {
                                    return "!";
                                });
                                text = Regex.Replace(text, @"\s{2,3}", delegate (Match e) {
                                    return " ";
                                });
                                text = Regex.Replace(text, @"\<h\>(?<text>([AaĄąBbCcĆćDdEeĘęFfGgHhIiJjKkLlŁłMmNnŃńOoÓóPpRrSsŚśTtUuWwYyZzŹźŻż\s\-\.\;\:\?\""\'\,])+)\<\/h\>", delegate (Match e) {
                                    chapter.Items.Add(
                                        new FormattedText(RemoveOrphans(e.Groups["text"].Value))
                                    );
                                    return "";
                                });
                                text = Regex.Replace(text, @"\<n\>(?<text>.+)\<\/n\>", delegate (Match e) {
                                    var footnoteText = e.Groups["text"].Value;
                                    if (footnoteText.IsNotNullOrEmpty()) {
                                        verse.Items.Add(new NoteModel() {
                                            Number = fn.ToString(),
                                            Type = NoteType.Default,
                                            Items = new List<object> {
                                            footnoteText
                                        }
                                        });
                                        fn++;
                                    }
                                    return "";
                                });

                                //text = text.Replace("<", "").Replace(">", "");

                                if (text.IsNotNullOrEmpty()) {
                                    text = RemoveOrphans(text);
                                    verse.Items.Add(new SpanModel(text));
                                }
                                chapter.Items.Add(verse);
                            }
                        }
                    }

                    using (var service = new LogosBibleModelService()) {
                        var bibleModelOutFilePath = @$"D:\OneDrive\WBST\2020\Fakultety\Biblistyka\EIB\SNP_BibleEngine\SNP_BibleEngine\SNPD\{dbTranslation.Name.Replace("'", "").Replace("+", "")}.docx";
                        model = service.Export(model, bibleModelOutFilePath, true);
                        srv.SaveBibleModelToFile(model, @$"D:\OneDrive\WBST\2020\Fakultety\Biblistyka\EIB\SNP_BibleEngine\SNP_BibleEngine\SNPD\{dbTranslation.Name.Replace("'", "").Replace("+", "")}.xml");
                        //System.Diagnostics.Process.Start("explorer.exe", bibleModelOutFilePath);
                    }
                }
            }
        }

        [TestMethod]
        public void TestNoteText() {
            var text = "bezładna i pusta, וּהֹב ָו  וּהֹת ( tohu wawohu ),  tak  opisywane  są  skutki  sądu,  np.  Iz 34:10-11; Jr 4:23. Odczyt w. 2: Ziemia zaś stała się bezładna i pusta i (l. tak, że ) ciemność [rozciągała się] nad otchłanią, a Duch Boży unosił się nad powierzchnią wód, służy za uzasadnienie teorii stworzenia – odnowy, głoszącej,  że  Bóg  stworzył  niebo  i  ziemię, doszło do katastrofy (zob. Iz 45:18; Ez 28:1119), a w. 3 rozpoczyna opis procesu odnowy. Stworzył, א ָר ָבּ ( bara’ ), lub: ukształtował, por.  Rdz  1:27; א ָר ָבּ  zawsze  opisuje  działanie Boga, jak również tworzenie czegoś na nowo, zob. Ps 51:12; Iz 43:15; 65:17.";
            var bservice = new BibleModelService();
            var result = bservice.RecognizeHebrewAndGreekAndBibleTags(text);
            if (result != null) {
                Assert.IsTrue(result.Length > 0);
            }
            else {
                Assert.Fail();
            }
        }
    }
}