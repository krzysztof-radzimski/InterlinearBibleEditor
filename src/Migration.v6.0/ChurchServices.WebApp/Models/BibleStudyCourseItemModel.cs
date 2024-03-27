/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.WebApp.Models {
    public class BibleStudyCourseItemModel {
        public bool Application { get; set; }
        public int Index { get; set; }
        public string Question { get; set; } = string.Empty;
        public string QuestionAnswer { get; set; } = string.Empty;
        public string TeacherComment { get; set; } = string.Empty;

        public bool ShouldSerializeApplication() => Application;
        public bool ShouldSerializeQuestion() => Question.IsNotNullOrEmpty();
        public bool ShouldSerializeQuestionAnswer() => QuestionAnswer.IsNotNullOrEmpty();
        public bool ShouldSerializeTeacherComment() => TeacherComment.IsNotNullOrEmpty();
    }
}
