var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
services.AddAuthentication("CookieAuthentication")
                .AddCookie("CookieAuthentication", config => {
                    config.Cookie.Name = "UserLoginCookie";
                    config.Cookie.SameSite = SameSiteMode.Strict;
                    config.Cookie.SecurePolicy = CookieSecurePolicy.None;
                    config.Cookie.MaxAge = TimeSpan.FromDays(2);
                    config.Cookie.HttpOnly = true;

                    config.LoginPath = "/Account/Index";
                    config.LogoutPath = "/Account/Logout";
                    config.ExpireTimeSpan = System.TimeSpan.FromDays(1);
                });
services.AddMemoryCache();
services.AddControllersWithViews();
services.AddScoped<ITranslationInfoController, TranslationInfoController>();
services.AddScoped<IBibleTagController, BibleTagController>();

// Add services to the container.
services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "bible",
    pattern: "{translationName}/{book?}/{chapter?}/{verse?}",
    defaults: new { controller = "Translation", action = "Index" });


var path = @$"{app.Environment.ContentRootPath}Data\IBE.SQLite3";

var connectionString = $"XpoProvider=SQLite;data source={path}";
new ChurchServices.Data.ConnectionHelper().Connect(connectionString: connectionString);

app.Run();