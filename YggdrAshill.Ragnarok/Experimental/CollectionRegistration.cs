using YggdrAshill.Ragnarok.Construction;
using YggdrAshill.Ragnarok.Hierarchization;
using YggdrAshill.Ragnarok.Materialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace YggdrAshill.Ragnarok
{
    internal sealed class CollectionRegistration :
        IRegistration
    {
        public static Type GetImplementedType(Type elementType)
        {
            return elementType.MakeArrayType();
        }

        public static bool TryGetElementType(Type type, out Type elementType)
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

            var openGenericType = type.GetGenericTypeDefinition();

            var isCollectionType
                = openGenericType == typeof(IEnumerable<>) ||
                  openGenericType == typeof(IReadOnlyList<>) ||
                  openGenericType == typeof(IReadOnlyCollection<>);

            if (isCollectionType)
            {
                // TODO: cache type data.
                elementType = type.GetGenericArguments()[0];

                return true;
            }

            return false;
        }

        private readonly IActivation activation;
        private readonly IRegistration[] registrationList;

        public Type ImplementedType { get; }
        public Lifetime Lifetime => Lifetime.Temporal;
        public Ownership Ownership => Ownership.External;
        public IReadOnlyList<Type> AssignedTypeList { get; }

        public CollectionRegistration(Type elementType, IActivation activation, IRegistration[] registrationList)
        {
            this.activation = activation;
            this.registrationList = registrationList;

            ImplementedType = elementType.MakeArrayType();

            // TODO: cache generated type information.
            AssignedTypeList = new List<Type>
            {
                ImplementedType,
                typeof(IEnumerable<>).MakeGenericType(elementType),
                typeof(IReadOnlyCollection<>).MakeGenericType(elementType),
                typeof(IReadOnlyList<>).MakeGenericType(elementType),
            };
        }

        internal IEnumerable<IRegistration> CollectAllRegistration(IScopedResolver resolver, bool localOnly)
        {
            var totalRegistrationList = resolver.ResolveAll(ImplementedType)
                .Where(candidate => candidate is CollectionRegistration)
                .SelectMany(registration => (registration as CollectionRegistration)!.registrationList);

            if (!localOnly)
            {
                return totalRegistrationList;
            }

            return totalRegistrationList.Where(registration => registration.Lifetime != Lifetime.Global)
                .Union(registrationList);
        }

        internal object Instantiate(IScopedResolver resolver, IReadOnlyList<IRegistration> totalRegistrationList)
        {
            // TODO: object pooling.
            var parameterList = totalRegistrationList
                .Select(resolver.Resolve)
                .ToArray();

            return activation.Activate(parameterList);
        }

        public object Instantiate(IScopedResolver resolver)
        {

            var totalRegistrationList = CollectAllRegistration(resolver, false);

            return Instantiate(resolver, totalRegistrationList.ToArray());
        }
    }
}
