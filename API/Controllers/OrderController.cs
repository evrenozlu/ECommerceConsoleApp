using API.DTOs.Order.CreateOrder;
using API.Services.Orders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _service;

        public OrderController(OrderService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            var order = await _service.CreateOrder(request);
            return Ok(order);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteOrders()
        {
            var order = await _service.DeleteOrders();
            return Ok(order);
        }
    }
}
