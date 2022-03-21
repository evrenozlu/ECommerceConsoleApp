using Domain.Base;

namespace Domain.Orders
{
    public partial class Order: IAggregateRoot
    {
        public Order(string productCode
            , int quantity)
        {
            ProductCode = productCode;
            Quantity = quantity;
        }
    }
}
