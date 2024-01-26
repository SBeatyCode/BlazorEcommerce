using BlazorEcommerce.Shared;
using BlazorEcommerce.Shared.DTOs;

namespace BlazorEcommerce.Server.Services.CartService
{
	public interface ICartService
	{
		Task<ServiceResponse<List<CartItemProductResponse>>> GetCartItemProducts(List<CartItem> cartItems);
	}
}
