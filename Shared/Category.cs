using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorEcommerce.Shared
{
	/// <summary>
	/// Rge Category into which a Product can be sorted (example, Book, Movie, etc)
	/// </summary>
	public class Category
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Url { get; set; } = string.Empty;
	}
}
