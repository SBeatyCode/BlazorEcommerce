using BlazorEcommerce.Shared;
using BlazorEcommerce.Shared.DTOs;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Security.Claims;

namespace BlazorEcommerce.Client.Services.CartService
{
	/// <summary>
	/// CartService Class. Interacts with the Cart stored on LocalStorage using
	/// LocalStorageService Library
	/// </summary>
	public class CartService : ICartService
	{
		private readonly ILocalStorageService _localStorageService;
		private readonly HttpClient _httpClient;
		private readonly AuthenticationStateProvider _authenticationStateProvider;

		public event Action OnCartChange;

		public CartService(ILocalStorageService localStorageService, HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider) 
		{
			_localStorageService = localStorageService;
			_httpClient = httpClient;
			_authenticationStateProvider = authenticationStateProvider;
		}

		/// <summary>
		/// Adds item to the Cart, on the Database if Authenticated, otherwise
		/// on the Local Storage. Stored as JSON.
		/// </summary>
		/// <param name="cartItem"></param>
		/// <returns></returns>
		public async Task AddToCart(CartItem cartItem)
		{
			if(await IsUserAuthenticated())
			{
				var response = await _httpClient.PostAsJsonAsync("api/cart/add", cartItem);
            }
			else
			{
                List<CartItem> cart = await GetCartFromLocalStorage();

                var existingItem = cart.Find(i => i.ProductId == cartItem.ProductId
                    && i.ProductTypeId == cartItem.ProductTypeId);

                if (existingItem != null)
                {
                    existingItem.Quantity += cartItem.Quantity;
                }
                else
                {
                    cart.Add(cartItem);
                }

                await _localStorageService.SetItemAsync("cart", cart);
            }
			await UpdateCartItemCount();
		}

		/// <summary>
		/// Removes an Item from the Cart searching by productID and productTypeId, 
		/// and saves the result to the Database or Local storage.
		/// </summary>
        public async Task RemoveFromCart(CartItemProductResponse cartItemProduct)
		{
            if (await IsUserAuthenticated())
            {
                CartItem cartItem = new CartItem
                {
                    UserId = int.Parse(_authenticationStateProvider
                        .GetAuthenticationStateAsync().Result.User
                        .FindFirst(ClaimTypes.NameIdentifier).Value),
                    ProductId = cartItemProduct.ProductId,
                    ProductTypeId = cartItemProduct.ProductTypeId,
                    Quantity = cartItemProduct.ProductQuantity
                };

                await _httpClient.DeleteAsync($"api/cart/delete-item/{cartItem}");
            }
			else
			{
                List<CartItem> cart = await GetCartFromLocalStorage();

                if (cart != null && cart.Count != 0)
                {
                    var cartItem = cart.Find(i => i.ProductId == cartItemProduct.ProductId
						&& i.ProductTypeId == cartItemProduct.ProductTypeId);
                    if (cartItem != null)
                    {
                        cart.Remove(cartItem);
                        await _localStorageService.SetItemAsync("cart", cart);
                        await UpdateCartItemCount();
                    }
                }
            }
            await UpdateCartItemCount();
        }

		/// <summary>
		/// Updates the Quantity of an Item in the Cart (Database or Local) with the
		/// quantity of the passed CartItemProduct
		/// </summary>
        public async Task UpdateItemQuantity(CartItemProductResponse cartItemProduct)
		{
			if(await IsUserAuthenticated())
			{
				CartItem cartItem = new CartItem
				{
					UserId = int.Parse(_authenticationStateProvider
						.GetAuthenticationStateAsync().Result.User
						.FindFirst(ClaimTypes.NameIdentifier).Value),
					ProductId = cartItemProduct.ProductId,
					ProductTypeId = cartItemProduct.ProductTypeId,
					Quantity = cartItemProduct.ProductQuantity
				};
				await _httpClient.PutAsJsonAsync("api/cart/update-quantity", cartItem);
			}
			else
			{
                List<CartItem> cart = await GetCartFromLocalStorage();

                if (cart != null && cart.Count != 0)
                {
                    var updateProduct = cart.Find(i => i.ProductId == cartItemProduct.ProductId
                        && i.ProductTypeId == cartItemProduct.ProductTypeId);

                    if (updateProduct != null)
                    {
                        updateProduct.Quantity = cartItemProduct.ProductQuantity;
                        await _localStorageService.SetItemAsync("cart", cart);
                        OnCartChange.Invoke();
                    }
                }
            }
        }

		/// <summary>
		/// Finds the Products that correalte to CartItems in the Cart in
		/// LocalStorage, returns a list of CartItemProductResponse DTO. 
		/// If user is Authenticated, gets Cart from Database, otherwise gets
		/// cart from LocalStorage
		/// </summary>
		/// <returns></returns>
		public async Task<List<CartItemProductResponse>> GetCartProducts()
		{
			if(await IsUserAuthenticated())
			{
				var response = await _httpClient.GetFromJsonAsync<ServiceResponse<List<CartItemProductResponse>>>("api/cart/get-cart-db");

				if (response != null && response.Success)
					return response.Data;
				else
					return new List<CartItemProductResponse>();
			}
			else
			{
				List<CartItem> cart = await GetCartFromLocalStorage();

				if (cart.Count == 0)
					return new List<CartItemProductResponse>();

				var response = await _httpClient.PostAsJsonAsync("api/cart/products", cart);
				var result = await response.Content.ReadFromJsonAsync<ServiceResponse<List<CartItemProductResponse>>>();

				if(result != null && result.Success)
					return result.Data;
				else
					return new List<CartItemProductResponse>();
			}
		}

		/// <summary>
		/// Stores CartItems from LocalStorage into the Database. Local Cart can then be
		/// emptied if needed.
		/// </summary>
		public async Task StoreCartItems(bool emptyLocalCart = false)
		{
			var localCart = await GetCartFromLocalStorage();

			if(localCart != null)
			{
				await _httpClient.PostAsJsonAsync("api/cart/store-cart", localCart);

				if(emptyLocalCart) 
				{
					await _localStorageService.RemoveItemAsync("cart");
				}
			}
		}

		/// <summary>
		/// Gets the number of items in the Cart from the Database if Authenticated, or
		/// from Local Storage if not, and sets the CartCount in LocalStorage based on how
		/// many items are in the Cart
		/// </summary>
		public async Task UpdateCartItemCount()
		{
			if(await IsUserAuthenticated())
			{
				//Synch LocalCart to DataBase before getting Count
				await StoreCartItems(false);

				var result = await _httpClient.GetFromJsonAsync<ServiceResponse<int>>("api/cart/count");

				if (result != null)
				{
					int count = result.Data;
					await _localStorageService.SetItemAsync("cartCount", count);
				}
			}
			else
			{
				var cart = await GetCartFromLocalStorage();
				int count = cart.Count;
				await _localStorageService.SetItemAsync("cartCount", count);
			}
			OnCartChange.Invoke();
		}

		/// <summary>
		/// Gets the Cart stored in LocalStorage. If no Cart exists, creates a new
		/// Cart, and returns it.
		/// </summary>
		/// <returns></returns>
		private async Task<List<CartItem>> GetCartFromLocalStorage()
		{
			var cart = await _localStorageService.GetItemAsync<List<CartItem>>("cart");
			if (cart == null)
			{
				cart = new List<CartItem>();
			}
			return cart;
		}

		/// <summary>
		/// Checks if the current User is Authenticated
		/// </summary>
		private async Task<bool> IsUserAuthenticated() => (await _authenticationStateProvider.GetAuthenticationStateAsync())
			.User.Identity.IsAuthenticated;
	}
}
