using Newtonsoft.Json;

namespace Lionholm.Serialization.JSON
{
    /// <summary>
    /// Adds support for reading specific JsonTokens from a Newtonsoft.Json JsonReader to an object.
    /// Instances will be automatically made for use in JsonCompoundDataReader.
    /// </summary>
    public interface IJsonObjectReader
    {
        bool CanRead(JsonToken token);

        object Read(JsonReader reader);
    }
}