namespace Lionholm.Core
{
    public interface IWithdrawIterator<T>
    {
        bool TryWithdraw(out T entry);
    }
}