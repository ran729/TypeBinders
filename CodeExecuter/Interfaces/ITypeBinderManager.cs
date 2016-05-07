namespace CodeExecuter.Interfaces
{
    public interface ITypeBinderManager
    {
        void AddTypeBinder(string namespaceName, string className, params object[] args);
        object Run(string fullClassName, string functionName, params object[] args);
        object Run(string namespaceName, string className, string functionName, params object[] args);
    }
}