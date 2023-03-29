using YggdrAshill.Ragnarok.Construction;
using YggdrAshill.Ragnarok.Hierarchization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace YggdrAshill.Ragnarok.Materialization
{
    internal sealed class CollectionRegistration :
        IRegistration
    {
        private readonly ICollectionGeneration generation;
        private readonly IRegistration[] registrationList;
        private readonly Type finderType;

        public Type ImplementedType { get; }
        public Lifetime Lifetime => Lifetime.Temporal;
        public Ownership Ownership => Ownership.External;
        public IReadOnlyList<Type> AssignedTypeList { get; }

        public CollectionRegistration(ICollectionGeneration generation, IRegistration[] registrationList)
        {
            this.generation = generation;
            this.registrationList = registrationList;

            var elementType = generation.ElementType;

            ImplementedType = elementType.MakeArrayType();

            finderType = ImplementedType;

            // TODO: cache generated type information.
            AssignedTypeList = new List<Type>
            {
                ImplementedType,
                typeof(IEnumerable<>).MakeGenericType(elementType),
                typeof(IReadOnlyCollection<>).MakeGenericType(elementType),
                typeof(IReadOnlyList<>).MakeGenericType(elementType),
            };
        }

        public object Instantiate(IScopedResolver resolver)
        {
            var additionalList = resolver.ResolveAll(finderType);

            var totalList = registrationList.Concat(additionalList).ToArray();

            return generation.Create(resolver, totalList);
        }
    }
}
