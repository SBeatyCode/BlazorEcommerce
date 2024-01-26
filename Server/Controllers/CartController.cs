using BlazorEcommerce.Server.Services.CartService;
using BlazorEcommerce.Shared.DTOs;
using BlazorEcommerce.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CartController : ControllerBase
	{
		private readonly ICartService _cartService;

		public CartController(ICartService cartService) 
		{
			_cartService = cartService;
		}

		[HttpPost("/products")]
		public async Task<ActionResult<ServiceResponse<List<CartItemProductResponse>>>> GetCartItemProducts(List<CartItem> cartItems)
		{
			var response = await _cartService.GetCartItemProducts(cartItems);

			if (response == null)
				return NotFound(response);
			else
				return Ok(response);
		}
	}
}
