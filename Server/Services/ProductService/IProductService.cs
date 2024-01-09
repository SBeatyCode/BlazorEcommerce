using BlazorEcommerce.Shared;

namespace BlazorEcommerce.Server.Services.ProductService
{
	public interface IProductService
	{
		public Task<ServiceResponse<List<Product>>> GetProductsAsync();
		public Task<ServiceResponse<Product>> GetProductByIdAsync(int productId);
	}
}
