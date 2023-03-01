using Lionholm.Core.DI;

namespace Lionholm.Core.Serialization
{
    public class Serializer : ISerializer
    {
        [field: Dependency] public ICompoundExporter Exporter { get; }

        [field: Dependency] public ICompoundImporter Importer { get; }
    }
}