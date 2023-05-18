using YggdrAshill.Ragnarok.Hierarchization;
using YggdrAshill.Ragnarok.Motorization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Materialization
{
    internal sealed class Registry :
        IRegistry
    {
        private readonly ICodeBuilder codeBuilder;

        private readonly Dictionary<Type, IRegistration?> dictionary;

        private readonly ConcurrentDictionary<Type, IRegistration> registrationCache
            = new ConcurrentDictionary<Type, IRegistration>();

        public Registry(ICodeBuilder codeBuilder, IDictionary<Type, IRegistration?> dictionary)
        {
            this.codeBuilder = codeBuilder;
            this.dictionary = new Dictionary<Type, IRegistration?>(dictionary);
        }

        private bool isDisposed;

        public bool TryGet(Type type, out IRegistration? registration)
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(IEngine));
            }

            if (dictionary.TryGetValue(type, out registration))
            {
                return registration != null;
            }

            return TryGetSingleElementCollection(type, out registration);
        }

        private bool TryGetSingleElementCollection(Type type, out IRegistration? registration)
        {
            registration = default;

            if (!CollectionRegistration.TryGetElementType(type, out var elementType))
            {
                return false;
            }

            if (!TryGet(elementType, out var elementRegistration))
            {
                return false;
            }

            registration = registrationCache.GetOrAdd(elementType, _ =>
            {
                var implementedType = CollectionRegistration.GetImplementedType(elementType);
                var activation = codeBuilder.GetActivation(implementedType);

                return new CollectionRegistration(elementType, activation, new[] { elementRegistration! });
            });

            return true;
        }

        public void Dispose()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(IEngine));
            }

            registrationCache.Clear();

            dictionary.Clear();

            isDisposed = false;
        }
    }
}
