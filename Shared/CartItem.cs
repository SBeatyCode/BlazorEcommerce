using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorEcommerce.Shared
{
	/// <summary>
	/// Represents an 'Item' to be placed in the Cart and which Product and 
	/// Type ID it matches with/represents
	/// </summary>
	public class CartItem
	{
		public int ProductId { get; set; }
		public int ProductTypeId { get; set; }
		public int Quantity { get; set; } = 1;
	}
}
