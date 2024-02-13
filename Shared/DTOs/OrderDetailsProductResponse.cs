using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorEcommerce.Shared.DTOs
{
	public class OrderDetailsProductResponse
	{
		public int ProductId { get; set; }
		public string ProductName { get; set; } = string.Empty;
		public string ProductTypeName {  get; set; } = string.Empty;
		public string ImageUrl {  get; set; } = string.Empty;
		public int Quantity { get; set; }
		public decimal TotalPrice { get; set; }
	}
}
