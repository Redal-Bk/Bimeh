
using Bimeh.Domain.Entities;
using Bimeh.Infrastructur;
using Bimeh.Rpositories;
using Microsoft.EntityFrameworkCore;

namespace Bimeh
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // افزودن سرویس‌ها
            builder.Services.AddControllers();
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddMvc();
            var ConnectionString = builder.Configuration.GetConnectionString("Bimeh");
            builder.Services.AddDbContext<BimehContext>(
                opt => opt.UseSqlServer(ConnectionString)
            );

            // افزودن Swagger
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddScoped<IManage, Manage>();
            // افزودن CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            // افزودن Authentication و JWT

            builder.Services.AddAuthorization();

            // افزودن پیکربندی از appsettings.json
            builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var app = builder.Build();


            // فعال کردن Middlewareها
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();

            // کنترل خطاها
            app.UseExceptionHandler("/error");

            // مپ کردن درخواست‌ها
            app.MapControllers();
            app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}
