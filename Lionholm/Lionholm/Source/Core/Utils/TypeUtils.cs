using System;
using System.Collections.Generic;
using System.Linq;

namespace Lionholm.Core.Utils
{
    public static class TypeUtils
    {
        public static IEnumerable<T> CreateAllSubTypes<T>()
        {
            return typeof(T).GetSubTypes()
                .Select(Activator.CreateInstance)
                .Cast<T>();
        }
    }
}