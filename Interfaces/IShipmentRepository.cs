using SklepAI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SklepAI.Interfaces
{
    public interface IShipmentRepository
    {
        IQueryable<Shipment> Shipments { get; } 
        void SaveShipment(Shipment shipment);
        Shipment DeleteShipment(int shipmentID);
    }
}
