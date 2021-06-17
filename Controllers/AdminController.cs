using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SklepAI.Interfaces;
using SklepAI.Models;
using SklepAI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SklepAI.Controllers
{
    public class AdminController : Controller
    {
        private IProductRepository productRepository;
        public AdminController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public IActionResult Index()
        {
            return View(productRepository.Products.AsEnumerable());
        }


        #region Product Methods 
        public ViewResult EditProduct(int productId) => View(productRepository.Products.FirstOrDefault(p => p.ProductID == productId));

        [HttpPost]
        public IActionResult EditProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                productRepository.SaveProduct(product);
                TempData["message"] = $"Zapisano {product.Name}.";
                return RedirectToAction("Index");
            }
            else
            {
                return View(product);
            }
        }
        public ViewResult CreateProduct() => View("EditProduct", new Product());

        [HttpPost]
        public IActionResult DeleteProduct(int productId)
        {
            Product deletedProduct = productRepository.DeleteProductByID(productId);
            if (deletedProduct != null)
            {
                TempData["message"] = $"Deleted {deletedProduct.Name}.";
            }
            return RedirectToAction("Index");
        }
        public ViewResult LoadFromCsv() => View();
        [HttpPost]
        public IActionResult ImportData([FromServices] IProductRepository productRepository, IFormFile postedFile, int quantity)
        {
            if (postedFile == null)
                return View(nameof(LoadFromCsv));
            var parser = new CsvParser(postedFile.OpenReadStream());
            var productList = parser.Parse();

            foreach (var item in productList.Take(quantity))
            {
                productRepository.SaveProduct(item);
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
