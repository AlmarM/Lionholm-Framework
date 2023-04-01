namespace Lionholm.DI
{
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