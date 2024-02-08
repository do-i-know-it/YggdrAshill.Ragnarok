using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class DependencyInjectionStatement : IStatement
    {
        private readonly Lazy<IInstantiation> instantiation;

        public Lifetime Lifetime { get; }
        public DependencyInjectionSource Source { get; }

        public DependencyInjectionStatement(ICompilation compilation, Type implementedType, Lifetime lifetime)
        {
            instantiation = new Lazy<IInstantiation>(() => Source.CreateInstantiation());
            Lifetime = lifetime;
            Source = new DependencyInjectionSource(implementedType, compilation);
        }

        public Type ImplementedType => Source.ImplementedType;

        public IReadOnlyList<Type> AssignedTypeList => Source.AssignedTypeList;

        public Ownership Ownership => Ownership.Internal;

        public IInstantiation Instantiation => instantiation.Value;
    }
}
