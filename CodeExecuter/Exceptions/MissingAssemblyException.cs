using System;

namespace CodeExecuter.Exceptions
{
    public class MissingAssemblyException : Exception
    {
        public MissingAssemblyException(string type)
        {
            Message = $"Could not bind to type {type}, assembly in not loaded for this type.";
        }

        public override string Message { get; }
    }
}   