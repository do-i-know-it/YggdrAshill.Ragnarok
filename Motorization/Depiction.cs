using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class Depiction : IDepiction
    {
        private readonly IInstantiationV2 instantiation;
        public Type ImplementedType { get; }
        public Lifetime Lifetime { get; }
        public Ownership Ownership { get; }

        public Depiction(IDescriptionV2 description)
        {
            instantiation = description.Instantiation;

            ImplementedType = description.ImplementedType;
            Lifetime = description.Lifetime;
            Ownership = description.Ownership;
        }

        public object Instantiate(IScopedResolverV2 resolver)
        {
            return instantiation.Instantiate(resolver);
        }
    }
}
