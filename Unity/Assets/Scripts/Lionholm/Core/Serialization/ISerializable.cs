namespace Lionholm.Core.Serialization
{
    public interface ISerializable
    {
        KeyValueCompound Serialize();

        void Deserialize(KeyValueCompound compound);
    }
}