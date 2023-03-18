using System;

namespace Lionholm.Core
{
    public abstract class Disposable : IDisposable
    {
        protected bool IsDisposed { get; private set; }

        public void Dispose()
        {
            OnDispose(true);

            GC.SuppressFinalize(this);
        }

        public virtual void OnDispose(bool disposing)
        {
            if (IsDisposed)
            {
                return;
            }

            if (disposing)
            {
                DisposeManaged();
            }

            DisposeUnmanaged();

            IsDisposed = true;
        }

        protected virtual void DisposeManaged()
        {
        }

        protected virtual void DisposeUnmanaged()
        {
        }

        ~Disposable() => OnDispose(false);
    }
}