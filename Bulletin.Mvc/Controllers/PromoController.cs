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

        // POST: Promo/Create
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

        // GET: Promo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await db.Promos.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPromo(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound($"404: PromoID {id} not found.");  //404
            }

            
            var model = await db.Promos.FirstOrDefaultAsync(p => p.PromoId == id);

            
            if (model == null)
            {
                return NotFound($"404: PromoID {id} not found.");
            }

            //TryUpdateModelAsync updates fields in retrieved entity based
            // on user input in the posted form data
#pragma warning disable CS8603 // Possible null reference return.
            if (await TryUpdateModelAsync<Promo>(
                model,
                "",
                p => p.ContactName,
                p => p.PromoType,
                p => p.Headline,
                p => p.Description,
                p => p.ContactInfo))
            {
                try
                {
                    await db.SaveChangesAsync();
                    return RedirectToAction("PromoDetail", new { id = model.PromoId });
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"The write to the database was unsuccessful. Exception: {ex.Message}");
                    ModelState.TryAddModelError("", "Unable to save changes in the database." + "Please try again.");
                }
            }
#pragma warning restore CS8603 // Possible null reference return.
            return View(model);
        }

        // GET: PromoController/Delete/5
        // GET: Promo/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await db.Promos
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.PromoId == id);
            if (model == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(model);
        }
        
        
        // POST: Promo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var model = await db.Promos.FindAsync(id);
            if (model == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                db.Promos.Remove(model);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Board");
            }
            catch (DbUpdateException ex )
            {
                //Log the error
                _logger.LogWarning($"The Remove operation in the database was unsuccessful. Exception: {ex.Message}");
                ModelState.TryAddModelError("", "Unable to save changes (delete) in the database." + "Please try again.");
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool PromoExists(int id)
        {
            return db.Promos.Any(e => e.PromoId == id);
        }

      
    }
}
