using Newtonsoft.Json;

namespace Lionholm.Core.Serialization.JSON
{
    public class JsonPrimitiveReader : IJsonObjectReader
    {
        public bool CanRead(JsonToken token)
        {
            switch (token)
            {
                case JsonToken.Integer:
                case JsonToken.Float:
                case JsonToken.String:
                case JsonToken.Boolean:
                    return true;
                default:
                    return false;
            }
        }

        public object Read(JsonReader reader)
        {
            return reader.Value;
        }
    }
}