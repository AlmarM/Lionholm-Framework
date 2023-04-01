using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lionholm.Core;

namespace Lionholm.DI
{
    public class BindingInstanceFactory
    {
        private readonly DependencyInjector _dependencyInjector;
        private readonly IList<Type> _currentConstructorTypes;

        public BindingInstanceFactory(DependencyInjector dependencyInjector)
        {
            _dependencyInjector = dependencyInjector;

            _currentConstructorTypes = new List<Type>();
        }

        public object CreateInstance(Type targetType)
        {
            object instance = CreateInstanceImpl(targetType);

            _currentConstructorTypes.Clear();

            return instance;
        }

        private object CreateInstanceImpl(Type targetType)
        {
            if (!TryFindConstructorDependency(targetType, out ConstructorInfo constructorInfo))
            {
                throw new Exception();
            }

            _currentConstructorTypes.Add(targetType);

            return CreateInstanceViaConstructor(constructorInfo, targetType);
        }

        private object CreateInstanceViaConstructor(ConstructorInfo constructorInfo, Type targetType)
        {
            if (constructorInfo == null)
            {
                return CreateConcreteInstance(targetType);
            }

            var parameterObjects = new List<object>();

            foreach (ParameterInfo parameterInfo in constructorInfo.GetParameters())
            {
                Type parameterType = parameterInfo.ParameterType;
                if (_currentConstructorTypes.Contains(parameterType))
                {
                    _currentConstructorTypes.Clear();

                    throw new CircularDependencyException("");
                }

                object parameterInstance = _dependencyInjector.ResolveType(parameterType);
                parameterObjects.Add(parameterInstance);
            }

            return CreateConcreteInstance(targetType, parameterObjects.ToArray());
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

            ConstructorInfo markedConstructor = constructorInfos.FirstOrDefault(HasConstructorDependency);
            if (markedConstructor == null)
            {
                return false;
            }

            constructorInfo = markedConstructor;
            return true;
        }

        private static bool HasConstructorDependency(ConstructorInfo constructorInfo)
        {
            return constructorInfo.GetCustomAttribute<ConstructorDependencyAttribute>() != null;
        }

        private static object CreateConcreteInstance(Type type)
        {
            return Activator.CreateInstance(type);
        }

        private static object CreateConcreteInstance(Type type, object[] args)
        {
            return Activator.CreateInstance(type, args);
        }
    }
}