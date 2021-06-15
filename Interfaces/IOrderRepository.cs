using SklepAI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SklepAI.Interfaces
{
    public interface IOrderRepository 
    {
        IQueryable<Order> Orders { get; }

        void SaveOrder(Order order);
        Order DeleteOrder(int orderID);
        Order GetOrder(int orderID);
    }
}
