using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ServiceBundleDescriptionV2 : IDescription
    {
        public static bool CanResolve(Type type, out Type elementType)
        {
            elementType = default!;

            if (!type.IsConstructedGenericType)
            {
                return false;
            }

            var openGenericType = TypeCache.OpenGenericTypeOf(type);

            if (openGenericType != TypeCache.OpenGenericServiceBundle)
            {
                return false;
            }

            elementType = TypeCache.GenericTypeParameterListOf(type)[0];

            return true;
        }

        private readonly IActivationV2 activation;
        private readonly CollectionDescriptionV2 collection;

        public Type ImplementedType { get; }
        public Lifetime Lifetime => Lifetime.Local;
        public Ownership Ownership => Ownership.Internal;

        public ServiceBundleDescriptionV2(Type implementedType, IActivationV2 activation, CollectionDescriptionV2 collection)
        {
            ImplementedType = implementedType;

            this.activation = activation;
            this.collection = collection;
        }

        public object Instantiate(IScopedResolver resolver)
        {
            var totalList = collection.Collect(resolver, true);

            var instance = collection.Instantiate(resolver, totalList);

            return activation.Activate(new []{ instance });
        }
    }
}
