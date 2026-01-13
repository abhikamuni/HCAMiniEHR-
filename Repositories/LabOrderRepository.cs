using Microsoft.EntityFrameworkCore;
using HCAMiniEHR.Models;

namespace HCAMiniEHR.Repositories
{
    public class LabOrderRepository : ILabOrderRepository
    {
        private readonly HCAMiniContext _context;

        public LabOrderRepository(HCAMiniContext context)
        {
            _context = context;
        }

        public List<LabOrder> GetAllLabOrders()
        {
            return _context.LabOrders
                           .Include(l => l.Appointment) // Join with Appointment
                           .ThenInclude(a => a.Patient) // Join with Patient
                           .ToList();
        }

        public void AddLabOrder(LabOrder order)
        {
            _context.LabOrders.Add(order);
            _context.SaveChanges();
        }

        public List<LabOrder> GetPendingOrders()
        {
            // LINQ Requirement: Simple Where clause
            return _context.LabOrders
                           .Include(l => l.Appointment)
                           .ThenInclude(a => a.Patient)
                           .Where(l => l.Status == "Pending")
                           .ToList();
        }
    }
}
