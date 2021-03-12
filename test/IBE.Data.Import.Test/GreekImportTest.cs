using DevExpress.Xpo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IBE.Data.Import.Test {
    [TestClass]
    public class GreekImportTest {
        [TestMethod]
        public void ImportGreekStrong() {
            ConnectionHelper.Connect();
            var uow = new UnitOfWork();
            uow.BeginTransaction();
            new StrongsDictionaryImporter().Import(@"..\..\..\..\db\import\Strong-pl.dictionary.zip", uow);
            uow.CommitChanges();
        }

    

        //[TestMethod]
        //public void BuildGreekDictionary() {
        //    ConnectionHelper.Connect();
        //    using (var uow = new UnitOfWork()) {
        //        using (var builder = new GreekDictionaryBuilder()) {
        //            builder.Build(uow,
        //                @"C:\Users\krzysztof.radzimski\Documents\GitHub\InterlinearBibleEditor\epub\BYZ'2005+.SQLite3",
        //                @"C:\Users\krzysztof.radzimski\Documents\GitHub\InterlinearBibleEditor\epub\TRO.SQLite3");
        //        }              
        //    }
        //}

        //[TestMethod]
        //public void CreateGreekInterlinearBible() {
        //    File.Copy(@"C:\Users\krzysztof.radzimski\Documents\GitHub\InterlinearBibleEditor\db\IBE-base.SQLite3",
        //        @"C:\Users\krzysztof.radzimski\Documents\GitHub\InterlinearBibleEditor\db\IBE.SQLite3", true);

        //    ConnectionHelper.Connect();
        //    var uow = new UnitOfWork();
        //    new GreekNewTestamentInterlinearBuilder().Build(uow,
        //         @"C:\Users\krzysztof.radzimski\Documents\GitHub\InterlinearBibleEditor\epub\BYZ'2005+.SQLite3",
        //         @"C:\Users\krzysztof.radzimski\Documents\GitHub\InterlinearBibleEditor\epub\TRO.SQLite3",
        //         @"C:\Users\krzysztof.radzimski\Documents\GitHub\InterlinearBibleEditor\epub\PBPW.SQLite3");
        //}


        //[TestMethod]
        //public void TestBiz() {
        //    new GreekNewTestamentInterlinearBuilder().Test(@"C:\Users\krzysztof.radzimski\Documents\GitHub\InterlinearBibleEditor\epub\BYZ'2005+.SQLite3");
        //}


        //[TestMethod]
        //public void RecognizeXML() {
        //    var text = @"Ἐν <S>1722</S><m>PREP</m> ἀρχῇ<S>746</S><m>N-DSF</m> ἐποίησεν<S>4160</S><m>V-AAI-3S</m> ὁ<S>3588</S><m>T-NSM</m> θεὸς<S>2316</S><m>N-NSM</m> τὸν<S>3588</S><m>T-ASM</m> οὐρανὸν<S>3772</S><m>N-ASM</m> καὶ<S>2532</S><m>CONJ</m> τὴν<S>3588</S><m>T-ASF</m> γῆν<S>1065</S><m>N-ASF</m>.";
        //    XElement xml = XElement.Parse($"<verse>{text}</verse>");
        //    foreach (var node in xml.Nodes()) {
        //        if (node.NodeType == System.Xml.XmlNodeType.Text) {

        //        }
        //    }
        //}

        //[TestMethod]
        //public void TransliterateGreek() {
        //    // nie obsługuje znaku ῾ 
        //    // bedzie trzeba dopisać wstawianie na początku h jeżeli 
        //    // w słowie nad którąś literą znajduje się znak a 
        //    // słowo zaczyna się od samogłoski a, o, i, u, e...
        //    var text = "Ἐν ἀρχῇ ἐποίησεν ὁ θεὸς τὸν οὐρανὸν καὶ τὴν γῆν. ὅτι ἑαυτοῖς ἡ ἧς ἑαυτῷ οὗτος υἱὸν Υἱὸν κυριος";
        //    var transliterit = text.TransliterateAncientGreek();
        //    Assert.IsTrue(transliterit == "En arche epoiesen ho theos ton uranon kai ten gen. hoti heautois he hes heauto hutos hyion Hyion kyrios");
        //}
        //[TestMethod]
        //public void TransliterateGreekWord() {
        //    // nie obsługuje znaku ῾ 
        //    // bedzie trzeba dopisać wstawianie na początku h jeżeli 
        //    // w słowie nad którąś literą znajduje się znak a 
        //    // słowo zaczyna się od samogłoski a, o, i, u, e...
        //    var text = "ἑαυτῷ";
        //    var transliterit = text.TransliterateAncientGreek();
        //    Assert.IsTrue(transliterit == "heauto");
        //}
    }
}
