using System.IO;
using System.Linq;
using Lionholm.Core.Utils;
using Newtonsoft.Json;

namespace Lionholm.Core.Serialization.JSON
{
    /// <summary>
    /// Deserialize a json string to a KeyValueCompound using Newtonsoft.Json.
    /// 
    /// TODO
    /// - Throw exception for unsupported types
    /// </summary>
    public class JsonCompoundDataReader : ICompoundDataReader
    {
        private static readonly IJsonObjectReader[] _jsonObjectReaders;

        static JsonCompoundDataReader()
        {
            _jsonObjectReaders = TypeUtils.CreateAllSubTypes<IJsonObjectReader>().ToArray();
        }

        public KeyValueCompound Read(string input)
        {
            var stringReader = new StringReader(input);
            using var jsonReader = new JsonTextReader(stringReader);

            // Read initial StartObject token
            jsonReader.Read();

            return ReadCompound(jsonReader);
        }

        private static KeyValueCompound ReadCompound(JsonTextReader reader)
        {
            var compound = new KeyValueCompound();
            var key = string.Empty;
            var currentToken = JsonToken.None;

            while (currentToken != JsonToken.EndObject)
            {
                reader.Read();

                currentToken = reader.TokenType;

                switch (currentToken)
                {
                    case JsonToken.PropertyName:
                        key = reader.Value as string;
                        continue;
                    case JsonToken.StartObject:
                        compound.Write(key, ReadCompound(reader));
                        key = string.Empty;
                        continue;
                }

                if (!TryGetObjectWriter(currentToken, out IJsonObjectReader objectReader))
                {
                    continue;
                }

                compound.Write(key, objectReader.Read(reader));
                key = string.Empty;
            }

            return compound;
        }

        private static bool TryGetObjectWriter(JsonToken token, out IJsonObjectReader objectReader)
        {
            foreach (IJsonObjectReader reader in _jsonObjectReaders)
            {
                if (!reader.CanRead(token))
                {
                    continue;
                }

                objectReader = reader;
                return true;
            }

            objectReader = null;
            return false;
        }
    }
}