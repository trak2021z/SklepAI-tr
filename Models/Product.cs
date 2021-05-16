using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SklepAI.Models
{
    public class Product
    { 
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Proszę podać nazwę produktu.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Proszę podać opis produktu.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Proszę podać dodatnią cenę.")]
        [Range(0, 999999999999999999.99)]
        [Column(TypeName = "decimal(18, 2)")]
        [RegularExpression(@"^\d+\,\d{0,2}$")]
        public decimal Price { get; set; }

        public string Size { get; set; }

        public string Colour { get; set; }

        [Required(ErrorMessage = "Proszę określić kategorię produktu.")]
        public string Category { get; set; }

        public string ImgUrl { get; set; }

        public int Quantity { get; set; }

        public ICollection<CartLine> Lines { get; set; }
    }
}
