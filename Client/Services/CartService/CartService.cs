using BlazorEcommerce.Shared;
using BlazorEcommerce.Shared.DTOs;
using Blazored.LocalStorage;
using System.Net.Http.Json;

namespace BlazorEcommerce.Client.Services.CartService
{
	/// <summary>
	/// CartService Class. Interacts with the Cart stored on LocalStorage using
	/// LocalStorageService Library
	/// </summary>
	public class CartService : ICartService
	{
		public event Action OnCartChange;

		private readonly ILocalStorageService _localStorageService;
		private readonly HttpClient _httpClient;

		public CartService(ILocalStorageService localStorageService, HttpClient httpClient) 
		{
			_localStorageService = localStorageService;
			_httpClient = httpClient;
		}

		/// <summary>
		/// Adds item to the Cart, stored on the Local Storage. Stored as JSON.
		/// </summary>
		/// <param name="cartItem"></param>
		/// <returns></returns>
		public async Task AddToCart(CartItem cartItem)
		{
			List<CartItem> cart = await GetCart();

			var existingItem = cart.Find(i => i.ProductId == cartItem.ProductId
				&& i.ProductTypeId == cartItem.ProductTypeId);

			if(existingItem != null) 
			{
				existingItem.Quantity += cartItem.Quantity;
			}
			else
			{
                cart.Add(cartItem);
            }

			await _localStorageService.SetItemAsync("cart", cart);
			OnCartChange.Invoke();
		}

		/// <summary>
		/// Removes an Item from the Cart searching by productID and productTypeId, 
		/// and saves the result to Local storage.
		/// </summary>
        public async Task RemoveFromCart(int productID, int productTypeId)
		{
			List<CartItem> cart = await GetCart();

			if(cart != null && cart.Count != 0)
			{
				var cartItem = cart.Find(i => i.ProductId == productID && i.ProductTypeId == productTypeId);
				if (cartItem != null)
				{
					cart.Remove(cartItem);
					await _localStorageService.SetItemAsync("cart", cart);
					OnCartChange.Invoke();
				}
			}
		}

        public async Task UpdateItemQuantity(CartItemProductResponse cartItemProduct)
		{
            List<CartItem> cart = await GetCart();

            if (cart != null && cart.Count != 0)
			{
				var updateProduct = cart.Find(i => i.ProductId == cartItemProduct.ProductId
					&& i.ProductTypeId == cartItemProduct.ProductTypeId);

				if(updateProduct != null) 
				{
					updateProduct.Quantity = cartItemProduct.ProductQuantity;
					await _localStorageService.SetItemAsync("cart", cart);
					OnCartChange.Invoke();
				}
			}
        }

        public async Task<List<CartItem>> GetCartItems()
		{
			List<CartItem> cart = await GetCart();
			return cart;
		}

		/// <summary>
		/// Finds the Products that correalte to the CartItems in the Cart in
		/// LocalStorage, returns a list of CartItemProductResponse DTO
		/// </summary>
		/// <returns></returns>
		public async Task<List<CartItemProductResponse>> GetCartProducts()
		{
			List<CartItem> cart = await GetCart();
			var response = await _httpClient.PostAsJsonAsync("api/cart/products", cart);
			var cartProductsResponse = await response.Content.ReadFromJsonAsync<ServiceResponse<List<CartItemProductResponse>>>();

			return cartProductsResponse.Data;
		}

		/// <summary>
		/// Gets the Cart stored in LocalStorage. If no Cart exists, creates a new
		/// Cart, and returns it.
		/// </summary>
		/// <returns></returns>
		private async Task<List<CartItem>> GetCart()
		{
			var cart = await _localStorageService.GetItemAsync<List<CartItem>>("cart");
			if (cart == null)
			{
				cart = new List<CartItem>();
			}
			return cart;
		}
	}
}
