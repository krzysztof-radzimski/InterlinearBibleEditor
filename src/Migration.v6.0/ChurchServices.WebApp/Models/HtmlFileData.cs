/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.WebApp.Models {
    public class HtmlFileData {
        public string Data { get; set; }
        public bool ShouldSerializeData() => Data != null;
    }

    public class HomePageModel {
        public HtmlFileData Info { get; set; }
        public List<ArticleInfo> Articles { get; set; }

        public bool ShouldSerializeInfo() => Info != null;
        public bool ShouldSerializeArticles() => Articles != null;
    }
}
