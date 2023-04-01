using System.Collections.Generic;

namespace Lionholm.Serialization
{
    public class KeyValueCompound
    {
        public IDictionary<string, object> Data { get; }

        public KeyValueCompound()
        {
            Data = new Dictionary<string, object>();
        }

        public void Write(string key, object value)
        {
            Data[key] = value;
        }

        public object Read(string key)
        {
            return Data[key];
        }

        public T Read<T>(string key)
        {
            return (T)Data[key];
        }

        public KeyValueCompound ReadCompound(string key)
        {
            return Read<KeyValueCompound>(key);
        }
    }
}