using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Lionholm.Core.Utils
{
    public static class ReflectionExtensions
    {
        public static IEnumerable<FieldInfo> GetFieldsWithAttribute<T>(this Type type) where T : Attribute
        {
            return type
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(fi => fi.GetCustomAttributes(typeof(T), false).Length > 0)
                .ToArray();
        }
    }
}