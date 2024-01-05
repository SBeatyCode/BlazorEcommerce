using BlazorEcommerce.Shared;
using System.Net.Http.Json;

namespace BlazorEcommerce.Client.Services.ProductServices
{
	public class ProductService : IProductService
	{
		private readonly HttpClient _httpClient;

		public ProductService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public List<Product> Products { get; set; } = new List<Product>();

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
