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

    }
}
