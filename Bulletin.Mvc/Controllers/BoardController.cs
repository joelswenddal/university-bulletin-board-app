using Bulletin.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BulletinApp.Shared;  // BulletinContext
using Microsoft.EntityFrameworkCore;


namespace Bulletin.Mvc.Controllers
{
    public class BoardController : Controller
    {
        private readonly ILogger<BoardController> _logger;
        private readonly BulletinContext db;

        public BoardController(ILogger<BoardController> logger,
            BulletinContext injectedContext)
        {
            _logger = logger;
            //.NET uses constructor parameter injection to pass an instance
            // of the BulletinContext db context using the the connection string
            //specified in Program.cs
            db = injectedContext;
        }

        // Board/Index
        //improve performance by caching with browser
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> Index()
        {
            BoardIndexViewModel model = new
               (
                Promos: await db.Promos.ToListAsync()
                );
            
            return View(model);
        }

        // Board/Search?searchString=
        //improve performance by caching with browser
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> Search(string searchString, string criteria = "keyword")
        {
            ViewData["searchString"] = searchString;
            //create LINQ query
            var promos = from p in db.Promos
                         where p != null
                         select p;
            
            if (criteria != "keyword" && criteria != "author")
            {
                criteria = "keyword";
            }

            if (!String.IsNullOrEmpty(searchString))
            {

                if (criteria == "keyword")
                {
                    promos = promos.Where(p => (!String.IsNullOrEmpty(p.Headline)
                    && p.Headline.Contains(searchString))
                    || (!String.IsNullOrEmpty(p.Description)
                    && p.Description.Contains(searchString)));
                }
                else if (criteria == "author")
                {
                    promos = promos.Where(p => (!String.IsNullOrEmpty(p.ContactName)
                    && p.ContactName.Contains(searchString)));
                }
            }
            BoardIndexViewModel model = new
               (
                Promos: await promos.ToListAsync()
                );

            return View(model);
        }

    }
}
