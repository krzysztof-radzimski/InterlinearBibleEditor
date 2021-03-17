using IBE.Data.Model;

namespace Church.WebApp.Models {
    public class TranslationInfo {
        public string Name { get; set; }
        public string Description { get; set; }
        public TranslationType Type { get; set; }
        public bool Catolic { get; set; }
        public bool Recommended { get; set; }
    }
}
