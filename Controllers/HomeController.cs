using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SklepAI.Interfaces;
using SklepAI.Models;
using SklepAI.Models.ViewModels;
using SklepAI.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SklepAI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index([FromServices] IProductRepository productRepository)
        {
            return View(new IndexViewModel(productRepository));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [Authorize]
        public IActionResult SeedDatabase([FromServices] IProductRepository productRepository)
        {
            productRepository.SaveProduct(new Product
            {
                Description = "Opis",
                Name = "Nazwa",
                Price = 12.50M,
                Category = "Kategoria"
            });
            return RedirectToAction(nameof(Index));
        }  
    
        [HttpPost]
        public IActionResult ImportData([FromServices] IProductRepository productRepository, IFormFile postedFile)
        {
            var parser = new CsvParser(postedFile.OpenReadStream());
            var productList = parser.Parse();

            foreach (var item in productList.Take(50))
            {
                productRepository.SaveProduct(item);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
