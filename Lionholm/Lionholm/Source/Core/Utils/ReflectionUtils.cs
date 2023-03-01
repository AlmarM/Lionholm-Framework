using System;
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

        public static FieldInfo[] GetFieldsWithAttribute<T>(Type type) where T : Attribute
        {
            return type
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(fi => fi.GetCustomAttributes(typeof(T), false).Length > 0)
                .ToArray();
        }
    }
}