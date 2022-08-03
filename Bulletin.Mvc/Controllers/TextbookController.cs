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
        //private readonly BulletinContext db;
        private readonly IHttpClientFactory clientFactory;

        public TextbookController(ILogger<TextbookController> logger,
            BulletinContext injectedContext,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            //.NET uses constructor parameter injection to pass an instance
            // of the BulletinContext db context using the the connection string
            //specified in Program.cs
            
            //db = injectedContext;
            clientFactory = httpClientFactory;
        }

        // Textbook/Index
        //improve performance by caching with browser
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any)]
        public IActionResult Index()
        {
            return View();
        }
        
        // Textbook/Text?searchTerm  GET
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
                //SearchResult? textbooks = JsonConvert.DeserializeObject<SearchResult>(json);
                return View(textbooks);
            }
            else
            {
                return View("Error");
            }

            //IEnumerable<Textbook>? model = await response.Content
            //  .ReadFromJsonAsync<IEnumerable<Textbook>>();

            //string? respString = await response.Content.ReadAsStringAsync().Result;
        }
    }
}
