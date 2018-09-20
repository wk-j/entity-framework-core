using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DynamicModel {

    class DynamicContext : DbContext {
        private readonly ModelOptions modelOptions;
        public DynamicContext(DbContextOptions options, ModelOptions modelOptions) : base(options) {
            this.modelOptions = modelOptions;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            var type = ModelUtility.GenerateModelType(modelOptions.ModelType, modelOptions.TypeName);
            var entity = modelBuilder.Entity(type);
            var ins = Activator.CreateInstance(type);
            entity.HasData(new[] {
                ins
            });
        }
    }

    class Program {

        static DynamicContext CreateContext(string connectionString, string tableName, Type model) {
            var obj = new {
                A = 100,
                B = 200,
                C = DateTime.Now
            };

            var modelOptions = new ModelOptions {
                ModelType = obj.GetType(),
                TypeName = tableName
            };

            var collection = new ServiceCollection();
            collection.AddLogging(builder => {
                builder.AddConsole();
            });

            collection.AddSingleton<ModelOptions>(modelOptions);
            collection.AddDbContext<DynamicContext>(builder => {
                builder.UseNpgsql(connectionString);
            });

            var provider = collection.BuildServiceProvider();
            var context = provider.GetService<DynamicContext>();
            return context;
        }

        static void Main(string[] args) {
            var a = new {
                A = 100,
                B = 200,
                C = DateTime.Now,
                D = 100.0
            };
            var conn = "Host=localhost;User Id=postgres; Password=1234;Database=DynamicModel";
            var context = CreateContext(conn, "MyTable", a.GetType());
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
    }
}
