using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ServiceBundleDepiction : IDepiction
    {
        public static bool TryToGetTargetType(Type type, out Type targetType)
        {
            targetType = default!;

            if (!TryGetElementType(type, out var elementType))
            {
                return false;
            }

            targetType = TypeCache.ReadOnlyListOf(elementType);

            return true;
        }

        public static bool TryGetElementType(Type type, out Type elementType)
        {
            elementType = default!;

            if (!type.IsConstructedGenericType)
            {
                return false;
            }

            var openGenericType = TypeCache.OpenGenericTypeOf(type);

            if (openGenericType != typeof(IServiceBundle<>))
            {
                return false;
            }

            elementType = TypeCache.GenericTypeParameterListOf(type)[0];

            return true;
        }

        private readonly IActivation activation;
        private readonly CollectionDepiction collection;

        public Type ImplementedType { get; }
        public Lifetime Lifetime => Lifetime.Local;
        public Ownership Ownership => Ownership.Internal;

        public ServiceBundleDepiction(Type implementedType, IActivation activation, CollectionDepiction collection)
        {
            ImplementedType = implementedType;

            this.activation = activation;
            this.collection = collection;
        }

        public object Instantiate(IScopedResolverV2 resolver)
        {
            var depictionList = collection.CollectDepictionList(resolver, true);

            var instance = collection.Instantiate(resolver, depictionList);

            return activation.Activate(new []{ instance });
        }
    }
}
