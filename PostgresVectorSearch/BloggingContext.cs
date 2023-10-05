using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq;

namespace PostgresVectorSearch
{
    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
               optionsBuilder.UseNpgsql("Host=localhost;Database=Blogging;Username=postgres;Password=1234567890",
                options => options.UseNetTopologySuite()).LogTo((message) =>
                {
                    Debug.WriteLine(message);
                });
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>()
         .Property(b => b.SearchVector)
         .HasComputedColumnSql(
             "setweight(to_tsvector('english', \"Blogs\".\"Title\"), 'A') || " +
             "setweight(to_tsvector('english', \"Blogs\".\"Subtitle\"), 'B') || " +
             "setweight(to_tsvector('english', \"Blogs\".\"Content\"), 'C')",
             stored: true);
        }
    }
}
