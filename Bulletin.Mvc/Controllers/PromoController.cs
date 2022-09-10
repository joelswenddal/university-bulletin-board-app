using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; //[BindProperty], IActionResult
using Bulletin.Mvc.Models;
using Bulletin.Mvc.Models.DataViewModels;
using System.Diagnostics;
using BulletinApp.Shared; // BulletinContext
using Microsoft.EntityFrameworkCore; //SingleOrDefaultAsync
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Security.Cryptography.Pkcs;

namespace Bulletin.Mvc.Controllers
{
    public class PromoController : Controller
    {
        private readonly ILogger<PromoController> _logger;
        private readonly BulletinContext db;

        /*******************************************************************************************/
        public PromoController(ILogger<PromoController> logger,
            BulletinContext injectedContext)
        {
            _logger = logger;
            db = injectedContext;
        }

        /*******************************************************************************************/
        /// <summary>
        /// Endpoint for GET: Promo/Index
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }

        /*******************************************************************************************/
        /// <summary>
        /// Endpoint for GET: Promo/PromoDetail/id
        /// </summary>
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> PromoDetail(int? id)
        {
            if (!id.HasValue)
            {
                _logger.LogError("Bad Request to Promo/PromoDetail/id. Id paramter must be included in query");
                
                return BadRequest("400: Must include a valid PromoID in route, e.g., /Promo/PromoDetail/5");
            }

            Promo? model = await db.Promos
                .Include(p => p.Categories)
                .SingleOrDefaultAsync(p => p.PromoId == id);

            if (model == null)
            {
                _logger.LogError("Request to Promo/PromoDetail/id invalid. PromoId {id} not found in database", id);
                return NotFound($"404: PromoID {id} not found. Please enter a valid id");
            }

            if (model.Categories.Count == 0)
            {
                BulletinApp.Shared.Category temp = new();
                temp.CategoryName = "NA";
                model.Categories.Add(temp);
            }

            return View(model);
        }

        /*******************************************************************************************/
        /// <summary>
        /// Endpoint for GET: Promo/Create
        /// </summary>
        public ActionResult Create()
        {
            var promo = new Promo();

            PopulateCategoriesData(promo);

            return View();
        }

        /*******************************************************************************************/
        /// <summary>
        /// Endpoint for POST: Promo/Create
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("ContactName, PromoType, Headline, Description, ContactInfo")] Promo model, string[] selectedCategories)
        {
            try
            {
                if (selectedCategories != null)
                {

                    foreach (var category in selectedCategories)
                    {
                        int categoryId = int.Parse(category);
                        //look up the category in db
                        var categoryToAdd = db.Categories.Find(categoryId);
                        //if found add it to the model
                        if (categoryToAdd != null)
                        {
                            model.Categories.Add(categoryToAdd);
                        }
                    }
                }

                if (ModelState.IsValid)
                {
                    model.PostDate = DateTime.Now;
                    db.Promos.Add(model);
                    
                    await db.SaveChangesAsync();

                    _logger.LogInformation("New Promo record created. PromoId: {}", model.PromoId);
                    return RedirectToAction("PromoDetail", new { id = model.PromoId });
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Write to database was unsuccessful. Exception: {ex.Message}");
                ModelState.TryAddModelError("", "Unable to save to database. Please try again.");
            }
            PopulateCategoriesData(model);
            return View(model);
        }

        /*******************************************************************************************/
        /// <summary>
        /// Endpoint for GET: Promo/Edit/id
        /// </summary>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogError("Invalid request to Promo/Edit/id. Id parameter must be included");
                return NotFound("400: Must include a valid PromoID in route, e.g., /Promo/Edit/5");
            }

            Promo? model = await db.Promos
                .Include(p => p.Categories)
                .SingleOrDefaultAsync(p => p.PromoId == id);

            if (model == null)
            {

                _logger.LogError("Bad Request to Promo/Edit/id. PromoId {id} not found in database", id);
                return NotFound($"404: PromoID {id} not found.");
            }

            //get list of all Categories and add to ViewBag
            PopulateCategoriesData(model);

            return View(model);
        }

        /*******************************************************************************************/
        /// <summary>
        /// Endpoint for POST: Promo/Edit/id
        /// </summary>
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPromo(int? id, string[] selectedCategories)
        {
            if (!id.HasValue)
            {
                return NotFound($"404: PromoID {id} not found.");
            }

            //get the promo from the db if it exists
            Promo? model = await db.Promos
                .Include(p => p.Categories)
                .FirstOrDefaultAsync(p => p.PromoId == id);

            if (model == null)
            {
                return NotFound($"404: PromoID {id} not found.");
            }
            

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
                    UpdatePromosCategories(selectedCategories, model);
                    await db.SaveChangesAsync();
                    return RedirectToAction("PromoDetail", new { id = model.PromoId });
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"Write to database was unsuccessful. Exception: {ex.Message}");
                    ModelState.TryAddModelError("", "Unable to save changes in db. Please try again.");
                }
            }
#pragma warning restore CS8603 // Possible null reference return.
            PopulateCategoriesData(model);
            return View(model);
        }

        /*******************************************************************************************/
        /// <summary>
        /// Endpoint for GET: Promo/Delete/id
        /// </summary>
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                _logger.LogError("Invalid request to Promo/Delete/id. Id parameter must be included");
                return NotFound("400: Must include a valid PromoID in route, e.g., /Promo/Delete/5");
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

        /*******************************************************************************************/
        /// <summary>
        /// Endpoint for POST: Promo/Delete/id
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var model = await db.Promos.FindAsync(id);
            Promo? model = await db.Promos
                .Include(p => p.Categories)
                .Where(p => p.PromoId == id)
                .SingleAsync(p => p.PromoId == id);

            if (model == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var promoCategories = new HashSet<int>(model.Categories.Select(c => c.CategoryId));

                foreach (var category in db.Categories)
                {
                    model.Categories.Remove(category);
                }

                db.Promos.Remove(model);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Board");
            }
            catch (DbUpdateException ex )
            {
                //Log the error
                _logger.LogWarning($"Remove op in database unsuccessful. Exception: {ex.Message}");
                ModelState.TryAddModelError("", "Unable to save changes in database. Please try again.");
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        /****************************HELPER Methods**********************************************************/
        /// <summary>
        /// Checks if a Promo record in the db has a matching id,
        /// returns true if it does, false if it does not.
        /// </summary>
        private bool PromoExists(int id)
        {
            return db.Promos.Any(e => e.PromoId == id);
        }

        /********************************************************************************************/

        private void PopulateCategoriesDropDownList(object? selectedCategory = null)
        {
            var categoriesQuery = from c in db.Categories
                                  orderby c.CategoryName
                                  select c;
            ViewBag.CategoryId = new SelectList(categoriesQuery, "CategoryId", "CategoryName", selectedCategory);
        }

        /// <summary>
        /// Takes retrieves all Category data from db and adds them to the current
        /// ViewBag with the key "Categories"
        /// </summary>
        private void PopulateCategoriesData(Promo promo)
        {
            var allCategories = db.Categories;
            var promoCategories = new HashSet<int>(promo.Categories.Select(c => c.CategoryId));
            var viewModel = new List<PromoCategoriesData>();
            foreach (var category in allCategories)
            {
                viewModel.Add(new PromoCategoriesData
                {
                    CategoryId = category.CategoryId,
                    CategoryName = category.CategoryName,
                    Associated = promoCategories.Contains(category.CategoryId)
                });
            }
            ViewBag.Categories = viewModel;
        }

        private void UpdatePromosCategories(string[] selectedCategories, Promo promoToUpdate)
        {
            if (selectedCategories == null)
            {
                promoToUpdate.Categories = new HashSet<BulletinApp.Shared.Category>();
                return;
            }

            //go through all categories in the db and check each against the ones selected
            // in the View (HashSet used for more efficient lookup
            var selectedCategoriesHS = new HashSet<string>(selectedCategories);
            var promoCategories = new HashSet<int>
                (promoToUpdate.Categories.Select(c => c.CategoryId));

            //each category in the database
            foreach (var category in db.Categories)
            {

                //CASE: The category is selected by user
                if (selectedCategoriesHS.Contains(category.CategoryId.ToString()))
                {
                    //It is not currently selected in the db
                    if (!promoCategories.Contains(category.CategoryId))
                    {
                        promoToUpdate.Categories.Add(category);
                    }
                }
                //CASE: The category is not selected by user
                else
                {
                    //It is currently selected in the db, so needs removal (from the join table)
                    if (promoCategories.Contains(category.CategoryId))
                    {
                        promoToUpdate.Categories.Remove(category);
   
                    }
                }
            }
        }
    }
}
