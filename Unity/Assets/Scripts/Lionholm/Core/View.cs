namespace Lionholm.Core
{
    public abstract class View<TModel> : IView
        where TModel : IModel
    {
        protected TModel Model { get; private set; }
    }
}