namespace Lionholm.Serialization
{
    /// <summary>
    /// Base type for serializing and saving a KeyValueCompound.
    /// </summary>
    public interface ICompoundExporter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="compound"></param>
        void Export(KeyValueCompound compound);
    }
}