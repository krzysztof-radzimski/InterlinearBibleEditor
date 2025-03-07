/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2025 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.Data.Model {
    public class AncientDictionary : XPObject {
        private Language language;
        public Language Language {
            get { return language; }
            set { SetPropertyValue(nameof(Language), ref language, value); }
        }

        [Association("DictionaryItems")]
        public XPCollection<AncientDictionaryItem> Items {
            get { return GetCollection<AncientDictionaryItem>(nameof(Items)); }
        }

        public AncientDictionary(Session session) : base(session) { }
    }
}
