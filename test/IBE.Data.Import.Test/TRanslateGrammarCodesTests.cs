using DevExpress.Xpo;
using IBE.Common.Extensions;
using IBE.Data.Model;
using IBE.Data.Model.Grammar;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBE.Data.Import.Test {
    [TestClass]
    public class TranslateGrammarCodesTests {

        [TestMethod]
        public void TranslateGrammarCode() {
            ConnectionHelper.Connect();
            var uow = new UnitOfWork();
            uow.BeginTransaction();
            new XPQuery<GrammarCode>(uow).Where(x => x.GrammarCodeVariant1!=null && x.GrammarCodeVariant1.StartsWith("A-")).ToList().ForEach(x => RecognizeAdjectives(x)); ;
            uow.CommitChanges();
        }

        private void RecognizeAdjectives(GrammarCode code) {
            if (code.GrammarCodeVariant1.StartsWith("A-")) {
                
                code.PartOfSpeech = PartOfSpeechType.Adjective;

                var codeText = code.GrammarCodeVariant1;
                if (codeText.StartsWith("A-NU")) {
                    code.PartOfSpeech = PartOfSpeechType.Indeclinable_NUmeral;
                    code.Number = NumberType.Plural;
                }

                if (codeText.Length > 3) {
                    var caseOfDeclination = codeText[2].ToString().GetEnumByCategory<CaseOfDeclinationType>();
                    if (caseOfDeclination != CaseOfDeclinationType.None) { code.CaseOfDeclination = caseOfDeclination; }

                    var number = codeText[3].ToString().GetEnumByCategory<NumberType>();
                    if (number != NumberType.None) { code.Number = number; }
                }

                if (codeText.Length > 4) {
                    var gender = codeText[4].ToString().GetEnumByCategory<GenderType>();
                    if (gender != GenderType.None) { code.Gender = gender; }
                }

                if (codeText.Length == 7) {
                    var degree = codeText[6].ToString().GetEnumByCategory<AdjectiveDegreeType>();
                    if (degree != AdjectiveDegreeType.None) { code.AdjectiveDegree = degree; }
                }
                else if (codeText.Length == 9) {
                    var form = codeText.Substring(6).GetEnumByCategory<FormType>();
                    if (form != FormType.None) { code.Form = form; }
                }

                code.Save();
            }
        }
    }
}
