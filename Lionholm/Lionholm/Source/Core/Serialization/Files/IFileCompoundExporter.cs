namespace Lionholm.Core.Serialization.Files
{
    public interface IFileCompoundExporter : ICompoundExporter
    {
        void Export(string path, KeyValueCompound compound);
    }
}