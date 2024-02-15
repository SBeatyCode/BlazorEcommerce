using BlazorEcommerce.Client.Services.AuthService;
using BlazorEcommerce.Shared;
using BlazorEcommerce.Shared.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace BlazorEcommerce.Client.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly NavigationManager _navigationManager;

        public OrderService(HttpClient httpClient, AuthenticationStateProvider authenticationState, NavigationManager navigationManager)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationState;
            _navigationManager = navigationManager;
        }

        public async Task<string> PlaceOrder()
        {
            if(await IsUserAuthenticated())
            {
                var result = await _httpClient.PostAsync("api/payment/checkout", null);
                return await result.Content.ReadAsStringAsync();
            }
            else
            {
                return "login";
            }
        }

        public async Task<List<OrderResponse>> GetOrders()
        {
            if(await IsUserAuthenticated()) 
            {
                var result = await _httpClient.GetFromJsonAsync<ServiceResponse<List<OrderResponse>>>("api/order/get-orders");

                if(result != null && result.Data != null && result.Success) 
                    return result.Data;
                else
                    return new List<OrderResponse>();
            }
            else
                return new List<OrderResponse>();
        }

        public async Task<OrderDetailsResponse> GetOrderDetails(int orderId)
        {
            if(await IsUserAuthenticated()) 
            {
				var response = await _httpClient.GetFromJsonAsync<ServiceResponse<OrderDetailsResponse>>($"api/order/order-details/{orderId}");

                if (response != null && response.Data != null && response.Success)
                    return response.Data;
                else
                    return new OrderDetailsResponse();
			}
            else 
                return new OrderDetailsResponse();
        }

		/// <summary>
		/// Checks if the current User is Authenticated
		/// </summary>
		private async Task<bool> IsUserAuthenticated() => (await _authenticationStateProvider.GetAuthenticationStateAsync())
            .User.Identity.IsAuthenticated;
    }
}
