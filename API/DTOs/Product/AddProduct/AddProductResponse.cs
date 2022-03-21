using API.DTOs.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Product.AddProduct
{
    public class AddProductResponse : BaseResponse
    {
        public string Id { get; set; }
        public int Stock { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal DefaultPrice { get; set; }
    }
}