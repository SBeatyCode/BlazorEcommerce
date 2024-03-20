using BlazorEcommerce.Shared;
using System.Net.Http.Json;

namespace BlazorEcommerce.Client.Services.CategoryService
{
	public class CategoryService : ICategoryService
	{
		public event Action OnChange;
		public List<Category> Categories { get; set; } = new List<Category>();
		public List<Category> AdminCategories { get; set; } = new List<Category>();

		private readonly HttpClient _httpClient;

		public CategoryService(HttpClient Httpclient)
		{
			_httpClient = Httpclient;
		}

		public async Task GetCategories()
		{
			var response = await _httpClient.GetFromJsonAsync<ServiceResponse<List<Category>>>("api/category");

			if(response != null && response.Data != null && response.Success == true)
			{
				Categories = response.Data;
			}
		}

		public async Task GetAdminCategories()
		{
			var response = await _httpClient.GetFromJsonAsync<ServiceResponse<List<Category>>>("api/category/admin");

			if (response != null && response.Data != null && response.Success == true)
			{
				AdminCategories = response.Data;
			}
		}

		public async Task AddCategory(Category category)
		{
			var response = await _httpClient.PostAsJsonAsync("api/category/add", category);

			await CategoriesChanged();
		}

		public async Task UpdateCategory(Category category)
		{
			var response = await _httpClient.PutAsJsonAsync("api/category/update", category);

			await CategoriesChanged();
		}

		public async Task DeleteCategory(int categoryId)
		{
			var response = await _httpClient.DeleteAsync($"api/category/delete/{categoryId}");

			await CategoriesChanged();
		}

		public Category CreateNewCategory()
		{
			Category newCategory = new Category
			{
				IsNew = true,
				Editing = true
			};

			AdminCategories.Add(newCategory);
			OnChange.Invoke();

			return newCategory;
		}

		private async Task CategoriesChanged()
		{
			await GetCategories();
			await GetAdminCategories();

			OnChange.Invoke();
		}
	}
}
