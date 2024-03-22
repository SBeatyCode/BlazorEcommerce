using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlazorEcommerce.Shared
{
	/// <summary>
	/// A class that represents a varient on a product. For example, for a book, varients
	/// could be Paperback, Ebook, Audiobook, etc. This object represents the fx actual
	/// Audiobook object itself, not the 'category' of Audiobook
	/// </summary>
	public class ProductVarient
	{
		[JsonIgnore]
		public Product? Product { get; set; }
		public int ProductId { get; set; }
		public ProductType? ProductType { get; set; }
		public int ProductTypeId { get; set; }

		[Column(TypeName ="decimal(18,2)")]
		public decimal Price { get; set; }
		[Column(TypeName = "decimal(18,2)")]
		public decimal OriginalPrice { get; set; }
		public bool Visible { get; set; } = true;
		public bool Deleted { get; set; } = false;
		[NotMapped]
		public bool Editing { get; set; } = false;
		[NotMapped]
		public bool IsNew { get; set; } = false;
	}
}
