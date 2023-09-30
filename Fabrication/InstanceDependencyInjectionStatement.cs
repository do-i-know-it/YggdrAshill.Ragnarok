using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Fabrication
{
    // TODO: add document comments.
    public sealed class InstanceDependencyInjectionStatement : IStatement
    {
        private readonly InstanceDependencyInjection injection;
        private readonly IInstantiation instantiation;

        public Lifetime Lifetime { get; }
        public Ownership Ownership { get; }

        public InstanceDependencyInjectionStatement(
            ICompilation compilation,
            Type implementedType, Lifetime lifetime, Ownership ownership, IInstantiation instantiation)
        {
            injection = new InstanceDependencyInjection(compilation, implementedType);
            Lifetime = lifetime;
            Ownership = ownership;
            this.instantiation = instantiation;
        }

        public IInstanceDependencyInjection Injection => injection;
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
