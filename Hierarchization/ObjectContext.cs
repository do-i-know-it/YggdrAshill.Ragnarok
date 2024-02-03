namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Implementation of <see cref="IObjectContext"/> using <see cref="IScopedResolver"/> and <see cref="IScopedResolverBuilder"/>.
    /// </summary>
    public sealed class ObjectContext : IObjectContext
    {
        private readonly IScopedResolverBuilder builder;
        private readonly Compilation compilation;
        private readonly Registration registration;

        /// <summary>
        /// Constructor of <see cref="ObjectContext"/>.
        /// </summary>
        /// <param name="builder">
        /// <see cref="IScopedResolverBuilder"/> for <see cref="ObjectContext"/>.
        /// </param>
        public ObjectContext(IScopedResolverBuilder builder)
        {
            this.builder = builder;
            compilation = new Compilation(builder.Interpretation);
            registration = new Registration(builder);
        }

        /// <inheritdoc/>
        public IRegistration Registration => registration;

        /// <inheritdoc/>
        public IObjectResolver Resolver => builder;

        /// <inheritdoc/>
        public ICompilation Compilation => compilation;

        /// <inheritdoc/>
        public IObjectContext CreateContext()
        {
            return new ObjectContext(builder);
        }

        /// <inheritdoc/>
        public IObjectScope CreateScope()
        {
            var resolver = registration.Build();

            return new ObjectScope(resolver);
        }
    }
}
