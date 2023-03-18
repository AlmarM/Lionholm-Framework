using System;
using System.Collections.Generic;
using System.Linq;

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

        public bool HasBinding(Type targetType)
        {
            return _bindInfos.Any(bi => bi.TargetType == targetType) ||
                   _lazyBindInfos.Any(bi => bi.TargetType == targetType);
        }

        public bool TryRemoveInstanceBinding(Type targetType, out BindInfo bindInfo)
        {
            bindInfo = _bindInfos.FirstOrDefault(bi => IsInstanceBind(bi) && bi.TargetType == targetType);
            if (_bindInfos.Remove(bindInfo))
            {
                return true;
            }

            bindInfo = _lazyBindInfos.FirstOrDefault(bi => IsInstanceBind(bi) && bi.TargetType == targetType);
            return _lazyBindInfos.Remove(bindInfo);
        }

        public bool TryRemoveBinding(Type targetType, out BindInfo bindInfo)
        {
            bindInfo = _bindInfos.FirstOrDefault(bi => bi.TargetType == targetType);
            if (_bindInfos.Remove(bindInfo))
            {
                return true;
            }

            bindInfo = _lazyBindInfos.FirstOrDefault(bi => bi.TargetType == targetType);
            return _lazyBindInfos.Remove(bindInfo);
        }

        public bool TryGetBinding(Type targetType, out BindInfo bindInfo)
        {
            bindInfo = _bindInfos.FirstOrDefault(bi => bi.TargetType == targetType) ??
                       _lazyBindInfos.FirstOrDefault(bi => bi.TargetType == targetType);

            return bindInfo != null;
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

        public static bool IsInstanceBind(BindInfo info)
        {
            return info.InstanceType == InstanceType.NewInstance;
        }
    }
}