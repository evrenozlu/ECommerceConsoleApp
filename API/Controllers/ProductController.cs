using API.DTOs.Product.AddProduct;
using API.DTOs.Product.GetProduct;
using API.DTOs.Product.UpdateProduct;
using API.Services.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _service;

        public ProductController(ProductService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetProduct([FromQuery] GetProductRequest request)
        {
            var product = await _service.GetProduct(request);
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] AddProductRequest request)
        {
            var product = await _service.AddProduct(request);
            return Ok(product);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProductQuantity([FromBody] UpdateProductQuantityRequest request)
        {
            var product = await _service.UpdateProductQuantity(request);
            return Ok(product);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProducts()
        {
            var product = await _service.DeleteProducts();
            return Ok(product);
        }
    }
}
