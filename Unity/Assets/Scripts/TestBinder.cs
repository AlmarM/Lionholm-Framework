using System;
using Lionholm.Core.DI;
using Lionholm.Core.Serialization;
using Lionholm.Core.Serialization.Files;
using Lionholm.Unity.DI;

public class TestBinder : MonoBinder
{
    public override void Bind()
    {
        Container.BindSelf<Game>();
        Container.Bind<IFileCompoundExporter>().To<JsonCompoundExporter>();
    }

    public class Game : IBindsResolvedHandler
    {
        [Dependency] private IFileCompoundExporter _compoundExporter;

        public void OnBindsResolved()
        {
            var one = new KeyValueCompound();
            one.Write("test", 123);
            one.Write("yes", "yes");

            var two = new KeyValueCompound();
            two.Write("one", one);
            two.Write("blah", true);
            two.Write("jawel", 123.53f);

            var three = new KeyValueCompound();
            three.Write("unos", "dos");
            three.Write("two", two);
            three.Write("lelele", 334342342663);

            _compoundExporter.Export(@"E:\", "testfile", three);
        }
    }
}