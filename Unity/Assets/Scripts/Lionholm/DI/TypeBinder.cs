namespace Lionholm.DI
{
    /// <summary>
    /// A type binder is a class that will create a relationship between concrete types and their (potential)
    /// generic form. Binds define how instances will be created by the dependency injection system.
    /// </summary>
    /// <typeparam name="T">Type of the derived class type.</typeparam>
    public abstract class TypeBinder<T> : ITypeBinder
        where T : TypeBinder<T>, new()
    {
        protected BindInfoCollection Binds { get; set; }

        public abstract void Bind();

        public static void Bind(BindInfoCollection bindInfoCollection)
        {
            new T
            {
                Binds = bindInfoCollection
            }.Bind();
        }
    }
}