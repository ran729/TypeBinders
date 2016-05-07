using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using CodeExecuter.Exceptions;
using CodeExecuter.Interfaces;

namespace CodeExecuter
{
    public class TypeBinder : ITypeBinder
    {
        private readonly object _instance;
        public readonly Type Type;

        public TypeBinder(string @namespace, string @class, params object[] args)
        {
            Type = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(o => o.GetTypes())
                    .FirstOrDefault(o => o.Namespace == @namespace && o.Name == @class);

            if (Type == null)
                throw new MissingAssemblyException(GenerateClassFullName(@namespace, @class));

            _instance = Type.Assembly.CreateInstance(
                GenerateClassFullName(@namespace, @class), 
                true, BindingFlags.CreateInstance, null,
                args, CultureInfo.CurrentCulture, null);
        }

        public TypeBinder(string @class, params object[] args)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(o => o.GetTypes())
                    .Where(o => o.Name == @class).ToList();

            if (types.Count > 1)
                throw new CouldNotDetermineTypeException(@class);

            if (!types.Any())
                throw new MissingAssemblyException(@class);

            Type = types.First();

            _instance = Type.Assembly.CreateInstance(
                GenerateClassFullName(Type.Namespace, @class),
                true, BindingFlags.CreateInstance, null,
                args, CultureInfo.CurrentCulture, null);
        }

        public static string GenerateClassFullName(string @namespace, string @class)
        {
            return @namespace + "." + @class;
        }

        public object Execute(string functionName, params object[] args)
        {
            MethodInfo method = Type.GetMethod(functionName);

            var numberOfAcceptedParameters = method.GetParameters().Count();

            if (numberOfAcceptedParameters < args.Length)
                args = args.Select(o => o).Take(numberOfAcceptedParameters).ToArray();

            return method.Invoke(_instance, args);
        }

        public object ExecuteStatic(string functionName, params object[] args)
        {
            MethodInfo method = Type.GetMethod(functionName);
            return method.Invoke(null, args);
        }

        public bool IsThisClass(string className, string namespaceName)
        {
            return GenerateClassFullName(namespaceName, className) == GenerateClassFullName(Type.Namespace, Type.Name);
        }

        public string GetClassFullName()
        {
            return GenerateClassFullName(Type.Namespace, Type.Name);
        }
    }
}
