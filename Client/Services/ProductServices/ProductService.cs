using BlazorEcommerce.Shared;
using System.Net.Http.Json;

namespace BlazorEcommerce.Client.Services.ProductServices
{
	/// <summary>
	/// Service class for the Client that fetches products from the database and holds a list
	/// of all products for other componenets to use
	/// </summary>
	public class ProductService : IProductService
	{
		private readonly HttpClient _httpClient;

		public ProductService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public List<Product> Products { get; set; } = new List<Product>();

		public async Task<ServiceResponse<Product>> GetProductbyIdAsync(int productId)
		{
			var result = await _httpClient.GetFromJsonAsync<ServiceResponse<Product>>($"api/product/{productId}");
			return result;
		}

		public async Task GetProductsAsync()
		{
			var result = await _httpClient.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/product");

			if(result != null && result.Data != null)
			{
				Products = result.Data;
			}
		}
	}
}
