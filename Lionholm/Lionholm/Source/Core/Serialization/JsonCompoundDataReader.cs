using System.IO;
using Newtonsoft.Json;

namespace Lionholm.Core.Serialization
{
    public class JsonCompoundDataReader : ICompoundDataReader
    {
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

            while (reader.Read())
            {
                
            }

            return compound;
        }
    }
}