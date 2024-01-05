using BlazorEcommerce.Shared;

namespace BlazorEcommerce.Client.Services.ProductServices
{
	public interface IProductService
	{
		List<Product> Products { get; set; }
		Task GetProductsAsync();
	}
}
