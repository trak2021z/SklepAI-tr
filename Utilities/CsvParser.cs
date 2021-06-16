using CsvHelper;
using CsvHelper.Configuration;
using SklepAI.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SklepAI.Utilities
{
    public class CsvParser
    {
        private readonly Stream fileStream;
        public CsvParser(Stream fileStream)
        {
            this.fileStream = fileStream;
        }

        public List<Product> Parse()
        {
            List<Product> finalProducts = new();
            using TextReader streamReader = new StreamReader(fileStream);
            using (var csv = new CsvReader(streamReader, new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ",", BadDataFound = null }))
            {
                csv.Context.RegisterClassMap<ProductMap>();
                var records = csv.GetRecords<CsvProduct>();
                foreach (var i in records)
                {
                    if (i.OptionName1 == "Colour" && i.OptionName2 == "Size")
                        finalProducts.Add(new Product
                        {
                            Name = i.Name,
                            Description = i.Description,
                            Size = i.OptionValue2,
                            Colour = i.OptionValue1,
                            Category = i.Tags,
                            Price = Convert.ToDecimal(i.BasePrice, CultureInfo.InvariantCulture),
                            ImgUrl = i.ImgURL,
                            Quantity = int.Parse(i.Quantity)
                        });
                }
            }

            return finalProducts;
        }


        public class ProductMap : ClassMap<CsvProduct>
        {
            public ProductMap()
            {
                Map(m => m.ProductID).Name("ProductID");
                Map(m => m.Name).Name("Name");
                Map(m => m.Description).Name("Description");
                Map(m => m.Type).Name("Type");
                Map(m => m.Tags).Name("Tags");
                Map(m => m.OptionName1).Name("OptionName1");
                Map(m => m.OptionName2).Name("OptionName2");
                Map(m => m.OptionName3).Name("OptionName3");
                Map(m => m.OptionValue1).Name("OptionValue1");
                Map(m => m.OptionValue2).Name("OptionValue2");
                Map(m => m.OptionValue3).Name("OptionValue3");
                Map(m => m.VariantPrice).Name("VariantPrice");
                Map(m => m.BasePrice).Name("BasePrice");
                Map(m => m.ImgURL).Name("ImgURL");
                Map(m => m.ProductComposition).Name("ProductComposition");
                Map(m => m.MadeIn).Name("MadeIn");
                Map(m => m.Quantity).Name("Variant Quantity");
            }
        }
    }
}
