namespace Lionholm.Core.Serialization
{
    /// <summary>
    /// Interface for deserializing a json string into a KeyValueCompound.
    /// </summary>
    public interface ICompoundDataReader
    {
        KeyValueCompound Read(string input);
    }
}