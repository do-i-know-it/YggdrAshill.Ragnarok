using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Fabrication
{
    // TODO: add document comments.
    public sealed class InstanceDependencyInjectionStatement : IStatement
    {
        private readonly InstanceInjection injection;
        private readonly IInstantiation instantiation;

        public Lifetime Lifetime { get; }
        public Ownership Ownership { get; }

        public InstanceDependencyInjectionStatement(
            ICompilation compilation,
            Type implementedType, Lifetime lifetime, Ownership ownership, IInstantiation instantiation)
        {
            injection = new InstanceInjection(compilation, implementedType);
            Lifetime = lifetime;
            Ownership = ownership;
            this.instantiation = instantiation;
        }

        public IInstanceInjection Injection => injection;
        public Type ImplementedType => injection.ImplementedType;
        public IReadOnlyList<Type> AssignedTypeList => injection.AssignedTypeList;
        public IInstantiation Instantiation
        {
            get
            {
                var candidate = injection.GetInjection();

                if (candidate == null)
                {
                    return instantiation;
                }

                return new InstantiateWithInjection(instantiation, candidate);
            }
        }
    }
}
