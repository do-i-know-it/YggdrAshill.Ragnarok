using System;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Default implementation of <see cref="IObjectContext"/>.
    /// </summary>
    public sealed class DependencyContext : IObjectContext
    {
        private readonly IObjectContext context;

        /// <summary>
        /// Creates <see cref="DependencyContext"/>.
        /// </summary>
        /// <param name="context">
        /// <see cref="IObjectContext"/> for <see cref="DependencyContext"/>.
        /// </param>
        public DependencyContext(IObjectContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Creates <see cref="DependencyContext"/>.
        /// </summary>
        /// <param name="resolverBuilder">
        /// <see cref="IScopedResolverBuilder"/> for <see cref="DependencyContext"/>.
        /// </param>
        public DependencyContext(IScopedResolverBuilder resolverBuilder) : this(new ObjectContext(resolverBuilder))
        {

        }

        public DependencyContext(IRootResolver resolver) : this(new ScopedResolverBuilder(resolver))
        {

        }

        /// <summary>
        /// Creates <see cref="DependencyContext"/>.
        /// </summary>
        /// <param name="selector">
        /// <see cref="ISelector"/> for <see cref="DependencyContext"/>.
        /// </param>
        /// /// <param name="solver">
        /// <see cref="ISolver"/> for <see cref="DependencyContext"/>.
        /// </param>
        public DependencyContext(ISelector selector, ISolver solver) : this(new RootResolver(selector, solver))
        {

        }

        /// <summary>
        /// Creates <see cref="DependencyContext"/>.
        /// </summary>
        /// <param name="solver">
        /// <see cref="ISolver"/> for <see cref="DependencyContext"/>.
        /// </param>
        /// <remarks>
        /// <see cref="ISelector"/> is <see cref="AnnotationSelector"/>.
        /// </remarks>
        public DependencyContext(ISolver solver) : this(AnnotationSelector.Instance, solver)
        {

        }

        /// <summary>
        /// Creates <see cref="DependencyContext"/>.
        /// </summary>
        /// <remarks>
        /// <see cref="ISelector"/> is <see cref="AnnotationSelector"/>, and
        /// <see cref="ISolver"/> is <see cref="ExpressionSolver"/>.
        /// </remarks>
        public DependencyContext() : this(AnnotationSelector.Instance, ExpressionSolver.Instance)
        {

        }

        /// <inheritdoc/>
        public IObjectResolver Resolver => context.Resolver;

        /// <inheritdoc/>
        public ICompilation Compilation => context.Compilation;

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

        /// <inheritdoc/>
        public int Count(IStatementSelection selection)
        {
            return context.Count(selection);
        }

        /// <inheritdoc/>
        public void Register(IStatement statement)
        {
            context.Register(statement);
        }

        /// <inheritdoc/>
        public void Register(IOperation operation)
        {
            context.Register(operation);
        }

        /// <inheritdoc/>
        public void Register(IDisposable disposable)
        {
            context.Register(disposable);
        }
    }
}
