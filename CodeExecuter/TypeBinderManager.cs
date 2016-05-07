using System;
using System.Collections.Generic;
using CodeExecuter.Interfaces;

namespace CodeExecuter
{
    public class TypeBinderManager : ITypeBinderManager
    {
        private readonly Dictionary<string, ITypeBinder> _typeBinders;
        
        public TypeBinderManager()
        {
            _typeBinders = new Dictionary<string, ITypeBinder>();
        }

        public void AddTypeBinder(string namespaceName, string className, params object[] args)
        {
            var key = TypeBinder.GenerateClassFullName(namespaceName, className);

            if (_typeBinders.ContainsKey(key))
                return;

            _typeBinders.Add(key, new TypeBinder(namespaceName, className, args));
        }

        public void AddTypeBinder(ITypeBinder typeBinder)
        {
            var key = typeBinder.GetClassFullName();

            if (_typeBinders.ContainsKey(key))
                return;

            _typeBinders.Add(key, typeBinder);
        }

        public object Run(string fullClassName, string functionName, params object[] args)
        {
            if (!_typeBinders.ContainsKey(fullClassName))
                throw new Exception("Could not run code, type binder was not found");

            return _typeBinders[fullClassName].Execute(functionName, args);
        }

        public object Run(string namespaceName, string className, string functionName, params object[] args)
        {
            var key = TypeBinder.GenerateClassFullName(namespaceName, className);

            if (!_typeBinders.ContainsKey(key))
                throw new Exception("Could not run code, type binder was not found");

            return Run(key, functionName, args);
        }
    }
}
