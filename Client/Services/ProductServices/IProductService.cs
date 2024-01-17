using BlazorEcommerce.Shared;

namespace BlazorEcommerce.Client.Services.ProductServices
{
	public interface IProductService
	{
		event Action ProductsChanged;
		string Message {  get; set; }
		List<Product> Products { get; set; }
		Task GetProductsAsync(string? categoryUrl = null);
		Task<ServiceResponse<Product>> GetProductbyIdAsync(int productId);
		Task SearchProducts(string searchText);
		Task<List<string>> GetProductSearchSuggestions(string searchText);
	}
}
