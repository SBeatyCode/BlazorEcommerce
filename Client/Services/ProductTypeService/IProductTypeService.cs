using BlazorEcommerce.Shared;

namespace BlazorEcommerce.Client.Services.ProductTypeService
{
    public interface IProductTypeService
    {
        event Action OnChange;

        public List<ProductType> ProductTypes { get; set; }

        Task GetProductTypes();
        Task AddProductType(ProductType newProductType);
		Task UpdateProductType(ProductType updateProductType);
		ProductType CreateNewProductType();
	}
}
