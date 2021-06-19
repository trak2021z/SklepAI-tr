using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SklepAI.Interfaces;
using SklepAI.Models;
using SklepAI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SklepAI.Controllers
{
    public class OrderController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly IOrderRepository orderRepository;
        private readonly IShipmentRepository shipmentRepository;
        private readonly Cart cart;
        private readonly UserManager<IdentityUser> userManager;

        public OrderController(
            IProductRepository productRepository,
            IOrderRepository orderRepository,
            IShipmentRepository shipmentRepository,
            Cart cartService,
            UserManager<IdentityUser> userManager)
        {
            this.productRepository = productRepository;
            this.orderRepository = orderRepository;
            this.shipmentRepository = shipmentRepository;
            this.cart = cartService;
            this.userManager = userManager;
        }

        [Authorize]
        public async Task<ViewResult> List()
        {
            if (User.IsInRole("Admins"))
            {
                return View(orderRepository.Orders);

            }
            else
            {
                IdentityUser currentUser = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                return View(orderRepository.Orders.Where(o => o.UserId == currentUser.Id));
            }
        }

        [Authorize]
        public ViewResult Checkout()
        {
            return View(new OrderViewModel
            {
                Order = new Order(),
                Shipments = new SelectList(shipmentRepository.Shipments, "ShipmentId", "Description")
            });

        }
        [HttpPost]
        public async Task<IActionResult> Checkout(OrderViewModel viewModel)
        {
            if (!cart.Lines.Any())
            {
                ModelState.AddModelError("", "Cart is empty!");
            }

            if (ModelState.IsValid)
            {

                IdentityUser currentUser = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                viewModel.Order.UserId = currentUser.Id;
                viewModel.Order.Lines = cart.Lines.ToArray();
                viewModel.Order.Shipment = shipmentRepository.Shipments.FirstOrDefault(s => s.ShipmentId == viewModel.Order.ShipmentId);
                orderRepository.SaveOrder(viewModel.Order);
                foreach (CartLine line in viewModel.Order.Lines)
                {
                    Product product = productRepository.Products.FirstOrDefault(p => p.ProductID == line.ProductId);
                    product.Quantity -= line.Quantity;
                    productRepository.SaveProduct(product);
                }
                return RedirectToAction(nameof(Summary), new { id = viewModel.Order.OrderID }); ;
            }
            else
            {
                viewModel.Shipments = new SelectList(shipmentRepository.Shipments, "ShipmentId", "Description");
                return View(viewModel);
            }
        }

        public IActionResult Summary(int id)
        {
            Order order = orderRepository.GetOrder(id);
            cart.Clear();
            return View(order);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int orderID)
        {
            Order order = orderRepository.GetOrder(orderID);
            if (order != null)
            {
                orderRepository.DeleteOrder(orderID);
            }
            return RedirectToAction(nameof(List));
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Pay(int orderId)
        {
            var order = orderRepository.GetOrder(orderId);
            order.PaymentStatus = "Payed";
            orderRepository.SaveOrder(order);
            return RedirectToAction(nameof(Completed));
        }
        public ViewResult Completed(string status) => View("Completed", status);

    }
}
