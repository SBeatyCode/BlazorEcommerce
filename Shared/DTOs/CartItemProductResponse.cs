using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorEcommerce.Shared.DTOs
{
	public class CartItemProductResponse
	{
		public int ProductId { get; set; }
		public int ProductTypeId { get; set; }
		public string ProductName { get; set; } = string.Empty;
		public string ProductTypeName {  get; set; } = string.Empty;
		public string ProductImageUrl { get; set; } = string.Empty;
		//[Column(TypeName = "decimal(18,2)")]
		public decimal ProductPrice { get; set; }
        public int ProductQuantity { get; set; }
    }
}
