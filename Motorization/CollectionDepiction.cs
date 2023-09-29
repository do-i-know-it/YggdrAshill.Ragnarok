using System;
using System.Collections.Generic;
using System.Linq;

namespace YggdrAshill.Ragnarok
{
    internal sealed class CollectionDepiction : IDepiction
    {
        public static Type GetImplementedType(Type elementType)
        {
            return TypeCache.ArrayTypeOf(elementType);
        }

        public static bool TryToGetElementType(Type type, out Type elementType)
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
                = openGenericType == typeof(IEnumerable<>) ||
                  openGenericType == typeof(IReadOnlyList<>) ||
                  openGenericType == typeof(IReadOnlyCollection<>);

            if (isCollectionType)
            {
                elementType = TypeCache.GenericTypeParameterListOf(type)[0];

                return true;
            }

            return false;
        }

        private readonly IActivation activation;
        private readonly IDepiction[] depictionList;

        public Type ImplementedType { get; }
        public Lifetime Lifetime => Lifetime.Temporal;
        public Ownership Ownership => Ownership.External;
        public IReadOnlyList<Type> AssignedTypeList { get; }

        public CollectionDepiction(Type elementType, IActivation activation, IDepiction[] depictionList)
        {
            this.activation = activation;
            this.depictionList = depictionList;

            ImplementedType = TypeCache.ArrayTypeOf(elementType);

            AssignedTypeList = new List<Type>
            {
                ImplementedType,
                TypeCache.EnumerableOf(elementType),
                TypeCache.ReadOnlyListOf(elementType),
                TypeCache.ReadOnlyCollectionOf(elementType),
            };
        }

        internal IEnumerable<IDepiction> CollectDepictionList(IScopedResolverV2 resolver, bool localOnly)
        {
            // TODO: object pooling.
            var buffer = new List<IDepiction>();

            while (true)
            {
                if (resolver.CanResolve(ImplementedType, out var depiction))
                {
                    buffer.Add(depiction);
                }

                if (!resolver.CanEscalate(out resolver))
                {
                    break;
                }
            }

            var totalDepictionList = buffer.Where(candidate => candidate is CollectionDepiction)
                .SelectMany(candidate => ((CollectionDepiction)candidate).depictionList);

            if (!localOnly)
            {
                return totalDepictionList;
            }

            return totalDepictionList.Where(registration => registration.Lifetime != Lifetime.Global)
                .Union(depictionList);
        }

        internal object Instantiate(IScopedResolverV2 resolver, IEnumerable<IDepiction> totalDepictionList)
        {
            // TODO: object pooling.
            var parameterList = totalDepictionList.Select(resolver.Resolve).ToArray();

            return activation.Activate(parameterList);
        }

        public object Instantiate(IScopedResolverV2 resolver)
        {
            var totalDepictionList = CollectDepictionList(resolver, false);

            return Instantiate(resolver, totalDepictionList);
        }
    }
}
