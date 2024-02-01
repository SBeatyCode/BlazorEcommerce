using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorEcommerce.Shared
{
	/// <summary>
	/// A model for sending a request to change a User's password
	/// </summary>
	public class ChangePassword
	{
		[Required, StringLength(50, MinimumLength = 6)]
		public string Password { get; set; } = string.Empty;
		[Required]
		public string OldPassword { get; set; } = string.Empty;
		[Compare("Password", ErrorMessage = "Passwords do not match")]
		public string ConfirmNewPassword { get; set; } = string.Empty;
	}
}
