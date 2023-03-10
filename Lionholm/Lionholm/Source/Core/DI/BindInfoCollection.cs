using System;
using System.Collections.Generic;
using System.Linq;
using Lionholm.Core.Utils;

namespace Lionholm.Core.DI
{
    public class BindInfoCollection : WithdrawIterator<BindInfo>
    {
        protected override IList<BindInfo> WithdrawalObjectList => _bindInfos;

        private readonly IList<BindInfo> _bindInfos;
        private readonly IList<BindInfo> _lazyBindInfos;

        public BindInfoCollection()
        {
            _bindInfos = new List<BindInfo>();
            _lazyBindInfos = new List<BindInfo>();
        }

        public bool HasBinding(Type targetType) => TryGetBinding(targetType, out BindInfo _);

        public bool TryGetBinding(Type targetType, out BindInfo bindInfo)
        {
            bindInfo = _bindInfos.FirstOrDefault(bi => bi.TargetType == targetType) ??
                       _lazyBindInfos.FirstOrDefault(bi => bi.TargetType == targetType);

            return bindInfo != null;
        }

        public BindInfo LazyAssignInterfacesAndSelf<T>() => LazyAssignInterfacesAndSelf(typeof(T));

        public BindInfo LazyAssignInterfacesAndSelf(Type type)
        {
            Type[] types = type.GetInterfacesAndSelf().ToArray();
            return Assign(type).To(types);
        }

        public BindInfo AssignInterfacesAndSelf<T>() => AssignInterfacesAndSelf(typeof(T));

        public BindInfo AssignInterfacesAndSelf(Type type)
        {
            Type[] types = type.GetInterfacesAndSelf().ToArray();
            return LazyAssign(type).To(types);
        }

        public BindInfo LazyAssignSelf(Type type) => LazyAssign(type).To(type);

        public BindInfo LazyAssignSelf<T>() => LazyAssign<T>().To<T>();

        public BindInfo AssignSelf(Type type) => Assign(type).To(type);

        public BindInfo AssignSelf<T>() => Assign<T>().To<T>();

        public BindInfo LazyAssign<T>() => LazyAssign(typeof(T));

        public BindInfo LazyAssign(Type targetType)
        {
            BindInfo info = CreateNewBinding(targetType);

            _lazyBindInfos.Add(info);

            return info;
        }

        public BindInfo Assign<T>() => Assign(typeof(T));

        public BindInfo Assign(Type targetType)
        {
            BindInfo info = CreateNewBinding(targetType);

            _bindInfos.Add(info);

            return info;
        }

        private BindInfo CreateNewBinding(Type targetType)
        {
            if (HasBinding(targetType))
            {
                throw new Exception();
            }

            return new BindInfo(targetType);
        }
    }
}