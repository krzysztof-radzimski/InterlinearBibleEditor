/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2025 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ChurchServices.WebApp.Models {
    public class BibleStudyCourseModel : HtmlFileData {
        private int _chapter;
        private string _verses;
        public string Passage { get; set; }
        public string Introduction { get; set; }
        public string YouTubeLink { get; set; }
        public BibleStudyCourseStaus Status { get; set; }
        public List<BibleStudyCourseItemModel> Items { get; set; }

        [Display(Name = "Twoje myśli, uwagi")]
        [MaxLength(5000)]
        public string Description { get; set; } = string.Empty;

        public int GetChapter() {
            if (_chapter > 0) { return _chapter; }
            if (Passage != null && Passage.Contains(":")) {
                var t = Passage.Split(":");
                if (t.Length > 0) {
                    _chapter = t[0].ToInt();
                    return _chapter;
                }
            }
            return 1;
        }
        public string GetVerses() {
            if (_verses.IsNotNullOrEmpty()) { return _verses; }
            if (Passage != null && Passage.Contains(":")) {
                var t = Passage.Split(":");
                if (t.Length > 1) {
                    var s = t[1];
                    if (s.Contains("-")) {
                        var t2 = s.Split("-");
                        var start = t2[0].ToInt();
                        var end = t2[1].ToInt();
                        var sb = new StringBuilder();
                        for (int i = start; i <= end; i++) {
                            if (i < end) { sb.Append($"{i},"); }
                            else { sb.Append($"{i}"); }
                        }
                        s = sb.ToString();
                    }
                    _verses = s;
                    return _verses;
                }
            }
            return "1";
        }
    }
}
