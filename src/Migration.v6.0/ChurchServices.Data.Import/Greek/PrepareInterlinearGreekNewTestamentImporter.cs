using DevExpress.Xpo;
using ChurchServices.Extensions;
using ChurchServices.Data.Import.Greek;
using ChurchServices.Data.Model;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ChurchServices.Data.Import {
    public class PrepareInterlinearGreekNewTestamentImporter : BaseImporter<Translation> {
        const string NAME = "NPI+";
        GreekTransliterationController TransliterationController = new GreekTransliterationController();
        public Translation Import(string nestleZipFilePath, string ajZipFilePath, UnitOfWork uow) {
            if (File.Exists(nestleZipFilePath) && File.Exists(ajZipFilePath)) {
                var nestleFileName = ExtractAndGetFirstArchiveItemFilePath(nestleZipFilePath);
                var ajFileName = ExtractAndGetFirstArchiveItemFilePath(ajZipFilePath);
                try {
                    var nestleConn = new SqliteConnection($"DataSource=\"{nestleFileName}\"");
                    SQLitePCL.Batteries.Init();
                    nestleConn.Open();

                    var alConn = new SqliteConnection($"DataSource=\"{ajFileName}\"");
                    SQLitePCL.Batteries.Init();
                    alConn.Open();

                    InitializeGrammarCodes(uow);

                    var translation = CreateTranslation(uow);
                    CreateBooks(uow, nestleConn, alConn, translation);
                    nestleConn.Close();

                    return translation;
                }
                finally {
                    try { File.Delete(nestleFileName); } catch { }
                }
            }

            return default;
        }

        private void CreateBooks(UnitOfWork uow, SqliteConnection nestleConn, SqliteConnection ajConn, Translation translation) {
            var list = new List<dynamic>();

            using (var command = nestleConn.CreateCommand()) {
                command.CommandText = "select book_number, short_name, long_name, book_color from books_all";
                using (var reader = command.ExecuteReader()) {
                    while (reader.Read()) {
                        var numberOfBook = reader.GetInt32(0);
                        var shortName = reader.GetString(1);
                        var longName = reader.GetString(2);
                        var bookColor = reader.GetString(3);

                        list.Add(new {
                            BookName = longName,
                            BookShortcut = shortName,
                            NumberOfBook = numberOfBook,
                            Color = bookColor,
                            BaseBook = new XPQuery<BookBase>(uow).Where(x => x.NumberOfBook == numberOfBook).FirstOrDefault()
                        });
                    }
                }
            }

            foreach (var item in list) {
                var book = new Book(uow) {
                    BookName = item.BookName,
                    BookShortcut = item.BookShortcut,
                    NumberOfBook = item.NumberOfBook,
                    Color = item.Color,
                    BaseBook = item.BaseBook,
                    ParentTranslation = translation,
                };
                book.Save();

                // New Testament
                if (item.NumberOfBook >= 470) {
                    CreateChapters(uow, nestleConn, book);
                }
                // Old Testament
                else {
                    CreateChapters(uow, ajConn, book);
                }

                book.Save();
            }
        }

        private void CreateChapters(UnitOfWork uow, SqliteConnection conn, Book book) {
            var list = new List<dynamic>();

            using (var command = conn.CreateCommand()) {
                command.CommandText = $"select distinct chapter from verses where book_number = {book.NumberOfBook}";

                using (var reader = command.ExecuteReader()) {
                    while (reader.Read()) {
                        var chapterNumber = reader.GetInt32(0);
                        list.Add(new { NumberOfChapter = chapterNumber });
                    }
                }
            }

            book.NumberOfChapters = list.Count;

            foreach (var item in list) {
                var chapter = new Chapter(uow) {
                    NumberOfChapter = item.NumberOfChapter,
                    ParentBook = book
                };
                chapter.Save();

                CreateVerses(uow, conn, chapter);

                chapter.Save();
            }
        }

        private void CreateVerses(UnitOfWork uow, SqliteConnection conn, Chapter chapter) {
            var list = new List<InterlinearVerseInfo>();

            using (var command = conn.CreateCommand()) {
                command.CommandText = $"select verse, text from verses where book_number = {chapter.ParentBook.NumberOfBook} AND chapter = {chapter.NumberOfChapter}";

                using (var reader = command.ExecuteReader()) {
                    while (reader.Read()) {
                        var number = reader.GetInt32(0);
                        var text = reader.IsDBNull(1) ? String.Empty : reader.GetString(1);

                        list.Add(new InterlinearVerseInfo() { NumberOfVerse = number, Text = text });
                    }
                }
            }

            chapter.NumberOfVerses = list.Count;

            foreach (var item in list) {
                var words = new List<InterlinearVerseWordInfo>();

                var verse = new Verse(uow) {
                    NumberOfVerse = item.NumberOfVerse,
                    Text = String.Empty,
                    ParentChapter = chapter,
                    StartFromNewLine = item.Text.StartsWith("<pb/>")
                };

                verse.Save();

                //System.Diagnostics.Debug.WriteLine(item.Text);

                /*
                ἡ<S>3588</S><m>T-NSF</m> 
                δὲ<S>1161</S><m>PRT</m> 
                γῆ<S>1065</S><m>N-NSF</m> 
                ἦν<S>1510</S><m>V-IAI-3S</m> 
                ἀόρατος<S>517</S><m>A-NSM</m> 
                καὶ<S>2532</S><m>CONJ</m> 
                ἀκατασκεύαστος<m>A-NSM</m>, 
                καὶ<S>2532</S><m>CONJ</m> 
                σκότος<S>4655</S><m>N-NSN</m> 
                ἐπάνω<S>1883</S><m>PREP</m> 
                τῆς<S>3588</S><m>T-GSF</m> 
                ἀβύσσου<S>12</S><m>N-GSF</m>, 
                καὶ<S>2532</S><m>CONJ</m> πνεῦμα<S>4151</S><m>N-NSN</m> θεοῦ<S>2316</S><m>N-GSM</m> ἐπεφέρετο<S>2018</S><m>V-IMI-3S</m> ἐπάνω<S>1883</S><m>PREP</m> τοῦ<S>3588</S><m>T-GSN</m> 
                ὕδατος<S>5204</S><m>N-GSN</m>.                 
                 */

                /*
                 καὶ<S>2532</S><m>CONJ</m> ἐλθὼν<S>2064</S><m>V-2AAP-NSM</m> κατῴκησεν<S>2730</S><m>V-AAI-3S</m> 
                εἰς<S>1519</S><m>PREP</m> πόλιν<S>4172</S><m>N-ASF</m> λεγομένην<S>3004</S><m>V-PPP-ASF</m> 
                Ναζαρέτ·<S>3478</S><m>N-PRI</m> 
                ὅπως<S>3704</S><m>ADV</m> πληρωθῇ<S>4137</S><m>V-APS-3S</m> τὸ<S>3588</S><m>T-NSN</m> ῥηθὲν<S>2046</S><m>V-APP-NSN</m> διὰ<S>1223</S><m>PREP</m> τῶν<S>3588</S><m>T-GPM</m> προφητῶν<S>4396</S><m>N-GPM</m> ὅτι<S>3754</S><m>CONJ</m> Ναζωραῖος<S>3480</S><m>N-NSM</m> κληθήσεται.<S>2564</S><m>V-FPI-3S</m>
                 */
                try {
                    InterlinearVerseWordInfo word = null;
                    // <t> - wcięcie akapitu 
                    // <e> - cytowanie
                    var xml = XElement.Parse($"<v>{item.Text.Replace("<t>", "").Replace("</t>", "")}</v>");
                    words = RecognizeVerseText(word, words, xml);
                }
                catch (Exception ex) {
                    if (ex.IsNotNull()) { }
                }

                var index = 1;
                foreach (var word in words) {
                    var verseWord = new VerseWord(uow) {
                        GrammarCode = GetGrammarCode(uow, word.GrammarCode),
                        StrongCode = GetStrongCode(uow, word.Strong.ToInt(), Language.Greek),
                        SourceWord = word.Text,
                        ParentVerse = verse,
                        Citation = word.Citation,
                        Transliteration = TransliterationController.TransliterateWord(word.Text),
                        NumberOfVerseWord = index
                    };
                    verseWord.Save();
                    index++;
                }
            }
        }

        private List<InterlinearVerseWordInfo> RecognizeVerseText(InterlinearVerseWordInfo word, List<InterlinearVerseWordInfo> words, XElement xml, bool citation = false) {
            if (xml.IsNotNull()) {
                foreach (var node in xml.Nodes()) {
                    if (node.NodeType == System.Xml.XmlNodeType.Text) {
                        var wordText = (node as XText).Value.Trim();
                        if (wordText.StartsWith(",") || wordText.StartsWith(";") || wordText.StartsWith(".") || wordText.StartsWith("·")) {
                            // ten znak interpunkcyjny trzeba przenieść do poprzedniego słowa
                            if (word.IsNotNull()) {
                                word.Text += wordText.Substring(0, 1);
                                wordText = wordText.Substring(1).Trim();
                            }
                        }
                        if (wordText.IsNotNullOrEmpty()) {
                            word = new InterlinearVerseWordInfo() {
                                Text = wordText,
                                Citation = citation
                            };
                            words.Add(word);
                        }
                    }
                    else if (node.NodeType == System.Xml.XmlNodeType.Element) {
                        var el = node as XElement;
                        if (el.Name.LocalName == "S") {
                            if (word.IsNotNull()) { word.Strong = el.Value.Trim(); }
                        }
                        else if (el.Name.LocalName == "m") {
                            if (word.IsNotNull()) { word.GrammarCode = el.Value.Trim(); }
                        }
                        else if (el.Name.LocalName == "e") {
                            words = RecognizeVerseText(word, words, el, true);
                        }
                    }
                }
            }
            return words;
        }

        public override Translation Import(string zipFilePath, UnitOfWork uow) {
            throw new NotImplementedException();
        }

        private void InitializeGrammarCodes(UnitOfWork uow) {
            var codesText = @"A-APF . A-APF-C . A-APF-S . A-APM . A-APM-C . A-APM-S . A-APN . A-APN-C . A-APN-S . A-ASF . A-ASF-C . A-ASF-N . A-ASF-S . A-ASM . A-ASM-C . A-ASM-N . A-ASM-S . A-ASN . A-ASN-C . A-ASN-N . A-ASN-S . A-DPF . A-DPF-C . A-DPF-S . A-DPM . A-DPM-C . A-DPM-S . A-DPN . A-DPN-S . A-DSF . A-DSF-C . A-DSF-S . A-DSM . A-DSM-C . A-DSM-N . A-DSM-S . A-DSN . A-DSN-C . A-DSN-N . A-DSN-S . A-GMS . A-GPF . A-GPF-S . A-GPM . A-GPM-C . A-GPM-S . A-GPN . A-GPN-C . A-GPN-S . A-GSF . A-GSF-C . A-GSF-S . A-GSM . A-GSM-C . A-GSM-N . A-GSM-S . A-GSN . A-GSN-N . A-GSN-S . A-NPF . A-NPF-C . A-NPF-S . A-NPM . A-NPM-C . A-NPM-S . A-NPN . A-NPN-C . A-NPN-S . A-NSF . A-NSF-C . A-NSF-N . A-NSF-S . A-NSM . A-NSM-ATT . A-NSM-C . A-NSM-N . A-NSM-S . A-NSN . A-NSN-C . A-NSN-N . A-NSN-S . A-NUI . A-NUI-ABB . A-VPM . A-VSF . A-VSM . A-VSM-S . A-VSN . ADV . ADV-C . ADV-I . ADV-K . ADV-N . ADV-S . ARAM . C-APM . C-DPM . C-DPN . C-GPM . C-GPN . COND . COND-K . CONJ . CONJ-N . D-APF . D-APM . D-APM-K . D-APN . D-APN-C . D-APN-K . D-ASF . D-ASM . D-ASM-K . D-ASN . D-DPF . D-DPM . D-DPM-C . D-DPM-K . D-DPN . D-DSF . D-DSM . D-DSN . D-GPF . D-GPM . D-GPN . D-GSF . D-GSM . D-GSN . D-NPF . D-NPM . D-NPM-K . D-NPN . D-NPN-K . D-NSF . D-NSM . D-NSM-K . D-NSN . F-1APM . F-1ASM . F-1DPM . F-1DSM . F-1GPM . F-1GSM . F-2APF . F-2APM . F-2APN . F-2ASM . F-2ASM-C . F-2DPF . F-2DPM . F-2DSM . F-2GPM . F-2GSM . F-3APF . F-3APM . F-3APN . F-3ASF . F-3ASM . F-3ASN . F-3DPF . F-3DPM . F-3DSF . F-3DSM . F-3GPF . F-3GPM . F-3GPN . F-3GSF . F-3GSM . F-3GSN . F-GPF . F-GSM . HEB . I-APF . I-APM . I-APN . I-ASF . I-ASM . I-ASN . I-DPM . I-DSF . I-DSM . I-DSN . I-GPM . I-GPN . I-GSF . I-GSM . I-GSN . I-NPM . I-NPN . I-NSF . I-NSM . I-NSN . INJ . K-APF . K-APM . K-APN . K-ASM . K-ASN . K-DSN . K-GPM . K-GSN . K-NPF . K-NPM . K-NPN . K-NSM . K-NSN . N-APF . N-APF-C . N-APF-S . N-APM . N-APN . N-APN-ATT . N-APN-C . N-APN-S . N-ASF . N-ASF-C . N-ASF-S . N-ASM . N-ASM-S . N-ASN . N-DPF . N-DPM . N-DPM-S . N-DPN . N-DSF . N-DSM . N-DSN . N-GMP . N-GPF . N-GPM . N-GPN . N-GSF . N-GSM . N-GSN . N-LI . N-NAM . N-NPF . N-NPM . N-NPM-S . N-NPN . N-NSF . N-NSM . N-NSM-S . N-NSN . N-NSN-C . N-OI . N-PRI . N-PRI-ABB . N-VPF . N-VPM . N-VPN . N-VSF . N-VSM . N-VSN . P-1AP . P-1AS . P-1AS-K . P-1DP . P-1DS . P-1DS-K . P-1GP . P-1GS . P-1NP . P-1NS . P-1NS-K . P-2AP . P-2AS . P-2DP . P-2DS . P-2GP . P-2GS . P-2NP . P-2NS . P-APF . P-APM . P-APN . P-ASF . P-ASM . P-ASN . P-DPF . P-DPM . P-DPN . P-DSF . P-DSM . P-DSN . P-GPF . P-GPM . P-GPN . P-GSF . P-GSM . P-GSN . P-NPM . P-NPN . P-NSF . P-NSM . P-NSN . PREP . PRT . PRT-I . PRT-K . PRT-N . Q-APF . Q-APM . Q-APN . Q-ASF . Q-ASN . Q-DSN . Q-GPN . Q-NPF . Q-NPM . Q-NSM . Q-NSN . R-APF . R-APM . R-APN . R-ASF . R-ASM . R-ASM-P . R-ASN . R-DPF . R-DPM . R-DPN . R-DSF . R-DSM . R-DSN . R-GPF . R-GPM . R-GPN . R-GSF . R-GSM . R-GSN . R-GSN-ATT . R-NPF . R-NPM . R-NPN . R-NSF . R-NSM . R-NSN . . S-1APF . S-1APM . S-1APN . S-1ASF . S-1ASM . S-1ASN . S-1DPF . S-1DPM . S-1DPN . S-1DSF . S-1DSM . S-1DSN . S-1GPF . S-1GPN . S-1GSF . S-1NPF . S-1NPM . S-1NPN . S-1NSF . S-1NSM . S-1NSN . S-1PAPM . S-1PASF . S-1PASM . S-1PDPF . S-1PDPM . S-1PGPF . S-1PGSF . S-1PNPF . S-1PNPM . S-1PNSF . S-1PNSM . S-1SAPF . S-1SAPM . S-1SAPN . S-1SASF . S-1SASM . S-1SASN . S-1SDPN . S-1SDSF . S-1SDSM . S-1SDSN . S-1SGPN . S-1SGSF . S-1SGSN . S-1SNPM . S-1SNPN . S-1SNSF . S-1SNSM . S-1SNSN . S-2APF . S-2APM . S-2APN . S-2ASF . S-2ASN . S-2DPF . S-2DPM . S-2DSF . S-2DSM . S-2GPF . S-2GSF . S-2NPF . S-2NPM . S-2NPN . S-2NSM . S-2NSN . S-2PASF . S-2PASM . S-2PASN . S-2PDSF . S-2PDSM . S-2PDSN . S-2PGSF . S-2PNSF . S-2PNSM . S-2SAPM . S-2SAPN . S-2SASF . S-2SASN . S-2SDSF . S-2SDSM . S-2SDSN . S-2SGSF . S-2SNPM . S-2SNPN . S-2SNSM . S-2SNSN . T-APF . T-APM . T-APN . T-ASF . T-ASM . T-ASN . T-DPF . T-DPM . T-DPN . T-DSF . T-DSM . T-DSN . T-GPF . T-GPM . T-GPN . T-GSF . T-GSM . T-GSN . T-NPF . T-NPM . T-NPN . T-NSF . T-NSM . T-NSN . V-2AAI-1P . V-2AAI-1S . V-2AAI-2P . V-2AAI-2S . V-2AAI-3P . V-2AAI-3P-ATT . V-2AAI-3S . V-2AAM-2P . V-2AAM-2S . V-2AAM-2S-AP . V-2AAM-2S-ATT . V-2AAM-3P . V-2AAM-3S . V-2AAN . V-2AAO-3P . V-2AAO-3S . V-2AAP-APF . V-2AAP-APM . V-2AAP-ASF . V-2AAP-ASM . V-2AAP-ASN . V-2AAP-DPF . V-2AAP-DPM . V-2AAP-DSF . V-2AAP-DSM . V-2AAP-GPM . V-2AAP-GSF . V-2AAP-GSM . V-2AAP-GSN . V-2AAP-NPF . V-2AAP-NPM . V-2AAP-NPN . V-2AAP-NSF . V-2AAP-NSM . V-2AAP-NSN . V-2AAS-1P . V-2AAS-1S . V-2AAS-2P . V-2AAS-2S . V-2AAS-3P . V-2AAS-3S . V-2ADI-1S . V-2ADI-2P . V-2ADI-2S . V-2ADI-3P . V-2ADI-3S . V-2ADM-2P . V-2ADM-2S . V-2ADM-3S . V-2ADN . V-2ADO-1S . V-2ADO-3S . V-2ADP-APM . V-2ADP-APN . V-2ADP-ASF . V-2ADP-ASM . V-2ADP-ASN . V-2ADP-DPM . V-2ADP-DPN . V-2ADP-GPF . V-2ADP-GPM . V-2ADP-GPN . V-2ADP-GSF . V-2ADP-GSM . V-2ADP-GSN . V-2ADP-NPF . V-2ADP-NPM . V-2ADP-NSM . V-2ADS-1P . V-2ADS-1S . V-2ADS-2P . V-2ADS-3P . V-2ADS-3S . V-2AMI-1P . V-2AMI-1S . V-2AMI-2P . V-2AMI-2S . V-2AMI-3P . V-2AMI-3S . V-2AMM-2P . V-2AMM-2S . V-2AMN . V-2AMP-GSM . V-2AMP-NPM . V-2AMP-NSM . V-2AMS-1P . V-2AMS-1S . V-2AMS-2S . V-2AMS-3P . V-2AMS-3S . V-2AOI-1P . V-2AOI-1S . V-2AOI-2P . V-2AOI-3P . V-2AOI-3S . V-2AOM-2P . V-2AON . V-2AOS-2P . V-2API-1P . V-2API-1S . V-2API-2P . V-2API-2S . V-2API-3P . V-2API-3S . V-2APM-2P . V-2APM-2S . V-2APM-3S . V-2APN . V-2APP-ASM . V-2APP-DSN . V-2APP-GPM . V-2APP-NPF . V-2APP-NPM . V-2APP-NSF . V-2APP-NSM . V-2APP-NSN . V-2APS-1P . V-2APS-2P . V-2APS-2S . V-2APS-3P . V-2APS-3S . V-2AXP-GPM . V-2AXS-2P . V-2FAI-1S . V-2FDI-3P . V-2FMI-3S . V-2FOI-1S . V-2FOI-3P . V-2FOI-3S . V-2FPI-1P . V-2FPI-2P . V-2FPI-2S . V-2FPI-3P . V-2FPI-3S . V-2LAI-3S . V-2RAI-1P . V-2RAI-1P-ATT . V-2RAI-1S . V-2RAI-2P . V-2RAI-2P-ATT . V-2RAI-2S . V-2RAI-3P . V-2RAI-3P-ATT . V-2RAI-3P-C . V-2RAI-3S . V-2RAI-3S-ATT . V-2RAN . V-2RAP-1S . V-2RAP-APM . V-2RAP-APM-ATT . V-2RAP-APN . V-2RAP-ASF . V-2RAP-ASM . V-2RAP-ASN . V-2RAP-DSN . V-2RAP-NPM . V-2RAP-NSF . V-2RAP-NSM . V-2RAP-NSN . V-2RPP-ASM . V-AAI-1P . V-AAI-1S . V-AAI-2P . V-AAI-2S . V-AAI-3P . V-AAI-3S . V-AAM-2P . V-AAM-2S . V-AAM-3P . V-AAM-3S . V-AAN . V-AAO-3P . V-AAO-3P-A . V-AAO-3S . V-AAP-APM . V-AAP-APN . V-AAP-ASF . V-AAP-ASM . V-AAP-ASN . V-AAP-DPM . V-AAP-DSM . V-AAP-GPM . V-AAP-GSF . V-AAP-GSM . V-AAP-NPF . V-AAP-NPM . V-AAP-NSF . V-AAP-NSM . V-AAP-NSN . V-AAS-1P . V-AAS-1S . V-AAS-2P . V-AAS-2S . V-AAS-3P . V-AAS-3S . V-ADI-1P . V-ADI-1S . V-ADI-2P . V-ADI-2S . V-ADI-3P . V-ADI-3P-ATT . V-ADI-3S . V-ADI-3S-ATT . V-ADM-2P . V-ADM-2S . V-ADM-3P . V-ADM-3S . V-ADN . V-ADO-1S . V-ADP-APM . V-ADP-ASM . V-ADP-DPM . V-ADP-GSF . V-ADP-NPM . V-ADP-NSF . V-ADP-NSM . V-ADS-1P . V-ADS-1S . V-ADS-2P . V-ADS-2S . V-ADS-3P . V-ADS-3S . V-AMI-1P . V-AMI-1S . V-AMI-2P . V-AMI-2S . V-AMI-3P . V-AMI-3S . V-AMM-2P . V-AMM-2S . V-AMM-3S . V-AMN . V-AMP-APM . V-AMP-ASN . V-AMP-DPM . V-AMP-GPM . V-AMP-GSM . V-AMP-NPM . V-AMP-NSF . V-AMP-NSM . V-AMP-NSN . V-AMS-1P . V-AMS-1S . V-AMS-2P . V-AMS-2S . V-AMS-3P . V-AMS-3S . V-ANI-3P . V-ANI-3S . V-ANP-APN . V-ANP-NSM . V-ANP-NSN .. V-AOI-1P . V-AOI-1P-ATT . V-AOI-1S . V-AOI-1S-ATT . V-AOI-2P . V-AOI-2P-ATT . V-AOI-3P . V-AOI-3P-ATT . V-AOI-3S . V-AOI-3S-ATT . V-AOM-2P . V-AOM-2S . V-AOM-3S . V-AON . V-AOO-3S . V-AOP-APM . V-AOP-ASM . V-AOP-DSM . V-AOP-GPM . V-AOP-GPN . V-AOP-GSM . V-AOP-NPF . V-AOP-NPM . V-AOP-NSF . V-AOP-NSM . V-AOP-NSN . V-AOS-1P . V-AOS-1S . V-AOS-2P . V-AOS-2S . V-AOS-3P . V-AOS-3S . V-API-1P . V-API-1S . V-API-2P . V-API-2S . V-API-3P . V-API-3S . V-API-3S-M . V-APM-2P . V-APM-2S . V-APM-3P . V-APM-3S . V-APN . V-APN-M . V-APO-3S . V-APP-APM . V-APP-APN . V-APP-ASF . V-APP-ASM . V-APP-ASN . V-APP-DPN . V-APP-DSF . V-APP-DSM . V-APP-GPF . V-APP-GPM . V-APP-GPN . V-APP-GSF . V-APP-GSM . V-APP-GSN . V-APP-NPM . V-APP-NPN . V-APP-NSF . V-APP-NSM . V-APP-NSM-M . V-APP-NSN . V-APS-1P . V-APS-1S . V-APS-2P . V-APS-2S . V-APS-3P . V-APS-3S . V-FAI-1P . V-FAI-1S . V-FAI-1S-ATT . V-FAI-2P . V-FAI-2S . V-FAI-3P . V-FAI-3P-ATT . V-FAI-3S . V-FAI-3S-ATT . V-FAN . V-FAP-APN . V-FAP-GPM . V-FAP-NPM . V-FAP-NSM . V-FDI-1P . V-FDI-1S . V-FDI-2P . V-FDI-2P-ATT . V-FDI-2S . V-FDI-2S-ATT . V-FDI-3P . V-FDI-3S . V-FDI-3S-ATT . V-FDN . V-FDP-ASN . V-FDP-NPM . V-FMI-1P . V-FMI-1S . V-FMI-2P . V-FMI-3P . V-FMI-3S . V-FMS-1P . V-FNI-3P . V-FNI-3S . V-FOI-1S . V-FOI-3P . V-FOI-3S . V-FPI-1P . V-FPI-1S . V-FPI-2P . V-FPI-2S . V-FPI-3P . V-FPI-3S . V-FPP-GPN . V-FPS-1S . V-FXI-1P . V-FXI-1S . V-FXI-2P . V-FXI-2S . V-FXI-3P . V-FXI-3S . V-FXN . V-FXP-ASN . V-FXP-NSM . V-IAI-1P . V-IAI-1S . V-IAI-2P . V-IAI-2S . V-IAI-3P . V-IAI-3P-ATT . V-IAI-3S . V-IAI-3S-ATT . V-IDI-3P . V-IEI-3S . V-IMI-1P . V-IMI-1S . V-IMI-3P . V-IMI-3S . V-INI-1P . V-INI-1S . V-INI-2P . V-INI-2P-ATT . V-INI-2S . V-INI-3P . V-INI-3P-ATT . V-INI-3S . V-INI-3S-ATT . V-IPI-1P . V-IPI-1S . V-IPI-2P . V-IPI-3P . V-IPI-3S . V-IQI-3S . V-IXI-1P . V-IXI-1S . V-IXI-2P . V-IXI-2S . V-IXI-3P . V-IXI-3S . V-LAI-1S . V-LAI-2P . V-LAI-2S . V-LAI-3P . V-LAI-3P-ATT . V-LAI-3S . V-LAI-3S-ATT . V-LDI-3S . V-LMI-3P . V-LPI-3S . V-PAI-1P . V-PAI-1S . V-PAI-1S-C . V-PAI-2P . V-PAI-2S . V-PAI-2S-IRR . V-PAI-3P . V-PAI-3P-ATT . V-PAI-3S . V-PAM-1S . V-PAM-2P . V-PAM-2S . V-PAM-3P . V-PAM-3S . V-PAN . V-PAO-1S . V-PAO-2P . V-PAO-3P . V-PAO-3S . V-PAP-APF . V-PAP-APM . V-PAP-APN . V-PAP-ASF . V-PAP-ASM . V-PAP-ASN . V-PAP-DPF . V-PAP-DPM . V-PAP-DPN . V-PAP-DSF . V-PAP-DSM . V-PAP-DSN . V-PAP-GPF . V-PAP-GPM . V-PAP-GPN . V-PAP-GSF . V-PAP-GSM . V-PAP-GSN . V-PAP-NPF . V-PAP-NPM . V-PAP-NPN . V-PAP-NSF . V-PAP-NSM . V-PAP-NSN . V-PAP-VSM . V-PAS-1P . V-PAS-1S . V-PAS-2P . V-PAS-2S . V-PAS-3P . V-PAS-3S  . V-PDI-1P . V-PDI-1S . V-PDP-NPM . V-PEI-1P . V-PEI-1S . V-PEI-2P . V-PEI-3P . V-PEI-3S . V-PEM-2P . V-PEM-2S . V-PEN . V-PEP-DPM . V-PEP-DSM . V-PEP-GPN . V-PEP-GSF . V-PEP-GSM . V-PEP-NPF . V-PEP-NPM . V-PEP-NSF . V-PMI-1P . V-PMI-1S . V-PMI-2P . V-PMI-3P . V-PMI-3S . V-PMM-2P . V-PMM-2S . V-PMM-3S . V-PMN . V-PMP-APM . V-PMP-ASF . V-PMP-ASM . V-PMP-ASN . V-PMP-DPM . V-PMP-DSM . V-PMP-GPF . V-PMP-GPM . V-PMP-GSF . V-PMP-GSM . V-PMP-GSM-T . V-PMP-GSN . V-PMP-NPF . V-PMP-NPM . V-PMP-NSF . V-PMP-NSM . V-PMP-NSN . V-PMS-1P . V-PMS-1S . V-PMS-2S . V-PMS-3S . V-PNI-1P . V-PNI-1S . V-PNI-1S-C . V-PNI-2P . V-PNI-2S . V-PNI-2S-ATT . V-PNI-2S-C . V-PNI-3P . V-PNI-3S . V-PNI-3S-I . V-PNM-2P . V-PNM-2S . V-PNM-3P . V-PNM-3S . V-PNN . V-PNO-1S . V-PNO-3P . V-PNO-3S . V-PNP-APF . V-PNP-APM . V-PNP-APN . V-PNP-ASF . V-PNP-ASM . V-PNP-ASN . V-PNP-DPF . V-PNP-DPM . V-PNP-DPN . V-PNP-DSF . V-PNP-DSM . V-PNP-DSN . V-PNP-GPF . V-PNP-GPM . V-PNP-GPN . V-PNP-GSF . V-PNP-GSM . V-PNP-GSN . V-PNP-NPF . V-PNP-NPM . V-PNP-NPN . V-PNP-NSF . V-PNP-NSM . V-PNP-NSN . V-PNS-1P . V-PNS-1S . V-PNS-2P . V-PNS-2S . V-PNS-3P . V-PNS-3S . V-POI-1S . V-POP-NPM . V-PPI-1P . V-PPI-1S . V-PPI-2P . V-PPI-2S . V-PPI-2S-IRR . V-PPI-3P . V-PPI-3S . V-PPM-2P . V-PPM-2S . V-PPM-3P . V-PPM-3S . V-PPN . V-PPN-2P . V-PPP-APF . V-PPP-APM . V-PPP-APN . V-PPP-ASF . V-PPP-ASM . V-PPP-ASN . V-PPP-DPM . V-PPP-DPN . V-PPP-DSF . V-PPP-DSM . V-PPP-DSN . V-PPP-GPM . V-PPP-GPN . V-PPP-GSF . V-PPP-GSM . V-PPP-GSN . V-PPP-NPF . V-PPP-NPM . V-PPP-NPN . V-PPP-NSF . V-PPP-NSM . V-PPP-NSN . V-PPS-1P . V-PPS-1S . V-PPS-2P . V-PPS-3P . V-PPS-3S . V-PQI-3S . V-PQN . V-PQP-APN . V-PQP-NSN . V-PQS-3S . V-PXI-1P . V-PXI-1S . V-PXI-2P . V-PXI-2S . V-PXI-3P . V-PXI-3S . V-PXM-2S . V-PXM-3P . V-PXM-3S . V-PXN . V-PXO-2S . V-PXO-3S . V-PXP-APM . V-PXP-APN . V-PXP-ASF . V-PXP-ASM . V-PXP-ASN . V-PXP-DPM . V-PXP-DPN . V-PXP-DSF . V-PXP-DSM . V-PXP-GPF . V-PXP-GPM . V-PXP-GPN . V-PXP-GSF . V-PXP-GSM . V-PXP-GSN . V-PXP-NPF . V-PXP-NPM . V-PXP-NPN . V-PXP-NSF . V-PXP-NSM . V-PXP-NSN . V-PXS-1P . V-PXS-1S . V-PXS-2P . V-PXS-2S . V-PXS-3P . V-PXS-3S . V-RAI-1P . V-RAI-1P-ATT . V-RAI-1S . V-RAI-1S-ATT . V-RAI-2P . V-RAI-2P-ATT . V-RAI-2S . V-RAI-2S-ATT . V-RAI-3P . V-RAI-3P-ATT . V-RAI-3S . V-RAI-3S-ATT . V-RAM-2P . V-RAN . V-RAN-ATT . V-RAP-APM . V-RAP-APN . V-RAP-ASF . V-RAP-ASM . V-RAP-ASM-C . V-RAP-ASN . V-RAP-DPM . V-RAP-DSF . V-RAP-DSM . V-RAP-DSN . V-RAP-GPM . V-RAP-GPN . V-RAP-GSF . V-RAP-GSM . V-RAP-GSM-ATT . V-RAP-GSN-ATT . V-RAP-NPF . V-RAP-NPM . V-RAP-NPM-ATT . V-RAP-NPM-C . V-RAP-NPN . V-RAP-NSF . V-RAP-NSM . V-RAP-NSM-ATT . V-RAP-NSN . V-RAS-1P . V-RAS-1S . V-RAS-2P . V-RAS-2S . V-RDI-3S . V-RMI-2P . V-RMI-2S . V-RMI-3S . V-RMP-ASM . V-RMP-NPM . V-RMP-NSM . V-RNI-1P . V-RNI-1S . V-RNI-3S . V-RNP-APM . V-RNP-APN . V-RNP-ASF . V-RNP-DPF . V-RNP-GSF . V-RNP-NPM . V-RNP-NSM . V-RPI-1P . V-RPI-1S . V-RPI-2P . V-RPI-2S . V-RPI-3P . V-RPI-3S . V-RPM-2P . V-RPM-2S . V-RPN . V-RPP-APF . V-RPP-APM . V-RPP-APN . V-RPP-ASF . V-RPP-ASF-ATT . V-RPP-ASM . V-RPP-ASN . V-RPP-ASN-ATT . V-RPP-DPM . V-RPP-DPN . V-RPP-DSF . V-RPP-DSF-ATT . V-RPP-DSM . V-RPP-DSN . V-RPP-GPF . V-RPP-GPM . V-RPP-GPN . V-RPP-GSF . V-RPP-GSM . V-RPP-GSN . V-RPP-NPF . V-RPP-NPM . V-RPP-NPN . V-RPP-NSF . V-RPP-NSM . V-RPP-NSN . V-RPP-NSN-ATT . V-RPP-VSM . V-XXM-2P . V-XXM-2S . X-APF . X-APM . X-APN . X-ASF . X-ASM . X-ASN . X-DPM . X-DSF . X-DSM . X-DSN . X-GPF . X-GPM . X-GPN . X-GSF . X-GSM . X-GSN . X-NPF . X-NPM . X-NPN . X-NSF . X-NSM . X-NSN";
            var codes = codesText.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in codes) {
                var gc = new GrammarCode(uow) { GrammarCodeVariant1 = item.Trim() };
                gc.Save();
            }

            uow.CommitChanges();
            uow.ReloadChangedObjects();
        }

        private StrongCode GetStrongCode(UnitOfWork uow, int code, Language lang) {
            return new XPQuery<StrongCode>(uow).Where(x => x.Code == code && x.Lang == lang).FirstOrDefault();
        }
        private GrammarCode GetGrammarCode(UnitOfWork uow, string grammarCode) {
            var existing = new XPQuery<GrammarCode>(uow).Where(x => x.GrammarCodeVariant1 == grammarCode).FirstOrDefault();
            if (existing.IsNull()) {
                existing = new GrammarCode(uow) { GrammarCodeVariant1 = grammarCode };
                existing.Save();

                uow.CommitChanges();
                uow.ReloadChangedObjects();
            }
            return existing;
        }
        private Translation CreateTranslation(UnitOfWork uow) {
            var result = new XPQuery<Translation>(uow).Where(x => x.Name == NAME).FirstOrDefault();
            if (result.IsNull()) {
                result = new Translation(uow) {
                    Name = NAME,
                    Description = "Nowodworski Grecko-Polski Interlinearny Przekład Pisma Świętego Starego i Nowego Przymierza",
                    ChapterPsalmString = "Psalm",
                    ChapterString = "Rozdział",
                    DetailedInfo = @"W pracach nad przekładem wykorzystano:
<ul>
    <li>Przekład Grecki Nestle 1904 Greek New Testament z kodami Stronga</li>    
    <li>Analytic Septuagint z kodami Stronga i imieniem Bożym JHWH</li>    
    <li>Interlinearny Przekład Nowego Testamentu Textus Receptus Oblubienicy</li>    
    <li>Grecko-Polski Nowy Testament wydanie interlinearne z kluczem gramatycznym Stronga i Popowskiego oraz pełną transliteracją greckiego tekstu</li>
    <li>Przekład NT Popowskiego i Wojciechowskiego</li>    
    <li>Septuaginta Remigiusza Popowskiego SDB</li>
    <li>Przekłąd dosłowny EIB</li>
    <li>Biblia Pierwszego Kościoła ks. prof. zw. dr hab. Waldemara Chrostowskiego</li>
</ul>
",
                    Language = Language.Polish,
                    Type = TranslationType.Interlinear,
                    Introduction = "",
                    Catolic = false,
                    Recommended = true
                };
                result.Save();
                uow.CommitChanges();
                uow.ReloadChangedObjects();
            }
            return result;
        }


        class InterlinearVerseWordInfo {
            public string Text { get; set; }
            public string Strong { get; set; }
            public string GrammarCode { get; set; }
            public bool Citation { get; set; }
        }

        class InterlinearVerseInfo {
            public int NumberOfVerse { get; set; }
            public string Text { get; set; }

        }
    }
}
