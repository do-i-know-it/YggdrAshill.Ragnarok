using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class CreateInstanceStatement<T> : IStatement
        where T : notnull
    {
        private readonly Func<T> createInstance;
        private readonly Lazy<IInstantiation> instantiationCache;

        public Lifetime Lifetime { get; }
        public Ownership Ownership { get; }
        public InstanceInjectionSource Source { get; }

        public CreateInstanceStatement(ICompilation compilation, Lifetime lifetime, Ownership ownership, Func<T> createInstance)
        {
            this.createInstance = createInstance;
            Lifetime = lifetime;
            Ownership = ownership;
            Source = new InstanceInjectionSource(typeof(T), compilation);
            instantiationCache = new Lazy<IInstantiation>(CreateInstantiation);
        }

        private IInstantiation CreateInstantiation()
        {
            var instantiation = new CreateInstance<T>(createInstance);

            if (!Source.CanInjectIntoInstance(out var injection))
            {
                return instantiation;
            }

            return injection.ToInstantiate(instantiation);
        }

        public Type ImplementedType => Source.ImplementedType;

        public IReadOnlyList<Type> AssignedTypeList => Source.AssignedTypeList;

        public IInstantiation Instantiation => instantiationCache.Value;
    }
}
