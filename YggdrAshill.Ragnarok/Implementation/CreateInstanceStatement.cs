using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class CreateInstanceStatement<T> : IStatement
        where T : notnull
    {
        private readonly ICreation<T> creation;
        private readonly Lazy<IInstantiation> instantiationCache;

        public Ownership Ownership { get; }
        public InstanceInjectionSource Source { get; }

        public CreateInstanceStatement(IObjectContainer container, Ownership ownership, ICreation<T> creation)
        {
            this.creation = creation;
            Ownership = ownership;
            Source = new InstanceInjectionSource(typeof(T), container);
            instantiationCache = new Lazy<IInstantiation>(CreateInstantiation);
        }

        private IInstantiation CreateInstantiation()
        {
            var instantiation = new InstantiateToCreate<T>(creation);

            if (!Source.CanInjectIntoInstance(out var injection))
            {
                return instantiation;
            }

            return injection.ToInstantiate(instantiation);
        }

        public Type ImplementedType => Source.ImplementedType;

        public IReadOnlyList<Type> AssignedTypeList => Source.AssignedTypeList;

        public Lifetime Lifetime => Lifetime.Global;

        public IInstantiation Instantiation => instantiationCache.Value;
    }
}
