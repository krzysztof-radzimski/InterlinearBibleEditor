/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2025 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.WebApp.Models {
    public class ApplicationUser {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class ApplicationUsers {
        private readonly IConfiguration Configuration;
        public ApplicationUsers(IConfiguration configuration) {
            Configuration = configuration;
        }
        public IEnumerable<ApplicationUser> GetUsers() {
            return new List<ApplicationUser>() { 
                new ApplicationUser { Id = 1, UserName = "WND", EmailAddress = "info@kosciol-jezusa.pl", Password = Configuration["TranslationPwd"] },
                new ApplicationUser { Id = 2, UserName = "petz@feib.pl", EmailAddress = "petz@feib.pl", Password = "$Harfeusz2022" }
            };
        }
    }
}
