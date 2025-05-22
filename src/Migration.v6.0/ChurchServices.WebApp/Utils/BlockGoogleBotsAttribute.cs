/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2025 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.WebApp.Filters {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class BlockGoogleBotsAttribute : ActionFilterAttribute {
        private readonly string[] _botSignatures = new[]
        {
            "Googlebot",
            "Google-Search",
            "APIs-Google",
            "AdsBot-Google",
            "Mediapartners-Google",
            "Google Favicon",
            "FeedFetcher-Google"
            // Możesz dodać więcej sygnatur botów według potrzeb
        };

        public override void OnActionExecuting(ActionExecutingContext context) {
            string userAgent = context.HttpContext.Request.Headers["User-Agent"].ToString();

            if (!string.IsNullOrEmpty(userAgent) && _botSignatures.Any(signature => userAgent.Contains(signature, StringComparison.OrdinalIgnoreCase))) {
                // Zwracamy status 403 (Forbidden) dla botów Google
                context.Result = new StatusCodeResult(403);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}