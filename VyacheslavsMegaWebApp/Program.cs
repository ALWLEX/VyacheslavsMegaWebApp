using Microsoft.AspNetCore.Identity;
using System.Globalization;
using VyacheslavsMegaWebApp.Data;
using VyacheslavsMegaWebApp.Data.Entities.Identity;
using VyacheslavsMegaWebApp.Data.JSON.Configuration;
using VyacheslavsMegaWebApp.Data.JSON.Repositories;
using VyacheslavsMegaWebApp.Data.Repositories;
using VyacheslavsMegaWebApp.Service;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();

        configurationBuilder.AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build()
            .Bind("Project", new ProjectConfig());
       
        // Add services to the container.
        builder.Services.AddAuthorization(x =>
            x.AddPolicy("AdminAreaPolicy", policy => policy.RequireRole("Admin")));

        builder.Services.AddControllersWithViews(x =>
            x.Conventions.Add(new AdminAreaAutorization("Admin", "AdminAreaPolicy")));

        builder.Services.AddDbContext<AppDbContext>();

        builder.Services.AddIdentity<VmsIdentityUser, IdentityRole>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";
            options.Password.RequireDigit = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
        })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders()
            .AddErrorDescriber<VmsIdentityErrorDescriber>()
            .AddUserValidator<VmsUserValidator>();

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.Name = "auth";
            options.Cookie.HttpOnly = true;
            options.LoginPath = "/account/login";
            options.AccessDeniedPath = "/account/denied";
        });

        builder.Services.AddLocalization(options => options.ResourcesPath = "AppResources/Resx");

        builder.Services.Configure<RequestLocalizationOptions>(options =>
        {
            options.SupportedCultures = new[]
            {
                new CultureInfo("ru-RU"),
            };
            options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("ru-RU");
        });

        builder.Services.AddTransient<MainPageRepository>();
        builder.Services.AddTransient<UserReportsRepository>();
        builder.Services.AddTransient<ContactsInfoRepository>();
        builder.Services.AddTransient<PartnersInfoRepository>();
        builder.Services.AddTransient<DonationCardsRepository>();
        builder.Services.AddTransient<ServerMediaLinksRepository>();
        builder.Services.AddTransient<ImageManager>();
        builder.Services.AddTransient<MCServerStatRepository>();
        builder.Services.AddTransient<FundRepository>();
        builder.Services.AddTransient<GoalRepository>();
        builder.Services.AddTransient<DownloadButtonsRepository>();
        builder.Services.AddTransient<EmailService>();
        builder.Services.AddTransient<LauncherSessionRepository>();
        builder.Services.AddSingleton<AppResourcesManager>();

        var app = builder.Build();

        app.UseRequestLocalization();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsProduction())
        {
            app.UseExceptionHandler("/Home/Error");

            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseStatusCodePagesWithReExecute("/Home/StatusCode", "?code={0}");

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseCookiePolicy();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute("admin", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
        app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}