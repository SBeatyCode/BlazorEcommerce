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
		private readonly HttpContextAccessor _contextAccessor;

		public AuthService(DataContext dataContext, IConfiguration configuration, HttpContextAccessor httpContextAccessor) 
		{
			_dataContext = dataContext;
			_configuration = configuration;
			_contextAccessor = httpContextAccessor;
		}

		/// <summary>
		/// Gets the ID of the Authnticated User. If the ID can't be found, returns -1
		/// </summary>
		/// <returns></returns>
		public int GetUserId()
		{
			int id = int.Parse(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

			if (id > 0)
				return id;
			else return -1;
		}

		/// <summary>
		/// Gets the email of the authenticated user. If this fails, an empty string is returned
		/// </summary>
		/// <returns></returns>
		public string GetUserEmail()
		{
            string email = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

            if (!string.IsNullOrEmpty(email))
                return email;
            else return "";
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

		public async Task<bool> UserExists(int userId)
		{
			//if email exists in Database already
			if (await _dataContext.Users.AnyAsync(user => user.Id.Equals(userId)))
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

		public async Task<ServiceResponse<bool>> ChangePassword(int userId, string newPassword)
		{
			var response = new ServiceResponse<bool>();
			User? user = _dataContext.Users.FindAsync(userId).Result;

			if(user == null)
			{
				response.Data = false;
				response.Success = false;
				response.Message = $"Could not find user with the ID of {userId}";
			}
			else
			{
				CreatePasswordHash(newPassword, out byte[] newPasswordHash, out byte[] newPasswordSalt);

				user.PasswordHash = newPasswordHash;
				user.PasswordSalt = newPasswordSalt;

				await _dataContext.SaveChangesAsync();

				response.Data = true;
				response.Success = true;
				response.Message = $"The password has been updated!";
			}

			return response;
		}

		public async Task<User> GetUserByEmail(string userEmail)
		{
			var result = await _dataContext.Users.FirstOrDefaultAsync(user => user.Email.Equals(userEmail));

			return result != null ? result : new User();
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
				new Claim(ClaimTypes.Email, user.Email),
				new Claim(ClaimTypes.Role, user.Role)
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
