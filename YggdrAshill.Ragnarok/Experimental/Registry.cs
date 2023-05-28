using YggdrAshill.Ragnarok.Hierarchization;
using YggdrAshill.Ragnarok.Motorization;
using YggdrAshill.Ragnarok.Materialization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Memorization
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

        public bool Have(IRegistration registration)
        {
            return dictionary.ContainsKey(registration.ImplementedType);
        }

        public bool Find(Type type, out IRegistration? registration)
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(IEngine));
            }

            if (dictionary.TryGetValue(type, out registration))
            {
                return registration != null;
            }

            return TryGetCollection(type, out registration) || TryGetServiceBundle(type, out registration);
        }

        private bool TryGetCollection(Type type, out IRegistration? registration)
        {
            registration = default;

            if (!CollectionRegistration.TryGetElementType(type, out var elementType))
            {
                return false;
            }

            var implementedType = CollectionRegistration.GetImplementedType(elementType);

            registration = registrationCache.GetOrAdd(implementedType, _ =>
            {
                var activation = codeBuilder.GetActivation(implementedType);

                if (!Find(elementType, out var elementRegistration))
                {
                    return new CollectionRegistration(elementType, activation, Array.Empty<IRegistration>());
                }

                return new CollectionRegistration(elementType, activation, new[] { elementRegistration! });
            });

            return true;
        }

        private bool TryGetServiceBundle(Type type, out IRegistration? registration)
        {
            registration = default;

            if (!ServiceBundleRegistration.TryGetTargetType(type, out var targetType))
            {
                return false;
            }

            if (Find(targetType, out var found) && found is CollectionRegistration collection)
            {
                registration = registrationCache.GetOrAdd(type, _ =>
                {
                    var activation = codeBuilder.GetActivation(type);

                    return new ServiceBundleRegistration(type, activation, collection);
                });

                return true;
            }

            return false;
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
