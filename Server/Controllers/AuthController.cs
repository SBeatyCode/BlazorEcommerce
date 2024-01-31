using BlazorEcommerce.Server.Services.AuthService;
using BlazorEcommerce.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
	}
}
