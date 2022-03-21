namespace API.DTOs.Product.UpdateProduct
{
    public class UpdateProductQuantityRequest
    {
        public string Id { get; set; }
        public int OrderQuantity { get; set; }
    }
}