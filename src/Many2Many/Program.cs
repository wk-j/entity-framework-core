using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Many2Many {
    public class Post {
        public int Id { set; get; }
        public string Name { set; get; }
        public ICollection<Tag> Tags { set; get; } = new List<Tag>();
    }

    public class Tag {
        public int Id { set; get; }
        public string Name { set; get; }
        public ICollection<Post> Posts { set; get; } = new List<Post>();
    }

    public class BlogContext : DbContext {
        public BlogContext(DbContextOptions options) : base(options) { }

        public DbSet<Post> Posts { set; get; }
        public DbSet<Tag> Tags { set; get; }
    }

    class Program {
        static void Main(string[] args) {
            var collection = new ServiceCollection();
            collection.AddDbContext<BlogContext>(opt => {
                opt.UseNpgsql("Host=localhost;User Id=postgres;Password=1234;Database=M2M", c => {
                    c.SetPostgresVersion(9, 4);
                });
            });

            var context = collection.BuildServiceProvider().GetService<BlogContext>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Posts.Add(new Post {
                Name = "Hello",
                Tags = { new() { Name = "T1" }, new() { Name = "T2" } }
            });

            context.SaveChanges();
        }
    }
}
