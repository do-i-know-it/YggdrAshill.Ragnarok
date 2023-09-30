using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Fabrication
{
    // TODO: add document comments.
    public sealed class InstanceDependencyInjectionDescription : IDescriptionV2
    {
        private readonly InstanceDependencyInjection injection;
        private readonly IInstantiationV2 instantiation;

        public Lifetime Lifetime { get; }
        public Ownership Ownership { get; }

        public InstanceDependencyInjectionDescription(
            ICompilationV2 compilation,
            Type implementedType, Lifetime lifetime, Ownership ownership, IInstantiationV2 instantiation)
        {
            injection = new InstanceDependencyInjection(compilation, implementedType);
            Lifetime = lifetime;
            Ownership = ownership;
            this.instantiation = instantiation;
        }

        public IInstanceDependencyInjection Injection => injection;
        public Type ImplementedType => injection.ImplementedType;
        public IReadOnlyList<Type> AssignedTypeList => injection.AssignedTypeList;
        public IInstantiationV2 Instantiation
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
