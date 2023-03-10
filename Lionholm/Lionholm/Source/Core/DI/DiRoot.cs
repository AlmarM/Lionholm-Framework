namespace Lionholm.Core.DI
{
    public class DiRoot
    {
        public DependencyContainer Container { get; private set; }

        public DiRoot(ITypeBinder[] rootBinders)
        {
            PerformDependencyInjection(rootBinders);
        }

        private void PerformDependencyInjection(ITypeBinder[] rootBinders)
        {
            Initialize();
            CreateBindingInfo(rootBinders);
            InjectDependencies();
        }

        private void Initialize()
        {
            Container = new DependencyContainer();
        }

        private void CreateBindingInfo(params ITypeBinder[] rootBinders)
        {
            // foreach (ITypeBinder binder in rootBinders)
            // {
            //     binder.Container = Container;
            //     binder.Bind();
            // }
        }

        private void InjectDependencies()
        {
            //Container.InjectUnresolvedBinds();
        }
    }
}