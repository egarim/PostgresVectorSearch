using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;
using System;
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

                // rank 1
                context.Blogs.Add(new Blog
                {
                    Title = "The Jungle's Hidden Treasures",
                    Subtitle = "Discovering Unknown Species",
                    Content = "Every year, numerous new species are discovered in the depths of the world's jungles. This article explores the most remarkable finds of the past decade."
                }); // High ranking due to Title

                // rank 4
                context.Blogs.Add(new Blog
                {
                    Title = "Music Inspired by Nature",
                    Subtitle = "From Jungle Drums to Ocean Waves",
                    Content = "Nature has always played a pivotal role in inspiring musicians. This piece dives into the various sounds of nature that have influenced different musical genres."
                }); // Medium ranking due to Subtitle
                
                // rank 5
                context.Blogs.Add(new Blog
                {
                    Title = "Adventurous Cuisines Around the World",
                    Subtitle = "A Gastronomic Journey",
                    Content = "The Amazon rainforest, often referred to as the 'lungs of the Earth' or simply 'the Jungle', offers a unique blend of flavors and ingredients found nowhere else."
                }); //Medium Relevance (Content focus):

                // rank 6
                context.Blogs.Add(new Blog
                {
                    Title = "The Topography of South America",
                    Subtitle = "Mountains, Plateaus, and Rivers",
                    Content = "While the continent is known for the Andes and the Amazon River, its jungles are a biodiverse paradise waiting to be explored."
                }); // Lower ranking due to Content mention

                // rank 7
                context.Blogs.Add(new Blog
                {
                    Title = "Digital Connectivity in Remote Areas",
                    Subtitle = "Bridging the Gap with Technology",
                    Content = "Even in the dense jungles of Borneo, the reach of the internet is expanding, bringing global connectivity to local communities."
                }); // Lower ranking due to Content mention

                // rank 8
                context.Blogs.Add(new Blog
                {
                    Title = "Adventures Across Continents",
                    Subtitle = "From the Sahara to Siberia",
                    Content = "Chapter 5 dives deep into the lush green jungles of South America, exploring its biodiversity."
                }); // Lower ranking due to Content mention

                // rank 3
                context.Blogs.Add(new Blog
                {
                    Title = "From City Streets to Unknown Beats",
                    Subtitle = "Jungle Rhythms and Their Influence on Modern Music",
                    Content = "How the sounds of the jungle have shaped contemporary music genres."
                }); // Medium ranking due to Subtitle


                // rank 2
                context.Blogs.Add(new Blog
                {
                    Title = "Exploring the Heart of the Jungle",
                    Subtitle = "A Traveler's Diary",
                    Content = "This book documents my journey through the dense forests, unveiling the mysteries of the jungle."
                }); // High ranking due to Title


                context.SaveChanges();

                Console.WriteLine("Blogs inserted:");

             
                var searchTerm = "Jungle";

                var blogs = context.Blogs
                    .Where(p => p.SearchVector.Matches(EF.Functions.ToTsQuery(searchTerm)))
                    .OrderByDescending(td => td.SearchVector.Rank(EF.Functions.ToTsQuery(searchTerm)))
                    .ToList();

                //Expected output

                //1 The Jungle's Hidden Treasures
                //2 Exploring the Heart of the Jungle
                //3 From City Streets to Unknown Beats
                //4 Music Inspired by Nature
                //5 Adventurous Cuisines Around the World
                //6 The Topography of South America
                //7 Digital Connectivity in Remote Areas
                //8 Adventures Across Continents
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

                var searchTerm = "Adventure"; // Example search term
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