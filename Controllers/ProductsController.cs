using Microsoft.AspNetCore.Mvc;
using SklepAI.Interfaces;
using SklepAI.Models;
using SklepAI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SklepAI.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly int pageSize = 9;

        public ProductsController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }


        public ViewResult List(string category, int productPage = 1)
        {
            return View(new ProductsListViewModel
            {
                Products = productRepository.Products
                               .AsEnumerable().Select(p => new Product
                               {
                                   ProductID = p.ProductID,
                                   Category = category ?? p.Category,
                                   Colour = p.Colour,
                                   Description = p.Description,
                                   ImgUrl = p.ImgUrl,
                                   Lines = p.Lines,
                                   Name = p.Name,
                                   Price = p.Price,
                                   Quantity = p.Quantity,
                                   Size = p.Size
                               })
                               .GroupBy(p => p.Name).Select(p => p.First())
                               .OrderBy(p => p.ProductID)
                               .Skip((productPage - 1) * pageSize)
                               .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = pageSize,
                    TotalItems = category == null ? productRepository.Products.AsEnumerable().GroupBy(p => p.Name).Select(p => p.First()).Count() : productRepository.Products.Where(e => e.Category == category)
                               .GroupBy(p => p.Name)
                               .Select(p => p.First())
                               .Count()
                },
                CurrentCategory = category
            });
        }
        public ViewResult Details(int productId, string returnUrl)
        {
            Product product = productRepository.Products.FirstOrDefault(p => p.ProductID == productId);
            return View(new ProductDetailsViewModel
            {
                Products = productRepository.Products
                .Where(p => p.Name == product.Name),
                Selected = product,
                ReturnUrl = returnUrl
            });
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
