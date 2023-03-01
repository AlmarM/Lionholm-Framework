namespace Lionholm.Core.DI
{
    public interface IBinder
    {
        DependencyContainer Container { get; set; }

        void Bind();
    }
}