using System;
using Newtonsoft.Json;

namespace Lionholm.Core.Serialization.JSON
{
    /// <summary>
    /// Adds support for writing specific types to a Newtonsoft.Json JsonWriter.
    /// Instances will be automatically made for use in JsonCompoundDataWriter.
    /// </summary>
    public interface IJsonObjectWriter
    {
        bool CanWrite(Type objectType);

        void Write(JsonWriter writer, object @object);
    }
}