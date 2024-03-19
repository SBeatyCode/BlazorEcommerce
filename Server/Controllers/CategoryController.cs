using Azure;
using BlazorEcommerce.Server.Services.CategoryService;
using BlazorEcommerce.Shared;
using Microsoft.AspNetCore.Authorization;
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

		[HttpGet("admin"), Authorize(Roles = "Admin")]
		public async Task<ActionResult<ServiceResponse<List<Category>>>> GetAdminCategoriesAsync()
		{
			var response = await _categoryService.GetAdminCategories();
			if (response == null)
				return NotFound(response);
			else
				return Ok(response);
		}

		[HttpPost("add"), Authorize(Roles = "Admin")]
		public async Task<ActionResult<ServiceResponse<Category>>> AddCategory(Category newCategory)
		{
			var response = await _categoryService.AddCategory(newCategory);
			if (response == null)
				return BadRequest(response);
			else
				return Ok(response);
		}

		[HttpPut("update"), Authorize(Roles = "Admin")]
		public async Task<ActionResult<ServiceResponse<Category>>> UpdateCategory(Category updatedCategory)
		{
			var response = await _categoryService.UpdateCategory(updatedCategory);
			if (response == null)
				return BadRequest(response);
			else
				return Ok(response);
		}

		[HttpDelete("delete/{catId}"), Authorize(Roles = "Admin")]
		public async Task<ActionResult<ServiceResponse<Category>>> DeleteCategory(int catId)
		{
			var response = await _categoryService.DeleteCategory(catId);
			if (response == null)
				return BadRequest(response);
			else
				return Ok(response);
		}
	}
}
