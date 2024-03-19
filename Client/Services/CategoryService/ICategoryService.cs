using BlazorEcommerce.Shared;

namespace BlazorEcommerce.Client.Services.CategoryService
{
	public interface ICategoryService
	{
		event Action OnChange;

		List<Category> Categories { get; set; }
		List<Category> AdminCategories { get; set; }

		Task GetCategories();
		Task GetAdminCategories();
		Task AddCategories(Category category);
		Task UpdateCategories(Category category);
		Task DeleteCategories(int categoryId);
		Category CreateNewCategory();
	}
}
