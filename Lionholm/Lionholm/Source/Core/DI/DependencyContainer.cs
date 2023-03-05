using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lionholm.Core.Utils;

namespace Lionholm.Core.DI
{
    /// <summary>
    /// TODO
    /// - Add support for constructor dependencies
    /// - Check for circular constructor dependencies
    /// </summary>
    public class DependencyContainer
    {
        private readonly List<BindInfo> _unresolvedBinds;
        private readonly IDictionary<Type, object> _dependencies;
        private readonly IList<BindInfo> _lazyBinds;
        private readonly IList<BindInfo> _newInstanceBinds;

        public DependencyContainer()
        {
            _unresolvedBinds = new List<BindInfo>();
            _dependencies = new Dictionary<Type, object>();
            _lazyBinds = new List<BindInfo>();
            _newInstanceBinds = new List<BindInfo>();

            AddDefaultDependencies();
        }

        public void InjectUnresolvedBinds()
        {
            ProcessUnresolvedBindings();
            InvokeCompleteHandlers();
        }

        public void Inject(object instance)
        {
            IEnumerable<FieldInfo> dependencies = ReflectionUtils.GetFieldsWithAttribute<DependencyAttribute>(instance.GetType());

            foreach (FieldInfo dependency in dependencies)
            {
                object injectionValue;

                if (IsResolved(dependency.FieldType))
                {
                    injectionValue = _dependencies[dependency.FieldType];
                }
                else if (IsInstanceBind(dependency.FieldType, out BindInfo instanceBind))
                {
                    injectionValue = CreateInstance(instanceBind.TargetType);
                }
                else if (HasBindingFor(dependency.FieldType, out BindInfo bindInfo))
                {
                    ResolveBinding(bindInfo);

                    injectionValue = _dependencies[dependency.FieldType];
                }
                else
                {
                    throw new Exception();
                }

                SetDependency(dependency, instance, injectionValue);
            }
        }

        private void ProcessUnresolvedBindings()
        {
            while (_unresolvedBinds.Count > 0)
            {
                BindInfo info = _unresolvedBinds[0];

                ResolveBinding(info);
            }
        }

        private void InvokeCompleteHandlers()
        {
            foreach (object value in _dependencies.Values)
            {
                if (value is IBindsResolvedHandler handler)
                {
                    handler.OnBindsResolved();
                }
            }
        }

        private void ResolveBinding(BindInfo info)
        {
            if (!ReflectionUtils.DerivesFrom(info.TargetType, info.SourceType))
            {
                throw new Exception();
            }

            _unresolvedBinds.Remove(info);

            if (info.IsLazy)
            {
                _lazyBinds.Add(info);

                return;
            }

            if (info.InstanceType == InstanceType.NewInstance)
            {
                _newInstanceBinds.Add(info);

                return;
            }

            object instance = CreateInstance(info.TargetType);
            _dependencies.Add(info.SourceType, instance);

            Inject(instance);
        }

        private bool IsResolved(Type type)
        {
            return _dependencies.ContainsKey(type);
        }

        private bool HasBindingFor(Type sourceType, out BindInfo bindInfo)
        {
            bindInfo = _unresolvedBinds.FirstOrDefault(bi => bi.SourceType == sourceType);
            bindInfo ??= _lazyBinds.FirstOrDefault(bi => bi.SourceType == sourceType);

            return bindInfo != null;
        }

        private bool IsInstanceBind(Type type, out BindInfo instanceBind)
        {
            instanceBind = _newInstanceBinds.FirstOrDefault(bi => bi.SourceType == type);
            return instanceBind != null;
        }

        private void AddDefaultDependencies()
        {
            _dependencies.Add(typeof(DependencyContainer), this);
        }

        #region Binds

        public BindInfo Bind(Type type)
        {
            return StartNewBind(type);
        }

        public BindInfo Bind<T>()
        {
            return Bind(typeof(T));
        }

        public BindInfo BindSelf(Type type)
        {
            return Bind(type).To(type);
        }

        public BindInfo BindSelf<T>()
        {
            return Bind<T>().To<T>();
        }

        private BindInfo StartNewBind(Type sourceType)
        {
            if (HasBindingFor(sourceType, out _))
            {
                throw new Exception();
            }

            var info = new BindInfo(sourceType);

            _unresolvedBinds.Add(info);

            return info;
        }

        #endregion

        private static object CreateInstance(Type type)
        {
            return Activator.CreateInstance(type);
        }

        private static void SetDependency(FieldInfo fieldInfo, object instance, object value)
        {
            fieldInfo.SetValue(instance, value);
        }
    }
}