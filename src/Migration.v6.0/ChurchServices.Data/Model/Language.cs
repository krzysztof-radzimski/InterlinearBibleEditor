/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2025 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.Data.Model {
    public enum Language {
        [Description("")][Category("")] None,
        [Description("hebrajski")][Category("H")] Hebrew,
        [Description("grecki")][Category("G")] Greek,
        [Description("łacina")][Category("L")] Latin,
        [Description("angielski")][Category("EN")] English,
        [Description("polski")][Category("PL")] Polish,
        [Description("ukraiński")][Category("UA")] Ukrainian
    }
}

namespace ChurchServices.Extensions {
    using ChurchServices.Data.Model;
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
