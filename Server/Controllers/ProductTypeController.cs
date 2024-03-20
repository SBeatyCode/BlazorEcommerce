using BlazorEcommerce.Server.Services.ProductTypeService;
using BlazorEcommerce.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles="Admin")]
    public class ProductTypeController : ControllerBase
    {
        private readonly IProductTypeService _productTypeService;

        public ProductTypeController(IProductTypeService productTypeService) 
        {
            _productTypeService = productTypeService;
        }

        [HttpGet("getProductTypes")]
        public async Task<ActionResult<ServiceResponse<List<ProductType>>>> GetProductTypes()
        {
            var response = await _productTypeService.GetProductTypes();

            if (response != null && response.Data != null)
                return Ok(response);
            else
                return NotFound(response);
        }

        [HttpPost("add")]
		public async Task<ActionResult<ServiceResponse<ProductType>>> AddProductType(ProductType newProductType)
        {
			var response = await _productTypeService.AddProductType(newProductType);

			if (response != null && response.Data != null)
				return Ok(response);
			else
				return NotFound(response);
		}


		[HttpPut("update")]
		public async Task<ActionResult<ServiceResponse<ProductType>>> UpdateProductType(ProductType updateProductType)
        {
            var response = await _productTypeService.UpdateProductType(updateProductType);

			if (response != null && response.Data != null)
				return Ok(response);
			else
				return NotFound(response);
		}

	}
}
