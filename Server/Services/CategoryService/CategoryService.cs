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
			var categoriesList = await _context.Categories.ToListAsync();

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
	}
}
