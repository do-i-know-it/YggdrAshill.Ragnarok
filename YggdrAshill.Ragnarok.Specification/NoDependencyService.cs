using System;

namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class NoDependencyService : IService, IDisposable
    {
        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            IsDisposed = true;
        }
    }
}
