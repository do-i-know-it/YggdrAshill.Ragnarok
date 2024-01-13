using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Implementation of <see cref="IScopedResolverBuilder"/>.
    /// </summary>
    public sealed class ScopedResolverBuilder : IScopedResolverBuilder
    {
        private static readonly Type installationType = typeof(IInstallation);

        private readonly Engine engine;
        private readonly IScopedResolver? parent;

        internal ScopedResolverBuilder(Engine engine, IScopedResolver? parent)
        {
            this.engine = engine;
            this.parent = parent;
        }

        /// <summary>
        /// Creates <see cref="ScopedResolverBuilder"/> for root <see cref="IScopedResolver"/>.
        /// </summary>
        /// <param name="resolver">
        /// <see cref="IRootResolver"/> for <see cref="ScopedResolverBuilder"/>.
        /// </param>
        public ScopedResolverBuilder(IRootResolver resolver) : this(new Engine(resolver), null)
        {

        }

        /// <inheritdoc/>
        public object Resolve(Type type)
        {
            if (installationType.IsAssignableFrom(type))
            {
                return engine.Resolver.Resolve(type);
            }

            if (parent != null)
            {
                return parent.Resolve(type);
            }

            throw new RagnarokNotRegisteredException(type);
        }

        /// <inheritdoc/>
        public ICompilation Compilation => engine;

        /// <inheritdoc/>
        public IScopedResolver Build(IReadOnlyList<IStatement> statementList)
        {
            using var factory = engine.CreateFactory(statementList);

            var content = factory.Create();

            var resolver = new ScopedResolver(content, engine, parent);

            factory.Validate(resolver);

            return resolver;
        }
    }
}
