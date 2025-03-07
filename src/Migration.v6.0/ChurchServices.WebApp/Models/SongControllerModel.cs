/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2025 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.WebApp.Models {
    public class SongControllerModel {
        public Song Song { get; set; }
        public int MaxNumber { get; set; }
    }

    public class SongsInfo {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Signature { get; set; }
        public string BPM { get; set; }
        public int Number { get; set; }
        public SongGroupType Type { get; set; }
    }
}
