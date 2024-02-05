namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Implementation of <see cref="IObjectContext"/> using <see cref="IScopedResolver"/> and <see cref="IScopedResolverContext"/>.
    /// </summary>
    public sealed class ObjectContext : IObjectContext
    {
        private readonly IScopedResolverContext context;
        private readonly Compilation compilation;
        private readonly Registration registration;

        /// <summary>
        /// Constructor of <see cref="ObjectContext"/>.
        /// </summary>
        /// <param name="context">
        /// <see cref="IScopedResolverContext"/> for <see cref="ObjectContext"/>.
        /// </param>
        public ObjectContext(IScopedResolverContext context)
        {
            this.context = context;
            compilation = new Compilation(context.Interpretation);
            registration = new Registration(context);
        }

        /// <inheritdoc/>
        public IRegistration Registration => registration;

        /// <inheritdoc/>
        public IObjectResolver Resolver => context;

        /// <inheritdoc/>
        public ICompilation Compilation => compilation;

        /// <inheritdoc/>
        public IObjectContext CreateContext()
        {
            return new ObjectContext(context);
        }

        /// <inheritdoc/>
        public IObjectScope CreateScope()
        {
            var resolver = registration.Build();

            return new ObjectScope(resolver);
        }
    }
}
