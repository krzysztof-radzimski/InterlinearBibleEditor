/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2025 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.Data.Export.Model {
    public class SiglumModel {
        public string TranslationName { get; set; }
        public string BookShortcut { get; set; }
        public int NumberOfChapter { get; set; }
        public int[] NumbersOfVerses { get; set; }
    }

    public class SimpleSiglumModel {
        public int BookNumber { get; set; }
        public int NumberOfChapter { get; set; }
        public int NumberOfVerse { get; set; }
    }
}
