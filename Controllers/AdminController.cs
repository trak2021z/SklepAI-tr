using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private IProductRepository productRepository;
        private readonly IShipmentRepository shipmentRepository;
        private readonly IOrderRepository orderRepository;
        public AdminController(
            IProductRepository productRepository,
            IShipmentRepository shipmentRepository,
            IOrderRepository orderRepository)
        {
            this.productRepository = productRepository;
            this.shipmentRepository = shipmentRepository;
            this.orderRepository = orderRepository;
        }

        public IActionResult Index()
        {
            return View(productRepository.Products.AsEnumerable());
        }
        public IActionResult Orders()
        {
            return View(orderRepository.Orders.AsEnumerable());
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
        #region ShipmentMethods 
        public IActionResult Shipments()
        {
            return View(shipmentRepository.Shipments.AsEnumerable());
        }

        public ViewResult CreateShipment() => View("EditShipment", new Shipment());
        public ViewResult EditShipment(int shipmentId) => View(shipmentRepository.Shipments.FirstOrDefault(s => s.ShipmentId == shipmentId));

        [HttpPost]
        public IActionResult EditShipment(Shipment shipment)
        {
            if (ModelState.IsValid)
            {
                shipmentRepository.SaveShipment(shipment);
                TempData["message"] = $"Saved {shipment.Description}.";
                return RedirectToAction(nameof(Shipments));
            }
            else
            {
                return View(shipment);
            }
        }

        [HttpPost]
        public IActionResult DeleteShipment(int shipmentId)
        {
            Shipment deletedShip = shipmentRepository.DeleteShipment(shipmentId);
            if (deletedShip != null)
            {
                TempData["message"] = $"Deleted {deletedShip.Description}.";
            }
            return RedirectToAction("Shipments");
        }
        #endregion
        #region Order Methods
        [HttpPost]
        public IActionResult DeleteOrder(int orderID)
        {
            Order order = orderRepository.GetOrder(orderID);
            if (order != null)
            {
                orderRepository.DeleteOrder(orderID);
            }
            return RedirectToAction(nameof(Orders));
        }
        #endregion
    }
}
