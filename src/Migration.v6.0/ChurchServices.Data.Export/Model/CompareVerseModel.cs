/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.Data.Export.Model {
    public class CompareVerseModel {
        public string BookName { get; set; }
        public string BookShortcut { get; set; }
        public BiblePart Part { get; set; }
        public VerseIndex Index { get; set; }
        public List<CompareVerseInfo> Verses { get; set; }
        public bool LiteralOnly { get; set; }

        public string GetSiglum() {
            return $"{BookShortcut} {Index.NumberOfChapter}:{Index.NumberOfVerse}";
        }
    }
}
