using BlazorEcommerce.Shared;

namespace BlazorEcommerce.Server.Services.ProductService
{
	public interface IProductService
	{
		public Task<ServiceResponse<List<Product>>> GetProductsAsync();
	}
}
