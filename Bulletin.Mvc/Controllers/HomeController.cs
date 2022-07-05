using Bulletin.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BulletinApp.Shared;  // BulletinContext

namespace Bulletin.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BulletinContext db;

        public HomeController(ILogger<HomeController> logger,
            BulletinContext injectedContext)
        {
            _logger = logger;
            //.NET uses constructor parameter injection to pass an instance
            // of the BulletinContext db context using the the connection string
            //specified in Program.cs
            db = injectedContext;
        }

        //improve performance by caching with browser
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any)]  
        public IActionResult Index()
        {
            HomeIndexViewModel model = new
               (
                UserCount: db.Users.ToList().Count,
                Users: db.Users.ToList(),
                Promos: db.Promos.ToList()
                );
            
            
            return View(model);
        }

        [Route("private")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}