using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Shared;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;

namespace BlazorEcommerce.Server.Services.CategoryService
{
	public class CategoryService: ICategoryService
	{
		private readonly DataContext _context;

		public CategoryService(DataContext dataContext) 
		{
			_context = dataContext;
		}

		public async Task<ServiceResponse<List<Category>>> GetCategoriesAsync()
		{
			var categoriesList = await _context.Categories
				.Where(c => !c.Deleted && c.Visible)
				.ToListAsync();

			var response = new ServiceResponse<List<Category>>();

			if(categoriesList != null)
			{
				response.Data = categoriesList;
				response.Success = true;
				response.Message = "Operation was completed";
			}
			else
			{
				response.Success = false;
				response.Message = "Could not get Categories";
			}

			return response;
		}

		/// <summary>
		/// Gets Categories, but includes Categories that are not Vsible. 
		/// Only for Admins. Should be used with proper auth checks when called
		/// </summary>
		/// <returns></returns>
		public async Task<ServiceResponse<List<Category>>> GetAdminCategories()
		{
			var categoriesList = await _context.Categories
				.Where(c => !c.Deleted)
				.ToListAsync();

			var response = new ServiceResponse<List<Category>>();

			if (categoriesList != null)
			{
				response.Data = categoriesList;
				response.Success = true;
				response.Message = "Operation was completed";
			}
			else
			{
				response.Success = false;
				response.Message = "Could not get Categories";
			}

			return response;
		}

		public async Task<ServiceResponse<Category>> AddCategory(Category newCategory)
		{
			newCategory.Editing = false;
			newCategory.IsNew = false;

			_context.Categories.Add(newCategory);
			await _context.SaveChangesAsync();

			return new ServiceResponse<Category> 
			{ 
				Data = newCategory,
				Success = true,
				Message = "New Category Added"
			};
		}

		public async Task<ServiceResponse<Category>> UpdateCategory(Category updatedCategory)
		{
			Category category = await GetCategoryById(updatedCategory.Id);

			if(category != null) 
			{
				category.Name = updatedCategory.Name;
				category.Url = updatedCategory.Url;
				category.Visible = updatedCategory.Visible;

				await _context.SaveChangesAsync();

				return new ServiceResponse<Category>
				{
					Data = category,
					Success = true,
					Message = "This Category has been updated"
				};
			}
			else
			{
				return new ServiceResponse<Category>
				{
					Data = null,
					Success = false,
					Message = $"A category with the id of {updatedCategory.Id} could not be found"
				};
			}
		}

		public async Task<ServiceResponse<Category>> DeleteCategory(int catId)
		{
			Category category = await GetCategoryById(catId);

			if (category != null)
			{
				//_context.Categories.Remove(category);
				category.Deleted = true;
				await _context.SaveChangesAsync();

				return new ServiceResponse<Category>
				{
					Data = category,
					Success = true,
					Message = "This Category has been deleted"
				};
			}
			else
			{
				return new ServiceResponse<Category>
				{
					Data = null,
					Success = false,
					Message = $"A category with the id of {catId} could not be found"
				};
			}
		}

		private async Task<Category?> GetCategoryById(int catId)
		{
			return await _context.Categories.FirstOrDefaultAsync(c => c.Id == catId);
		}
	}
}
