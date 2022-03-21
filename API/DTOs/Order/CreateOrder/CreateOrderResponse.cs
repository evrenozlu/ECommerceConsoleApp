using API.DTOs.Base;

namespace API.DTOs.Order.CreateOrder
{
    public class CreateOrderResponse : BaseResponse
    {
        public string ProductCode { get; set; }
        public int Quantity { get; set; }
    }
}