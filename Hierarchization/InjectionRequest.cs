namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines request for to instantiate.
    /// </summary>
    public sealed class InjectionRequest
    {
        /// <summary>
        /// <see cref="IDependency"/>s to instantiate.
        /// </summary>
        public IDependency Dependency { get; }

        /// <summary>
        /// <see cref="IActivation"/>s to instantiate.
        /// </summary>
        public IInfusion Infusion { get; }

        /// <summary>
        /// Constructor of <see cref="InstantiationRequest"/>.
        /// </summary>
        /// <param name="dependency">
        /// <see cref="IDependency"/> for <see cref="Dependency"/>.
        /// </param>
        /// <param name="infusion">
        /// <see cref="IInfusion"/>s for <see cref="Infusion"/>.
        /// </param>
        public InjectionRequest(IDependency dependency, IInfusion infusion)
        {
            Dependency = dependency;
            Infusion = infusion;
        }
    }
}
