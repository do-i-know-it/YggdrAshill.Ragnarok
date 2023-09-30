using System;
using System.Collections.Generic;
using System.Linq;

namespace YggdrAshill.Ragnarok
{
    internal sealed class CollectionDescription : IDescription
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
        private readonly IDescription[] descriptionList;

        public Type ImplementedType { get; }
        public Lifetime Lifetime => Lifetime.Temporal;
        public Ownership Ownership => Ownership.External;
        public IReadOnlyList<Type> AssignedTypeList { get; }

        public CollectionDescription(Type elementType, IActivation activation, IDescription[] descriptionList)
        {
            this.activation = activation;
            this.descriptionList = descriptionList;

            ImplementedType = TypeCache.ArrayTypeOf(elementType);

            AssignedTypeList = new List<Type>
            {
                ImplementedType,
                TypeCache.EnumerableOf(elementType),
                TypeCache.ReadOnlyListOf(elementType),
                TypeCache.ReadOnlyCollectionOf(elementType),
            };
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
                .SelectMany(candidate => ((CollectionDescription)candidate).descriptionList);

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
