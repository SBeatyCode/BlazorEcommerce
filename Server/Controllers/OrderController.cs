using BlazorEcommerce.Server.Services.OrderService;
using BlazorEcommerce.Shared;
using BlazorEcommerce.Shared.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService) 
        {
            _orderService = orderService;
        }

        [HttpGet("/get-orders")]
        public async Task<ActionResult<ServiceResponse<List<OrderResponse>>>> GetOrders()
        {
            var response = await _orderService.GetOrders();

            if (response == null || response.Success == false)
                return BadRequest(response);
            else
                return Ok(response);
        }

        [HttpGet("/order-details/{orderId}")]
		public async Task<ActionResult<ServiceResponse<OrderDetailsResponse>>> GetOrderDetails(int orderId)
        {
            var response = await _orderService.GetOrderDetails(orderId);

			if (response == null || response.Success == false)
				return BadRequest(response);
			else
				return Ok(response);
		}

	}
}
