using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorEcommerce.Shared
{
	/// <summary>
	/// A class that represents the 'types' that a Product can be. paperback, Audiobook,
	/// DVD, etc. This does NOT represent the actual Product object, just the classification
	/// of the Product objects
	/// </summary>
	public class ProductType
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
	}
}
