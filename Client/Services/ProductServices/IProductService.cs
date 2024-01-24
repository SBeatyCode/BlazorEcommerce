using BlazorEcommerce.Shared;
using BlazorEcommerce.Shared.DTOs;

namespace BlazorEcommerce.Client.Services.ProductServices
{
	public interface IProductService
	{
		event Action ProductsChanged;
		string Message {  get; set; }
		List<Product> Products { get; set; }
		int CurrentPage { get; set; }
		int PageCount { get; set; }
		string CurrentSearchText { get; set; }

		Task GetProductsAsync(string? categoryUrl = null);
		Task<ServiceResponse<Product>> GetProductbyIdAsync(int productId);
		Task SearchProducts(string searchText, int page);
		Task<List<string>> GetProductSearchSuggestions(string searchText);
	}
}
