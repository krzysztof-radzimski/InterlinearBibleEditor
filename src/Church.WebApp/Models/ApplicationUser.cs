using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Church.WebApp.Models {
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
            return new List<ApplicationUser>() { new ApplicationUser { Id = 1, UserName = "WND", EmailAddress = "info@kosciol-jezusa.pl", Password = Configuration["TranslationPwd"] } };
        }
    }
}
