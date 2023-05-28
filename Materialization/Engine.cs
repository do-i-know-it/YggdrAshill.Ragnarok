using System;
using System.Collections.Concurrent;

namespace YggdrAshill.Ragnarok
{
    internal sealed class Engine :
        IEngine
    {
        private readonly IRegistry registry;

        public Engine(IRegistry registry)
        {
            this.registry = registry;
        }

        private readonly ConcurrentDictionary<IRegistration, object> instanceCache
            = new ConcurrentDictionary<IRegistration, object>();

        private readonly CompositeDisposable compositeDisposable = new CompositeDisposable();

        private bool isDisposed;

        public bool Have(IRegistration registration)
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(IEngine));
            }

            return registry.Have(registration);
        }

        public bool Find(Type type, out IRegistration registration)
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(IEngine));
            }

            registration = default!;

            if (registry.Find(type, out var found) && found != null)
            {
                registration = found;

                return true;
            }

            return false;
        }

        public object GetInstance(IRegistration registration, Func<IRegistration, object> factory)
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(IEngine));
            }

            return instanceCache.GetOrAdd(registration, factory);
        }

        public void Bind(IDisposable disposable)
        {
            compositeDisposable.Add(disposable);
        }

        public void Dispose()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(IEngine));
            }

            registry.Dispose();

            instanceCache.Clear();

            compositeDisposable.Dispose();

            isDisposed = true;
        }
    }
}
