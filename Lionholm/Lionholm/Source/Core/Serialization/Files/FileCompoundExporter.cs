using System.IO;
using Lionholm.Core.DI;

namespace Lionholm.Core.Serialization.Files
{
    public class FileCompoundExporter : IFileCompoundExporter
    {
        [field: Dependency] public ICompoundDataWriter DataWriter { get; set; }

        private string _path;

        public virtual void Export(KeyValueCompound compound)
        {
            string data = DataWriter.Write(compound);

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