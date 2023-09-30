using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Fabrication
{
    // TODO: add document comments.
    public sealed class ConstructorDependencyInjectionStatement : IStatement
    {
        private readonly ConstructorDependencyInjection injection;

        public Lifetime Lifetime { get; }

        public ConstructorDependencyInjectionStatement(ICompilation compilation, Type implementedType, Lifetime lifetime)
        {
            injection = new ConstructorDependencyInjection(compilation, implementedType);
            Lifetime = lifetime;
        }

        public IConstructorDependencyInjection Injection => injection;
        public Type ImplementedType => injection.ImplementedType;
        public IReadOnlyList<Type> AssignedTypeList => injection.AssignedTypeList;
        public Ownership Ownership => Ownership.Internal;
        public IInstantiation Instantiation => injection.GetInstantiation();
    }
}
