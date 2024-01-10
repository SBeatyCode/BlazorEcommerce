using BlazorEcommerce.Server.Services.CategoryService;
using BlazorEcommerce.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoryController : ControllerBase
	{
		private readonly ICategoryService _categoryService;
		public CategoryController(ICategoryService categoryService) 
		{
			_categoryService = categoryService;
		}

		[HttpGet]
		public async Task<ActionResult<ServiceResponse<List<Category>>>> GetCategoriesAsync()
		{
			var response = await _categoryService.GetCategoriesAsync();
			if (response == null)
				return NotFound(response);
			else 
				return Ok(response);
		}
	}
}
