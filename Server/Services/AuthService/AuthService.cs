using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BlazorEcommerce.Server.Services.AuthService
{
	public class AuthService : IAuthService
	{
		private readonly DataContext _dataContext;
		private readonly IConfiguration _configuration;

		public AuthService(DataContext dataContext, IConfiguration configuration) 
		{
			_dataContext = dataContext;
			_configuration = configuration;
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

		public async Task<ServiceResponse<string>> Login(string username, string password)
		{
			var response = new ServiceResponse<string>();
			var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.UserName.Equals(username));
			
			if (user == null)
			{
				response.Success = false;
				response.Message = $"Could not find user with the name {username}. Remember, usernames are case sensitive.";
			}
			else if(!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
			{
				response.Success = false;
				response.Message = "The password was not correct. Passwords are case-sensitive.";
			}
			else
			{
				response.Data = CreateToken(user);
				response.Success = true;
				response.Message = "User has been Logged In!";
			}

			return response;
		}

		private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
		{
			var hmac = new HMACSHA512();

			passwordSalt = hmac.Key;
			passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
		}

		private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
		{
			var hmac = new HMACSHA512(passwordSalt);
			var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

			return computedHash.SequenceEqual(passwordHash);
		}

		private string CreateToken(User user)
		{
			List<Claim> claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.Name, user.UserName),
				new Claim(ClaimTypes.Email, user.Email)
			};

			var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

			var token = new JwtSecurityToken(
				claims: claims,
				expires: DateTime.Now.AddDays(1),
				signingCredentials: creds
			);

			var jwt = new JwtSecurityTokenHandler().WriteToken(token);

			return jwt;
		}
	}
}
