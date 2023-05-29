using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class Engine :
        IEngine
    {
        private readonly EngineContext context;
        private readonly Dictionary<Type, IRegistration?> dictionary;

        public Engine(EngineContext context, IDictionary<Type, IRegistration?> dictionary)
        {
            this.context = context;
            this.dictionary = new Dictionary<Type, IRegistration?>(dictionary);
        }

        private readonly ConcurrentDictionary<Type, IRegistration> registrationCache
            = new ConcurrentDictionary<Type, IRegistration>();
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

            return dictionary.ContainsKey(registration.ImplementedType);
        }

        public bool Find(Type type, out IRegistration registration)
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(IEngine));
            }

            registration = default!;

            if (dictionary.TryGetValue(type, out var found) && found != null)
            {
                registration = found;

                return true;
            }

            return TryGetCollection(type, out registration) || TryGetServiceBundle(type, out registration);
        }

        private bool TryGetCollection(Type type, out IRegistration registration)
        {
            registration = default!;

            if (!CollectionRegistration.TryGetElementType(type, out var elementType))
            {
                return false;
            }

            var implementedType = CollectionRegistration.GetImplementedType(elementType);

            registration = registrationCache.GetOrAdd(implementedType, _ =>
            {
                var activation = context.GetActivation(implementedType);

                if (!Find(elementType, out var elementRegistration))
                {
                    return new CollectionRegistration(elementType, activation, Array.Empty<IRegistration>());
                }

                return new CollectionRegistration(elementType, activation, new[] { elementRegistration! });
            });

            return true;
        }

        private bool TryGetServiceBundle(Type type, out IRegistration registration)
        {
            registration = default!;

            if (!ServiceBundleRegistration.TryGetTargetType(type, out var targetType))
            {
                return false;
            }

            if (Find(targetType, out var found) && found is CollectionRegistration collection)
            {
                registration = registrationCache.GetOrAdd(type, _ =>
                {
                    var activation = context.GetActivation(type);

                    return new ServiceBundleRegistration(type, activation, collection);
                });

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

        public IEngineContext CreateContext()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(IEngine));
            }

            return new EngineContext(context);
        }

        public void Dispose()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(IEngine));
            }

            dictionary.Clear();

            registrationCache.Clear();

            instanceCache.Clear();

            compositeDisposable.Dispose();

            isDisposed = true;
        }
    }
}
