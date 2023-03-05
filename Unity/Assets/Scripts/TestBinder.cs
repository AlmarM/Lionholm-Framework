using System;
using System.Linq;
using Lionholm.Core.DI;
using Lionholm.Core.Serialization;
using Lionholm.Core.Serialization.JSON;
using Lionholm.Unity.DI;

public class TestBinder : MonoBinder
{
    public override void Bind()
    {
        Container.BindSelf<Game>();
    }

    public class Game : IBindsResolvedHandler
    {
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

            var a = new JsonCompoundDataWriter();
            string json = a.Write(three);

            var b = new JsonCompoundDataReader();
            KeyValueCompound compound = b.Read(json);
        }
    }
}