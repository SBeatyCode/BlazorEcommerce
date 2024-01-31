using BlazorEcommerce.Shared;

namespace BlazorEcommerce.Client.Services.AuthService
{
	public interface IAuthService
	{
		Task<ServiceResponse<int>> RegisterUser(UserRegister register);
		Task<ServiceResponse<string>> LoginUser(UserLogin user);
	}
}
