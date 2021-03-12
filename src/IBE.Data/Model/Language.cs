/*=====================================================================================

	Interlinear Bible Editor
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using System.ComponentModel;

namespace IBE.Data.Model {
    public enum Language {
        [Category("")]
        None,
        [Category("H")]
        Hebrew,
        [Category("G")]
        Greek,
        [Category("L")]
        Latin,
        [Category("EN")]
        English,
        [Category("PL")]
        Polish
    }
    public static class LanguageExtensions {
        public static Language GetLanguage(this string value) {
            if (value != null) {
                if (value.ToLower().Trim() == "pl") {
                    return Language.Polish;
                }
                if (value.ToLower().Trim() == "en") {
                    return Language.English;
                }
                if (value.ToLower().Trim() == "el" || value.ToLower().Trim() == "grc") {
                    return Language.Greek;
                }
                if (value.ToLower().Trim() == "iw") {
                    return Language.Hebrew;
                }
            }
            return Language.None;
        }
    }
}
