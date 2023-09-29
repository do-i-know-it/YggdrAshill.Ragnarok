using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ObjectResolverDescription : IDescriptionV2, IInstantiationV2
    {
        public static ObjectResolverDescription Instance { get; } = new ObjectResolverDescription();

        private ObjectResolverDescription()
        {

        }

        public Type ImplementedType { get; } = typeof(IResolver);
        public IReadOnlyList<Type> AssignedTypeList { get; } = Array.Empty<Type>();
        public Lifetime Lifetime => Lifetime.Temporal;
        public Ownership Ownership => Ownership.External;
        public IInstantiationV2 Instantiation => this;

        public object Instantiate(IObjectResolver resolver)
        {
            return resolver;
        }
    }
}
