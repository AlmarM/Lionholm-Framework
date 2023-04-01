using System;

namespace Lionholm.Core
{
    public class CircularDependencyException : Exception
    {
        public CircularDependencyException()
        {
        }

        public CircularDependencyException(string message) : base(message)
        {
        }

        public CircularDependencyException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}