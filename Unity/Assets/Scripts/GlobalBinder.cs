using Lionholm.DI;
using Lionholm.DI.Unity;

namespace DefaultNamespace
{
    public class GlobalBinder : MonoTypeBinder
    {
        public override void Bind()
        {
            TestBinder.Bind(Binds);
        }
    }

    public class TestBinder : TypeBinder<TestBinder>
    {
        public override void Bind()
        {
        }
    }
}