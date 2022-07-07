using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; //[BindProperty], IActionResult
using Bulletin.Mvc.Models;
using System.Diagnostics;
using BulletinApp.Shared;  // BulletinContext
using Microsoft.EntityFrameworkCore;  //SingleOrDefaultAsync

namespace Bulletin.Mvc.Controllers
{
    public class PromoController : Controller
    {

        private readonly ILogger<PromoController> _logger;
        private readonly BulletinContext db;

        public PromoController(ILogger<PromoController> logger,
            BulletinContext injectedContext)
        {
            _logger = logger;
            //.NET uses constructor parameter injection to pass an instance
            // of the BulletinContext db context using the the connection string
            //specified in Program.cs
            db = injectedContext;
        }
        
        

        // GET: PromoController
        public ActionResult Index()
        {
            return View();
        }

        // GET: PromoController/PromoDetail/5
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> PromoDetail(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest("You must include a PromoID in the route, for example, /Promo/PromoDetail/5");
            }

            Promo? model = await db.Promos
                .SingleOrDefaultAsync(p => p.PromoId == id);
            if (model == null)
            {
                return NotFound($"404: PromoID {id} not found.");
            }

            return View(model);
        }

        // GET: PromoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PromoController/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("ContactName, PromoType, Headline, Description, ContactInfo")] Promo model)
        
        {
            try
            {
               if (ModelState.IsValid)
                {
                    model.PostDate = DateTime.Now;
                    db.Promos.Add(model);
                    await db.SaveChangesAsync();
                    return RedirectToAction("PromoDetail", new { id = model.PromoId });
                }
                
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"The write to the database was unsuccessful. Exception: {ex.Message}");
                ModelState.TryAddModelError("", "Unable to save changes in the database." + "Please try again.");
                
            }
            return View(model);
        }

        // GET: PromoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PromoController/Edit/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PromoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PromoController/Delete/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
