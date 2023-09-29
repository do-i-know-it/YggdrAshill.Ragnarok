namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Implementation of <see cref="IObjectContext"/> using <see cref="IScopedResolverV2"/> and <see cref="IScopedResolverBuilder"/>.
    /// </summary>
    public sealed class ObjectContext : IObjectContext
    {
        private readonly IScopedResolverBuilder resolverBuilder;

        /// <summary>
        /// Creates <see cref="ObjectContext"/>.
        /// </summary>
        /// <param name="resolverBuilder">
        /// <see cref="IScopedResolverBuilder"/> for <see cref="ObjectContext"/>.
        /// </param>
        public ObjectContext(IScopedResolverBuilder resolverBuilder)
        {
            this.resolverBuilder = resolverBuilder;
        }

        /// <inheritdoc/>
        public IObjectResolver Resolver => resolverBuilder;

        /// <inheritdoc/>
        public ICompilationV2 Compilation => resolverBuilder;

        /// <inheritdoc/>
        public IRegistrationV2 Registration => resolverBuilder;

        /// <inheritdoc/>
        public IObjectContext CreateContext()
        {
            return new ObjectContext(resolverBuilder.CreateBuilder());
        }

        /// <inheritdoc/>
        public IObjectScope CreateScope()
        {
            var resolver = resolverBuilder.Build();

            return new ObjectScope(resolver);
        }
    }
}
