/*=====================================================================================

	Interlinear Bible Editor
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace IBE.Data.Export.Model {
    public class SiglumModel {
        public string TranslationName { get; set; }
        public string BookShortcut { get; set; }
        public int NumberOfChapter { get; set; }
        public int[] NumbersOfVerses { get; set; }
    }
}
