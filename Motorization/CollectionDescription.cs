using System;
using System.Collections.Generic;
using System.Linq;

namespace YggdrAshill.Ragnarok
{
    internal sealed class CollectionDescription : IDescription
    {
        private readonly IActivation activation;
        private readonly IDescription[] descriptionList;

        public Type ImplementedType { get; }
        public Lifetime Lifetime => Lifetime.Temporal;
        public Ownership Ownership => Ownership.External;

        public CollectionDescription(Type implementedType, IActivation activation, IDescription[] descriptionList)
        {
            ImplementedType = implementedType;
            this.activation = activation;
            this.descriptionList = descriptionList;
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
