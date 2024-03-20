using BlazorEcommerce.Shared;

namespace BlazorEcommerce.Server.Services.ProductTypeService
{
    public interface IProductTypeService
    {
        Task<ServiceResponse<List<ProductType>>> GetProductTypes();
        Task<ServiceResponse<ProductType>> AddProductType(ProductType newProductType);
		Task<ServiceResponse<ProductType>> UpdateProductType(ProductType updateProductType);
	}
}
