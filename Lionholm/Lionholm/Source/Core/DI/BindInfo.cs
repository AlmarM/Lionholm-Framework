using System;

namespace Lionholm.Core.DI
{
    public class BindInfo
    {
        public TypeConstraint TypeConstraint { get; private set; }

        public Type TargetType { get; }

        public InstanceType InstanceType { get; private set; }

        public BindInfo(Type targetType)
        {
            TargetType = targetType;
        }

        public BindInfo To<T1, T2>() => To(typeof(T1), typeof(T2));

        public BindInfo To<T1, T2, T3>() => To(typeof(T1), typeof(T2), typeof(T3));

        public BindInfo To<T1, T2, T3, T4>() => To(typeof(T1), typeof(T2), typeof(T3), typeof(T4));

        public BindInfo To<T>() => To(typeof(T));

        public BindInfo To(Type sourceType) => To(new[] { sourceType });

        public BindInfo To(params Type[] sourceTypes)
        {
            TypeConstraint = TypeConstraint.From(sourceTypes);

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