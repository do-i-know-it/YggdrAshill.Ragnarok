using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Fabrication
{
    // TODO: add document comments.
    public sealed class ConstructorDependencyInjectionDescription : IDescriptionV2
    {
        private readonly ConstructorDependencyInjection injection;

        public Lifetime Lifetime { get; }

        public ConstructorDependencyInjectionDescription(ICompilationV2 compilation, Type implementedType, Lifetime lifetime)
        {
            injection = new ConstructorDependencyInjection(compilation, implementedType);
            Lifetime = lifetime;
        }

        public IConstructorDependencyInjection Injection => injection;
        public Type ImplementedType => injection.ImplementedType;
        public IReadOnlyList<Type> AssignedTypeList => injection.AssignedTypeList;
        public Ownership Ownership => Ownership.Internal;
        public IInstantiationV2 Instantiation => injection.GetInstantiation();
    }
}
