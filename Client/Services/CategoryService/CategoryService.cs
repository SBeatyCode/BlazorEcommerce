using BlazorEcommerce.Shared;
using System.Net.Http.Json;

namespace BlazorEcommerce.Client.Services.CategoryService
{
	public class CategoryService : ICategoryService
	{
		private readonly HttpClient client;
		public List<Category> Categories { get; set; } = new List<Category>();

		public CategoryService(HttpClient Httpclient)
		{
			client = Httpclient;
		}

		public async Task GetCategories()
		{
			var response = await client.GetFromJsonAsync<ServiceResponse<List<Category>>>("api/category");

			if(response != null && response.Data != null && response.Success == true)
			{
				Categories = response.Data;
			}
		}
	}
}
