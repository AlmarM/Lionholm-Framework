using Lionholm.Core.DI;
using Lionholm.Unity.DI;

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
            Binds.AssignSelf<Person>();
            Binds.LazyAssign<Apple>().To<IFruit>();
        }
    }

    public class Person : IInjectionCompleteHandler
    {
        private IFruit _fruit;

        [Dependency] private DependencyContainer _container;

        public Person(IFruit fruit)
        {
            _fruit = fruit;
        }

        public void OnInjectionComplete()
        {
            int a;
        }
    }

    public interface IFruit
    {
    }

    public class Apple : IFruit
    {
    }
}