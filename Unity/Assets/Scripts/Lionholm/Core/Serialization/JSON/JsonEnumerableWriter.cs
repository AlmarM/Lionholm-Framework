using System;
using System.Collections;
using Newtonsoft.Json;

namespace Lionholm.Core.Serialization.JSON
{
    public class JsonEnumerableWriter : IJsonObjectWriter
    {
        public bool CanWrite(Type objectType)
        {
            return objectType == typeof(IEnumerable);
        }

        public void Write(JsonWriter writer, object @object)
        {
            var enumerable = (IEnumerable)@object;

            writer.WriteStartArray();

            foreach (object value in enumerable)
            {
                writer.WriteValue(value);
            }

            writer.WriteEndArray();
        }
    }
}