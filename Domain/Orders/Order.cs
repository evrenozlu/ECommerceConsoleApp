using Domain.Base;

namespace Domain.Orders
{
    public partial class Order : BaseEntity<int>
    {
        public string ProductCode { get; private set; }
        public int Quantity { get; private set; }
    }
}