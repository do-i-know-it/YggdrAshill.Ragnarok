namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Default implementation of <see cref="IObjectContext"/>.
    /// </summary>
    public sealed class DependencyContextV2 : IObjectContext
    {
        private readonly IObjectContext context;

        /// <summary>
        /// Creates <see cref="DependencyContextV2"/>.
        /// </summary>
        /// <param name="context">
        /// <see cref="IObjectContext"/> for <see cref="DependencyContextV2"/>.
        /// </param>
        public DependencyContextV2(IObjectContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Creates <see cref="DependencyContextV2"/>.
        /// </summary>
        /// <param name="resolverBuilder">
        /// <see cref="IScopedResolverBuilder"/> for <see cref="DependencyContextV2"/>.
        /// </param>
        public DependencyContextV2(IScopedResolverBuilder resolverBuilder)
            : this(new ObjectContext(resolverBuilder))
        {

        }

        /// <summary>
        /// Creates <see cref="DependencyContextV2"/>.
        /// </summary>
        /// <param name="selector">
        /// <see cref="ISelector"/> for <see cref="DependencyContextV2"/>.
        /// </param>
        /// /// <param name="solver">
        /// <see cref="ISolver"/> for <see cref="DependencyContextV2"/>.
        /// </param>
        public DependencyContextV2(ISelector selector, ISolver solver)
            : this(new ScopedResolverBuilder(selector, solver))
        {

        }

        /// <summary>
        /// Creates <see cref="DependencyContextV2"/>.
        /// </summary>
        /// <param name="solver">
        /// <see cref="ISolver"/> for <see cref="DependencyContextV2"/>.
        /// </param>
        /// <remarks>
        /// <see cref="ISelector"/> is <see cref="AnnotationSelector"/>.
        /// </remarks>
        public DependencyContextV2(ISolver solver)
            : this(AnnotationSelector.Instance, solver)
        {

        }

        /// <summary>
        /// Creates <see cref="DependencyContextV2"/>.
        /// </summary>
        /// <remarks>
        /// <see cref="ISelector"/> is <see cref="AnnotationSelector"/>, and
        /// <see cref="ISolver"/> is <see cref="ExpressionSolver"/>.
        /// </remarks>
        public DependencyContextV2()
            : this(AnnotationSelector.Instance, ExpressionSolver.Instance)
        {

        }

        /// <inheritdoc/>
        public IObjectResolver Resolver => context.Resolver;

        /// <inheritdoc/>
        public ICompilationV2 Compilation => context.Compilation;

        /// <inheritdoc/>
        public IRegistrationV2 Registration => context.Registration;

        /// <inheritdoc/>
        public IObjectContext CreateContext()
        {
            return context.CreateContext();
        }

        /// <inheritdoc/>
        public IObjectScope CreateScope()
        {
            return context.CreateScope();
        }
    }
}
