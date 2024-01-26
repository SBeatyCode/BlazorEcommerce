using BlazorEcommerce.Shared;
using BlazorEcommerce.Shared.DTOs;

namespace BlazorEcommerce.Client.Services.CartService
{
	public interface ICartService
	{
		event Action OnCartChange;
		Task AddToCart(CartItem cartItem);
		Task RemoveFromCart(int productID, int productTypeId);
		Task UpdateItemQuantity(CartItemProductResponse cartItemProduct);
		Task<List<CartItem>> GetCartItems();
		Task<List<CartItemProductResponse>> GetCartProducts();
	}
}
