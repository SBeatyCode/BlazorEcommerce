using BlazorEcommerce.Shared.DTOs;
using BlazorEcommerce.Shared;
using BlazorEcommerce.Server.Data;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Security.Claims;
using System.Linq;
using BlazorEcommerce.Server.Services.AuthService;

namespace BlazorEcommerce.Server.Services.CartService
{
	public class CartService : ICartService
	{
		private readonly DataContext _dataContext;
		private readonly IAuthService _authService;

		public CartService(DataContext dataContext, IAuthService authService) 
		{
			_dataContext = dataContext;
			_authService = authService;
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
			int userId = _authService.GetUserId();

			if (cartItems != null && cartItems.Count > 0 && userId > 0)
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
			int userId = _authService.GetUserId();

			if(userId > 0)
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
			var cart = await _dataContext.CartItems.Where(ci => ci.UserId == _authService.GetUserId()).ToListAsync();
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
			cartItem.UserId = _authService.GetUserId();

			if(cartItem.UserId > 0)
            {
                var existingItem = await GetExistingCartItem(cartItem);

                if (existingItem != null)
                    existingItem.Quantity += cartItem.Quantity;
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


        public async Task<ServiceResponse<bool>> UpdateItemQuantity(CartItem cartItem)
		{
			var response = new ServiceResponse<bool>();
			var existingItem = await GetExistingCartItem(cartItem);

			if(existingItem != null)
			{
				existingItem.Quantity = cartItem.Quantity;
				await _dataContext.SaveChangesAsync();

				response.Data = true;
				response.Success = true;
				response.Message = "Updated Item Quantity";
			}
			else
			{
				response.Data= false;
				response.Success = false;
				response.Message = $"The item {cartItem} is not in the DataBase for user #{_authService.GetUserId()}";
			}
			return response;
		}

		public async Task<ServiceResponse<bool>> DeleteItem(CartItem cartItem)
		{
			var response = new ServiceResponse<bool>();
			CartItem? existingItem = await GetExistingCartItem(cartItem);

			if(existingItem != null) 
			{
				_dataContext.Remove(existingItem);
				await _dataContext.SaveChangesAsync();

				response.Data = true;
				response.Success = true;
				response.Message = "Item was removed";
			}
			else
			{
				response.Data = false;
				response.Success = false;
				response.Message = "Could not remove Item";
			}
			return response;
        }

		/// <summary>
		/// Empties all of the current user's items from the Database
		/// </summary>
		public async Task<ServiceResponse<bool>> EmptyCart()
		{
            var response = new ServiceResponse<bool>();
			int userId = _authService.GetUserId();


            if (userId > 0)
			{
                _dataContext.CartItems.RemoveRange(_dataContext.CartItems
					.Where(ci => ci.UserId == userId));

                await _dataContext.SaveChangesAsync();

                return new ServiceResponse<bool>
                {
                    Data = true,
                    Success = false,
                    Message = "All Items Deleted"
                };
            }
			else
				return new ServiceResponse<bool> 
				{
					Data = false,
					Success = false,
					Message= "Could not get the UserID to Clear the Cart"
				};
        }

        /// <summary>
        /// Checks to see if a particular CartItem exists already for the current User
        /// in their Cart on the DataBase. If so returns it, otherwise returns NULL
        /// </summary>
        private async Task<CartItem?> GetExistingCartItem(CartItem cartItem)
        {
			if (cartItem.UserId <= 0)
				cartItem.UserId = _authService.GetUserId();

            return await _dataContext.CartItems
                                .FirstOrDefaultAsync(ci => ci.ProductId == cartItem.ProductId
                                && ci.ProductTypeId == cartItem.ProductTypeId
                                && ci.UserId == cartItem.UserId);
        }
	}
}
