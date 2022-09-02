using Bulletin.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BulletinApp.Shared;  // BulletinContext
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Bulletin.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BulletinContext db;

        /*******************************************************************************************/
        public HomeController(ILogger<HomeController> logger,
            BulletinContext injectedContext)
        {
            _logger = logger;
            db = injectedContext;
        }

        /*******************************************************************************************/
        /// <summary>
        /// Endpoint for GET: Home/Index
        /// </summary>
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any)]  
        public IActionResult Index()
        {
            HomeIndexViewModel model = new
               (
                    Categories: db.Categories.Include(c => c.Promos).ToList()
                );
            
            return View(model);
        }

        /*******************************************************************************************/
        /// <summary>
        /// Endpoint for GET: Home/private
        /// </summary>
        [Route("private")]
        public IActionResult Privacy()
        {
            return View();
        }

        /*******************************************************************************************/
        /// <summary>
        /// Action on Error -- passes error model to View
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? 
                                            HttpContext.TraceIdentifier 
            });
        }
    }
}