using System;

namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class DisposableOutput : IDisposable
    {
        public object Instance { get; }

        [Inject]
        public DisposableOutput(object instance)
        {
            Instance = instance;
        }

        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            IsDisposed = true;
        }
    }
}
