using BlazorEcommerce.Shared;
using BlazorEcommerce.Shared.DTOs;

namespace BlazorEcommerce.Server.Services.ProductService
{
	public interface IProductService
	{
		public Task<ServiceResponse<List<Product>>> GetProductsAsync();
		public Task<ServiceResponse<Product>> GetProductByIdAsync(int productId);
        public Task<ServiceResponse<List<Product>>> GetProductsByCategoryAsync(string category);
		public Task<ServiceResponse<ProductSearchResult>> SearchProducts(string searchText, int pageNumber);
		public Task<ServiceResponse<List<string>>> ProductSearchSuggestions(string searchText);
		public Task<ServiceResponse<List<Product>>> GetFeaturedProducts();
	}
}
