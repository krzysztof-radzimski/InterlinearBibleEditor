/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.WebApp.Utils {
    public interface IBibleStudyCourseTeacherHelper {
        IEnumerable<BibleStudyCourseInfoModel> GetSentCourses();
        IEnumerable<BibleStudyCourseInfoModel> GetApprovedCourses(string userName = null);
    }
}
