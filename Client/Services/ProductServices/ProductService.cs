using BlazorEcommerce.Shared;
using BlazorEcommerce.Shared.DTOs;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorEcommerce.Client.Services.ProductServices
{
	/// <summary>
	/// Service class for the Client that fetches products from the database and holds a list
	/// of all products for other componenets to use.
	/// </summary>
	public class ProductService : IProductService
	{
		public List<Product> Products { get; set; } = new List<Product>();
		public string Message { get; set; }
		public int CurrentPage { get; set; }
		public int PageCount { get; set; }
		public string CurrentSearchText { get; set; } = string.Empty;

		private readonly HttpClient _httpClient;

		public event Action ProductsChanged;

		public ProductService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<ServiceResponse<Product>> GetProductbyIdAsync(int productId)
		{
			var result = await _httpClient.GetFromJsonAsync<ServiceResponse<Product>>($"api/product/{productId}");
			
			if (result == null || result.Data == null)
			{
				return new ServiceResponse<Product>();
			}
			else
			{
				return result;
			}
		}

		/// <summary>
		/// Searches for Products. If CategoryUrl is provided, will search using that
		/// category, otherwise will return Featured products.
		/// </summary>
		/// <param name="categoryUrl">The Category to search for (optional)</param>
		/// <returns></returns>
		public async Task GetProductsAsync(string? categoryUrl = null)
		{
			var result = categoryUrl == null ?
				await _httpClient.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/product/featured") :
				await _httpClient.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/product/category/{categoryUrl}");


            if (result != null && result.Data != null)
			{
				Products = result.Data;
			}
			else 
			{
				Products = new List<Product>();
			}

			CurrentPage = 1;
			PageCount = 1;

			if (Products == null || Products.Count == 0)
				Message = "No Products Found";

			ProductsChanged.Invoke();
		}

		public async Task SearchProducts(string searchText, int page)
		{
			var result = await _httpClient.GetFromJsonAsync<ServiceResponse<ProductSearchResult>>($"api/product/search/{searchText}/{page}");

			if (result != null && result.Data != null)
			{
				Products = result.Data.Products;
				PageCount = result.Data.PageCount;
				CurrentPage = result.Data.CurrentPage;
			}
			else
			{
				Products = new List<Product>();
			}

			if (Products.Count == 0)
				Message = "No Products Found";

			CurrentSearchText = searchText;

			ProductsChanged.Invoke();
		}

		public async Task<List<string>> GetProductSearchSuggestions(string searchText)
		{
			var result = await _httpClient.GetFromJsonAsync<ServiceResponse<List<string>>>($"api/product/search_suggestions/{searchText}");

			if (result == null || result.Data == null)
			{
				return new List<string>();
			}
			else
			{
				return result.Data;
			}
		}
	}
}
