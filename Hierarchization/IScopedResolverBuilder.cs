namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines how to build <see cref="IScopedResolver"/>.
    /// </summary>
    public interface IScopedResolverBuilder : IRegistration
    {
        /// <summary>
        /// <see cref="IObjectResolver"/> to resolve dependency.
        /// </summary>
        IObjectResolver Resolver { get; }

        /// <summary>
        /// <see cref="ICompilation"/> to resolve dependency.
        /// </summary>
        ICompilation Compilation { get; }

        /// <summary>
        /// Creates a <see cref="IScopedResolverBuilder"/>.
        /// </summary>
        /// <returns>
        /// <see cref="IScopedResolverBuilder"/> created.
        /// </returns>
        IScopedResolverBuilder CreateBuilder();

        /// <summary>
        /// Creates a <see cref="IScopedResolver"/>.
        /// </summary>
        /// <returns>
        /// <see cref="IScopedResolver"/> created.
        /// </returns>
        IScopedResolver Build();
    }
}
