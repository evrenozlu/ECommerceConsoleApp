using API.DTOs.Base;

namespace API.DTOs.Product.UpdateProduct
{
    public class UpdateProductQuantityResponse : BaseResponse
    {
        public string Id { get; set; }
        public int Stock { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal DefaultPrice { get; set; }
    }
}