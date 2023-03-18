using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Lionholm.Core.Utils;
using Newtonsoft.Json;

namespace Lionholm.Core.Serialization.JSON
{
    /// <summary>
    /// Serialize a KeyValueCompound using Newtonsoft.Json.
    /// 
    /// TODO
    /// - Throw exception for unsupported types
    /// - Add support for configuring JsonTextWriter settings
    /// </summary>
    public class JsonCompoundDataWriter : ICompoundDataWriter
    {
        private static readonly IJsonObjectWriter[] _jsonObjectWriters;

        static JsonCompoundDataWriter()
        {
            _jsonObjectWriters = TypeUtils.CreateAllSubTypes<IJsonObjectWriter>().ToArray();
        }

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

                if (kv.Value is KeyValueCompound nestedCompound)
                {
                    WriteCompound(writer, nestedCompound);
                    continue;
                }

                if (TryGetObjectWriter(kv.Value.GetType(), out IJsonObjectWriter objectWriter))
                {
                    objectWriter.Write(writer, kv.Value);
                    continue;
                }

                writer.WriteValue(kv.Value);
            }

            writer.WriteEndObject();
        }

        private static bool TryGetObjectWriter(Type type, out IJsonObjectWriter objectWriter)
        {
            foreach (IJsonObjectWriter writer in _jsonObjectWriters)
            {
                if (!writer.CanWrite(type))
                {
                    continue;
                }

                objectWriter = writer;
                return true;
            }

            objectWriter = null;
            return false;
        }
    }
}