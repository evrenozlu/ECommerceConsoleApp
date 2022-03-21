using Domain.Orders;

namespace Infrastructure.Data.Repositories
{
    public class OrderRepository : RepositoryBase<Order>
        , IOrderRepository
    {
        public OrderRepository(EFContext dbContext) : base(dbContext)
        {
        }
    }
}