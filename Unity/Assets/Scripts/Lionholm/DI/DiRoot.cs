namespace Lionholm.DI
{
    public class DiRoot
    {
        public BindInfoCollection InfoCollection { get; }

        private readonly DependencyInjector _dependencyInjector;
        private readonly DependencyContainer _dependencyContainer;

        public DiRoot()
        {
            InfoCollection = new BindInfoCollection();
            _dependencyInjector = new DependencyInjector(InfoCollection);
            _dependencyContainer = new DependencyContainer(_dependencyInjector);
        }

        public void PerformDependencyInjection(ITypeBinder[] rootBinders)
        {
            CreateBindInfos(rootBinders);
            ResolveTypes();
        }

        private void CreateBindInfos(params ITypeBinder[] rootBinders)
        {
            foreach (ITypeBinder binder in rootBinders)
            {
                binder.Bind();
            }
        }

        private void ResolveTypes()
        {
            _dependencyInjector.SetDependencyInstance(typeof(DependencyContainer), _dependencyContainer);
            _dependencyInjector.PerformInjection();
        }
    }
}