namespace Lionholm.Core
{
    public interface IWithdrawIterator<T>
    {
        bool TryWithdrawNext(out T entry);
    }
}