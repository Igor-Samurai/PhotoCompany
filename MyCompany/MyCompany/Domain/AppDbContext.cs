using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyCompany.Domain.Entities;

namespace MyCompany.Domain
{
    //Контекст БД
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        //Добавляем созданные классы категорий и услуг
        public DbSet<ServiceCategory> ServiceCategories { get; set; } = null!;
        public DbSet<Service> Services { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            string adminName = "admin";
            string roleAdminId = "F8721134-D5EF-4C8C-95FE-20770C4D8127";
            string userAdminId = "42DE96B4-756C-4D7A-9C48-348FC1B2876E";

            //Добавляем роль администратора сайта
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole()
            {
                Id = roleAdminId,
                Name = adminName,
                NormalizedName = adminName.ToUpper()
            });

            //Добавляем нового IdentityUser в качестве администратора сайта
            modelBuilder.Entity<IdentityUser>().HasData(new IdentityUser
            {
                Id = userAdminId,
                UserName = adminName,
                NormalizedUserName = adminName.ToUpper(),
                Email = "admin@admin.com",
                NormalizedEmail = "admin@admin.com".ToUpper(),
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(new IdentityUser(), adminName),
                SecurityStamp = string.Empty,
                PhoneNumberConfirmed = true,
            });

            //Определяем админа в соответствующую роль
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>()
            {
                RoleId = roleAdminId,
                UserId = userAdminId
            });
        }
    }
}
