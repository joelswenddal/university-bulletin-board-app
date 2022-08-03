using BulletinApp.Shared; // AddBulletinContext extension method
using System.Net.Http.Headers; // MediaTypeWithQualityHeaderValue

namespace Bulletin.Mvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHttpClient(name: "Amazon.Search.Microservice",
                configureClient: options =>
                {
                    options.BaseAddress = new Uri("https://project-357202.wl.r.appspot.com/");
                    options.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue(
                            "application/json", 1.0));
                });

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Load the connection string and then register the Bulletin database context (using the extension method)
            string sqlServerConnection = builder.Configuration
                .GetConnectionString("BulletinConnection");  //appsettings.json

            builder.Services.AddBulletinContext(sqlServerConnection);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}