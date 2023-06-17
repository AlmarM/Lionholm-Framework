namespace Lionholm.Core
{
    /// <summary>
    /// Provides an iterator that will loop through a list and remove the next entry each move.
    /// Inserting and removing entries from the collection is allowed. Entries will be removed in FILO order.
    /// </summary>
    /// <typeparam name="T">Type of the instances in the list.</typeparam>
    public interface IWithdrawIterator<T>
    {
        /// <summary>
        /// Withdraws the next entry from the list if possible.
        /// </summary>
        /// <param name="entry">Successfully withdrawn entry.</param>
        /// <returns>true if there is an entry available, otherwise false.</returns>
        bool TryWithdrawNext(out T entry);
    }
}