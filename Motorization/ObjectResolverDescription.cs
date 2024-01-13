using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ObjectResolverDescription : IDescription
    {
        public static ObjectResolverDescription Instance { get; } = new();

        private ObjectResolverDescription()
        {

        }

        public Type ImplementedType { get; } = typeof(IObjectResolver);
        public Lifetime Lifetime => Lifetime.Temporal;
        public Ownership Ownership => Ownership.External;
        public object Instantiate(IScopedResolver resolver)
        {
            return resolver;
        }
    }
}
