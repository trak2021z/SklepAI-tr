using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SklepAI.Models
{
    public class CsvProduct
    {
        public string ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Tags { get; set; }
        public string OptionName1 { get; set; }
        public string OptionName2 { get; set; }
        public string OptionName3 { get; set; }
        public string OptionValue3 { get; set; }
        public string OptionValue2 { get; set; }
        public string OptionValue1 { get; set; }
        public string VariantPrice { get; set; }
        public string BasePrice { get; set; }
        public string ImgURL { get; set; }
        public string ProductComposition { get; set; }
        public string MadeIn { get; set; }
        public string Quantity { get; set; }
    }
}
