using API.DTOs.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Product.GetProduct
{
    public class GetProductResponse : BaseResponse
    {
        public string Id { get; set; }
        public int Stock { get; set; }
        public decimal CurrentPrice { get; set; }
    }
}