/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.Data.Export.Model {
    public class TranslationInfo {
        public string Name { get; set; }
        public string Description { get; set; }
        public TranslationType Type { get; set; }
        public bool Catholic { get; set; }
        public bool Recommended { get; set; }
        public bool PasswordRequired { get; set; }
        public string TranslationType { get { return Type.GetDescription(); } }
        public Language Language { get; set; }        
    }
}
