using BlazorEcommerce.Shared;

namespace BlazorEcommerce.Client.Services.ProductServices
{
	public interface IProductService
	{
		event Action ProductsChanged;
		List<Product> Products { get; set; }
		Task GetProductsAsync(string? categoryUrl = null);
		Task<ServiceResponse<Product>> GetProductbyIdAsync(int productId);
	}
}
