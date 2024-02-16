using BlazorEcommerce.Shared;
using System.Net.Http.Json;

namespace BlazorEcommerce.Client.Services.AddressService
{
	public class AddressService : IAddressService
	{
		private readonly HttpClient _httpClient;

		public AddressService(HttpClient httpClient) 
		{
			_httpClient = httpClient;
		}

		public async Task<ServiceResponse<Address>> GetAddress(int addressId = 0)
		{
			return await _httpClient.GetFromJsonAsync<ServiceResponse<Address>>("api/address/get/addressId");
		}

		public async Task<ServiceResponse<Address>> AddAddress(Address newAddress)
		{
			return await _httpClient.PostAsJsonAsync<ServiceResponse<Address>>("api/address/newAddress", null)
				.Result.Content.ReadFromJsonAsync<ServiceResponse<Address>>();
		}
	}
}
