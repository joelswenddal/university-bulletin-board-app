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

        /*******************************************************************************************/
        public BoardController(ILogger<BoardController> logger,
            BulletinContext injectedContext)
        {
            _logger = logger;
            db = injectedContext;
        }

        /*******************************************************************************************/
        /// <summary>
        /// Endpoint for GET: Board/Index
        /// </summary>
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Board/Index page visited at {DT}",
            DateTime.UtcNow.ToLongTimeString());


            BoardIndexViewModel model = new
               (
                Promos: await db.Promos.ToListAsync()
                );
            
            return View(model);
        }

        /*******************************************************************************************/
        /// <summary>
        /// Endpoint for GET: Board/Search?searchString=[]?criteria=[]
        /// </summary>
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> Search(string searchString, string criteria = "keyword")
        {
            ViewData["searchString"] = searchString;

            _logger.LogInformation("Board/Search action executed at {DT}",
            DateTime.UtcNow.ToLongTimeString());

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
