using BlazorEcommerce.Shared;
using BlazorEcommerce.Shared.DTOs;

namespace BlazorEcommerce.Server.Services.CartService
{
	public interface ICartService
	{
		Task<ServiceResponse<List<CartItemProductResponse>>> GetCartItemProducts(List<CartItem> cartItems);
		Task<ServiceResponse<List<CartItemProductResponse>>> StoreCartItems(List<CartItem> cartItems);
		Task<ServiceResponse<int>> GetCartCount();
		Task<ServiceResponse<List<CartItemProductResponse>>> GetCartProductsFromDatabase();
		Task<ServiceResponse<bool>> AddToCart(CartItem cartItem);
		Task<ServiceResponse<bool>> UpdateItemQuantity(CartItem cartItem);
		Task<ServiceResponse<bool>> DeleteItem(CartItem cartItem);
		Task<ServiceResponse<bool>> EmptyCart();
	}
}
