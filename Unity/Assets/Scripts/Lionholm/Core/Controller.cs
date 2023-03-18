namespace Lionholm.Core
{
    public abstract class Controller<TModel, TView> : Disposable, IController
        where TModel : IModel
        where TView : IView
    {
        protected TModel Model { get; private set; }

        protected TView View { get; }
    }
}