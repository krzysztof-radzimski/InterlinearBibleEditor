using IBE.Data.Model;
using IBE.Common.Extensions;

namespace IBE.Data.Export.Model {
    public class TranslationInfo {
        public string Name { get; set; }
        public string Description { get; set; }
        public TranslationType Type { get; set; }
        public bool Catholic { get; set; }
        public bool Recommended { get; set; }
        public bool PasswordRequired { get; set; }
        public string TranslationType { get { return Type.GetDescription(); } }
    }
}
