/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2025 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.Data.Model {
    public class BookInfo {
        public int Id { get; private set; }
        public string Caption { get; private set; }
        public int NumberOfChapters { get; private set; }

        private BookInfo() { }
        public BookInfo(Book book) : this() {
            this.Id = book.Oid;
            this.Caption = book.ToString();
            this.NumberOfChapters = book.NumberOfChapters;
        }
        public override string ToString() {
            return Caption;
        }
    }
}
