using Domain.Interfaces;

namespace Domain.Orders
{
    public interface IOrderRepository : IAsyncRepository<Order>
    {
    }
}
