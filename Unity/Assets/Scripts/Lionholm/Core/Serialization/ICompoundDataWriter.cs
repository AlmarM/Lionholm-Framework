namespace Lionholm.Core.Serialization
{
    /// <summary>
    /// Interface for serializing a KeyValueCompound into a string.
    /// </summary>
    public interface ICompoundDataWriter
    {
        string Write(KeyValueCompound compound);
    }
}