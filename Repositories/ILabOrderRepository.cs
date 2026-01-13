using HCAMiniEHR.Models;

namespace HCAMiniEHR.Repositories
{
    public interface ILabOrderRepository
    {
        // Basic CRUD
        List<LabOrder> GetAllLabOrders();
        void AddLabOrder(LabOrder order);

        // Custom query for Reports later
        List<LabOrder> GetPendingOrders();
    }
}
