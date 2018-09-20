using System;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Linq;

namespace DynamicModel {
    class ModelUtility {
        public static Type GenerateModelType(Type source, string name) {
            var src = $@"
                using DynamicModel;
                using System;
                public class {name}: ModelBase {{
                    {
                        source.GetProperties()
                            .Select(x => $"public {x.PropertyType} {x.Name} {{ set;get; }}")
                            .Aggregate((a, x) => $"{a}\n{x}")
                    }
                }}
                return typeof({name});
            ";
            var reference = ScriptOptions.Default.WithReferences(Assembly.GetExecutingAssembly());
            var script = CSharpScript.Create(src, reference);
            var compile = script.Compile();
            var modelType = script.RunAsync().Result.ReturnValue as Type;
            return modelType;
        }
    }
}
