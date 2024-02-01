using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace BlazorEcommerce.Client
{
	public class CustomAuthStateProvider : AuthenticationStateProvider
	{
		ILocalStorageService _localStorageService;
		HttpClient _httpClient;

		public CustomAuthStateProvider(ILocalStorageService localStorageService, HttpClient httpClient)
		{
			_localStorageService = localStorageService;
			_httpClient = httpClient;
		}

		/// <summary>
		/// Gets the AuthToken from LocalStorage and pass the Claims, and create a new
		/// Claims identity. Notifies components of new Claims identity: Allows the 
		/// application to know if a user is authenticated or not
		/// </summary>
		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			var authToken = _localStorageService.GetItemAsStringAsync("authToken").ToString();
			var identity = new ClaimsIdentity();

			//while null, user is unauthorized. Null is set as default
			_httpClient.DefaultRequestHeaders.Authorization = null;

			if(!string.IsNullOrEmpty(authToken)) 
			{
				try
				{
					identity = new ClaimsIdentity(ParseClaimsFromJwt(authToken), "jwt");

					_httpClient.DefaultRequestHeaders.Authorization =
						new AuthenticationHeaderValue("Bearer", authToken.Replace("\"",""));
				}
				catch
				{
					await _localStorageService.RemoveItemAsync("authToken");
					identity = new ClaimsIdentity();
				}
			}

			var user = new ClaimsPrincipal(identity);
			var state = new AuthenticationState(user);

			NotifyAuthenticationStateChanged(Task.FromResult(state));

			return state;
		}

		private byte[] ParseBase64WithoutPadding(string base64)
		{
			switch(base64.Length % 4) 
			{
				case 2: base64 += "==";
					break;
				case 3: base64 += "=";
					break;
			}

			return Convert.FromBase64String(base64);
		}

		private IEnumerable<Claim>? ParseClaimsFromJwt(string authToken)
		{
			var payload = authToken.Split(".")[1];
			var jsonBytes = ParseBase64WithoutPadding(payload);
			var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
			
			var claims = keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));

			return claims;
		}
	}
}
