using BulletinApp.Shared; // AddBulletinContext extension method
using System.Configuration;
using System.Net.Http.Headers; // MediaTypeWithQualityHeaderValue
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace Bulletin.Mvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add microservice client to services
            builder.Services.AddHttpClient(name: "Amazon.Search.Microservice",
                configureClient: options =>
                {
                    options.BaseAddress = new Uri("https://project-357202.wl.r.appspot.com/");
                    options.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue(
                            "application/json", 1.0));
                });
            
            builder.Services.AddControllersWithViews();

            // Load the connection string -> register Bulletin db context
            string sqlServerConnection = builder.Configuration
                .GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
                //.GetConnectionString("BulletinConnection");

            builder.Services.AddBulletinContext(sqlServerConnection);

            var app = builder.Build();
            
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days
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