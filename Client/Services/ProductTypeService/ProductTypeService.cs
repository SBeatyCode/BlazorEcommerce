using BlazorEcommerce.Shared;
using System.Net.Http.Json;

namespace BlazorEcommerce.Client.Services.ProductTypeService
{
    public class ProductTypeService : IProductTypeService
    {
        public event Action OnChange;

        public List<ProductType> ProductTypes { get; set;} = new List<ProductType>();

        private readonly HttpClient _httpClient;

        public ProductTypeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task GetProductTypes()
        {
            var result = await _httpClient.GetFromJsonAsync<ServiceResponse<List<ProductType>>>("api/ProductType/getProductTypes");

            if(result != null && result.Data != null && result.Success) 
                ProductTypes = result.Data;
        }

		public async Task AddProductType(ProductType newProductType)
        {
            var response = await _httpClient.PostAsJsonAsync("api/ProductType/add", newProductType);
            
            await GetProductTypes();
			OnChange.Invoke();
		}

		public async Task UpdateProductType(ProductType updateProductType)
        {
            var response = await _httpClient.PutAsJsonAsync("api/ProductType/update", updateProductType);

			await GetProductTypes();
			OnChange.Invoke();
		}

		public ProductType CreateNewProductType()
        {
            ProductType newProductType = new ProductType()
            {
                IsNew = true,
                Editing = true
            };

            ProductTypes.Add(newProductType);
            OnChange.Invoke();

            return newProductType;
        }
	}
}
