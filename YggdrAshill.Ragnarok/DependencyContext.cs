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

        /// <summary>
        /// Creates <see cref="DependencyContext"/>.
        /// </summary>
        /// <param name="decision">
        /// <see cref="IDecision"/> for <see cref="DependencyContext"/>.
        /// </param>
        /// <param name="operation">
        /// <see cref="IOperation"/> for <see cref="DependencyContext"/>.
        /// </param>
        public DependencyContext(IDecision decision, IOperation operation) : this(new ScopedResolverBuilder(decision, operation))
        {

        }

        /// <summary>
        /// Creates <see cref="DependencyContext"/>.
        /// </summary>
        /// <param name="operation">
        /// <see cref="IOperation"/> for <see cref="DependencyContext"/>.
        /// </param>
        /// <remarks>
        /// <see cref="IDecision"/> is <see cref="AnnotateToDecide"/>.
        /// </remarks>
        public DependencyContext(IOperation operation) : this(AnnotateToDecide.Instance, operation)
        {

        }

        /// <summary>
        /// Creates <see cref="DependencyContext"/>.
        /// </summary>
        /// <remarks>
        /// <see cref="IDecision"/> is <see cref="AnnotateToDecide"/>, and
        /// <see cref="IOperation"/> is <see cref="ExpressionToOperate"/>.
        /// </remarks>
        public DependencyContext() : this(AnnotateToDecide.Instance, ExpressionToOperate.Instance)
        {

        }

        /// <inheritdoc/>
        public IObjectResolver Resolver => context.Resolver;

        /// <inheritdoc/>
        public IRegistration Registration => context.Registration;

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
    }
}
