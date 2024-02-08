using BlazorEcommerce.Server.Services.CartService;
using BlazorEcommerce.Shared.DTOs;
using BlazorEcommerce.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

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

		[HttpPost("/store-cart"), Authorize]
		public async Task<ActionResult<ServiceResponse<List<CartItemProductResponse>>>> StoreCartItems(List<CartItem> cartItems)
		{
			var response = await _cartService.StoreCartItems(cartItems);

			if (response == null)
				return BadRequest(response);
			else
				return Ok(response);
		}

		[HttpGet("/count")]
		public async Task<ActionResult<ServiceResponse<int>>> GetCartCount()
		{
			var response = await _cartService.GetCartCount();

			if (response == null)
				return BadRequest(response);
			else
				return Ok(response);
		}

		[HttpGet("/get-cart-db")]
		public async Task<ActionResult<ServiceResponse<List<CartItemProductResponse>>>> GetCartProductsFromDatabase()
		{
			var response = await _cartService.GetCartProductsFromDatabase();

			if (response == null)
				return BadRequest(response);
			else
				return Ok(response);
		}

		[HttpPost("/add")]
        public async Task<ActionResult<ServiceResponse<bool>>> AddToCart(CartItem cartItem)
		{
			var response = await _cartService.AddToCart(cartItem);

            if (response == null)
                return BadRequest(response);
            else
                return Ok(response);
        }

		[HttpPut("/update-quantity")]
        public async Task<ActionResult<ServiceResponse<bool>>> UpdateItemQuantity(CartItem cartItem)
		{
			var response = await _cartService.UpdateItemQuantity(cartItem);

            if (response == null)
                return BadRequest(response);
            else
                return Ok(response);
        }

		[HttpDelete("/delete-item/{cartItem}")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteItem(CartItem cartItem)
		{
			var response = _cartService.DeleteItem(cartItem);

            if (response == null)
                return BadRequest(response);
            else
                return Ok(response);
        }

    }
}
