namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines how to build <see cref="IScopedResolverV2"/>.
    /// </summary>
    public interface IScopedResolverBuilder : IRegistrationV2, IObjectResolver
    {
        /// <summary>
        /// <see cref="ICompilationV2"/> to resolve dependency.
        /// </summary>
        ICompilationV2 Compilation { get; }

        /// <summary>
        /// Creates a <see cref="IScopedResolverBuilder"/>.
        /// </summary>
        /// <returns>
        /// <see cref="IScopedResolverBuilder"/> created.
        /// </returns>
        IScopedResolverBuilder CreateBuilder();

        /// <summary>
        /// Creates a <see cref="IScopedResolverV2"/>.
        /// </summary>
        /// <returns>
        /// <see cref="IScopedResolverV2"/> created.
        /// </returns>
        IScopedResolverV2 Build();
    }
}
