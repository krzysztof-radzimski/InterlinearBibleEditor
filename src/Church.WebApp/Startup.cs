using Church.WebApp.Utils;
using IBE.Data.Export.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IBE.WebApp {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddAuthentication("CookieAuthentication")
                .AddCookie("CookieAuthentication", config => {

                    config.Cookie.Name = "UserLoginCookie";
                    config.Cookie.SameSite = SameSiteMode.Strict;
                    config.Cookie.SecurePolicy = CookieSecurePolicy.None;
                    //config.Cookie.Expiration = System.TimeSpan.FromDays(1);
                    config.Cookie.MaxAge = System.TimeSpan.FromDays(2);
                    config.Cookie.HttpOnly = true;

                    config.LoginPath = "/Account/Index";
                    config.LogoutPath = "/Account/Logout";
                    config.ExpireTimeSpan = System.TimeSpan.FromDays(1);
                });
            services.AddMemoryCache();
            services.AddControllersWithViews();
            services.AddScoped<ITranslationInfoController, TranslationInfoController>();
            services.AddScoped<IBibleTagController, BibleTagController>();            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                //app.UseDeveloperExceptionPage();
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            //var cookiePolicyOptions = new CookiePolicyOptions {
            //    MinimumSameSitePolicy = SameSiteMode.Strict,
            //    HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,
            //    Secure = CookieSecurePolicy.None,
            //};

            //app.UseCookiePolicy(cookiePolicyOptions);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "bible",
                    pattern: "{translationName}/{book?}/{chapter?}/{verse?}",
                    defaults: new { controller = "Translation", action = "Index" }
                );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            var path = $"{env.ContentRootPath}\\Data\\IBE.SQLite3";

            var connectionString = $"XpoProvider=SQLite;data source={path}";
            Data.ConnectionHelper.Connect(connectionString: connectionString);
        }
    }
}
