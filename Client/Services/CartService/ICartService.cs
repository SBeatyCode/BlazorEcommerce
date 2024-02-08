using BlazorEcommerce.Shared;
using BlazorEcommerce.Shared.DTOs;

namespace BlazorEcommerce.Client.Services.CartService
{
	public interface ICartService
	{
		event Action OnCartChange;
		Task AddToCart(CartItem cartItem);
		Task RemoveFromCart(CartItemProductResponse cartItemProduct);
		Task UpdateItemQuantity(CartItemProductResponse cartItemProduct);
		Task<List<CartItemProductResponse>> GetCartProducts();
		Task StoreCartItems(bool emptyLocalCart);
		Task UpdateCartItemCount();
	}
}
