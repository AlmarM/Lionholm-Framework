namespace Lionholm.Core.DI
{
    public abstract class Binder<T> : IBinder
        where T : IBinder, new()
    {
        public DependencyContainer Container { get; set; }

        public abstract void Bind();

        public static void Bind(DependencyContainer container)
        {
            var t = new T
            {
                Container = container
            };

            t.Bind();
        }
    }
}