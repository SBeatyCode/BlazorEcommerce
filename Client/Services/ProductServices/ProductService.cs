using BlazorEcommerce.Shared;
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

		private readonly HttpClient _httpClient;

		public event Action ProductsChanged;

		public ProductService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<ServiceResponse<Product>> GetProductbyIdAsync(int productId)
		{
			var result = await _httpClient.GetFromJsonAsync<ServiceResponse<Product>>($"api/product/{productId}");
			return result;
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

			ProductsChanged.Invoke();
		}

		public async Task SearchProducts(string searchText)
		{
			var result = await _httpClient.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/product/search/{searchText}");

			if (result != null && result.Data != null)
			{
				Products = result.Data;
			}

			if (Products.Count == 0)
				Message = "No Products Found";

			ProductsChanged.Invoke();
		}

		public async Task<List<string>> GetProductSearchSuggestions(string searchText)
		{
			var result = await _httpClient.GetFromJsonAsync<ServiceResponse<List<string>>>($"api/product/search_suggestions/{searchText}");
			return result.Data;
		}
	}
}
