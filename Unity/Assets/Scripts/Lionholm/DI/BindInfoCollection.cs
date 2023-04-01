using System;
using System.Collections.Generic;
using System.Linq;
using Lionholm.Core;

namespace Lionholm.DI
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

        public bool TryRemoveBinding(Type type, out BindInfo bindInfo) =>
            TryRemoveBinding(HasType, type, out bindInfo);

        public bool TryRemoveInstanceBinding(Type type, out BindInfo bindInfo) =>
            TryRemoveBinding(HasInstanceType, type, out bindInfo);

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

        private bool HasBinding(Type targetType)
        {
            return _bindInfos.Any(bi => bi.TargetType == targetType) ||
                   _lazyBindInfos.Any(bi => bi.TargetType == targetType);
        }

        private bool TryRemoveBinding(Func<BindInfo, Type, bool> predicate, Type type, out BindInfo bindInfo)
        {
            bindInfo = _bindInfos.FirstOrDefault(bi => predicate(bi, type));
            if (_bindInfos.Remove(bindInfo))
            {
                return true;
            }

            bindInfo = _lazyBindInfos.FirstOrDefault(bi => predicate(bi, type));
            return _lazyBindInfos.Remove(bindInfo);
        }

        public static bool IsInstanceBind(BindInfo info)
        {
            return info.InstanceType == InstanceType.NewInstance;
        }

        private static bool HasInstanceType(BindInfo bindInfo, Type type)
        {
            return IsInstanceBind(bindInfo) && HasType(bindInfo, type);
        }

        private static bool HasType(BindInfo bindInfo, Type type)
        {
            return bindInfo.SourceType == type || bindInfo.TargetType == type;
        }
    }
}