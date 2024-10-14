using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VyacheslavsMegaServer.Data.Entities;
using VyacheslavsMegaServer.Service;

namespace VyacheslavsMegaServer.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public AppDbContext() : base()
        {
        }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ContactLink> ContactLinks { get; set; }
        public DbSet<MainPageData> MainPageData { get; set; }
        public DbSet<UserReport> UserReports { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<PartnerLink> PartnerLinks { get; set; }
        public DbSet<DonationCard> DonationCards { get; set; }
        public DbSet<ServerMediaLink> ServerMediaLinks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(ProjectConfig.ConnectionString);
            optionsBuilder.UseLazyLoadingProxies(true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Identity config

            modelBuilder.Entity<IdentityUser>().HasData(new IdentityUser()
            {
                Id = "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                UserName = "Evray",
                NormalizedUserName = "EVRAY",
                PhoneNumber = "+7 (705) 806 3101",
                Email = "vya4slav4617@gmail.com",
                NormalizedEmail = "VYA4SLAV4617@GMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, "admin")
            });

            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole()
            {
                Id = "db158bac-48d4-47c3-a6b0-c6d8b5c9ee55",
                Name = "Admin",
                NormalizedName = "ADMIN"
            });

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>()
            {
                RoleId = "db158bac-48d4-47c3-a6b0-c6d8b5c9ee55",
                UserId = "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303"
            });

            #endregion

            modelBuilder.Entity<ContactLink>().HasData(new ContactLink()
            {
                Id = 1,
                Url = "https://vk.com/sansei_1",
                Content = "Вячеслав Костырев",
                Description = "VK: ",
                ContactId = 1,
            });

            modelBuilder.Entity<ContactLink>().HasData(new ContactLink()
            {
                Id = 2,
                Url = "https://t.me/vyacheslav_minecraft",
                Content = "@vyacheslav_minecraft",
                Description = "Telegram: ",
                ContactId = 1,
            });

            modelBuilder.Entity<Contact>().HasData(new Contact()
            {
                Id = 1,
                DisplayName = "Вячеслав",
                Description = "Царь и бог этого сервера",
                DescriptionLarge = "",
                UserId = "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                ShowContact = true,
            });

            modelBuilder.Entity<MainPageData>().HasData(new MainPageData()
            {
                Id = 1,
                PageTitle = "V-Server",
                ErrorMessage = "",
                ShowErrorMessage = false,
                Title = "Открой для себя мир Minecraft с\r\nVyacheslav's mega server",
                YellowHint = "Теперь с модами",
                ServerAddress = "109.248.157.54:25565",
                Description = "На сервере установлена сборка интересных модов. Этот модпак превращает Minecraft в сложное и увлекательное приключение с множеством новых возможностей.\r\nПогружайтесь в новые приключения с:\r\n\r\nУлучшенным ИИ мобов и механикой боя\r\nОбширными новыми биомами для исследования\r\nРазнообразными и опасными новыми мобами\r\nПродвинутой магией и зачарованиями\r\nИ многим другим",
                MetatagDescription = "Мега сервер Вячеслава",
                MetatagKeywords = "Minecraft, server, Вячеслав, мега, сервер",
            });
        }
    }
}
