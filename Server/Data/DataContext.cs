using BlazorEcommerce.Shared;
using Microsoft.EntityFrameworkCore;

namespace BlazorEcommerce.Server.Data
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{ 
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Product>().HasData(
				new Product
				{
					Id = 1,
					Name = "Gideon the Ninth",
					Description = "Gideon the Ninth is a 2019 science fantasy novel by the New Zealand writer Tamsyn Muir. It is Muir's debut novel and the first in her Locked Tomb series",
					ImageUrl = "https://upload.wikimedia.org/wikipedia/en/thumb/6/66/Gideon_the_Ninth_-_US_hardback_print_cover.jpg/220px-Gideon_the_Ninth_-_US_hardback_print_cover.jpg",
					Price = 12.99m
				},
				new Product
				{
					Id = 2,
					Name = "Harrow the Ninth",
					Description = "Harrow the Ninth is a 2020 science fantasy novel by the New Zealand writer Tamsyn Muir. It is the second in Muir's Locked Tomb series",
					ImageUrl = "https://upload.wikimedia.org/wikipedia/en/thumb/0/02/Harrow_the_Ninth.jpg/220px-Harrow_the_Ninth.jpg",
					Price = 12.99m
				},
				new Product
				{
					Id = 3,
					Name = "Nona the Ninth",
					Description = "Nona the Ninth is a 2022 science fantasy novel by the New Zealand writer Tamsyn Muir. It is the third book in the Locked Tomb series",
					ImageUrl = "https://upload.wikimedia.org/wikipedia/en/thumb/8/81/Nona_the_Ninth.jpg/220px-Nona_the_Ninth.jpg",
					Price = 15.99m
				}
			);
		}

		public DbSet<Product> Products { get; set; }
	}
}
