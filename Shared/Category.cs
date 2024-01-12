using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorEcommerce.Shared
{
	/// <summary>
	/// The Category into which a Product can be sorted (example, Book, Movie, etc). This
	/// is used for presenting and organizing products for the Ecommerce Site itself.
	/// The class 'ProductType' represents the actual varients that the ProductVarient
	///  can have
	/// </summary>
	public class Category
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Url { get; set; } = string.Empty;
	}
}
