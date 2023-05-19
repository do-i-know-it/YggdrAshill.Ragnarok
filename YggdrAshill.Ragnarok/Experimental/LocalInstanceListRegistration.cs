using YggdrAshill.Ragnarok.Construction;
using YggdrAshill.Ragnarok.Hierarchization;
using YggdrAshill.Ragnarok.Materialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace YggdrAshill.Ragnarok
{
    internal sealed class LocalInstanceListRegistration :
        IRegistration
    {
        public static bool TryGetReadOnlyListType(Type type, out Type elementType, out Type readOnlyListType)
        {
            readOnlyListType = default!;
            elementType = default!;

            if (!type.IsConstructedGenericType)
            {
                return false;
            }

            var openGenericType = type.GetGenericTypeDefinition();

            if (openGenericType == typeof(ILocalInstanceList<>))
            {
                // TODO: cache type data.
                elementType = type.GetGenericArguments()[0];
                readOnlyListType = typeof(IReadOnlyList<>).MakeGenericType(elementType);

                return true;
            }

            return false;
        }

        private readonly IActivation activation;
        private readonly CollectionRegistration collection;

        public Type ImplementedType { get; }
        public Lifetime Lifetime => Lifetime.Local;
        public Ownership Ownership => Ownership.Internal;

        public LocalInstanceListRegistration(Type implementedType, IActivation activation, CollectionRegistration collection)
        {
            ImplementedType = implementedType;

            this.activation = activation;
            this.collection = collection;
        }

        public object Instantiate(IScopedResolver resolver)
        {
            var registrationList = collection.CollectAllRegistration(resolver, true);

            var instance = collection.Instantiate(resolver, registrationList.ToArray());

            return activation.Activate(new []{ instance });
        }
    }
}
