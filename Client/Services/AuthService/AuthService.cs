using BlazorEcommerce.Shared;
using System.Net.Http.Json;

namespace BlazorEcommerce.Client.Services.AuthService
{
	public class AuthService : IAuthService
	{
		private readonly HttpClient _httpClient;

		public AuthService(HttpClient httpClient) 
		{
			_httpClient = httpClient;
		}

		public async Task<ServiceResponse<int>> RegisterUser(UserRegister register)
		{
			var result = await _httpClient.PostAsJsonAsync("api/auth/register", register);

			return await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();
		}
	}
}
