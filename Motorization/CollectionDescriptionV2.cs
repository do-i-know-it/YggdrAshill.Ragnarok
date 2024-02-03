using System;
using System.Collections.Generic;
using System.Linq;

namespace YggdrAshill.Ragnarok
{
    internal sealed class CollectionDescriptionV2 : IDescription
    {
        public static IEnumerable<Type> AssignedTypeListOf(Type elementType)
        {
            return new[]
            {
                TypeCache.ArrayTypeOf(elementType),
                TypeCache.EnumerableOf(elementType),
                TypeCache.ReadOnlyCollectionOf(elementType),
                TypeCache.ReadOnlyListOf(elementType),
            };
        }

        public static bool CanResolve(Type type, out Type elementType)
        {
            elementType = default!;

            if (type.IsArray)
            {
                elementType = type.GetElementType()!;

                return true;
            }

            if (!type.IsConstructedGenericType)
            {
                return false;
            }

            var openGenericType = TypeCache.OpenGenericTypeOf(type);

            var isCollectionType
                = openGenericType == TypeCache.OpenGenericEnumerable ||
                  openGenericType == TypeCache.OpenGenericReadOnlyCollection ||
                  openGenericType == TypeCache.OpenGenericReadOnlyList;

            if (!isCollectionType)
            {
                return false;
            }

            elementType = TypeCache.GenericTypeParameterListOf(type)[0];

            return true;
        }

        private readonly IActivationV2 activation;
        private readonly IDescription[] descriptionList;

        public Type ImplementedType { get; }
        public Lifetime Lifetime => Lifetime.Temporal;
        public Ownership Ownership => Ownership.External;

        public CollectionDescriptionV2(Type elementType, IActivationV2 activation, IDescription[] descriptionList)
        {
            this.activation = activation;
            this.descriptionList = descriptionList;

            ImplementedType = TypeCache.ArrayTypeOf(elementType);
        }

        internal IEnumerable<IDescription> Collect(IScopedResolver resolver, bool localOnly)
        {
            // TODO: object pooling.
            var buffer = new List<IDescription>();

            while (true)
            {
                if (resolver.CanResolve(ImplementedType, out var description))
                {
                    buffer.Add(description);
                }

                if (!resolver.CanEscalate(out resolver))
                {
                    break;
                }
            }

            var totalList = buffer.Where(candidate => candidate is CollectionDescription)
                .SelectMany(candidate => ((CollectionDescriptionV2)candidate).descriptionList);

            if (!localOnly)
            {
                return totalList;
            }

            return totalList.Where(candidate => candidate.Lifetime != Lifetime.Global)
                .Union(descriptionList);
        }

        internal object Instantiate(IScopedResolver resolver, IEnumerable<IDescription> totalList)
        {
            // TODO: object pooling.
            var parameterList = totalList.Select(resolver.Resolve).ToArray();

            return activation.Activate(parameterList);
        }

        public object Instantiate(IScopedResolver resolver)
        {
            var totalList = Collect(resolver, false);

            return Instantiate(resolver, totalList);
        }
    }
}
