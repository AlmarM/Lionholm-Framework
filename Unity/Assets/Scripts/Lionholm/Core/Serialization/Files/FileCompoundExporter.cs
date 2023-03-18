using System.IO;

namespace Lionholm.Core.Serialization.Files
{
    public class FileCompoundExporter : IFileCompoundExporter
    {
        private readonly ICompoundDataWriter _compoundDataWriter;
        private string _path;

        public FileCompoundExporter(ICompoundDataWriter compoundDataWriter)
        {
            _compoundDataWriter = compoundDataWriter;
        }

        public virtual void Export(KeyValueCompound compound)
        {
            string data = _compoundDataWriter.Write(compound);

            WriteFile(data);
            ClearCache();
        }

        public virtual void Export(string path, KeyValueCompound compound)
        {
            _path = path;

            Export(compound);
        }

        protected virtual void WriteFile(string data)
        {
            File.WriteAllText(_path, data);
        }

        protected virtual void ClearCache()
        {
            _path = string.Empty;
        }
    }
}