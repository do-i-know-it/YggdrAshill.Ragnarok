using YggdrAshill.Ragnarok.Hierarchization;
using YggdrAshill.Ragnarok.Motorization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Materialization
{
    internal sealed class Engine :
        IEngine
    {
        private readonly ICodeBuilder codeBuilder;
        private readonly Dictionary<Type, IRegistration?> dictionary;

        public Engine(ICodeBuilder codeBuilder, IDictionary<Type, IRegistration?> table)
        {
            this.codeBuilder = codeBuilder;
            dictionary = new Dictionary<Type, IRegistration?>(table);
        }

        private bool isDisposed;

        private readonly ConcurrentDictionary<IRegistration, object> instanceCache
            = new ConcurrentDictionary<IRegistration, object>();

        private readonly ConcurrentDictionary<Type, IRegistration> registrationCache
            = new ConcurrentDictionary<Type, IRegistration>();

        private readonly CompositeDisposable compositeDisposable = new CompositeDisposable();

        public bool Have(IRegistration registration)
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(IEngine));
            }

            return dictionary.ContainsValue(registration);
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

            return FindSingleElementCollection(type, out registration);
        }

        private bool FindSingleElementCollection(Type type, out IRegistration registration)
        {
            registration = default!;

            if (!CollectionRegistration.TryGetElementType(type, out var elementType))
            {
                return false;
            }

            if (!Find(elementType, out var elementRegistration))
            {
                return false;
            }

            registration = registrationCache.GetOrAdd(elementType, _ =>
            {
                var implementedType = CollectionRegistration.GetImplementedType(elementType);

                var activation = codeBuilder.CreateActivation(implementedType);

                return new CollectionRegistration(elementType, activation, new[] { elementRegistration });
            });

            return true;
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

            lock (dictionary)
            {
                dictionary.Clear();
            }

            instanceCache.Clear();

            registrationCache.Clear();

            compositeDisposable.Dispose();

            isDisposed = true;
        }
    }
}
