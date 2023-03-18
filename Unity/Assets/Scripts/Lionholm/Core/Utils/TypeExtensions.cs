using System;
using System.Collections.Generic;
using System.Linq;

namespace Lionholm.Core.Utils
{
    public static class TypeExtensions
    {
        public static bool DerivesFrom(this Type derived, Type @base)
        {
            return @base.IsAssignableFrom(derived);
        }

        public static IEnumerable<Type> GetSubTypes(this Type type)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(t => DerivesFrom(t, type) && !t.IsAbstract)
                .ToArray();
        }

        public static IEnumerable<Type> GetInterfacesAndSelf(this Type type)
        {
            return type.GetInterfaces().Concat(new[] { type }).ToArray();
        }
    }
}