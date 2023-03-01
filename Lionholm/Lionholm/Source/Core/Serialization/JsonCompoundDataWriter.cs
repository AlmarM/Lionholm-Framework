using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Lionholm.Core.Serialization
{
    /// <summary>
    /// TODO
    /// - Throw exception for unsupported types
    /// </summary>
    public class JsonCompoundDataWriter : ICompoundDataWriter
    {
        public string Write(KeyValueCompound compound)
        {
            var stringBuilder = new StringBuilder();
            var stringWriter = new StringWriter(stringBuilder);

            using var jsonWriter = new JsonTextWriter(stringWriter);
            jsonWriter.Formatting = Formatting.Indented;

            WriteCompound(jsonWriter, compound);

            return stringBuilder.ToString();
        }

        private static void WriteCompound(JsonWriter writer, KeyValueCompound compound)
        {
            writer.WriteStartObject();

            foreach (KeyValuePair<string, object> kv in compound.Data)
            {
                writer.WritePropertyName(kv.Key);

                switch (kv.Value)
                {
                    case KeyValueCompound innerCompound:
                        WriteCompound(writer, innerCompound);
                        continue;
                    case IList list:
                        WriteEnumerable(writer, list);
                        continue;
                    default:
                        writer.WriteValue(kv.Value);
                        break;
                }
            }

            writer.WriteEndObject();
        }

        private static void WriteEnumerable(JsonWriter writer, IEnumerable list)
        {
            writer.WriteStartArray();

            foreach (object value in list)
            {
                writer.WriteValue(value);
            }

            writer.WriteEndArray();
        }
    }
}