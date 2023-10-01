using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Fabrication
{
    // TODO: add document comments.
    public sealed class ConstructorInjectionStatement : IStatement
    {
        private readonly ConstructorInjection injection;

        public Lifetime Lifetime { get; }

        public ConstructorInjectionStatement(ICompilation compilation, Type implementedType, Lifetime lifetime)
        {
            injection = new ConstructorInjection(compilation, implementedType);
            Lifetime = lifetime;
        }

        public IConstructorInjection Injection => injection;
        public Type ImplementedType => injection.ImplementedType;
        public IReadOnlyList<Type> AssignedTypeList => injection.AssignedTypeList;
        public Ownership Ownership => Ownership.Internal;
        public IInstantiation Instantiation => injection.GetInstantiation();
    }
}
