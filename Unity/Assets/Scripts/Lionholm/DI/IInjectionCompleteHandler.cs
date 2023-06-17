namespace Lionholm.DI
{
    /// <summary>
    /// Provides a callback then the dependency injection system is done with resolving and injecting.
    /// </summary>
    public interface IInjectionCompleteHandler
    {
        /// <summary>
        /// Called when all BindInfos have been resolved.
        /// </summary>
        void OnInjectionComplete();
    }
}