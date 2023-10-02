using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public sealed class CreateInstanceStatement<T> : IStatement
        where T : notnull
    {
        private readonly InstanceInjectionSource source;
        private readonly Func<T> createInstance;
        private readonly Lazy<IInstantiation> instantiationCache;

        public Lifetime Lifetime { get; }
        public Ownership Ownership { get; }

        public CreateInstanceStatement(ICompilation compilation, Lifetime lifetime, Ownership ownership, Func<T> createInstance)
        {
            this.createInstance = createInstance;
            Lifetime = lifetime;
            Ownership = ownership;
            source = new InstanceInjectionSource(typeof(T), compilation);
            instantiationCache = new Lazy<IInstantiation>(CreateInstantiation);
        }

        private IInstantiation CreateInstantiation()
        {
            var instantiation = new CreateInstance<T>(createInstance);

            if (!source.CanInjectIntoInstance(out var injection))
            {
                return instantiation;
            }

            return new InstantiateThenInject(instantiation, injection);
        }

        public IInstanceInjection InstanceInjection => source;

        public Type ImplementedType => source.ImplementedType;

        public IReadOnlyList<Type> AssignedTypeList => source.AssignedTypeList;

        public IInstantiation Instantiation => instantiationCache.Value;
    }
}
