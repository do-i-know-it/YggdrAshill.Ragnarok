using System;

namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class ObjectDependentDisposable : IDisposable
    {
        private readonly object instance;

        [Inject]
        public ObjectDependentDisposable(object instance)
        {
            this.instance = instance;
        }

        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            IsDisposed = true;
        }
    }
}
