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
        static void Main(string[] args) {

            var obj = new {
                A = 100,
                B = 200,
                C = DateTime.Now
            };

            var modelOptions = new ModelOptions {
                ModelType = obj.GetType(),
                TypeName = "MyModel"
            };

            var collection = new ServiceCollection();
            collection.AddLogging(builder => {
                builder.AddConsole();
            });

            collection.AddSingleton<ModelOptions>(modelOptions);
            collection.AddDbContext<DynamicContext>(builder => {
                var conn = "Host=localhost;User Id=postgres; Password=1234;Database=DynamicModel";
                builder.UseNpgsql(conn);
            });

            var provider = collection.BuildServiceProvider();
            var context = provider.GetService<DynamicContext>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
    }
}
