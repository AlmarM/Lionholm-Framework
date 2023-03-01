namespace Lionholm.Core.Serialization
{
    public interface ICompoundDataWriter
    {
        string Write(KeyValueCompound compound);
    }
}