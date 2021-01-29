using DevExpress.Xpo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace IBE.Data.Import.Test {
    [TestClass]
    public class GreekImportTest {
        [TestMethod]
        public void MainTest() {
            ConnectionHelper.Connect();
            var uow = new UnitOfWork();
            uow.BeginTransaction();
            new GreekImporter().Import(@"C:\Users\krzysztof.radzimski\Documents\GitHub\InterlinearBibleEditor\epub\28th-Novum-Testamentum-Graece-Nestle-Aland.epub", uow);
            uow.CommitChanges();
        }
    }
}
