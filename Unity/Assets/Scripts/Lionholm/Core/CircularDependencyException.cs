using System;

namespace Lionholm.Core
{
    public class CircularDependencyException : Exception
    {
        public override string Message
        {
            get
            {
                if (ParentType == null || ChildType == null)
                {
                    return base.Message;
                }

                return $"Circular dependency found between {ParentType} and {ChildType}.";
            }
        }

        public Type ParentType { get; }

        public Type ChildType { get; }

        public CircularDependencyException()
        {
        }

        public CircularDependencyException(string message) : base(message)
        {
        }

        public CircularDependencyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CircularDependencyException(Type parentType, Type childType)
        {
            ParentType = parentType;
            ChildType = childType;
        }
    }
}