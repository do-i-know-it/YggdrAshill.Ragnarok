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
        /// <param name="context">
        /// <see cref="IScopedResolverContext"/> for <see cref="DependencyContext"/>.
        /// </param>
        public DependencyContext(IScopedResolverContext context) : this(new ObjectContext(context))
        {

        }

        /// <summary>
        /// Creates <see cref="DependencyContext"/>.
        /// </summary>
        /// <param name="enumeration">
        /// <see cref="IDependencyEnumeration"/> for <see cref="ScopedResolverContext"/>.
        /// </param>
        /// <param name="selection">
        /// <see cref="IDependencySelection"/> for <see cref="ScopedResolverContext"/>.
        /// </param>
        /// <param name="operation">
        /// <see cref="IDependencyOperation"/> for <see cref="ScopedResolverContext"/>.
        /// </param>
        public DependencyContext(IDependencyEnumeration enumeration, IDependencySelection selection, IDependencyOperation operation) : this(new ScopedResolverContext(enumeration, selection, operation))
        {

        }

        /// <summary>
        /// Creates <see cref="DependencyContext"/>.
        /// </summary>
        /// <param name="operation">
        /// <see cref="IDependencyOperation"/> for <see cref="ScopedResolverContext"/>.
        /// </param>
        /// <remarks>
        /// <see cref="IDependencyEnumeration"/> is <see cref="DependencyEnumeration"/> and
        /// <see cref="IDependencySelection"/> is <see cref="AnnotateToSelect"/>.
        /// </remarks>
        public DependencyContext(IDependencyOperation operation) : this(DependencyEnumeration.Instance, AnnotateToSelect.Instance, operation)
        {

        }

        /// <summary>
        /// Creates <see cref="DependencyContext"/>.
        /// </summary>
        /// <remarks>
        /// <see cref="IDependencyEnumeration"/> is <see cref="DependencyEnumeration"/>,
        /// <see cref="IDependencySelection"/> is <see cref="AnnotateToSelect"/> and
        /// <see cref="IDependencyOperation"/> is <see cref="ExpressionToOperate"/>.
        /// </remarks>
        public DependencyContext() : this(DependencyEnumeration.Instance, AnnotateToSelect.Instance, ExpressionToOperate.Instance)
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
