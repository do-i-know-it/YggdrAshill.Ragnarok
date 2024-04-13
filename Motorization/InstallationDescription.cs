using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class InstallationDescription : IDescription
    {
        public static bool CanResolve(Type type)
        {
            return typeof(IInstallation).IsAssignableFrom(type);
        }

        public static object Resolve(InstantiationRequest request, IObjectResolver resolver)
        {
            var realization = request.Dependency.CreateRealization(Array.Empty<IParameter>());
            var instanceList = realization.Realize(resolver);
            return request.Activation.Activate(instanceList);
        }

        private readonly IRealization realization;
        private readonly IActivation activation;

        public Type ImplementedType { get; }

        public InstallationDescription(Type implementedType, InstantiationRequest request)
        {
            ImplementedType = implementedType;
            activation = request.Activation;
            realization = request.Dependency.CreateRealization(Array.Empty<IParameter>());
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
