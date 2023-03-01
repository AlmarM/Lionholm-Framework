namespace Lionholm.Core.Serialization
{
    public interface ISerializer
    {
        ICompoundExporter Exporter { get; }

        ICompoundImporter Importer { get; }
    }
}