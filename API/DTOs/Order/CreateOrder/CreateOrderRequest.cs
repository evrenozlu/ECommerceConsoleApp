namespace API.DTOs.Order.CreateOrder
{
    public class CreateOrderRequest
    {
        public string ProductCode { get; set; }
        public int Quantity { get; set; }
    }
}