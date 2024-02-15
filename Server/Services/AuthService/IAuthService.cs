using BlazorEcommerce.Shared;

namespace BlazorEcommerce.Server.Services.AuthService
{
	public interface IAuthService
	{
		Task<ServiceResponse<int>> RegisterUser(User user, string password);
		Task<bool> UserExists (string email);
		Task<bool> UserExists(int userId);
		Task<ServiceResponse<string>> Login(string username, string password);
		Task<ServiceResponse<bool>> ChangePassword(int userId, string newPassword);
		int GetUserId();
		string GetUserEmail();
		Task<User> GetUserByEmail(string userEmail);
	}
}
