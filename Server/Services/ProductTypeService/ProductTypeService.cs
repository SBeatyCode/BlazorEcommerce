using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Shared;
using Microsoft.EntityFrameworkCore;

namespace BlazorEcommerce.Server.Services.ProductTypeService
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly DataContext _dataContext;

        public ProductTypeService(DataContext dataContext) 
        {
            _dataContext = dataContext;
        }

        public async Task<ServiceResponse<List<ProductType>>> GetProductTypes()
        {
            var productTypes = await _dataContext.ProductTypes.ToListAsync();

            if(productTypes != null && productTypes.Count >= 0)
            {
                return new ServiceResponse<List<ProductType>>
                {
                    Data = productTypes,
                    Success = true,
                    Message = "Success"
                };
            }
            else
            {
                return new ServiceResponse<List<ProductType>>
                {
                    Data = null,
                    Success = false,
                    Message = "There was a problem getting the ProductTypes, or there are none"
                };
            }
        }

		public async Task<ServiceResponse<ProductType>> AddProductType(ProductType newProductType)
        {
            if(newProductType != null)
            {
                newProductType.Editing = false;
                newProductType.IsNew = false;

				_dataContext.ProductTypes.Add(newProductType);
				await _dataContext.SaveChangesAsync();

				return new ServiceResponse<ProductType>
				{
					Data = newProductType,
					Success = true,
					Message = "Success"
				};
			}
            else
            {
				return new ServiceResponse<ProductType>
				{
					Data = null,
					Success = false,
					Message = "The ProductType was null"
				};
			}
        }
		public async Task<ServiceResponse<ProductType>> UpdateProductType(ProductType updateProductType)
        {
			ProductType productType = await _dataContext.ProductTypes.FirstOrDefaultAsync(pt => pt.Id == updateProductType.Id);
			if (productType != null)
            {
                productType.Name = updateProductType.Name;

                await _dataContext.SaveChangesAsync();

				return new ServiceResponse<ProductType>
				{
					Data = productType,
					Success = true,
					Message = "The ProductType was Updated"
				};
			}
			else
			{
				return new ServiceResponse<ProductType>
				{
					Data = null,
					Success = false,
					Message = "The Updated ProductType was null"
				};
			}
		}
	}
}
