using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorEcommerce.Shared
{
	/// <summary>
	/// Model used to Register a new User
	/// </summary>
	public class UserRegister
	{
		[Required, StringLength(30, MinimumLength = 2)]
		public string UserName { get; set; } = string.Empty;
		[Required, EmailAddress]
		public string Email { get; set; } = string.Empty;
		[Required, StringLength(50, MinimumLength = 6)]
		public string Password { get; set; } = string.Empty;
		[Compare("Password", ErrorMessage = "The Entered Passwords Do Not Match")]
		public string ConfirmPassword { get; set; } = string.Empty;
	}
}