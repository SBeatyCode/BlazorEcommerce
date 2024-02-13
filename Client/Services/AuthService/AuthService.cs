using BlazorEcommerce.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Security.Claims;

namespace BlazorEcommerce.Client.Services.AuthService
{
	public class AuthService : IAuthService
	{
		private readonly HttpClient _httpClient;
		private readonly AuthenticationStateProvider _authenticationStateProvider;

		public AuthService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider) 
		{
			_httpClient = httpClient;
			_authenticationStateProvider = authenticationStateProvider;
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

		public async Task<bool> IsUserAuthenticated()
		{
			return (await _authenticationStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
		}

		public async Task<int> GetUserId()
		{
			return int.Parse(_authenticationStateProvider
						.GetAuthenticationStateAsync().Result.User
						.FindFirst(ClaimTypes.NameIdentifier).Value);
		}
	}
}
