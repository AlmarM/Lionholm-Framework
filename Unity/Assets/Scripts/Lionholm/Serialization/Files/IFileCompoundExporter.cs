namespace Lionholm.Serialization.Files
{
    /// <summary>
    /// Base type for serializing and saving a KeyValueCompound to a file.
    /// </summary>
    public interface IFileCompoundExporter : ICompoundExporter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="compound"></param>
        void Export(string path, KeyValueCompound compound);
    }
}