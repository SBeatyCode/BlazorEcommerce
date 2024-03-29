﻿using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Server.Services.ProductService;
using BlazorEcommerce.Shared;
using BlazorEcommerce.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorEcommerce.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly IProductService _productService;

		public ProductController(IProductService productService) 
		{
			_productService = productService;
		}

		[HttpGet]
		public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProductsAsync() 
		{
			var response = await _productService.GetProductsAsync();

			if(response != null && response.Data != null) 
				return Ok(response);
			else
				return NotFound(response);
		}

		[HttpGet("{productId}")]
		public async Task<ActionResult<ServiceResponse<Product>>> GetProductAsync(int productId)
		{
			var response = await _productService.GetProductByIdAsync(productId);

			if(response != null && response.Data != null)
				return Ok(response);
			else
				return NotFound(response);
		}

		[HttpGet("category/{categoryUrl}")]
		public async Task<ActionResult<ServiceResponse<List<Category>>>> GetProductsByCategoryAsync(string categoryUrl)
		{
			var response = await _productService.GetProductsByCategoryAsync(categoryUrl);

            if (response != null && response.Data != null)
                return Ok(response);
            else
                return NotFound(response);
        }

		[HttpGet("search/{searchText}/{page}")]
		public async Task<ActionResult<ServiceResponse<ProductSearchResult>>> SearchProducts(string searchText, int page = 1)
		{
			var response = await _productService.SearchProducts(searchText, page);

			if (response != null && response.Data != null)
				return Ok(response);
			else
				return NotFound(response);
		}

		[HttpGet("search_suggestions/{searchText}")]
		public async Task<ActionResult<ServiceResponse<List<string>>>> GetProductSearchSuggestions(string searchText)
		{
			var response = await _productService.ProductSearchSuggestions(searchText);

			if (response != null && response.Data != null)
				return Ok(response);
			else
				return NotFound(response);
		}

		[HttpGet("featured")]
		public async Task<ActionResult<ServiceResponse<List<Product>>>> GetFeaturedProducts()
		{
			var response = await _productService.GetFeaturedProducts();

			if (response != null && response.Data != null)
				return Ok(response);
			else
				return NotFound(response);
		}

		[HttpGet("admin-get"), Authorize(Roles="Admin")]
		public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProductsAdmin()
		{
			var response = await _productService.GetProductsAdmin();

			if (response != null && response.Data != null)
				return Ok(response);
			else
				return NotFound(response);
		}

		[HttpPost("create"), Authorize(Roles="Admin")]
		public async Task<ActionResult<ServiceResponse<Product>>> CreateProduct(Product newProduct)
		{
			var response = await _productService.CreateProduct(newProduct);

			if (response != null && response.Data != null)
				return Ok(response);
			else
				return BadRequest(response);
		}

		[HttpPut("update"), Authorize(Roles = "Admin")]
		public async Task<ActionResult<ServiceResponse<Product>>> UpdateProduct(Product updateProduct)
		{
			var response = await _productService.UpdateProduct(updateProduct);

			if (response != null && response.Data != null)
				return Ok(response);
			else
				return NotFound(response);
		}

		[HttpDelete("delete/{productId}"), Authorize(Roles = "Admin")]
		public async Task<ActionResult<ServiceResponse<Product>>> DeleteProduct(int productId)
		{
			var response = await _productService.DeleteProduct(productId);

			if (response != null && response.Data != null)
				return Ok(response);
			else
				return NotFound(response);
		}
	}
}
