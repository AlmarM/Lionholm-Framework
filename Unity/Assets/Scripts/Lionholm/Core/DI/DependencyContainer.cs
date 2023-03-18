using System;

namespace Lionholm.Core.DI
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

        public void InjectDependencies(object instance)
        {
            _dependencyInjector.InjectDependencies(instance);
        }

        public bool TryGetResolvedInstance(Type type, out object instance)
        {
            return _dependencyInjector.TryGetResolvedInstance(type, out instance);
        }

        public object ResolveType<T>() where T : Type
        {
            return ResolveType(typeof(T));
        }

        public object ResolveType(Type type)
        {
            return _dependencyInjector.ResolveType(type);
        }
    }
}