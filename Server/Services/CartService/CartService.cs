using BlazorEcommerce.Shared.DTOs;
using BlazorEcommerce.Shared;
using BlazorEcommerce.Server.Data;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Security.Claims;
using System.Linq;

namespace BlazorEcommerce.Server.Services.CartService
{
	public class CartService : ICartService
	{
		private readonly DataContext _dataContext;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public CartService(DataContext dataContext, IHttpContextAccessor httpContextAccessor) 
		{
			_dataContext = dataContext;
			_httpContextAccessor = httpContextAccessor;
		}

		/// <summary>
		/// "Converts" The list of CartItems into a list of CartItemProductResponse DTO's
		/// </summary>
		public async Task<ServiceResponse<List<CartItemProductResponse>>> GetCartItemProducts(List<CartItem> cartItems)
		{
			var response = new ServiceResponse<List<CartItemProductResponse>>();
			var cartItemResponses = new List<CartItemProductResponse>();

            foreach (var item in cartItems)
            {
				var product = await _dataContext.Products
					.Where(p => p.Id == item.ProductId).FirstOrDefaultAsync();

				if (product != null)
				{
					var productVarient = await _dataContext.ProductVarients
						.Where(v => v.ProductId == item.ProductId &&
							v.ProductTypeId == item.ProductTypeId)
						.Include(v => v.ProductType)
						.FirstOrDefaultAsync();

					if (productVarient != null)
					{
						CartItemProductResponse cartItemProductResponse = new CartItemProductResponse
						{
							ProductId = product.Id,
							ProductName = product.Name,
							ProductImageUrl = product.ImageUrl,
							ProductTypeId = productVarient.ProductTypeId,
							ProductPrice = productVarient.Price,
							ProductTypeName = productVarient.ProductType.Name,
							ProductQuantity = item.Quantity
						};

						cartItemResponses.Add(cartItemProductResponse);
					}
					else continue;
				}
				else continue;
            }

			response.Data = cartItemResponses;
			response.Success = true;

			if (response.Data.Count == 0) 
				response.Message = "No matching Products found";
			else
				response.Message = "Products that correlate to CartItems found";

			return response;
        }

		/// <summary>
		/// Saves the List of CartItems to the database, and returns the updated List of
		/// Cart Items that was given to the method along with any that were already stored
		/// based on the UserId. Should be called after a User sucessfully Logs In,
		/// and is Authenticated, so that Local and DataBase Carts are Synched
		/// </summary>
		public async Task<ServiceResponse<List<CartItemProductResponse>>> StoreCartItems(List<CartItem> cartItems)
		{
			var response = new ServiceResponse<List<CartItemProductResponse>>();
			int? userId = GetUserID();

			if (cartItems != null && cartItems.Count > 0 && userId > 0 && userId != null)
			{
				//put in a try - catch in case UserId is bad
				try
				{
					cartItems.ForEach(cartItem => cartItem.UserId = (int)userId);
					_dataContext.CartItems.AddRange(cartItems);
					await _dataContext.SaveChangesAsync();

					response = await GetCartProductsFromDatabase();
				}
				catch (Exception ex) 
				{
					response.Data = null;
					response.Success = false;
					response.Message = "An error occurred: " + ex.Message;
				}
			}
			else
			{
				response.Data = null;
				response.Success = false;
				response.Message = "The CartItem List was empty or null, or the UserID is invalid or Unauthorized";
			}

			return response;
		}

		public async Task<ServiceResponse<int>> GetCartCount()
		{
			var response = new ServiceResponse<int>();
			int? userId = GetUserID();

			if(userId > 0 && userId != null)
			{
				int count = await _dataContext.CartItems
					.Where(ci => ci.UserId == userId)
					.CountAsync();

				response.Data = count;
				response.Success = true;
				response.Message = $"There are {count} CartItems in the Cart on the Database";
			}
			else
			{
				response.Data = -1;
				response.Success = false;
				response.Message = "Could not get an Authenticated UserId";
			}
			return response;
		}

		/// <summary>
		/// Gets the Cart from the DataBase, and returns a list of CartItemProductResponses
		/// </summary>
		public async Task<ServiceResponse<List<CartItemProductResponse>>> GetCartProductsFromDatabase()
		{
			var cart = await _dataContext.CartItems.Where(ci => ci.UserId == GetUserID()).ToListAsync();
			var response = await GetCartItemProducts(cart);

			if(response != null && response.Data != null)
			{
				response.Success = true;
				response.Message = "Operation was sucessful";
			}
			else
			{
				response.Success = false;
				response.Message = "Could not get CartItems from the Database";
			}
			return response;
		}

		public async Task<ServiceResponse<bool>> AddToCart(CartItem cartItem)
		{
			var response = new ServiceResponse<bool>();
			cartItem.UserId = (int)GetUserID();

			if(cartItem.UserId > 0)
			{
				var existingItem = await _dataContext.CartItems
					.FirstOrDefaultAsync(ci => ci.ProductId == cartItem.ProductId 
					&& ci.ProductTypeId == cartItem.ProductTypeId 
					&& ci.UserId == cartItem.UserId);

				if(existingItem != null) 
					existingItem.Quantity+= cartItem.Quantity;
				else
					_dataContext.CartItems.Add(cartItem);

				await _dataContext.SaveChangesAsync();

				response.Data = true;
				response.Success = true;
				response.Message = "Cart Item added";
			}
			else //if something went wrong getting the UserID
			{
                response.Data = false;
                response.Success = false;
				response.Message = "Cart Item could not be added.";
            }
			return response;
		}

        /// <summary>
        /// Gets the ID of the Authorized User on the Client
        private int? GetUserID() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
	}
}
