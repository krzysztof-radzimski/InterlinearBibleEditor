using EIB.SNPPD.Importer.Configuration;
using Microsoft.Extensions.Configuration;

namespace EIB.SNPPD.Importer.Controllers {
    public class ConfigurationController {
        public string GetBaseDirectory() {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false);

            IConfiguration config = builder.Build();
            var settings = config.GetSection("Settings").Get<Settings>();
            if (settings != null) {
                return settings.BaseDirectory;
            }
            return "../../../../../db/import/";
        }

        public string GetLicenseKeyFilePath() {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false);

            IConfiguration config = builder.Build();
            var settings = config.GetSection("Settings").Get<Settings>();
            if (settings != null) {
                return settings.AsposeLicenseFilePath;
            }
            return "../../../../../db/Aspose.Total.lic";
        }
    }
}
