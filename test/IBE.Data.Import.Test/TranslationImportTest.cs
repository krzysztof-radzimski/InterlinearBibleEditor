using DevExpress.Xpo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBE.Data.Import.Test {
    [TestClass]
    public class TranslationImportTest {
        [TestMethod]
        public void Import_SNP18() {
            ConnectionHelper.Connect();
            var uow = new UnitOfWork();
            uow.BeginTransaction();
            new TranslationImporter().Import(@"C:\Users\krzysztof.radzimski\Documents\GitHub\InterlinearBibleEditor\epub\SNP'18.SQLite3", uow);
            uow.CommitChanges();
        }
    }
}
