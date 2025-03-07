/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2025 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.WebApp.Controllers {
    public class AccountController : Controller {
        private readonly IConfiguration Configuration;
        public AccountController(IConfiguration configuration) {
            Configuration = configuration;
        }
        public IActionResult Index() {
            var qs = Request.QueryString;
            if (qs.IsNotNull() && qs.Value.IsNotNullOrEmpty() && qs.Value.Length > 3) {
                var value = qs.Value;
                if (value.Contains("&")) {
                    value = value.Substring(0, value.IndexOf("&"));
                }
                var returnUrl = value.Replace("?ReturnUrl=", "").Trim();
                returnUrl = HttpUtility.UrlDecode(returnUrl);
                return View(new ApplicationUser() { UserName = "WND", ReturnUrl = returnUrl });
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login([Bind] ApplicationUser user) {
            var users = new ApplicationUsers(Configuration);
            var allUsers = users.GetUsers().FirstOrDefault();
            if (users.GetUsers().Any(u => u.UserName == user.UserName && u.Password == user.Password)) {
                var userClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.UserName)
                };

                var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");

                var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });
                HttpContext.SignInAsync(userPrincipal);

                if (user.ReturnUrl != null) {
                    return Redirect(user.ReturnUrl);
                }
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }
        public IActionResult Logout() {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class TranslationAuthorizeAttribute : TypeFilterAttribute {
        public TranslationAuthorizeAttribute() : base(typeof(TranslationFilter)) { }
    }

    public class TranslationFilter : IAuthorizationFilter {

        public TranslationFilter() { }

        public void OnAuthorization(AuthorizationFilterContext context) {
            if (!context.HttpContext.User.Identity.IsAuthenticated) {
                if (context.IsNotNull() && context.RouteData.IsNotNull() && context.RouteData.Values.IsNotNull() && context.RouteData.Values.Count > 2) {
                    var translationName = context.RouteData.Values["translationName"].ToString();
                    if (translationName.IsNotNull()) {
                        if (translationName == "SNPPD" || translationName == "SNPD") { translationName = "PBD"; }
                        if (translationName == "SNPL") { translationName = "SNP18"; }
                        if (translationName == "PNS") { translationName = "PNS1997"; }
                        var translation = new XPQuery<Translation>(new UnitOfWork()).Where(x => x.Name.Replace("'", "").Replace("+", "") == translationName && !x.OpenAccess).FirstOrDefault();
                        if (translation.IsNotNull()) {
                            context.Result = new RedirectResult("/Account/Index?ReturnUrl=" + context.HttpContext.Request.Path.Value);
                        }
                    }
                }
            }
        }
    }

    public class AuthException : Exception { }
}
