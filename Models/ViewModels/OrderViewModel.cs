using Microsoft.AspNetCore.Mvc.Rendering;

namespace SklepAI.Models.ViewModels
{
    public class OrderViewModel
    {
        public SelectList Shipments { get; set; }
        public Order Order { get; set; }

    }
}
