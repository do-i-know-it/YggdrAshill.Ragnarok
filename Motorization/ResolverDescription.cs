using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ResolverDescription :
        IDescription, IInstantiation
    {
        public static ResolverDescription Instance { get; } = new ResolverDescription();

        private ResolverDescription()
        {

        }

        public Type ImplementedType { get; } = typeof(IResolver);
        public IReadOnlyList<Type> AssignedTypeList { get; } = Array.Empty<Type>();
        public Lifetime Lifetime => Lifetime.Temporal;
        public Ownership Ownership => Ownership.External;
        public IInstantiation Instantiation => this;

        public object Instantiate(IResolver resolver)
        {
            return resolver;
        }
    }
}
