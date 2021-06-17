using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SklepAI.Models.ViewModels
{
    public class ProductDetailsViewModel
    {
        public IQueryable<Product> Products { get; set; }
        public Product Selected { get; set; }
        public string ReturnUrl { get; set; }
    }
}
