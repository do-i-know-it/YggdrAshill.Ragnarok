using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class InstallationDescription : IDescription
    {
        public static bool CanResolve(Type type)
        {
            return TypeCache.Installation.IsAssignableFrom(type);
        }

        public static object Resolve(IActivation activation, IObjectResolver resolver)
        {
            var realization = activation.Dependency.CreateRealization(Array.Empty<IParameter>());
            var instanceList = realization.Realize(resolver);
            return activation.Activate(instanceList);
        }

        private readonly IRealization realization;
        private readonly IActivation activation;

        public Type ImplementedType { get; }

        public InstallationDescription(Type implementedType, IActivation activation)
        {
            ImplementedType = implementedType;
            this.activation = activation;
            realization = activation.Dependency.CreateRealization(Array.Empty<IParameter>());
        }

        public Lifetime Lifetime => Lifetime.Temporal;
        public Ownership Ownership => Ownership.Internal;

        public object Instantiate(IScopedResolver resolver)
        {
            var instanceList = realization.Realize(resolver);

            return activation.Activate(instanceList);
        }
    }
}
