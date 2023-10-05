using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace PostgresVectorSearch
{
    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=Blogging;Username=postgres;Password=1234567890",
                options => options.UseNetTopologySuite());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>()
                .Property(b => b.SearchVector)
              .HasComputedColumnSql("to_tsvector('english', \"Blogs\".\"Title\")", stored: true);
        }
    }
}
