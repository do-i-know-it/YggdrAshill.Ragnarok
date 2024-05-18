namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines request for to instantiate.
    /// </summary>
    public sealed class InstantiationRequest
    {
        /// <summary>
        /// <see cref="IDependency"/>s to instantiate.
        /// </summary>
        public IDependency Dependency { get; }

        /// <summary>
        /// <see cref="IActivation"/>s to instantiate.
        /// </summary>
        public IActivation Activation { get; }

        /// <summary>
        /// Constructor of <see cref="InstantiationRequest"/>.
        /// </summary>
        /// <param name="dependency">
        /// <see cref="IDependency"/> for <see cref="Dependency"/>.
        /// </param>
        /// <param name="activation">
        /// <see cref="IActivation"/>s for <see cref="Activation"/>.
        /// </param>
        public InstantiationRequest(IDependency dependency, IActivation activation)
        {
            Dependency = dependency;
            Activation = activation;
        }
    }
}
