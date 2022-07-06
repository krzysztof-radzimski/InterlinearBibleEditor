/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.Data.Export.Model {
    public class CompareVerseInfo {
        public VerseIndex Index { get; set; }
        public string TranslationName { get; set; }
        public string TranslationDescription { get; set; }
        public TranslationType TranslationType { get; set; }
        public string Text { get; set; }
        public string HtmlText { get; set; }
        public string SimpleText { get; set; }
        public int SortIndex { get; set; }
    }
}
