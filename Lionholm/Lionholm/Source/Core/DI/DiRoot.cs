namespace Lionholm.Core.DI
{
    public class DiRoot
    {
        public DependencyContainer Container { get; private set; }

        public DiRoot(IBinder[] rootBinders)
        {
            PerformDependencyInjection(rootBinders);
        }

        private void PerformDependencyInjection(IBinder[] rootBinders)
        {
            Initialize();
            CreateBindingInfo(rootBinders);
            InjectDependencies();
        }

        private void Initialize()
        {
            Container = new DependencyContainer();
        }

        private void CreateBindingInfo(params IBinder[] rootBinders)
        {
            foreach (IBinder binder in rootBinders)
            {
                binder.Container = Container;
                binder.Bind();
            }
        }

        private void InjectDependencies()
        {
            Container.InjectUnresolvedBinds();
        }
    }
}