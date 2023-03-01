using System;

namespace Lionholm.Core.DI
{
    public class BindInfo
    {
        public Type SourceType { get; private set; }

        public Type TargetType { get; private set; }

        public InstanceType InstanceType { get; private set; }

        public bool IsLazy { get; private set; }

        public BindInfo(Type type)
        {
            SourceType = type;
        }

        public BindInfo To(Type type)
        {
            TargetType = type;

            return this;
        }

        public BindInfo To<T>()
        {
            To(typeof(T));

            return this;
        }

        public BindInfo AsSingleton()
        {
            InstanceType = InstanceType.Singleton;

            return this;
        }

        public BindInfo AsNew()
        {
            InstanceType = InstanceType.NewInstance;

            return this;
        }

        public BindInfo Lazy()
        {
            IsLazy = true;

            return this;
        }
    }
}