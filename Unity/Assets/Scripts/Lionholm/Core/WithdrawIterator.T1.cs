using System.Collections.Generic;

namespace Lionholm.Core
{
    /// <summary>
    /// Provides an iterator that will loop through a list and remove the next entry each move.
    /// Inserting and removing entries from the collection is allowed. Entries will be removed in FILO order.
    /// </summary>
    public abstract class WithdrawIterator<T> : IWithdrawIterator<T>
    {
        protected abstract IList<T> WithdrawalObjectList { get; }

        public bool TryWithdrawNext(out T entry)
        {
            if (WithdrawalObjectList == null || WithdrawalObjectList.Count == 0)
            {
                entry = default;
                return false;
            }

            var lastIndex = WithdrawalObjectList.Count - 1;

            entry = WithdrawalObjectList[lastIndex];

            WithdrawalObjectList.RemoveAt(lastIndex);

            return true;
        }
    }
}