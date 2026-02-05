using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using System.Configuration;
using VyacheslavsMegaServer.Data;
using VyacheslavsMegaServer.Data.JSON.Repositories;
using VyacheslavsMegaServer.Data.Repositories;
using VyacheslavsMegaServer.Service;

internal class Program
{
    private static IConfiguration? _configuration;
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();

        configurationBuilder.AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .Build()
            .Bind("Project", new ProjectConfig());
        _configuration = configurationBuilder.Build();

        // Add services to the container.
        builder.Services.AddAuthorization(x =>
            x.AddPolicy("AdminAreaPolicy", policy => policy.RequireRole("Admin")));

        builder.Services.AddControllersWithViews(x =>
            x.Conventions.Add(new AdminAreaAutorization("Admin", "AdminAreaPolicy")));

        builder.Services.AddDbContext<AppDbContext>();

        builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequireDigit = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
        }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.Name = "auth";
            options.Cookie.HttpOnly = true;
            options.LoginPath = "/account/login";
            options.AccessDeniedPath = "/account/denied";
        });

        builder.Services.AddTransient<MainPageRepository>();
        builder.Services.AddTransient<UserReportsRepository>();
        builder.Services.AddTransient<ContactsInfoRepository>();
        builder.Services.AddTransient<PartnersInfoRepository>();
        builder.Services.AddTransient<DonationCardsRepository>();
        builder.Services.AddTransient<ServerMediaLinksRepository>();
        builder.Services.AddTransient<ImageManager>();
        builder.Services.AddTransient<MCServerStatRepository>();
        builder.Services.AddTransient<IdentityRepository>();
        builder.Services.AddTransient<FundRepository>();
        builder.Services.AddTransient<GoalRepository>();
        builder.Services.AddTransient<DownloadButtonsRepository>();

        var app = builder.Build();

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