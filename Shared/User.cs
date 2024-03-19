using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorEcommerce.Shared
{
	/// <summary>
	/// Model that represents a User
	/// </summary>
	public class User
	{
		public int Id { get; set; }
		public string UserName { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public byte[] PasswordHash { get; set; } = new byte[0];
		public byte[] PasswordSalt { get; set; } = new byte[0];
		public DateTime DateCreated { get; set; } = DateTime.Now;
		public List<Address> Addresses { get; set; } = new List<Address>();
		public string Role { get; set; } = "User";
	}
}
