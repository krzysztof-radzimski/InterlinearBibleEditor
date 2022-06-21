using DevExpress.Xpo;
using IBE.Common.Extensions;
using IBE.Data.Export.Model;
using IBE.Data.Model;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace IBE.Data.Export.Controllers {
    public class BibleTagController : IBibleTagController {
        public const string SIGLUM_PATTERN_STRICT = @"(?<book>([0-9]\s+)?[0-9\u0041-\u005A\u0061-\u007A\u00AA\u00B5\u00BA\u00C0-\u00D6\u00D8-\u00F6\u00F8-\u02C1\u02C6-\u02D1\u02E0-\u02E4\u02EC\u02EE\u0370-\u0374\u0376\u0377\u037A-\u037D\u0386\u0388-\u038A\u038C\u038E-\u03A1\u03A3-\u03F5\u03F7-\u0481\u048A-\u0527\u0531-\u0556\u0559\u0561-\u0587\u05D0-\u05EA\u05F0-\u05F2\u0620-\u064A\u066E\u066F\u0671-\u06D3\u06D5\u06E5\u06E6\u06EE\u06EF\u06FA-\u06FC\u06FF\u0710\u0712-\u072F\u074D-\u07A5\u07B1\u07CA-\u07EA\u07F4\u07F5\u07FA\u0800-\u0815\u081A\u0824\u0828\u0840-\u0858\u08A0\u08A2-\u08AC\u0904-\u0939\u093D\u0950\u0958-\u0961\u0971-\u0977\u0979-\u097F\u0985-\u098C\u098F\u0990\u0993-\u09A8\u09AA-\u09B0\u09B2\u09B6-\u09B9\u09BD\u09CE\u09DC\u09DD\u09DF-\u09E1\u09F0\u09F1\u0A05-\u0A0A\u0A0F\u0A10\u0A13-\u0A28\u0A2A-\u0A30\u0A32\u0A33\u0A35\u0A36\u0A38\u0A39\u0A59-\u0A5C\u0A5E\u0A72-\u0A74\u0A85-\u0A8D\u0A8F-\u0A91\u0A93-\u0AA8\u0AAA-\u0AB0\u0AB2\u0AB3\u0AB5-\u0AB9\u0ABD\u0AD0\u0AE0\u0AE1\u0B05-\u0B0C\u0B0F\u0B10\u0B13-\u0B28\u0B2A-\u0B30\u0B32\u0B33\u0B35-\u0B39\u0B3D\u0B5C\u0B5D\u0B5F-\u0B61\u0B71\u0B83\u0B85-\u0B8A\u0B8E-\u0B90\u0B92-\u0B95\u0B99\u0B9A\u0B9C\u0B9E\u0B9F\u0BA3\u0BA4\u0BA8-\u0BAA\u0BAE-\u0BB9\u0BD0\u0C05-\u0C0C\u0C0E-\u0C10\u0C12-\u0C28\u0C2A-\u0C33\u0C35-\u0C39\u0C3D\u0C58\u0C59\u0C60\u0C61\u0C85-\u0C8C\u0C8E-\u0C90\u0C92-\u0CA8\u0CAA-\u0CB3\u0CB5-\u0CB9\u0CBD\u0CDE\u0CE0\u0CE1\u0CF1\u0CF2\u0D05-\u0D0C\u0D0E-\u0D10\u0D12-\u0D3A\u0D3D\u0D4E\u0D60\u0D61\u0D7A-\u0D7F\u0D85-\u0D96\u0D9A-\u0DB1\u0DB3-\u0DBB\u0DBD\u0DC0-\u0DC6\u0E01-\u0E30\u0E32\u0E33\u0E40-\u0E46\u0E81\u0E82\u0E84\u0E87\u0E88\u0E8A\u0E8D\u0E94-\u0E97\u0E99-\u0E9F\u0EA1-\u0EA3\u0EA5\u0EA7\u0EAA\u0EAB\u0EAD-\u0EB0\u0EB2\u0EB3\u0EBD\u0EC0-\u0EC4\u0EC6\u0EDC-\u0EDF\u0F00\u0F40-\u0F47\u0F49-\u0F6C\u0F88-\u0F8C\u1000-\u102A\u103F\u1050-\u1055\u105A-\u105D\u1061\u1065\u1066\u106E-\u1070\u1075-\u1081\u108E\u10A0-\u10C5\u10C7\u10CD\u10D0-\u10FA\u10FC-\u1248\u124A-\u124D\u1250-\u1256\u1258\u125A-\u125D\u1260-\u1288\u128A-\u128D\u1290-\u12B0\u12B2-\u12B5\u12B8-\u12BE\u12C0\u12C2-\u12C5\u12C8-\u12D6\u12D8-\u1310\u1312-\u1315\u1318-\u135A\u1380-\u138F\u13A0-\u13F4\u1401-\u166C\u166F-\u167F\u1681-\u169A\u16A0-\u16EA\u1700-\u170C\u170E-\u1711\u1720-\u1731\u1740-\u1751\u1760-\u176C\u176E-\u1770\u1780-\u17B3\u17D7\u17DC\u1820-\u1877\u1880-\u18A8\u18AA\u18B0-\u18F5\u1900-\u191C\u1950-\u196D\u1970-\u1974\u1980-\u19AB\u19C1-\u19C7\u1A00-\u1A16\u1A20-\u1A54\u1AA7\u1B05-\u1B33\u1B45-\u1B4B\u1B83-\u1BA0\u1BAE\u1BAF\u1BBA-\u1BE5\u1C00-\u1C23\u1C4D-\u1C4F\u1C5A-\u1C7D\u1CE9-\u1CEC\u1CEE-\u1CF1\u1CF5\u1CF6\u1D00-\u1DBF\u1E00-\u1F15\u1F18-\u1F1D\u1F20-\u1F45\u1F48-\u1F4D\u1F50-\u1F57\u1F59\u1F5B\u1F5D\u1F5F-\u1F7D\u1F80-\u1FB4\u1FB6-\u1FBC\u1FBE\u1FC2-\u1FC4\u1FC6-\u1FCC\u1FD0-\u1FD3\u1FD6-\u1FDB\u1FE0-\u1FEC\u1FF2-\u1FF4\u1FF6-\u1FFC\u2071\u207F\u2090-\u209C\u2102\u2107\u210A-\u2113\u2115\u2119-\u211D\u2124\u2126\u2128\u212A-\u212D\u212F-\u2139\u213C-\u213F\u2145-\u2149\u214E\u2183\u2184\u2C00-\u2C2E\u2C30-\u2C5E\u2C60-\u2CE4\u2CEB-\u2CEE\u2CF2\u2CF3\u2D00-\u2D25\u2D27\u2D2D\u2D30-\u2D67\u2D6F\u2D80-\u2D96\u2DA0-\u2DA6\u2DA8-\u2DAE\u2DB0-\u2DB6\u2DB8-\u2DBE\u2DC0-\u2DC6\u2DC8-\u2DCE\u2DD0-\u2DD6\u2DD8-\u2DDE\u2E2F\u3005\u3006\u3031-\u3035\u303B\u303C\u3041-\u3096\u309D-\u309F\u30A1-\u30FA\u30FC-\u30FF\u3105-\u312D\u3131-\u318E\u31A0-\u31BA\u31F0-\u31FF\u3400-\u4DB5\u4E00-\u9FCC\uA000-\uA48C\uA4D0-\uA4FD\uA500-\uA60C\uA610-\uA61F\uA62A\uA62B\uA640-\uA66E\uA67F-\uA697\uA6A0-\uA6E5\uA717-\uA71F\uA722-\uA788\uA78B-\uA78E\uA790-\uA793\uA7A0-\uA7AA\uA7F8-\uA801\uA803-\uA805\uA807-\uA80A\uA80C-\uA822\uA840-\uA873\uA882-\uA8B3\uA8F2-\uA8F7\uA8FB\uA90A-\uA925\uA930-\uA946\uA960-\uA97C\uA984-\uA9B2\uA9CF\uAA00-\uAA28\uAA40-\uAA42\uAA44-\uAA4B\uAA60-\uAA76\uAA7A\uAA80-\uAAAF\uAAB1\uAAB5\uAAB6\uAAB9-\uAABD\uAAC0\uAAC2\uAADB-\uAADD\uAAE0-\uAAEA\uAAF2-\uAAF4\uAB01-\uAB06\uAB09-\uAB0E\uAB11-\uAB16\uAB20-\uAB26\uAB28-\uAB2E\uABC0-\uABE2\uAC00-\uD7A3\uD7B0-\uD7C6\uD7CB-\uD7FB\uF900-\uFA6D\uFA70-\uFAD9\uFB00-\uFB06\uFB13-\uFB17\uFB1D\uFB1F-\uFB28\uFB2A-\uFB36\uFB38-\uFB3C\uFB3E\uFB40\uFB41\uFB43\uFB44\uFB46-\uFBB1\uFBD3-\uFD3D\uFD50-\uFD8F\uFD92-\uFDC7\uFDF0-\uFDFB\uFE70-\uFE74\uFE76-\uFEFC\uFF21-\uFF3A\uFF41-\uFF5A\uFF66-\uFFBE\uFFC2-\uFFC7\uFFCA-\uFFCF\uFFD2-\uFFD7\uFFDA-\uFFDC]+)\s+(?<chapter>[0-9]+)\:(?<verseStart>[0-9]+)(\-(?<verseEnd>[0-9]+))?";
        public const string SIGLUM_PATTERN = @"((?<translation>[A-Z]{2,6}([0-9]+)?)\s+)?(?<book>([0-9]\s+)?[0-9\u0041-\u005A\u0061-\u007A\u00AA\u00B5\u00BA\u00C0-\u00D6\u00D8-\u00F6\u00F8-\u02C1\u02C6-\u02D1\u02E0-\u02E4\u02EC\u02EE\u0370-\u0374\u0376\u0377\u037A-\u037D\u0386\u0388-\u038A\u038C\u038E-\u03A1\u03A3-\u03F5\u03F7-\u0481\u048A-\u0527\u0531-\u0556\u0559\u0561-\u0587\u05D0-\u05EA\u05F0-\u05F2\u0620-\u064A\u066E\u066F\u0671-\u06D3\u06D5\u06E5\u06E6\u06EE\u06EF\u06FA-\u06FC\u06FF\u0710\u0712-\u072F\u074D-\u07A5\u07B1\u07CA-\u07EA\u07F4\u07F5\u07FA\u0800-\u0815\u081A\u0824\u0828\u0840-\u0858\u08A0\u08A2-\u08AC\u0904-\u0939\u093D\u0950\u0958-\u0961\u0971-\u0977\u0979-\u097F\u0985-\u098C\u098F\u0990\u0993-\u09A8\u09AA-\u09B0\u09B2\u09B6-\u09B9\u09BD\u09CE\u09DC\u09DD\u09DF-\u09E1\u09F0\u09F1\u0A05-\u0A0A\u0A0F\u0A10\u0A13-\u0A28\u0A2A-\u0A30\u0A32\u0A33\u0A35\u0A36\u0A38\u0A39\u0A59-\u0A5C\u0A5E\u0A72-\u0A74\u0A85-\u0A8D\u0A8F-\u0A91\u0A93-\u0AA8\u0AAA-\u0AB0\u0AB2\u0AB3\u0AB5-\u0AB9\u0ABD\u0AD0\u0AE0\u0AE1\u0B05-\u0B0C\u0B0F\u0B10\u0B13-\u0B28\u0B2A-\u0B30\u0B32\u0B33\u0B35-\u0B39\u0B3D\u0B5C\u0B5D\u0B5F-\u0B61\u0B71\u0B83\u0B85-\u0B8A\u0B8E-\u0B90\u0B92-\u0B95\u0B99\u0B9A\u0B9C\u0B9E\u0B9F\u0BA3\u0BA4\u0BA8-\u0BAA\u0BAE-\u0BB9\u0BD0\u0C05-\u0C0C\u0C0E-\u0C10\u0C12-\u0C28\u0C2A-\u0C33\u0C35-\u0C39\u0C3D\u0C58\u0C59\u0C60\u0C61\u0C85-\u0C8C\u0C8E-\u0C90\u0C92-\u0CA8\u0CAA-\u0CB3\u0CB5-\u0CB9\u0CBD\u0CDE\u0CE0\u0CE1\u0CF1\u0CF2\u0D05-\u0D0C\u0D0E-\u0D10\u0D12-\u0D3A\u0D3D\u0D4E\u0D60\u0D61\u0D7A-\u0D7F\u0D85-\u0D96\u0D9A-\u0DB1\u0DB3-\u0DBB\u0DBD\u0DC0-\u0DC6\u0E01-\u0E30\u0E32\u0E33\u0E40-\u0E46\u0E81\u0E82\u0E84\u0E87\u0E88\u0E8A\u0E8D\u0E94-\u0E97\u0E99-\u0E9F\u0EA1-\u0EA3\u0EA5\u0EA7\u0EAA\u0EAB\u0EAD-\u0EB0\u0EB2\u0EB3\u0EBD\u0EC0-\u0EC4\u0EC6\u0EDC-\u0EDF\u0F00\u0F40-\u0F47\u0F49-\u0F6C\u0F88-\u0F8C\u1000-\u102A\u103F\u1050-\u1055\u105A-\u105D\u1061\u1065\u1066\u106E-\u1070\u1075-\u1081\u108E\u10A0-\u10C5\u10C7\u10CD\u10D0-\u10FA\u10FC-\u1248\u124A-\u124D\u1250-\u1256\u1258\u125A-\u125D\u1260-\u1288\u128A-\u128D\u1290-\u12B0\u12B2-\u12B5\u12B8-\u12BE\u12C0\u12C2-\u12C5\u12C8-\u12D6\u12D8-\u1310\u1312-\u1315\u1318-\u135A\u1380-\u138F\u13A0-\u13F4\u1401-\u166C\u166F-\u167F\u1681-\u169A\u16A0-\u16EA\u1700-\u170C\u170E-\u1711\u1720-\u1731\u1740-\u1751\u1760-\u176C\u176E-\u1770\u1780-\u17B3\u17D7\u17DC\u1820-\u1877\u1880-\u18A8\u18AA\u18B0-\u18F5\u1900-\u191C\u1950-\u196D\u1970-\u1974\u1980-\u19AB\u19C1-\u19C7\u1A00-\u1A16\u1A20-\u1A54\u1AA7\u1B05-\u1B33\u1B45-\u1B4B\u1B83-\u1BA0\u1BAE\u1BAF\u1BBA-\u1BE5\u1C00-\u1C23\u1C4D-\u1C4F\u1C5A-\u1C7D\u1CE9-\u1CEC\u1CEE-\u1CF1\u1CF5\u1CF6\u1D00-\u1DBF\u1E00-\u1F15\u1F18-\u1F1D\u1F20-\u1F45\u1F48-\u1F4D\u1F50-\u1F57\u1F59\u1F5B\u1F5D\u1F5F-\u1F7D\u1F80-\u1FB4\u1FB6-\u1FBC\u1FBE\u1FC2-\u1FC4\u1FC6-\u1FCC\u1FD0-\u1FD3\u1FD6-\u1FDB\u1FE0-\u1FEC\u1FF2-\u1FF4\u1FF6-\u1FFC\u2071\u207F\u2090-\u209C\u2102\u2107\u210A-\u2113\u2115\u2119-\u211D\u2124\u2126\u2128\u212A-\u212D\u212F-\u2139\u213C-\u213F\u2145-\u2149\u214E\u2183\u2184\u2C00-\u2C2E\u2C30-\u2C5E\u2C60-\u2CE4\u2CEB-\u2CEE\u2CF2\u2CF3\u2D00-\u2D25\u2D27\u2D2D\u2D30-\u2D67\u2D6F\u2D80-\u2D96\u2DA0-\u2DA6\u2DA8-\u2DAE\u2DB0-\u2DB6\u2DB8-\u2DBE\u2DC0-\u2DC6\u2DC8-\u2DCE\u2DD0-\u2DD6\u2DD8-\u2DDE\u2E2F\u3005\u3006\u3031-\u3035\u303B\u303C\u3041-\u3096\u309D-\u309F\u30A1-\u30FA\u30FC-\u30FF\u3105-\u312D\u3131-\u318E\u31A0-\u31BA\u31F0-\u31FF\u3400-\u4DB5\u4E00-\u9FCC\uA000-\uA48C\uA4D0-\uA4FD\uA500-\uA60C\uA610-\uA61F\uA62A\uA62B\uA640-\uA66E\uA67F-\uA697\uA6A0-\uA6E5\uA717-\uA71F\uA722-\uA788\uA78B-\uA78E\uA790-\uA793\uA7A0-\uA7AA\uA7F8-\uA801\uA803-\uA805\uA807-\uA80A\uA80C-\uA822\uA840-\uA873\uA882-\uA8B3\uA8F2-\uA8F7\uA8FB\uA90A-\uA925\uA930-\uA946\uA960-\uA97C\uA984-\uA9B2\uA9CF\uAA00-\uAA28\uAA40-\uAA42\uAA44-\uAA4B\uAA60-\uAA76\uAA7A\uAA80-\uAAAF\uAAB1\uAAB5\uAAB6\uAAB9-\uAABD\uAAC0\uAAC2\uAADB-\uAADD\uAAE0-\uAAEA\uAAF2-\uAAF4\uAB01-\uAB06\uAB09-\uAB0E\uAB11-\uAB16\uAB20-\uAB26\uAB28-\uAB2E\uABC0-\uABE2\uAC00-\uD7A3\uD7B0-\uD7C6\uD7CB-\uD7FB\uF900-\uFA6D\uFA70-\uFAD9\uFB00-\uFB06\uFB13-\uFB17\uFB1D\uFB1F-\uFB28\uFB2A-\uFB36\uFB38-\uFB3C\uFB3E\uFB40\uFB41\uFB43\uFB44\uFB46-\uFBB1\uFBD3-\uFD3D\uFD50-\uFD8F\uFD92-\uFDC7\uFDF0-\uFDFB\uFE70-\uFE74\uFE76-\uFEFC\uFF21-\uFF3A\uFF41-\uFF5A\uFF66-\uFFBE\uFFC2-\uFFC7\uFFCA-\uFFCF\uFFD2-\uFFD7\uFFDA-\uFFDC]+)(\s+(?<chapter>[0-9]+))?(\:(?<verseStart>[0-9]+))?(\-(?<verseEnd>[0-9]+))?";
        public string AppendNonBreakingSpaces(string text) {
            text = Regex.Replace(text, @"[\s\(\,\;][a,i,o,w,z]\s", delegate (Match m) {
                return " " + m.Value.Trim() + "&nbsp;";
            }, RegexOptions.IgnoreCase);

            return text;
        }
        public string CleanVerseText(string text) {
            text = Regex.Replace(text, @"\<a\s+href\=[\""\'].+[\""\']\>(?<value>.+)\<\/a\>", delegate (Match m) {
                return m.Groups["value"].Value;
            }, RegexOptions.IgnoreCase);
            return text.Replace("</t>", "")
                       .Replace("<t>", "")
                       .Replace("<pb/>", "")
                       .Replace("<n>", "")
                       .Replace("</n>", "")
                       .Replace("<e>", "")
                       .Replace("</e>", "")
                       .Replace("―", "")
                       .Replace('\'', '”')
                       .Replace("<J>", "")
                       .Replace("</J>", "")
                       .Replace("<i>", "")
                       .Replace("</i>", "");
        }
        public string GetVerseSimpleText(string verseText, VerseIndex index, string baseBookShortcut) {
            var translation = index.TranslationName;
            var simpleText = verseText.Replace("</t>", "").Replace("<t>", "").Replace("<pb/>", "").Replace("<n>", "").Replace("</n>", "").Replace("<e>", "").Replace("</e>", "").Replace("―", "").Replace('\'', ' ').Replace("<J>", "").Replace("</J>", "").Replace("<i>", "").Replace("</i>", "");
            if (translation == "NPI" || translation == "IPD") {
                simpleText = simpleText.Replace("―", "");
            }
            if (translation == "PBD") { translation = "SNPPD"; }
            simpleText = Regex.Replace(simpleText, @"\<f\>\[[0-9]+\]\<\/f\>", "");
            simpleText = $"{baseBookShortcut} {index.NumberOfChapter}:{index.NumberOfVerse} „{simpleText}” ({translation})";
            return simpleText;
        }

        public string GetMultiChapterRangeText(string input, TranslationControllerModel model) {
            var pattern = @"\<x\>(?<book>[0-9]+)\s(?<chapterStart>[0-9]+)(\s)?\:(\s)?(?<verseStart>[0-9]+)(\s)?\-(\s)?(?<chapterEnd>[0-9]+)(\s)?\:(\s)?(?<verseEnd>[0-9]+)";
            input = Regex.Replace(input, pattern, delegate (Match m) {
                var translationName = model.Translation.Name.Replace("+", "").Replace("'", "");
                var numberOfBook = m.Groups["book"].Value.ToInt();
                var bookShortcut = model.Books.Where(x => x.NumberOfBook == numberOfBook).First().BookShortcut;
                var chapterStart = m.Groups["chapterStart"].Value.ToInt();
                var chapterEnd = m.Groups["chapterEnd"].Value.ToInt();
                var verseStart = m.Groups["verseStart"].Value.ToInt();
                var verseEnd = m.Groups["verseEnd"].Value.ToInt();

                return $"{bookShortcut} {chapterStart}:{verseStart}-{chapterEnd}:{verseEnd}";
            });
            return input;
        }
        public string GetMultiChapterRangeHtml(string input, TranslationControllerModel model) {
            var pattern = @"\<x\>(?<book>[0-9]+)\s(?<chapterStart>[0-9]+)(\s)?\:(\s)?(?<verseStart>[0-9]+)(\s)?\-(\s)?(?<chapterEnd>[0-9]+)(\s)?\:(\s)?(?<verseEnd>[0-9]+)";
            input = Regex.Replace(input, pattern, delegate (Match m) {
                var translationName = model.Translation.Name.Replace("+", "").Replace("'", "");
                var numberOfBook = m.Groups["book"].Value.ToInt();
                var bookShortcut = model.Books.Where(x => x.NumberOfBook == numberOfBook).First().BookShortcut;
                var chapterStart = m.Groups["chapterStart"].Value.ToInt();
                var chapterEnd = m.Groups["chapterEnd"].Value.ToInt();
                var verseStart = m.Groups["verseStart"].Value.ToInt();
                var verseEnd = m.Groups["verseEnd"].Value.ToInt();

                return $"<a href=\"/{translationName}/{numberOfBook}/{chapterStart}/{verseStart}\">{bookShortcut}&nbsp;{chapterStart}:{verseStart}</a>-<a href=\"/{translationName}/{numberOfBook}/{chapterEnd}/{verseEnd}\">{chapterEnd}:{verseEnd}</a>";
            });
            return input;
        }

        public string GetInternalVerseListText(string input, TranslationControllerModel model) {
            var pattern = @"\<x\>(?<book>[0-9]+)\s(?<chapter>[0-9]+)(\s)?\:(\s)?(?<verse1>[0-9]+)(\,)(\s)?(?<verse2>[0-9]+)(\,)?(\s)?(?<verse3>[0-9]+)?(\,)?(\s)?(?<verse4>[0-9]+)?(\,)?(\s)?(?<verse5>[0-9]+)?(\,)?(\s)?(?<verse6>[0-9]+)?(\,)?(\s)?(?<verse7>[0-9]+)?(\,)?(\s)?(?<verse8>[0-9]+)?(\,)?(\s)?(?<verse9>[0-9]+)?\<\/x\>";
            input = Regex.Replace(input, pattern, delegate (Match m) {
                var translationName = model.Translation.Name.Replace("+", "").Replace("'", "");
                var numberOfBook = m.Groups["book"].Value.ToInt();
                var bookShortcut = model.Books.Where(x => x.NumberOfBook == numberOfBook).First().BookShortcut;
                var numberOfChapter = m.Groups["chapter"].Value.ToInt();

                var text = String.Empty;
                var preText = $"{bookShortcut} {numberOfChapter}:";

                text += $"{preText}{m.Groups["verse1"].Value}";

                for (int i = 2; i < 10; i++) {
                    if (m.Groups[$"verse{i}"] != null && m.Groups[$"verse{i}"].Success) {
                        text += $", {m.Groups[$"verse{i}"].Value}";
                    }
                }

                return text;
            });
            return input;
        }
        public string GetInternalVerseListHtml(string input, TranslationControllerModel model) {
            var pattern = @"\<x\>(?<book>[0-9]+)\s(?<chapter>[0-9]+)(\s)?\:(\s)?(?<verse1>[0-9]+)(\,)(\s)?(?<verse2>[0-9]+)(\,)?(\s)?(?<verse3>[0-9]+)?(\,)?(\s)?(?<verse4>[0-9]+)?(\,)?(\s)?(?<verse5>[0-9]+)?(\,)?(\s)?(?<verse6>[0-9]+)?(\,)?(\s)?(?<verse7>[0-9]+)?(\,)?(\s)?(?<verse8>[0-9]+)?(\,)?(\s)?(?<verse9>[0-9]+)?\<\/x\>";
            input = Regex.Replace(input, pattern, delegate (Match m) {
                var translationName = model.Translation.Name.Replace("+", "").Replace("'", "");
                var numberOfBook = m.Groups["book"].Value.ToInt();
                var bookShortcut = model.Books.Where(x => x.NumberOfBook == numberOfBook).First().BookShortcut;
                var numberOfChapter = m.Groups["chapter"].Value.ToInt();

                var html = String.Empty;
                var preLink = $"<a href=\"/{translationName}/{numberOfBook}/{numberOfChapter}/";
                var preText = $"{bookShortcut}&nbsp;{numberOfChapter}:";

                html += $"{preLink}{m.Groups["verse1"].Value}\">{preText}{m.Groups["verse1"].Value}</a>";

                for (int i = 2; i < 10; i++) {
                    if (m.Groups[$"verse{i}"] != null && m.Groups[$"verse{i}"].Success) {
                        html += $", {preLink}{m.Groups[$"verse{i}"].Value}\">{m.Groups[$"verse{i}"].Value}</a>";
                    }
                }

                return html;
            });
            return input;
        }

        public string GetInternalVerseRangeHtml(string input, TranslationControllerModel model) {
            input = Regex.Replace(input, @"\<x\>(?<book>[0-9]+)\s(?<chapter>[0-9]+)(\s)?\:(\s)?(?<verseStart>[0-9]+)\-(?<verseEnd>[0-9]+)\<\/x\>", delegate (Match m) {
                var translationName = model.Translation.Name.Replace("+", "").Replace("'", "");
                var numberOfBook = m.Groups["book"].Value.ToInt();
                var bookShortcut = model.Books.Where(x => x.NumberOfBook == numberOfBook).First().BookShortcut;
                var numberOfChapter = m.Groups["chapter"].Value.ToInt();
                var verseStart = m.Groups["verseStart"].Value.ToInt();
                var verseEnd = m.Groups["verseEnd"].Value.ToInt();
                var versesText = String.Empty;
                for (int i = verseStart; i <= verseEnd; i++) {
                    versesText += $"{i}";
                    if (i != verseEnd) { versesText += ","; }
                }

                var verseText = String.Empty;

                var link = $"<a href=\"/{translationName}/{numberOfBook}/{numberOfChapter}/{versesText}\">{bookShortcut}&nbsp;{numberOfChapter}:{verseStart}-{verseEnd}</a>";

                if (verseText.IsNotNullOrEmpty()) {
                    return $"{link} - <i>{verseText}</i>";
                }
                return link;
            });
            return input;
        }
        public string GetInternalVerseRangeText(string input, TranslationControllerModel model) {
            input = Regex.Replace(input, @"\<x\>(?<book>[0-9]+)\s(?<chapter>[0-9]+)(\s)?\:(\s)?(?<verseStart>[0-9]+)\-(?<verseEnd>[0-9]+)\<\/x\>", delegate (Match m) {
                var translationName = model.Translation.Name.Replace("+", "").Replace("'", "");
                var numberOfBook = m.Groups["book"].Value.ToInt();
                var bookShortcut = model.Books.Where(x => x.NumberOfBook == numberOfBook).First().BookShortcut;
                var numberOfChapter = m.Groups["chapter"].Value.ToInt();
                var verseStart = m.Groups["verseStart"].Value.ToInt();
                var verseEnd = m.Groups["verseEnd"].Value.ToInt();

                return $"{bookShortcut} {numberOfChapter}:{verseStart}-{verseEnd}";
            });
            return input;
        }


        public string GetInternalVerseHtml(string input, TranslationControllerModel model) {
            input = Regex.Replace(input, @"\<x\>(?<book>[0-9]+)\s(?<chapter>[0-9]+)(\s)?\:(\s)?(?<verseStart>[0-9]+)\<\/x\>", delegate (Match m) {
                var translationName = model.Translation.Name.Replace("+", "").Replace("'", "");
                var numberOfBook = m.Groups["book"].Value.ToInt();
                var bookShortcut = model.Books.Where(x => x.NumberOfBook == numberOfBook).First().BookShortcut;
                var numberOfChapter = m.Groups["chapter"].Value.ToInt();
                var verseStart = m.Groups["verseStart"].Value.ToInt();

                var verseText = String.Empty;

                var link = $"<a href=\"/{translationName}/{numberOfBook}/{numberOfChapter}/{verseStart}\">{bookShortcut}&nbsp;{numberOfChapter}:{verseStart}</a>";

                if (verseText.IsNotNullOrEmpty()) {
                    return $"{link} - <i>{verseText}</i>";
                }
                return link;
            });
            return input;
        }
        public string GetInternalVerseText(string input, TranslationControllerModel model) {
            input = Regex.Replace(input, @"\<x\>(?<book>[0-9]+)\s(?<chapter>[0-9]+)(\s)?\:(\s)?(?<verseStart>[0-9]+)\<\/x\>", delegate (Match m) {
                var numberOfBook = m.Groups["book"].Value.ToInt();
                var bookShortcut = model.Books.Where(x => x.NumberOfBook == numberOfBook).First().BookShortcut;
                var numberOfChapter = m.Groups["chapter"].Value.ToInt();
                var verseStart = m.Groups["verseStart"].Value.ToInt();

                return $"{bookShortcut} {numberOfChapter}:{verseStart}";
            });
            return input;
        }

        public string GetExternalVerseRangeHtml(string input, TranslationControllerModel model) {
            input = Regex.Replace(input, @"\<x\>(?<translationName>[a-zA-Z0-9]+)\s(?<book>[0-9]+)\s(?<chapter>[0-9]+)(\s)?\:(\s)?(?<verseStart>[0-9]+)\-(?<verseEnd>[0-9]+)\<\/x\>", delegate (Match m) {
                var translationName = m.Groups["translationName"].Value;
                var numberOfBook = m.Groups["book"].Value.ToInt();
                var baseBook = model.Books.Where(x => x.NumberOfBook == numberOfBook).FirstOrDefault();
                var bookShortcut = baseBook.IsNotNull() ? baseBook.BookShortcut : "";
                var numberOfChapter = m.Groups["chapter"].Value.ToInt();
                var verseStart = m.Groups["verseStart"].Value.ToInt();
                var verseEnd = m.Groups["verseEnd"].Value.ToInt();
                var versesText = String.Empty;
                for (int i = verseStart; i <= verseEnd; i++) {
                    versesText += $"{i}";
                    if (i != verseEnd) { versesText += ","; }
                }

                var verseText = String.Empty;

                var link = $"<a href=\"/{translationName}/{numberOfBook}/{numberOfChapter}/{versesText}\">{bookShortcut}&nbsp;{numberOfChapter}:{verseStart}-{verseEnd}</a>";

                if (verseText.IsNotNullOrEmpty()) {
                    return $"{link} - <i>{verseText}</i>";
                }
                return link;
            });
            return input;
        }
        public string GetExternalVerseRangeText(string input, TranslationControllerModel model) {
            input = Regex.Replace(input, @"\<x\>(?<translationName>[a-zA-Z0-9]+)\s(?<book>[0-9]+)\s(?<chapter>[0-9]+)(\s)?\:(\s)?(?<verseStart>[0-9]+)\-(?<verseEnd>[0-9]+)\<\/x\>", delegate (Match m) {
                var numberOfBook = m.Groups["book"].Value.ToInt();
                var baseBook = model.Books.Where(x => x.NumberOfBook == numberOfBook).FirstOrDefault();
                var bookShortcut = baseBook.IsNotNull() ? baseBook.BookShortcut : "";
                var numberOfChapter = m.Groups["chapter"].Value.ToInt();
                var verseStart = m.Groups["verseStart"].Value.ToInt();
                var verseEnd = m.Groups["verseEnd"].Value.ToInt();

                return $"{bookShortcut} {numberOfChapter}:{verseStart}-{verseEnd}";
            });
            return input;
        }

        public string GetExternalVerseHtml(string input, TranslationControllerModel model) {
            input = Regex.Replace(input, @"\<x\>(?<translationName>[a-zA-Z0-9]+)\s(?<book>[0-9]+)\s(?<chapter>[0-9]+)(\s)?\:(\s)?(?<verseStart>[0-9]+)\<\/x\>", delegate (Match m) {
                var translationName = m.Groups["translationName"].Value;
                var numberOfBook = m.Groups["book"].Value.ToInt();
                var baseBook = model.Books.Where(x => x.NumberOfBook == numberOfBook).FirstOrDefault();
                var bookShortcut = baseBook.IsNotNull() ? baseBook.BookShortcut : "";
                var numberOfChapter = m.Groups["chapter"].Value.ToInt();
                var verseStart = m.Groups["verseStart"].Value.ToInt();

                var verseText = String.Empty;

                var link = $"<a href=\"/{translationName}/{numberOfBook}/{numberOfChapter}/{verseStart}\">{bookShortcut} {numberOfChapter}:{verseStart}</a>";
                if (verseText.IsNotNullOrEmpty()) {
                    return $"{link} - <i>{verseText}</i>";
                }
                return link;
            });
            return input;
        }
        public string GetExternalVerseText(string input, TranslationControllerModel model) {
            input = Regex.Replace(input, @"\<x\>(?<translationName>[a-zA-Z0-9]+)\s(?<book>[0-9]+)\s(?<chapter>[0-9]+)(\s)?\:(\s)?(?<verseStart>[0-9]+)\<\/x\>", delegate (Match m) {
                var translationName = m.Groups["translationName"].Value;
                var numberOfBook = m.Groups["book"].Value.ToInt();
                var baseBook = model.Books.Where(x => x.NumberOfBook == numberOfBook).FirstOrDefault();
                var bookShortcut = baseBook.IsNotNull() ? baseBook.BookShortcut : "";
                var numberOfChapter = m.Groups["chapter"].Value.ToInt();
                var verseStart = m.Groups["verseStart"].Value.ToInt();

                return $"{bookShortcut} {numberOfChapter}:{verseStart}";
            });
            return input;
        }

        public string GetVerseTranslation(Session session, int numberOfBook, int numberOfChapter, int verseStart, int verseEnd = 0, string translationName = "NPI") {
            if (verseEnd == 0) {
                var index = $"{translationName}.{numberOfBook}.{numberOfChapter}.{verseStart}";
                var verse = new XPQuery<Verse>(session).Where(x => x.Index == index).FirstOrDefault();
                if (verse.IsNotNull()) {
                    var verseText = verse.GetTranslationText();
                    if (verseText.IsNotNullOrWhiteSpace()) {
                        return verseText;
                    }
                    else {
                        return GetOtherVerseTranslation(session, numberOfBook, numberOfChapter, verseStart);
                    }
                }
                else {
                    return GetOtherVerseTranslation(session, numberOfBook, numberOfChapter, verseStart);
                }
            }
            else {
                var predicate = PredicateBuilder.New<Verse>();
                for (int i = verseStart; i < verseEnd + 1; i++) {
                    var index = $"{translationName}.{numberOfBook}.{numberOfChapter}.{i}";
                    predicate = predicate.Or(x => x.Index == index);
                }
                var verses = new XPQuery<Verse>(session).Where(predicate);

                if (verses.Count() > 0) {
                    if (verses.First().GetTranslationText().IsNotNullOrWhiteSpace()) {
                        var versesText = String.Empty;
                        foreach (var item in verses) {
                            versesText += item.GetTranslationText() + " ";
                        }
                        return versesText.Trim();
                    }
                    else {
                        return GetOtherVersesTranslation(session, numberOfBook, numberOfChapter, verseStart, verseEnd);
                    }
                }
                else {
                    return GetOtherVersesTranslation(session, numberOfBook, numberOfChapter, verseStart, verseEnd);
                }
            }
        }

        public string RepairStrongs(string input) {
            var result = input.Replace(@"href=""S:G", @"href=""/StrongsCode?id=G")
                .Replace(@"href=""S:H", @"href=""/StrongsCode?id=H")
                .Replace(@"href='S:G", @"href='/StrongsCode?id=G")
                .Replace(@"href='S:H", @"href='/StrongsCode?id=H");
            return result;
        }
        private string GetOtherVersesTranslation(Session session, int numberOfBook, int numberOfChapter, int verseStart, int verseEnd) {
            IEnumerable<Verse> verses = null;
            ExpressionStarter<Verse> predicate = null;
            if (numberOfBook > 460) {
                predicate = PredicateBuilder.New<Verse>();
                for (int i = verseStart; i < verseEnd + 1; i++) {
                    var index = $"PBPW.{numberOfBook}.{numberOfChapter}.{i}";
                    predicate = predicate.Or(x => x.Index == index);
                }

                verses = new XPQuery<Verse>(session).Where(predicate);
            }
            else {
                predicate = PredicateBuilder.New<Verse>();
                for (int i = verseStart; i < verseEnd + 1; i++) {
                    var index = $"SNP18.{numberOfBook}.{numberOfChapter}.{i}";
                    predicate = predicate.Or(x => x.Index == index);
                }

                verses = new XPQuery<Verse>(session).Where(predicate);
            }

            if (verses.Count() > 0) {
                var versesText = String.Empty;
                foreach (var item in verses) {
                    versesText += item.Text + " ";
                }
                versesText = Regex.Replace(versesText, @"\[[0-9]+\]", "");

                return versesText.Trim();
            }
            return String.Empty;
        }
        private string GetOtherVerseTranslation(Session session, int numberOfBook, int numberOfChapter, int verseStart) {
            var index = String.Empty;
            Verse verse = null;
            if (numberOfBook > 460) {
                index = $"PBPW.{numberOfBook}.{numberOfChapter}.{verseStart}";
                verse = new XPQuery<Verse>(session).Where(x => x.Index == index).FirstOrDefault();
            }
            else {
                index = $"SNP18.{numberOfBook}.{numberOfChapter}.{verseStart}";
                verse = new XPQuery<Verse>(session).Where(x => x.Index == index).FirstOrDefault();
            }
            if (verse.IsNotNull()) {
                var verseText = verse.Text;
                verseText = Regex.Replace(verseText, @"\[[0-9]+\]", "");

                return verseText;
            }
            return String.Empty;
        }

        public Verse GetRecognizedSiglumVerse(Session session, string input) {
            var siglum = RecognizeSiglum(input);
            if (siglum != null) {
                var books = new XPQuery<BookBase>(session).ToList();
                var baseBooks = books
                    .Where(x => x.StatusBookType == TheBookType.Bible)
                    .Select(x => new { x.NumberOfBook, x.BookShortcut, x.BookName });
                var baseBook = baseBooks.Where(x => x.BookShortcut.ToLower() == siglum.BookShortcut.ToLower()).FirstOrDefault();
                if (baseBook == null) {
                    baseBook = baseBooks.Where(x => x.BookName.ToLower() == siglum.BookShortcut.ToLower()).FirstOrDefault();
                }
                if (baseBook != null) {
                    var verseNumber = siglum.NumbersOfVerses.FirstOrDefault();
                    var index = $"{siglum.TranslationName}.{baseBook.NumberOfBook}.{siglum.NumberOfChapter}.{verseNumber}";
                    return new XPQuery<Verse>(session).Where(x => x.Index == index).FirstOrDefault();
                }
            }
            return default;
        }

        public string GetRecognizedSiglumUrl(Session session, string input) {
            var siglum = RecognizeSiglum(input);
            if (siglum != null) {
                var books = new XPQuery<BookBase>(session).ToList();
                var baseBooks = books
                    .Where(x => x.StatusBookType == TheBookType.Bible)
                    .Select(x => new { x.NumberOfBook, x.BookShortcut, x.BookName });
                var baseBook = baseBooks.Where(x => x.BookShortcut.ToLower() == siglum.BookShortcut.ToLower()).FirstOrDefault();
                if (baseBook == null) {
                    baseBook = baseBooks.Where(x => x.BookName.ToLower() == siglum.BookShortcut.ToLower()).FirstOrDefault();
                }
                if (baseBook != null) {
                    var result = $"/{siglum.TranslationName}/{baseBook.NumberOfBook}/{siglum.NumberOfChapter}";
                    if (siglum.NumbersOfVerses != null) {
                        if (siglum.NumbersOfVerses.Length == 1 && siglum.NumbersOfVerses[0] == 1) {
                            // nothing
                        }
                        else {
                            result += "/";
                            foreach (var verseNumber in siglum.NumbersOfVerses) {
                                result += $"{verseNumber},";
                            }
                            return result.Substring(0, result.Length - 1);
                        }
                    }
                    return result;
                }
            }
            return default;
        }

        public SiglumModel RecognizeSiglum(string input) {
            if (input.IsNotNullOrEmpty()) {
                var regex = new Regex(SIGLUM_PATTERN);
                var m = regex.Match(input);
                if (m.Success) {
                    var bookShortcut = m.Groups["book"].Success ? m.Groups["book"].Value : String.Empty;
                    bookShortcut = bookShortcut.Replace(" ", String.Empty);
                    switch (bookShortcut) {
                        case "1Moj":
                        case "1moj":
                        case "Rodz":
                        case "rodz": { bookShortcut = "Rdz"; break; }
                        case "2moj":
                        case "wyj":
                        case "2Moj":
                        case "Wyj": { bookShortcut = "Wj"; break; }
                        case "3moj":
                        case "kapł":
                        case "3Moj":
                        case "Kapł": { bookShortcut = "Kpł"; break; }
                        case "4moj":                        
                        case "4Moj": { bookShortcut = "Lb"; break; }
                        case "5moj":
                        case "5Moj": { bookShortcut = "Pwt"; break; }
                        case "sedz":
                        case "Sedz": { bookShortcut = "Sdz"; break; }
                        case "job":
                        case "Job": { bookShortcut = "Hi"; break; }
                        case "jer":
                        case "Jer": { bookShortcut = "Jr"; break; }
                        case "dan":
                        case "Dan": { bookShortcut = "Dn"; break; }
                        case "lam":
                        case "Lam": { bookShortcut = "Lm"; break; }
                        case "sof":
                        case "Sof": { bookShortcut = "So"; break; }
                        case "jon":
                        case "Jon": { bookShortcut = "Jo"; break; }
                        case "zach":
                        case "Zach": { bookShortcut = "Za"; break; }
                        case "mal":
                        case "Mal": { bookShortcut = "Ml"; break; }

                        case "mat":
                        case "Mat": { bookShortcut = "Mt"; break; }
                        case "mar":
                        case "Mar": { bookShortcut = "Mk"; break; }
                        case "łuk":
                        case "Łuk": { bookShortcut = "Łk"; break; }
                        case "jan":
                        case "Jan": { bookShortcut = "J"; break; }
                        case "dza":
                        case "Dza": { bookShortcut = "Dz"; break; }
                        case "rzym":
                        case "Rzym":
                        case "rzm":
                        case "Rzm": { bookShortcut = "Rz"; break; }
                        case "Gal": { bookShortcut = "Gl"; break; }
                        case "1tym":
                        case "1Tym": { bookShortcut = "1Tm"; break; }
                        case "2tym":
                        case "2Tym": { bookShortcut = "2Tm"; break; }
                        case "efz":
                        case "Efz": { bookShortcut = "Ef"; break; }
                        case "tyt":
                        case "Tyt": { bookShortcut = "Tt"; break; }
                        case "heb":
                        case "Heb": { bookShortcut = "Hbr"; break; }
                        case "jak":
                        case "Jak": { bookShortcut = "Jk"; break; }
                        case "1pi":
                        case "1Pi": { bookShortcut = "1P"; break; }
                        case "2pi":
                        case "2Pi": { bookShortcut = "2P"; break; }
                        case "1jan":
                        case "1Jan": { bookShortcut = "1J"; break; }
                        case "2jan":
                        case "2Jan": { bookShortcut = "2J"; break; }
                        case "3jan":
                        case "3Jan": { bookShortcut = "3J"; break; }
                        case "ap":
                        case "Ap": { bookShortcut = "Obj"; break; }
                    }

                    var translation = m.Groups["translation"].Success ? m.Groups["translation"].Value : "BW";
                    switch (translation) {
                        case "SNP": { translation = "SNP18"; break; }
                        case "UBG": { translation = "UBG18"; break; }
                        case "EKU": { translation = "EKU18"; break; }
                        case "NBG": { translation = "NBG18"; break; }
                        case "BT": { translation = "BT99"; break; }
                        case "PNS": { translation = "PNS1997"; break; }
                        case "POZ": { translation = "POZ75"; break; }
                        case "SNPD": { translation = "PBD"; break; }
                        case "SNPPD": { translation = "PBD"; break; }
                    }

                    var result = new SiglumModel() {
                        TranslationName = translation,
                        BookShortcut = bookShortcut,
                        NumberOfChapter = m.Groups["chapter"].Success ? m.Groups["chapter"].Value.ToInt() : 1
                    };

                    if (m.Groups["verseStart"].Success && m.Groups["verseEnd"].Success) {
                        var start = m.Groups["verseStart"].Value.ToInt();
                        var end = m.Groups["verseEnd"].Value.ToInt();
                        var list = new List<int>();
                        for (var i = start; i <= end; i++) {
                            list.Add(i);
                        }
                        result.NumbersOfVerses = list.ToArray();
                    }
                    else if (m.Groups["verseStart"].Success) {
                        result.NumbersOfVerses = new int[] { m.Groups["verseStart"].Value.ToInt() };
                    }
                    else {
                        result.NumbersOfVerses = new int[] { 1 };
                    }

                    return result;
                }
            }
            return default;
        }
    }
}
