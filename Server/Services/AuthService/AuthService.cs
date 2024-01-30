using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Shared;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace BlazorEcommerce.Server.Services.AuthService
{
	public class AuthService : IAuthService
	{
		private readonly DataContext _dataContext;

		public AuthService(DataContext dataContext) 
		{
			_dataContext = dataContext;
		}

		public async Task<ServiceResponse<int>> RegisterUser(User user, string password)
		{
			var response = new ServiceResponse<int>();

			if(await UserExists(user.Email))
			{
				response.Data = -1;
				response.Success = false;
				response.Message = $"A user with the email {user.Email} already exists";
			}
			else
			{
				CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

				user.PasswordHash = passwordHash;
				user.PasswordSalt = passwordSalt;

				_dataContext.Users.Add(user);
				await _dataContext.SaveChangesAsync();

				response.Data = user.Id;
				response.Success = true;
				response.Message = $"Your new user '{user.UserName}' has been created!";
			}

			return response;
		}

		/// <summary>
		/// Checks if an Email is already used for a user in the Database. Returns
		/// TRUE if that Email is in use already, FALSE if not
		/// </summary>
		public async Task<bool> UserExists(string email)
		{
			//if email exists in Database already
			if (await _dataContext.Users.AnyAsync(user => user.Email.ToLower().Equals(email.ToLower())))
				return true;
			else 
				return false;
		}

		private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
		{
			var hmac = new HMACSHA512();

			passwordSalt = hmac.Key;
			passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
		}
	}
}
