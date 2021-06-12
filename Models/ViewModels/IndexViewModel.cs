using SklepAI.EFRepositoryControllers;
using SklepAI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SklepAI.Models.ViewModels
{
    public class IndexViewModel
    {
        public IProductRepository ProductRepository { get; }
        public bool ShowSeedDatabaseButton => !ProductRepository.Products.Any();
        public IndexViewModel(IProductRepository productRepository)
        {
            this.ProductRepository = productRepository;
        }

    }
}
