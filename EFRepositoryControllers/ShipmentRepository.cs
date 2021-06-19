using SklepAI.Data;
using SklepAI.Interfaces;
using SklepAI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SklepAI.EFRepositoryControllers
{
    public class ShipmentRepository : IShipmentRepository
    {
        private readonly ApplicationDbContext context;

        public ShipmentRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IQueryable<Shipment> Shipments => context.Shipments;

        public Shipment DeleteShipment(int shipmentID)
        {
            Shipment shipment = context.Shipments.FirstOrDefault(s => s.ShipmentId == shipmentID);
            if (shipment != null)
            {
                context.Shipments.Remove(shipment);
                context.SaveChanges();
            }
            return shipment;
        }

        public void SaveShipment(Shipment shipment)
        {
            if (shipment.ShipmentId == 0)
            {
                context.Shipments.Add(shipment);
            }
            else
            {
                Shipment dbEntry = context.Shipments.FirstOrDefault(p => p.ShipmentId == shipment.ShipmentId);
                if (dbEntry != null)
                {
                    dbEntry.Description = shipment.Description;
                    dbEntry.Price = shipment.Price;
                }
            }
            context.SaveChanges();
        }
    }
}
