using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class DependencyInjectionStatement : IStatement
    {
        private readonly Lazy<IInstantiation> instantiation;

        public Lifetime Lifetime { get; }
        public DependencyInjectionSource Source { get; }

        public DependencyInjectionStatement(IObjectContainer container, Type implementedType, Lifetime lifetime)
        {
            Lifetime = lifetime;
            Source = new DependencyInjectionSource(implementedType, container);
            instantiation = new Lazy<IInstantiation>(Source.CreateInstantiation);
        }

        public Type ImplementedType => Source.ImplementedType;

        public IReadOnlyList<Type> AssignedTypeList => Source.AssignedTypeList;

        public Ownership Ownership => Lifetime is Lifetime.Temporal ? Ownership.External : Ownership.Internal;

        public IInstantiation Instantiation => instantiation.Value;
    }
}
