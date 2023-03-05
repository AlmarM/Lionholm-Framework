using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Lionholm.Core.Utils
{
    public static class ReflectionUtils
    {
        public static bool DerivesFrom(Type derived, Type @base)
        {
            return @base.IsAssignableFrom(derived);
        }

        public static IEnumerable<FieldInfo> GetFieldsWithAttribute<T>(Type type) where T : Attribute
        {
            return type
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(fi => fi.GetCustomAttributes(typeof(T), false).Length > 0)
                .ToArray();
        }

        public static IEnumerable<Type> GetSubTypes(Type type)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(t => DerivesFrom(t, type) && !t.IsAbstract)
                .ToArray();
        }
    }
}