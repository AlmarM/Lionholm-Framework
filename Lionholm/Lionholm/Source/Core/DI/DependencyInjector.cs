using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lionholm.Core.Utils;

namespace Lionholm.Core.DI
{
    public class DependencyInjector
    {
        private readonly IDictionary<TypeConstraint, object> _resolvedDependencies;

        public DependencyInjector()
        {
            _resolvedDependencies = new Dictionary<TypeConstraint, object>();
        }

        public void PerformInjection(BindInfoCollection bindInfoCollection)
        {
            while (bindInfoCollection.TryWithdraw(out BindInfo bindInfo))
            {
                ResolveBinding(bindInfo);
            }
        }

        public void InjectDependencies(object instance)
        {
            IEnumerable<FieldInfo> dependencies = instance.GetType().GetFieldsWithAttribute<DependencyAttribute>();

            foreach (FieldInfo fieldInfo in dependencies)
            {
                if (TryGetResolvedInstance(fieldInfo.FieldType, out object resolvedInstance))
                {
                    fieldInfo.SetValue(instance, resolvedInstance);
                }

                // how to remove from lazy binds
            }
        }

        public bool TryGetResolvedInstance(Type type, out object instance)
        {
            instance = _resolvedDependencies.Values.FirstOrDefault(o => o.GetType() == type);
            return instance != null;
        }

        public void ResolveBinding(BindInfo bindInfo)
        {
            if (!TryFindConstructorDependency(bindInfo.TargetType, out ConstructorInfo constructorInfo))
            {
                throw new Exception();
            }

            object instance = CreateInstanceViaConstructor(constructorInfo, bindInfo);

            SetDependencyInstance(bindInfo.TypeConstraint, instance);
            InjectDependencies(instance);
        }

        private object CreateInstanceViaConstructor(ConstructorInfo constructorInfo, BindInfo bindInfo)
        {
            // TryFindConstructorDependency returns true with a null value when no constructor exists
            if (constructorInfo == null)
            {
                return CreateConcreteInstance(bindInfo.TargetType);
            }

            // fill constructor with values
            return null;
        }

        private void SetDependencyInstance(TypeConstraint typeConstraint, object instance)
        {
            _resolvedDependencies.Add(typeConstraint, instance);
        }

        private static bool TryFindConstructorDependency(Type type, out ConstructorInfo constructorInfo)
        {
            constructorInfo = null;

            ConstructorInfo[] constructorInfos = type.GetConstructors();
            switch (constructorInfos.Length)
            {
                case 0:
                    return true;
                case 1:
                    constructorInfo = constructorInfos[0];
                    return true;
            }

            ConstructorInfo markedConstructor = constructorInfos
                .FirstOrDefault(ci => ci.GetCustomAttribute<ConstructorDependencyAttribute>() != null);

            if (markedConstructor == null)
            {
                return false;
            }

            constructorInfo = markedConstructor;
            return true;
        }

        private static object CreateConcreteInstance(Type type)
        {
            return Activator.CreateInstance(type);
        }
    }
}