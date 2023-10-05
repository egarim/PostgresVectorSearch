using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;
using System.Diagnostics;

namespace PostgresVectorSearch
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            using (var context = new BloggingContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                context.Blogs.Add(new Blog { Title = "Journey to the Heart of the Jungle" });
                context.Blogs.Add(new Blog { Title = "Mastering the Art of French Cooking" });
                context.Blogs.Add(new Blog { Title = "A Night Under the Starlit Sky" });
                context.Blogs.Add(new Blog { Title = "The Secrets of the Deep Blue Sea" });
                context.Blogs.Add(new Blog { Title = "Chasing Dreams: My Year in Space" });
                context.Blogs.Add(new Blog { Title = "The Last Dragon: Myths and Legends" });
                context.Blogs.Add(new Blog { Title = "Beyond the Horizon: Adventures in the Desert" });
                context.Blogs.Add(new Blog { Title = "Whispers of the Ancient Forest" });
                context.Blogs.Add(new Blog { Title = "Dance of the Northern Lights" });
                context.Blogs.Add(new Blog { Title = "Melodies of a Wandering Bard" });

                context.SaveChanges();

                Console.WriteLine("Blogs inserted:");

                var searchTerm = "Jungle"; // Example search term
                var searchVector = NpgsqlTsVector.Parse(searchTerm);

                var blogs = context.Blogs
                    .Where(p => p.SearchVector.Matches(searchTerm))
                    .OrderByDescending(td => td.SearchVector.Rank(EF.Functions.ToTsQuery(searchTerm))).ToList();
                   
                foreach (var blog in blogs)
                {
                    Debug.WriteLine(blog.Title);
                }
            }
            Assert.Pass();
        }
        [Test]
        public void Test2()
        {
            using (var context = new BloggingContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                context.Blogs.Add(new Blog { Title = "Journey to the Heart of the Jungle" });
                context.Blogs.Add(new Blog { Title = "Mastering the Art of French Cooking" });
                context.Blogs.Add(new Blog { Title = "A Night Under the Starlit Sky" });
                context.Blogs.Add(new Blog { Title = "The Secrets of the Deep Blue Sea" });
                context.Blogs.Add(new Blog { Title = "Chasing Dreams: My Year in Space" });
                context.Blogs.Add(new Blog { Title = "The Last Dragon: Myths and Legends" });
                context.Blogs.Add(new Blog { Title = "Beyond the Horizon: Adventures in the Desert" });
                context.Blogs.Add(new Blog { Title = "Whispers of the Ancient Forest" });
                context.Blogs.Add(new Blog { Title = "Dance of the Northern Lights" });
                context.Blogs.Add(new Blog { Title = "Melodies of a Wandering Bard" });
                context.Blogs.Add(new Blog { Title = "Ultimate Adventure in the Amazon Rainforest" });
                context.Blogs.Add(new Blog { Title = "My Culinary Adventure in Italy" });
                context.Blogs.Add(new Blog { Title = "Reading: An Intellectual Adventure" });
                context.Blogs.Add(new Blog { Title = "Adventures of a Solo Traveler" });
                context.Blogs.Add(new Blog { Title = "Learning to Play the Guitar: A Musical Journey, Not an Adventure" });

                context.SaveChanges();

                Console.WriteLine("Blogs inserted:");

                var searchTerm = "Aventura"; // Example search term
                var searchVector = NpgsqlTsVector.Parse(searchTerm);

                //the rest result is the most relevant and the last is the least relevant
                var blogs = context.Blogs
                    .Where(p => p.SearchVector.Matches(searchTerm))
                    .OrderByDescending(td => td.SearchVector.Rank(EF.Functions.ToTsQuery(searchTerm))).ToList();

                foreach (var blog in blogs)
                {
                    Debug.WriteLine(blog.Title);
                }
            }
            Assert.Pass();
        }
    }
}