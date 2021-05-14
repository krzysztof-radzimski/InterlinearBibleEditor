using DevExpress.Xpo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBE.Data.Import.Test {
    [TestClass]
    public class TRanslateGrammarCodesTests {

        [TestMethod]
        public void TranslateGrammarCode() {
            ConnectionHelper.Connect();
            var uow = new UnitOfWork();
            uow.BeginTransaction();

            uow.CommitChanges();
        }

        //private GrammarCodeInfo UpdateAdjective(string code) {
        //    if (code.StartsWith("A-")) {

        //        var _case = code[2];


        //        var result = new GrammarCodeInfo() { };
        //    }

        //    return default;
        //}


    }

    public class GrammarCodeInfo {
        public string ShortDefinition { get; set; }
        public string GrammarCodeDescription { get; set; }

    }
}
