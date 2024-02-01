using BlazorEcommerce.Server.Services.AuthService;
using BlazorEcommerce.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlazorEcommerce.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;

		public AuthController(IAuthService authService) 
		{
			_authService = authService;
		}

		[HttpPost("register")]
		public async Task<ActionResult<ServiceResponse<int>>> RegisterUser(UserRegister register)
		{
			User user = new User
			{
				UserName = register.UserName,
				Email = register.Email
			};

			var response = await _authService.RegisterUser(user, register.Password);

			if (response == null || response.Success == false)
				return BadRequest(response);
			else
				return Ok(response);
		}

		[HttpPost("login")]
		public async Task<ActionResult<ServiceResponse<string>>> Login(UserLogin user)
		{
			var response = await _authService.Login(user.UserName, user.Password);

			if (response == null || response.Success == false)
				return BadRequest(response);
			else
				return Ok(response);
		}

		[HttpPost("change-password"), Authorize]
		public async Task<ActionResult<ServiceResponse<bool>>> ChangePassword([FromBody] string newPassword)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var response = await _authService.ChangePassword(int.Parse(userId), newPassword);

			if (response == null || response.Success == false)
				return BadRequest(response);
			else
				return Ok(response);
		}
	}
}
