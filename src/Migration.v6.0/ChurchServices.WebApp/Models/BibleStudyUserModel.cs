/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using System.ComponentModel.DataAnnotations;

namespace ChurchServices.WebApp.Models {
    public class BibleStudyUserModel : HtmlFileData {
        [Required]
        [EmailAddress]
        [Display(Name = "Adres email")]
        public string EmailAddress { get; set; }

        [Required]
        [MinLength(4)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }
        public int CourseLevel { get; set; }

        public BibleStudyCourseModel CourseItem { get; set; }

        public bool ShouldSerializeCourseItem() => CourseItem != null;
    }
}
