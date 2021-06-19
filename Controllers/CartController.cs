using Microsoft.AspNetCore.Mvc;
using SklepAI.Interfaces;
using SklepAI.Models;
using SklepAI.Models.ViewModels;
using System.Linq;

namespace SklepAI.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly Cart cart;

        public CartController(IProductRepository productRepository, Cart cartService)
        {
            this.productRepository = productRepository;
            this.cart = cartService;
        } 
        public RedirectToActionResult AddToCart(int productId, string returnUrl)
        {
            Product product = productRepository.Products.FirstOrDefault(p => p.ProductID == productId);
            if (product != null)
            {
                cart.AddItem(product, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToActionResult RemoveFromCart(int productId, string returnUrl)
        {
            Product product = productRepository.Products.FirstOrDefault(p => p.ProductID == productId);
            if (product != null)
            {
                cart.RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
        
        public ViewResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            }); 
        }
    }
}
