using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Linq;

namespace DynamicModel {
    public class ModelBase {
        [Key]
        public int _Id { set; get; } = 1;
    }

    class Utility {
        public static Type BuildClass() {
            var src = @"
                using DynamicModel;
                using System;
                class A: ModelBase {
                    public string Name { set; get; } = ""Name"";
                    public string LastName { set; get; } = ""LastName"";
                }
                return typeof(A);
            ";

            var reference = ScriptOptions.Default.WithReferences(Assembly.GetExecutingAssembly());
            var script = CSharpScript.Create(src, reference);
            var compile = script.Compile();
            var a = script.RunAsync().Result.ReturnValue as Type;
            return a;
        }
    }

    class Context : DbContext {
        public Context(DbContextOptions options) : base(options) {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            // var entity = modelBuilder.Entity<Student>();
            var type = Utility.BuildClass();
            var entity = modelBuilder.Entity(type);

            var a = Activator.CreateInstance(type);
            //type.GetProperty("Name").SetValue(a, "Name");
            //type.GetProperty("LastName").SetValue(a, "LastName");

            entity.HasData(new[] {
                a
            });
        }
    }

    class Program {
        static void Main(string[] args) {
            var collection = new ServiceCollection();
            collection.AddLogging(builder => {
                builder.AddConsole();
            });

            collection.AddDbContext<Context>(builder => {
                var conn = "Host=localhost;User Id=postgres; Password=1234;Database=DynamicModel";
                builder.UseNpgsql(conn);
            });

            var provider = collection.BuildServiceProvider();
            var context = provider.GetService<Context>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();


        }
    }
}
