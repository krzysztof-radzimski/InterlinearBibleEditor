/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.Data.Export.Model {
    public class BookBaseInfo {
        public int NumberOfBook { get; set; }
        public string BookShortcut { get; set; }
        public string BookName { get; set; }
        public string BookTitle { get; set; }
        public string Color { get; set; }
        public string TimeOfWriting { get; set; }
        public string PlaceWhereBookWasWritten { get; set; }
        public string Purpose { get; set; }
        public string Subject { get; set; }
        public string AuthorName { get; set; }
        public string Preface { get; set; }
        public BiblePart StatusBiblePart { get; set; }
        public CanonType StatusCanonType { get; set; }
        public TheBookType StatusBookType { get; set; }
    }
}
