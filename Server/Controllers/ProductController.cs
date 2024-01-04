using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorEcommerce.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly DataContext _dataContext;

		public ProductController(DataContext dataContext) 
		{
			_dataContext = dataContext;
		}

		[HttpGet]
		public async Task<ActionResult<List<Product>>> GetProducts() 
		{
			var products = await _dataContext.Products.ToListAsync();
			return Ok(products);
		}
	}
}
