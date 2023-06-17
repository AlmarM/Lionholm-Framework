namespace Lionholm.DI
{
    public interface ITypeBinder
    {
        /// <summary>
        /// Called by the dependency injection system to populate BindInfoCollection with BindInfos.
        /// </summary>
        void Bind();
    }
}