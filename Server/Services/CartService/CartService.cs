using BlazorEcommerce.Shared.DTOs;
using BlazorEcommerce.Shared;
using BlazorEcommerce.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazorEcommerce.Server.Services.CartService
{
	public class CartService : ICartService
	{
		private readonly DataContext _dataContext;

		public CartService(DataContext dataContext) 
		{
			_dataContext = dataContext;
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
	}
}
