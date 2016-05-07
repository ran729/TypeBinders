using System;

namespace CodeExecuter.Exceptions
{
    public class CouldNotDetermineTypeException : Exception
    {
        public CouldNotDetermineTypeException(string @class)
        {
            Message = $"Could not bind to type {@class}, there were multiple types with the same name loaded in the AppDomain assemblies, solution - please provide namespace of the necessary type you want to bind to";
        }

        public override string Message { get; }
    }
}