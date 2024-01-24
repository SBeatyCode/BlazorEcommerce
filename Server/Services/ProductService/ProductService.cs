using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Shared;
using BlazorEcommerce.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BlazorEcommerce.Server.Services.ProductService
{
	public class ProductService : IProductService
	{
		private readonly DataContext _context;

		public ProductService(DataContext dataContext) 
		{
			_context = dataContext;
		}

		public async Task<ServiceResponse<Product>> GetProductByIdAsync(int productId)
		{
			var response = new ServiceResponse<Product>();
			var product = await _context.Products
				.Include(p => p.Varients)
				.ThenInclude(v => v.ProductType)
				.FirstOrDefaultAsync(p => p.Id == productId);

			if(product == null) 
			{
				response.Data = null;
				response.Success = false;
				response.Message = $"Could not find product with the id {productId}";
			}
            else
            {
				response.Data = product;
				response.Success = true;
				response.Message = "Operation was succesful!";
			}

			return response;
        }

		public async Task<ServiceResponse<List<Product>>> GetProductsAsync()
		{
			var response = new ServiceResponse<List<Product>>
			{
				Data = await _context.Products.Include(p => p.Varients).ToListAsync()
			};

			if(response.Data != null) 
			{
				response.Success = true;
				response.Message = "The operation was succesful!";
			}
			else
			{
				response.Success = false;
				response.Message = "There was a problem fetching the List of Products";
			}

			return response;
		}

        public async Task<ServiceResponse<List<Product>>> GetProductsByCategoryAsync(string categoryUrl)
		{
			var response = new ServiceResponse<List<Product>>
			{
				Data = await _context.Products
					.Where(p => p.Category!.Url.ToLower().Equals(categoryUrl.ToLower()))
					.Include(p => p.Varients)
					.ToListAsync()
			};

			if( response.Data != null )
			{ 
				response.Success = true;
                response.Message = "The operation was succesful!";
            }
			else
			{
				response.Success= false;
                response.Message = "There was a problem fetching the List of Products";
            }

			return response;
		}

		public async Task<ServiceResponse<ProductSearchResult>> SearchProducts(string searchText, int page)
		{
			if(page < 1) page = 1;

			var productsPerPage = 3f;
			var pageCount = Math.Ceiling((await FindProductsBySearchText(searchText)).Count / productsPerPage);
			
			//Return list of products based on the current page number, and only get
			//enough products that will fill a page
			var products = await _context.Products
						.Where(p => p.Name.ToLower().Contains(searchText.ToLower())
						||
						p.Description.ToLower().Contains(searchText.ToLower()))
						.Include(p => p.Varients)
						.Skip((page - 1) * (int)productsPerPage)
						.Take((int)productsPerPage)
						.ToListAsync();

			var response = new ServiceResponse<ProductSearchResult>
			{
				Data = new ProductSearchResult
				{
					Products = products,
					PageCount = (int)pageCount,
					CurrentPage = page
				}
			};

			if (response.Data != null && products != null)
			{
				response.Success = true;
				response.Message = "The operation was succesful!";
			}
			else
			{
				response.Success = false;
				response.Message = "No Products Found";
			}

			return response;
		}

		public async Task<ServiceResponse<List<string>>> ProductSearchSuggestions(string searchText)
		{
			var response = new ServiceResponse<List<string>>();

			List<string> suggestions = new List<string>();
			List<Product>? products = await FindProductsBySearchText(searchText);

			if(products != null)
			{
				foreach (var product in products)
				{
					if (product.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase))
					{
						suggestions.Add(product.Name);
					}
					if(product.Description != null)
					{
						//remove punctutation and make product description into an array of strings
						var punctuation = product.Description.Where(char.IsPunctuation)
							.Distinct().ToArray();
						var words = product.Description.Split()
							.Select(s => s.Trim());

						foreach (var word in words) 
						{
							if(word.Contains(searchText, StringComparison.OrdinalIgnoreCase)
								&& !suggestions.Contains(word))
							{ 
								suggestions.Add(word); 
							}
						}
					}
				}

				response.Data = suggestions;
				response.Success = true;
				response.Message = "Opperation success";
			}
			else
			{
				response.Data = null;
				response.Success = false;
				response.Message = "Could not retrieve a list of search suggestions";
			}
			
			return response;
		}

		public async Task<ServiceResponse<List<Product>>> GetFeaturedProducts()
		{
			var response = new ServiceResponse<List<Product>>();

			var featured = await _context.Products
				.Where(p => p.Featured == true)
				.Include(p => p.Varients)
				.ToListAsync();

			if(featured == null)
			{
				response.Data = null;
				response.Success = false;
				response.Message = "Could not get Featured Products";
			}
			else
			{
				response.Data = featured;
				response.Success = true;
				response.Message = "Operation was a success";
			}

			return response;
		}

		/// <summary>
		/// Performs a querry to find a match from searchText in a product's name or
		/// description, returns the result as a List<Product> of any matches
		/// </summary>
		/// <param name="searchText">The text to search for in the query</param>
		/// <returns></returns>
		private async Task<List<Product>> FindProductsBySearchText(string searchText)
		{
			return await _context.Products
						.Where(p => p.Name.ToLower().Contains(searchText.ToLower())
						||
						p.Description.ToLower().Contains(searchText.ToLower()))
						.Include(p => p.Varients)
						.ToListAsync();
		}
	}
}
