namespace Lionholm.DI
{
    /// <summary>
    /// Facade that will restrict access to DependencyInjector.
    /// </summary>
    public class DependencyContainer
    {
        private readonly DependencyInjector _dependencyInjector;

        public DependencyContainer(DependencyInjector dependencyInjector)
        {
            _dependencyInjector = dependencyInjector;
        }

        public void InjectDependencies(object instance) => _dependencyInjector.InjectDependencies(instance);
    }
}