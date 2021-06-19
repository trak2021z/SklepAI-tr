﻿using Microsoft.EntityFrameworkCore;
using SklepAI.Data;
using SklepAI.Interfaces;
using SklepAI.Models;
using System.Linq;

namespace SklepAI.EFRepositoryControllers
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext context;

        public OrderRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IQueryable<Order> Orders => 
            context.Orders.Include(o => o.Lines)
            .ThenInclude(l => l.Product)
            .Include(o => o.Shipment);

        public Order DeleteOrder(int orderID)
        {
            Order dbEntry = GetOrder(orderID);
            if (dbEntry != null)
            {
                context.Orders.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }

        public Order GetOrder(int orderID) =>
            Orders.FirstOrDefault(o => o.OrderID == orderID);

        public void SaveOrder(Order order)
        {
            context.AttachRange(order.Lines.Select(l => l.Product));
            context.Attach(order.Shipment);
            if (order.OrderID == 0)
            {
                context.Orders.Add(order);
            }
            context.SaveChanges();
        }
    }
}
