using Church.WebApp.Models;
using System.Collections.Generic;
using IBE.Common.Extensions;
using DevExpress.Xpo;
using IBE.Data.Model;
using System.Linq;
using IBE.Data.Export.Model;

namespace Church.WebApp.Utils {
    public interface ITranslationInfoController {
        List<Grouping<string, TranslationInfo>> GetBibleTranslations();
        List<Grouping<string, TranslationInfo>> GetPatrologyTranslations();
    }
    public class TranslationInfoController : ITranslationInfoController {
        public TranslationInfoController() { }
        public List<Grouping<string, TranslationInfo>> GetBibleTranslations() {
            var view = new XPView(new UnitOfWork(), typeof(Translation)) {
                CriteriaString = "[BookType] = 1 AND [Hidden] = 0"
            };
            view.Properties.Add(new ViewProperty("Name", SortDirection.None, "[Name]", false, true));
            view.Properties.Add(new ViewProperty("Description", SortDirection.None, "[Description]", false, true));
            view.Properties.Add(new ViewProperty("Type", SortDirection.None, "[Type]", false, true));
            view.Properties.Add(new ViewProperty("Catolic", SortDirection.None, "[Catolic]", false, true));
            view.Properties.Add(new ViewProperty("Recommended", SortDirection.None, "[Recommended]", false, true));
            view.Properties.Add(new ViewProperty("OpenAccess", SortDirection.None, "[OpenAccess]", false, true));

            var model = new List<TranslationInfo>();
            foreach (ViewRecord item in view) {
                model.Add(new TranslationInfo() {
                    Type = (TranslationType)item["Type"],
                    Name = item["Name"].ToString().Replace("'", ""),
                    Description = item["Description"].ToString(),
                    Catholic = (bool)item["Catolic"],
                    Recommended = (bool)item["Recommended"],
                    PasswordRequired = !((bool)item["OpenAccess"])
                });
            }

            var _allTranslations = new List<Grouping<string, TranslationInfo>> {
                new Grouping<string, TranslationInfo>(TranslationType.Interlinear.GetDescription(), model.Where(x => x.Type == TranslationType.Interlinear)),
                new Grouping<string, TranslationInfo>(TranslationType.Literal.GetDescription(), model.Where(x => x.Type == TranslationType.Literal)),
                new Grouping<string, TranslationInfo>(TranslationType.Default.GetDescription(), model.Where(x => x.Type == TranslationType.Default)),
                new Grouping<string, TranslationInfo>(TranslationType.Dynamic.GetDescription(), model.Where(x => x.Type == TranslationType.Dynamic))
            };

            return _allTranslations;
        }
        public List<Grouping<string, TranslationInfo>> GetPatrologyTranslations() {
            var view = new XPView(new UnitOfWork(), typeof(Translation)) {
                CriteriaString = "[BookType] = 2 AND [Hidden] = 0"
            };
            view.Properties.Add(new ViewProperty("Name", SortDirection.None, "[Name]", false, true));
            view.Properties.Add(new ViewProperty("Description", SortDirection.None, "[Description]", false, true));
            view.Properties.Add(new ViewProperty("Type", SortDirection.None, "[Type]", false, true));
            view.Properties.Add(new ViewProperty("Catolic", SortDirection.None, "[Catolic]", false, true));
            view.Properties.Add(new ViewProperty("Recommended", SortDirection.None, "[Recommended]", false, true));
            view.Properties.Add(new ViewProperty("OpenAccess", SortDirection.None, "[OpenAccess]", false, true));

            var model = new List<TranslationInfo>();
            foreach (ViewRecord item in view) {
                model.Add(new TranslationInfo() {
                    Type = (TranslationType)item["Type"],
                    Name = item["Name"].ToString().Replace("'", ""),
                    Description = item["Description"].ToString(),
                    Catholic = (bool)item["Catolic"],
                    Recommended = (bool)item["Recommended"],
                    PasswordRequired = !((bool)item["OpenAccess"])
                }); 
            }

            var _allTranslations = new List<Grouping<string, TranslationInfo>> {
                new Grouping<string, TranslationInfo>(TranslationType.Interlinear.GetDescription(), model.Where(x => x.Type == TranslationType.Interlinear)),
                new Grouping<string, TranslationInfo>(TranslationType.Literal.GetDescription(), model.Where(x => x.Type == TranslationType.Literal)),
                new Grouping<string, TranslationInfo>(TranslationType.Default.GetDescription(), model.Where(x => x.Type == TranslationType.Default)),
                new Grouping<string, TranslationInfo>(TranslationType.Dynamic.GetDescription(), model.Where(x => x.Type == TranslationType.Dynamic))
            };

            return _allTranslations;
        }
    }
}
