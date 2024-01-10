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
			modelBuilder.Entity<Category>().HasData(
				new Category
				{
					Id = 1,
					Name = "Books",
					Url = "books"
				},
				new Category
				{
					Id = 2,
					Name = "Movies",
					Url = "movies"
				},
				new Category
				{
					Id = 3,
					Name = "Video Games",
					Url = "video-games"
				}
			);

			modelBuilder.Entity<Product>().HasData(
				new Product
				{
					Id = 1,
					Name = "Gideon the Ninth",
					Description = "Gideon the Ninth is a 2019 science fantasy novel by the New Zealand writer Tamsyn Muir. It is Muir's debut novel and the first in her Locked Tomb series",
					ImageUrl = "https://upload.wikimedia.org/wikipedia/en/thumb/6/66/Gideon_the_Ninth_-_US_hardback_print_cover.jpg/220px-Gideon_the_Ninth_-_US_hardback_print_cover.jpg",
					Price = 12.99m,
					CategoryId = 1
				},
				new Product
				{
					Id = 2,
					Name = "Harrow the Ninth",
					Description = "Harrow the Ninth is a 2020 science fantasy novel by the New Zealand writer Tamsyn Muir. It is the second in Muir's Locked Tomb series",
					ImageUrl = "https://upload.wikimedia.org/wikipedia/en/thumb/0/02/Harrow_the_Ninth.jpg/220px-Harrow_the_Ninth.jpg",
					Price = 12.99m,
					CategoryId = 1
				},
				new Product
				{
					Id = 3,
					Name = "Nona the Ninth",
					Description = "Nona the Ninth is a 2022 science fantasy novel by the New Zealand writer Tamsyn Muir. It is the third book in the Locked Tomb series",
					ImageUrl = "https://upload.wikimedia.org/wikipedia/en/thumb/8/81/Nona_the_Ninth.jpg/220px-Nona_the_Ninth.jpg",
					Price = 15.99m,
					CategoryId = 1
				},
				new Product
				{
					Id = 4,
					CategoryId = 2,
					Name = "The Matrix",
					Description = "The Matrix is a 1999 science fiction action film written and directed by the Wachowskis, and produced by Joel Silver. Starring Keanu Reeves, Laurence Fishburne, Carrie-Anne Moss, Hugo Weaving, and Joe Pantoliano, and as the first installment in the Matrix franchise, it depicts a dystopian future in which humanity is unknowingly trapped inside a simulated reality, the Matrix, which intelligent machines have created to distract humans while using their bodies as an energy source. When computer programmer Thomas Anderson, under the hacker alias \"Neo\", uncovers the truth, he \"is drawn into a rebellion against the machines\" along with other people who have been freed from the Matrix.",
					ImageUrl = "https://upload.wikimedia.org/wikipedia/en/c/c1/The_Matrix_Poster.jpg",
				},
				new Product
				{
					Id = 5,
					CategoryId = 2,
					Name = "Back to the Future",
					Description = "Back to the Future is a 1985 American science fiction film directed by Robert Zemeckis. Written by Zemeckis and Bob Gale, it stars Michael J. Fox, Christopher Lloyd, Lea Thompson, Crispin Glover, and Thomas F. Wilson. Set in 1985, the story follows Marty McFly (Fox), a teenager accidentally sent back to 1955 in a time-traveling DeLorean automobile built by his eccentric scientist friend Doctor Emmett \"Doc\" Brown (Lloyd). Trapped in the past, Marty inadvertently prevents his future parents' meeting—threatening his very existence—and is forced to reconcile the pair and somehow get back to the future.",
					ImageUrl = "https://upload.wikimedia.org/wikipedia/en/d/d2/Back_to_the_Future.jpg"
				},
				new Product
				{
					Id = 6,
					CategoryId = 2,
					Name = "Toy Story",
					Description = "Toy Story is a 1995 American computer-animated comedy film produced by Pixar Animation Studios and released by Walt Disney Pictures. The first installment in the Toy Story franchise, it was the first entirely computer-animated feature film, as well as the first feature film from Pixar. The film was directed by John Lasseter (in his feature directorial debut), and written by Joss Whedon, Andrew Stanton, Joel Cohen, and Alec Sokolow from a story by Lasseter, Stanton, Pete Docter, and Joe Ranft. The film features music by Randy Newman, was produced by Bonnie Arnold and Ralph Guggenheim, and was executive-produced by Steve Jobs and Edwin Catmull. The film features the voices of Tom Hanks, Tim Allen, Don Rickles, Wallace Shawn, John Ratzenberger, Jim Varney, Annie Potts, R. Lee Ermey, John Morris, Laurie Metcalf, and Erik von Detten. Taking place in a world where anthropomorphic toys come to life when humans are not present, the plot focuses on the relationship between an old-fashioned pull-string cowboy doll named Woody and an astronaut action figure, Buzz Lightyear, as they evolve from rivals competing for the affections of their owner, Andy Davis, to friends who work together to be reunited with Andy after being separated from him.",
					ImageUrl = "https://upload.wikimedia.org/wikipedia/en/1/13/Toy_Story.jpg"
				},
				new Product
				{
					Id = 7,
					CategoryId = 3,
					Name = "Borderlands 2",
					Description = "Borderlands 2 is a 2012 first-person shooter video game developed by Gearbox Software and published by 2K. Taking place five years following the events of Borderlands (2009), the game is again set on the planet of Pandora.",
					ImageUrl = "https://upload.wikimedia.org/wikipedia/en/thumb/5/51/Borderlands_2_cover_art.png/220px-Borderlands_2_cover_art.png"
				},
				new Product
				{
					Id = 8,
					CategoryId = 3,
					Name = "Hades",
					Description = "Hades is a 2020 roguelike video game developed and published by Supergiant Games. It was released for macOS, Nintendo Switch, and Windows following an early access release in December 2018.",
					ImageUrl = "https://upload.wikimedia.org/wikipedia/en/thumb/c/cc/Hades_cover_art.jpg/220px-Hades_cover_art.jpg"
				},
				new Product
				{
					Id = 9,
					CategoryId = 3,
					Name = "Pokémon Legends: Arceus",
					Description = "Pokémon Legends: Arceus[a] is a 2022 action role-playing game developed by Game Freak and published by Nintendo and The Pokémon Company for the Nintendo Switch. It is part of the eighth generation of the Pokémon video game series and serves as a prequel to Pokémon Diamond and Pearl (2006).",
					ImageUrl = "https://upload.wikimedia.org/wikipedia/en/thumb/9/9c/Pokemon_Legends_Arceus_cover.jpg/220px-Pokemon_Legends_Arceus_cover.jpg"
				},
				new Product
				{
					Id = 10,
					CategoryId = 3,
					Name = "Xbox",
					Description = "The Xbox is a home video game console and the first installment in the Xbox series of video game consoles manufactured by Microsoft.",
					ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/4/43/Xbox-console.jpg",
				},
				new Product
				{
					Id = 11,
					CategoryId = 3,
					Name = "Super Nintendo Entertainment System",
					Description = "The Super Nintendo Entertainment System (SNES), also known as the Super NES or Super Nintendo, is a 16-bit home video game console developed by Nintendo that was released in 1990 in Japan and South Korea.",
					ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/e/ee/Nintendo-Super-Famicom-Set-FL.jpg",
				}
			);
		}

		public DbSet<Product> Products { get; set; }
		public DbSet<Category> Categories { get; set; }
	}
}
