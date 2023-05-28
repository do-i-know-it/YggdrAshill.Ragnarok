using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class Registration :
        IRegistration
    {
        private readonly IInstantiation instantiation;
        public Type ImplementedType { get; }
        public Lifetime Lifetime { get; }
        public Ownership Ownership { get; }

        public Registration(IDescription description)
        {
            instantiation = description.Instantiation;

            ImplementedType = description.ImplementedType;
            Lifetime = description.Lifetime;
            Ownership = description.Ownership;
        }

        public object Instantiate(IScopedResolver resolver)
        {
            return instantiation.Instantiate(resolver);
        }
    }
}
