using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Query;

namespace SqlGenerate {
    public static class IQueryExtensions {
        private static readonly FieldInfo _queryCompilerField =
            typeof(EntityQueryProvider).GetTypeInfo().DeclaredFields.Single(x => x.Name == "_queryCompiler");

        private static readonly TypeInfo _queryCompilerTypeInfo =
            typeof(QueryCompiler).GetTypeInfo();

        private static readonly FieldInfo _queryModelGeneratorField =
            _queryCompilerTypeInfo.DeclaredFields.Single(x => x.Name == "_queryModelGenerator");

        private static readonly FieldInfo _databaseField =
            _queryCompilerTypeInfo.DeclaredFields.Single(x => x.Name == "_database");

        private static readonly PropertyInfo _dependenciesProperty =
            typeof(Database).GetTypeInfo().DeclaredProperties.Single(x => x.Name == "Dependencies");

        public static string ToSql<TEntity>(this IQueryable<TEntity> queryable)
            where TEntity : class {
            if (!(queryable is EntityQueryable<TEntity>) && !(queryable is InternalDbSet<TEntity>))
                throw new ArgumentException();
            var queryCompiler = (IQueryCompiler)_queryCompilerField.GetValue(queryable.Provider);
            var queryModelGenerator = (IQueryModelGenerator)_queryModelGeneratorField.GetValue(queryCompiler);
            var queryModel = queryModelGenerator.ParseQuery(queryable.Expression);
            var database = _databaseField.GetValue(queryCompiler);
            var queryCompilationContextFactory = ((DatabaseDependencies)_dependenciesProperty.GetValue(database)).QueryCompilationContextFactory;
            var queryCompilationContext = queryCompilationContextFactory.Create(false);
            var modelVisitor = (RelationalQueryModelVisitor)queryCompilationContext.CreateQueryModelVisitor();
            modelVisitor.CreateQueryExecutor<TEntity>(queryModel);
            return modelVisitor.Queries.Join(Environment.NewLine + Environment.NewLine);
        }
    }

    class Student {
        [Key]
        public int Id { set; get; }
        public string Name { set; get; }
        public DateTime Birth { set; get; }
        public float Gpa { set; get; }
    }

    class MyContext : DbContext {
        public MyContext(DbContextOptions options) : base(options) { }
        public DbSet<Student> Students { set; get; }
    }

    class Program {
        static void Main(string[] args) {
            var collection = new ServiceCollection();
            collection.AddDbContext<MyContext>(options => {
                options.UseNpgsql("Host=localhost;User Id=postgres;Password=1234;Database=MyDB");
            });

            var provider = collection.BuildServiceProvider();
            var context = provider.GetService<MyContext>();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Add(new Student {
                Name = "wk",
                Birth = new DateTime(2000, 10, 10),
                Gpa = 4f
            });

            context.SaveChanges();

            var query =
                from student in context.Students
                let gpa = 3.5
                where student.Gpa >= gpa && student.Name.ToUpper().Contains("w")
                select new {
                    Name = student.Name,
                    Gpa = student.Gpa,
                    Birth = student.Birth
                };

            Console.WriteLine(query.ToSql());
        }
    }
}
