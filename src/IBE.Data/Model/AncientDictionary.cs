/*=====================================================================================

	Interlinear Bible Editor
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using DevExpress.Xpo;

namespace IBE.Data.Model {
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
