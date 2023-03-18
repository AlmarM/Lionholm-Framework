using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Lionholm.Core.DI
{
    public class BindingInstanceFactory
    {
        private readonly DependencyInjector _dependencyInjector;

        public BindingInstanceFactory(DependencyInjector dependencyInjector)
        {
            _dependencyInjector = dependencyInjector;
        }

        public object CreateInstance(Type targetType)
        {
            return CreateInstanceRecursively(targetType, new List<Type>());
        }

        private object CreateInstanceRecursively(Type targetType, IList<Type> constructorTypes)
        {
            if (!TryFindConstructorDependency(targetType, out ConstructorInfo constructorInfo))
            {
                throw new Exception();
            }

            constructorTypes.Add(targetType);

            return CreateInstanceViaConstructor(constructorInfo, targetType, constructorTypes);
        }

        private object CreateInstanceViaConstructor(ConstructorInfo constructorInfo,
            Type targetType,
            IList<Type> constructorTypes)
        {
            // TryFindConstructorDependency returns true with a null value when no constructor exists
            if (constructorInfo == null)
            {
                return CreateConcreteInstance(targetType);
            }

            var parameterObjects = new List<object>();

            foreach (ParameterInfo parameterInfo in constructorInfo.GetParameters())
            {
                Type parameterType = parameterInfo.ParameterType;
                if (constructorTypes.Contains(parameterType))
                {
                    throw new Exception();
                }

                object parameterInstance = _dependencyInjector.ResolveType(parameterType);
                parameterObjects.Add(parameterInstance);
            }

            return CreateConcreteInstance(targetType, parameterObjects.ToArray());
        }

        private bool TryFindConstructorDependency(Type type, out ConstructorInfo constructorInfo)
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

        private object CreateConcreteInstance(Type type)
        {
            return Activator.CreateInstance(type);
        }

        private object CreateConcreteInstance(Type type, object[] args)
        {
            return Activator.CreateInstance(type, args);
        }
    }
}