using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MyCompany.Domain;
using MyCompany.Domain.Repositories.Abstract;
using MyCompany.Domain.Repositories.EntityFramework;
using MyCompany.Infrastructure;
using Serilog;

namespace MyCompany
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            //Подключаем в конфигурацию файл appsettings.json
            IConfigurationBuilder configBuilder = new ConfigurationBuilder()
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            //Оборачиваем секцию Project в объектную форму для удобства
            IConfiguration configuration = configBuilder.Build();
            AppConfig config = configuration.GetSection("Project").Get<AppConfig>()!;

            //Подключаем контекст БД
            builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(config.Database.ConnectionString)
                //На момент создания приложения в данной версии EF был баг, хотя ошибки нет, поэтому подавляем предупреждения
                .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning)));

            //Подключаем наши созданные сервисы
            builder.Services.AddTransient<IServiceCategoriesRepository, EFServiceCategoriesRepository>();
            builder.Services.AddTransient<IServicesRepository, EFServicesRepository>();
            builder.Services.AddTransient<DataManager>();

            //Настраиваем Identity систему
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(opts =>
            {
                opts.User.RequireUniqueEmail = true;
                opts.Password.RequiredLength = 6;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
            //Настраиваем Authentication cookie
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "myCompanyAuth";
                options.Cookie.HttpOnly = true;
                options.LoginPath = "/account/login";
                options.AccessDeniedPath = "/account/accessdenied";
                options.SlidingExpiration = true;
            });

            //Подключаем функционал контроллеров
            builder.Services.AddControllersWithViews();

            //Подключаем логи
            builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

            //Собираем конфигурацию
            WebApplication app = builder.Build();

            //! Порядок следования middleware очень важен, они будут выполняться согласно нему

            //Сразу же используем логирование
            app.UseSerilogRequestLogging();

            //Далее подключаем обработку исключений
            if (app.Environment.IsDevelopment())
                app.UseDeveloperExceptionPage();

            //Подключаем использование статических файлов (js, css, любых)
            app.UseStaticFiles();

            //Подключаем систему маршрутизации
            app.UseRouting();

            //Подключаем аутентификацию и авторизацию
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            //Регистриуруем нужные нам маршруты
            app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

            //Стартуем приложение
            await app.RunAsync();
        }
    }
}
