using System;
using System.Collections.Generic;
using System.Reflection;
using Lionholm.Core.Utils;

namespace Lionholm.Core.DI
{
    public class DependencyInjector
    {
        private readonly BindInfoCollection _bindInfoCollection;
        private readonly BindingInstanceFactory _bindingInstanceFactory;
        private readonly IDictionary<Type, object> _resolvedDependencies;
        private readonly IDictionary<Type, BindInfo> _instanceBinds;

        public DependencyInjector(BindInfoCollection bindInfoCollection)
        {
            _resolvedDependencies = new Dictionary<Type, object>();
            _instanceBinds = new Dictionary<Type, BindInfo>();
            _bindInfoCollection = bindInfoCollection;
            _bindingInstanceFactory = new BindingInstanceFactory(this);
        }

        public void PerformInjection()
        {
            while (_bindInfoCollection.TryWithdrawNext(out BindInfo bindInfo))
            {
                ResolveBinding(bindInfo);
            }

            InvokeCompletionHandlers();
        }

        public void InjectDependencies(object instance)
        {
            IEnumerable<FieldInfo> dependencies = instance.GetType().GetFieldsWithAttribute<DependencyAttribute>();

            foreach (FieldInfo fieldInfo in dependencies)
            {
                Type fieldType = fieldInfo.FieldType;
                object injectionInstance = ResolveType(fieldType);

                fieldInfo.SetValue(instance, injectionInstance);
            }
        }

        public bool TryGetResolvedInstance(Type type, out object instance)
        {
            return _resolvedDependencies.TryGetValue(type, out instance);
        }

        public object ResolveType<T>() where T : Type
        {
            return ResolveType(typeof(T));
        }

        public object ResolveType(Type type)
        {
            object instance;

            if (TryGetResolvedInstance(type, out object resolvedInstance))
            {
                instance = resolvedInstance;
            }
            else if (TryCreateNewInstanceType(type, out object newInstance))
            {
                instance = newInstance;
            }
            else if (_bindInfoCollection.TryRemoveBinding(type, out BindInfo bindInfo))
            {
                ResolveBinding(bindInfo);

                instance = GetResolvedInstance(type);
            }
            else
            {
                AddConcreteBindInfo(type);
                instance = ResolveType(type);
            }

            return instance;
        }

        public void SetDependencyInstance(Type sourceType, object instance)
        {
            _resolvedDependencies.Add(sourceType, instance);
        }

        private void ResolveBinding(BindInfo bindInfo)
        {
            if (BindInfoCollection.IsInstanceBind(bindInfo))
            {
                _instanceBinds.Add(bindInfo.SourceType, bindInfo);
                return;
            }

            CreateAndInjectObject(bindInfo);
        }

        private void AddConcreteBindInfo(Type concreteType)
        {
            _bindInfoCollection.AssignSelf(concreteType);
        }

        private object GetResolvedInstance(Type type)
        {
            return _resolvedDependencies[type];
        }

        private bool TryCreateNewInstanceType(Type type, out object newInstance)
        {
            if (HasInstanceBinding(type))
            {
                newInstance = CreateAndInjectObject(_instanceBinds[type]);
                return true;
            }

            if (_bindInfoCollection.TryRemoveInstanceBinding(type, out BindInfo instanceBindInfo))
            {
                _instanceBinds.Add(instanceBindInfo.SourceType, instanceBindInfo);

                newInstance = CreateAndInjectObject(_instanceBinds[type]);
                return true;
            }

            newInstance = null;
            return false;
        }

        private bool HasInstanceBinding(Type type)
        {
            return _instanceBinds.ContainsKey(type);
        }

        private object CreateAndInjectObject(BindInfo bindInfo)
        {
            object instance = _bindingInstanceFactory.CreateInstance(bindInfo.TargetType);

            if (!BindInfoCollection.IsInstanceBind(bindInfo))
            {
                SetDependencyInstance(bindInfo.SourceType, instance);
            }

            InjectDependencies(instance);

            return instance;
        }

        private void InvokeCompletionHandlers()
        {
            foreach (object instance in _resolvedDependencies.Values)
            {
                if (instance is IInjectionCompleteHandler handler)
                {
                    handler.OnInjectionComplete();
                }
            }
        }
    }
}