using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lionholm.Core.Utils;

namespace Lionholm.DI
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

        public T GetResolvedInstance<T>() => (T)GetResolvedInstance(typeof(T));

        public object GetResolvedInstance(Type type)
        {
            return _resolvedDependencies.Values.First(o => o.GetType() == type);
        }

        public bool TryGetResolvedInstance<T>(out T instance)
        {
            if (TryGetResolvedInstance(typeof(T), out object resolvedInstance))
            {
                instance = (T)resolvedInstance;
                return true;
            }

            instance = default;
            return false;
        }

        public object ResolveType(Type type)
        {
            if (TryGetResolvedInstance(type, out object instance) ||
                TryCreateNewInstanceType(type, out instance))
            {
                return instance;
            }

            if (!_bindInfoCollection.TryRemoveBinding(type, out BindInfo bindInfo))
            {
                throw new Exception();
            }

            ResolveBinding(bindInfo);

            return GetResolvedInstanceFromSource(type);
        }

        public void SetDependencyInstance(Type sourceType, object instance)
        {
            _resolvedDependencies.Add(sourceType, instance);
        }

        private object GetResolvedInstanceFromSource(Type sourceType)
        {
            return _resolvedDependencies[sourceType];
        }

        private bool TryGetResolvedInstance(Type type, out object instance)
        {
            if (_resolvedDependencies.TryGetValue(type, out instance))
            {
                return true;
            }

            instance = _resolvedDependencies.Values.FirstOrDefault(o => o.GetType() == type);
            return instance != null;
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

        private bool TryCreateNewInstanceType(Type type, out object newInstance)
        {
            if (TryGetInstanceBinding(type, out BindInfo existingBindInfo))
            {
                newInstance = CreateAndInjectObject(existingBindInfo);
                return true;
            }

            if (_bindInfoCollection.TryRemoveInstanceBinding(type, out BindInfo instanceBindInfo))
            {
                _instanceBinds.Add(instanceBindInfo.SourceType, instanceBindInfo);

                newInstance = CreateAndInjectObject(instanceBindInfo);
                return true;
            }

            newInstance = null;
            return false;
        }

        private bool TryGetInstanceBinding(Type type, out BindInfo instanceBindInfo)
        {
            foreach (KeyValuePair<Type, BindInfo> instanceBind in _instanceBinds)
            {
                BindInfo bindInfo = instanceBind.Value;
                if (bindInfo.SourceType != type && bindInfo.TargetType != type)
                {
                    continue;
                }

                instanceBindInfo = bindInfo;
                return true;
            }

            instanceBindInfo = default;
            return false;
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