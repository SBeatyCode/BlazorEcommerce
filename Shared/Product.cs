﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorEcommerce.Shared
{
    /// <summary>
    /// A class that represents a Product in the ECommerce Web App
    /// </summary>
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public bool Featured { get; set; } = false;
        public Category? Category { get; set; }
        public int CategoryId { get; set; }
        public List<ProductVarient> Varients { get; set; } = new List<ProductVarient>();
		public bool Visible { get; set; } = true;
        public bool Deleted { get; set; } = false;
        [NotMapped]
		public bool Editing { get; set; } = false;
		[NotMapped]
		public bool IsNew { get; set; } = false;
	}
}
