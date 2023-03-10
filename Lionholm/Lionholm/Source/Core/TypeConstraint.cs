using System;
using System.Collections.Generic;
using System.Linq;
using Lionholm.Core.Utils;

namespace Lionholm.Core
{
    public readonly struct TypeConstraint
    {
        private readonly IList<Type> _typeConstraints;

        public TypeConstraint(Type[] types)
        {
            _typeConstraints = new List<Type>(types);
        }

        public static TypeConstraint From(params Type[] types)
        {
            return new TypeConstraint(types);
        }

        public bool Matches<T>() where T : Type => Matches(typeof(T));

        public bool Matches(Type type)
        {
            return _typeConstraints.All(type.DerivesFrom);
        }
    }
}