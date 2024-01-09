using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Shared;
using Microsoft.EntityFrameworkCore;

namespace BlazorEcommerce.Server.Services.ProductService
{
	public class ProductService : IProductService
	{
		private readonly DataContext _dataContext;

		public ProductService(DataContext dataContext) 
		{
			_dataContext = dataContext;
		}

		public async Task<ServiceResponse<Product>> GetProductByIdAsync(int productId)
		{
			var response = new ServiceResponse<Product>();
			var product = await _dataContext.Products.FindAsync(productId);

			if(product == null) 
			{
				response.Data = null;
				response.Success = false;
				response.Message = $"Could not find product with the id {productId}";
			}
            else
            {
				response.Data = product;
				response.Success = true;
				response.Message = "Operation was succesful!";
			}

			return response;
        }

		public async Task<ServiceResponse<List<Product>>> GetProductsAsync()
		{
			var response = new ServiceResponse<List<Product>>
			{
				Data = await _dataContext.Products.ToListAsync()
			};

			if(response.Data != null) 
			{
				response.Success = true;
				response.Message = "The operation was succesful!";
			}
			else
			{
				response.Success = false;
				response.Message = "There was a problem fetching the List of Products";
			}

			return response;
		}
	}
}
