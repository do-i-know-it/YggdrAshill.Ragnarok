using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class Description : IDescription
    {
        private readonly IInstantiation instantiation;
        public Type ImplementedType { get; }
        public Lifetime Lifetime { get; }
        public Ownership Ownership { get; }

        public Description(IStatement statement)
        {
            instantiation = statement.Instantiation;
            ImplementedType = statement.ImplementedType;
            Lifetime = statement.Lifetime;
            Ownership = statement.Ownership;
        }

        public object Instantiate(IScopedResolver resolver)
        {
            return instantiation.Instantiate(resolver);
        }
    }
}
