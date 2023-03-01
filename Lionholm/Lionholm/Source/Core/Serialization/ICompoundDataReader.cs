namespace Lionholm.Core.Serialization
{
    public interface ICompoundDataReader
    {
        KeyValueCompound Read(string input);
    }
}