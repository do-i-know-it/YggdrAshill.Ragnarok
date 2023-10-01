namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Implementation of <see cref="IObjectContext"/> using <see cref="IScopedResolver"/> and <see cref="IScopedResolverBuilder"/>.
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
        public IObjectResolver Resolver => resolverBuilder.Resolver;

        /// <inheritdoc/>
        public ICompilation Compilation => resolverBuilder.Compilation;

        /// <inheritdoc/>
        public IRegistration Registration => resolverBuilder;

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
