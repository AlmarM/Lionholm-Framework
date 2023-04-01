using System;

namespace Lionholm.DI
{
    public class BindInfo
    {
        public Type SourceType { get; private set; }

        public Type TargetType { get; }

        public InstanceType InstanceType { get; private set; }

        public BindInfo(Type targetType)
        {
            TargetType = targetType;
        }

        public BindInfo To<T>() => To(typeof(T));

        public BindInfo To(Type sourceType)
        {
            SourceType = sourceType;

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
    }
}