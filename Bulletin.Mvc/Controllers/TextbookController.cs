using Microsoft.AspNetCore.Mvc;
using Bulletin.Mvc.Models;
using System.Diagnostics;
using BulletinApp.Shared;  // BulletinContext
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Bulletin.Mvc.Controllers
{
    public class TextbookController : Controller
    {
        private readonly ILogger<TextbookController> _logger;
        private readonly IHttpClientFactory clientFactory;

        /*******************************************************************************************/
        public TextbookController(ILogger<TextbookController> logger,
            BulletinContext injectedContext,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            clientFactory = httpClientFactory;
        }

        /*******************************************************************************************/
        /// <summary>
        /// Endpoint for GET Textbook/Index
        /// </summary>
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any)]
        public IActionResult Index()
        {
            return View();
        }

        /*******************************************************************************************/
        /// <summary>
        /// Endpoint for GET Textbook/Text?searchTerm=[]
        /// </summary>
        [ResponseCache(Duration = 20, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> Text(string? searchTerm)
        {
            string uri;

            if (string.IsNullOrEmpty(searchTerm))
            {
                ViewData["Title"] = "Search on popular textbooks";
                uri = $"search?search_term=linux";
                ViewData["searchTerm"] = "default search (linux)";
            }
            else
            {
                ViewData["Title"] = $"Textbooks with search term {searchTerm}";
                uri = $"search?search_term={searchTerm}";
                ViewData["searchTerm"] = searchTerm;
            }

            HttpClient client = clientFactory.CreateClient(
              name: "Amazon.Search.Microservice");

            HttpRequestMessage request = new(
              method: HttpMethod.Get, requestUri: uri);

            HttpResponseMessage response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                Root? textbooks = JsonConvert.DeserializeObject<Root>(json);
                return View(textbooks);
            }
            else
            {
                return View("Error");
            }
        }
    }
}
