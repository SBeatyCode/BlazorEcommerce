using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorEcommerce.Shared.DTOs
{
	/// <summary>
	/// DTO Class to handle Product Search Requsts. Contains a list of Products,
	/// the number of pages from a querry, and the current page
	/// </summary>
	public class ProductSearchResult
	{
		public List<Product> Products { get; set; } = new List<Product>();
		public int PageCount { get; set; } //how many pages of products are there?
		public int CurrentPage { get; set; }
	}
}
