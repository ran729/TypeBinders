namespace CodeExecuter.Interfaces
{
    public interface ITypeBinder
    {
        object Execute(string functionName, params object[] args);
        object ExecuteStatic(string functionName, params object[] args);
        bool IsThisClass(string className, string namespaceName);
        string GetClassFullName();
    }
}