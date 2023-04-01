using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lionholm.Serialization.JSON
{
    public class JsonArrayReader : IJsonObjectReader
    {
        public bool CanRead(JsonToken token)
        {
            return token == JsonToken.StartArray;
        }

        public object Read(JsonReader reader)
        {
            var collection = new List<object>();
            var currentToken = JsonToken.None;

            while (currentToken != JsonToken.EndArray)
            {
                reader.Read();

                currentToken = reader.TokenType;

                switch (currentToken)
                {
                    case JsonToken.Integer:
                    case JsonToken.Float:
                    case JsonToken.String:
                    case JsonToken.Boolean:
                        collection.Add(reader.Value);
                        break;
                }
            }

            return collection.ToArray();
        }
    }
}