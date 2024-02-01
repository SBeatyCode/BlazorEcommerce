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

		public async Task<ServiceResponse<string>> LoginUser(UserLogin user)
		{
			var result = await _httpClient.PostAsJsonAsync("api/auth/login", user);
			return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
		}

		public async Task<ServiceResponse<bool>> ChangePassword(ChangePassword changePassword)
		{
			var result = await _httpClient.PostAsJsonAsync("api/auth/change-password", changePassword.Password);
			return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
		}
	}
}
