using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class DependencyInjectionStatement : IStatement
    {
        private readonly DependencyInjectionSource source;
        private readonly Lazy<IInstantiation> instantiation;

        public Lifetime Lifetime { get; }

        public DependencyInjectionStatement(ICompilation compilation, Type implementedType, Lifetime lifetime)
        {
            source = new DependencyInjectionSource(implementedType, compilation);
            instantiation = new Lazy<IInstantiation>(() => source.CreateInstantiation());
            Lifetime = lifetime;
        }

        public IDependencyInjection DependencyInjection => source;

        public Type ImplementedType => source.ImplementedType;

        public IReadOnlyList<Type> AssignedTypeList => source.AssignedTypeList;

        public Ownership Ownership => Ownership.Internal;

        public IInstantiation Instantiation => instantiation.Value;
    }
}
