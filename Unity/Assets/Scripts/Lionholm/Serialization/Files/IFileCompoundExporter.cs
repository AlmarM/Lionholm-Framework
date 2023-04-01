namespace Lionholm.Serialization.Files
{
    public interface IFileCompoundExporter : ICompoundExporter
    {
        void Export(string path, KeyValueCompound compound);
    }
}