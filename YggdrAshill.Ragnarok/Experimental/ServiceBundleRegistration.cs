using YggdrAshill.Ragnarok.Construction;
using YggdrAshill.Ragnarok.Hierarchization;
using YggdrAshill.Ragnarok.Materialization;
using System;
using System.Linq;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ServiceBundleRegistration :
        IRegistration
    {
        public static bool TryGetTargetType(Type type, out Type targetType)
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
        private readonly CollectionRegistration collection;

        public Type ImplementedType { get; }
        public Lifetime Lifetime => Lifetime.Local;
        public Ownership Ownership => Ownership.Internal;

        public ServiceBundleRegistration(Type implementedType, IActivation activation, CollectionRegistration collection)
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
