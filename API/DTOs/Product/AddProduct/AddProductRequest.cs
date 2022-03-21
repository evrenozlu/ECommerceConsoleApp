namespace API.DTOs.Product.AddProduct
{
    public class AddProductRequest
    {
        public string Id { get; set; }
        public int Stock { get; set; }
        public decimal DefaultPrice { get; set; }
    }
}