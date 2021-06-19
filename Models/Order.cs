using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SklepAI.Models
{
    public class Order
    {
        [BindNever]
        public int OrderID { get; set; }

        public string UserId { get; set; }

        [BindNever]
        public ICollection<CartLine> Lines { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string Building { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Zip { get; set; }
        [Required]
        public string Country { get; set; } 
        public bool GiftWrap { get; set; }
        [BindNever]
        public bool Shipped { get; set; }
        public string PaymentStatus { get; set; }
        public string Token { get; set; }
        [BindNever]
        public Shipment Shipment { get; set; }
        public int ShipmentId { get; set; }

        public decimal ComputeTotalValue() => Lines.Sum(e => e.Product.Price * e.Quantity);
    }
}
