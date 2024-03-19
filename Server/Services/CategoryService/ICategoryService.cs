using BlazorEcommerce.Shared;

namespace BlazorEcommerce.Server.Services.CategoryService
{
	public interface ICategoryService
	{
		Task<ServiceResponse<List<Category>>> GetCategoriesAsync();
		Task<ServiceResponse<List<Category>>> GetAdminCategories();
		Task<ServiceResponse<Category>> AddCategory(Category newCategory);
		Task<ServiceResponse<Category>> UpdateCategory(Category updatedCategory);
		Task<ServiceResponse<Category>> DeleteCategory(int catId);
	}
}
