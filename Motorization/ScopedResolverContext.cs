using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Implementation of <see cref="IScopedResolverContext"/>.
    /// </summary>
    public sealed class ScopedResolverContext : IScopedResolverContext
    {
        private readonly Interpretation interpretation;
        private readonly IScopedResolver? parent;

        internal ScopedResolverContext(Interpretation interpretation, IScopedResolver? parent)
        {
            this.interpretation = interpretation;
            this.parent = parent;
        }

        /// <summary>
        /// Constructor of <see cref="ScopedResolverContext"/> for root <see cref="IScopedResolver"/>.
        /// </summary>
        /// <param name="decision">
        /// <see cref="IDecision"/> for <see cref="ScopedResolverContext"/>.
        /// </param>
        /// /// <param name="operation">
        /// <see cref="IOperation"/> for <see cref="ScopedResolverContext"/>.
        /// </param>
        public ScopedResolverContext(IDecision decision, IOperation operation)
            : this(new Interpretation(decision, operation), null)
        {

        }

        /// <inheritdoc/>
        public object Resolve(Type type)
        {
            if (parent != null)
            {
                return parent.Resolve(type);
            }

            if (InstallationDescription.CanResolve(type))
            {
                var activation = interpretation.GetActivation(type);

                return InstallationDescription.Resolve(activation, this);
            }

            throw new RagnarokNotRegisteredException(type);
        }

        /// <inheritdoc/>
        public IInterpretation Interpretation => interpretation;

        /// <inheritdoc/>
        public IScopedResolver Build(IReadOnlyList<IStatement> statementList)
        {
            using var factory = new DictionaryFactory(interpretation, statementList);

            var content = factory.Create();

            var resolver = new ScopedResolver(content, interpretation, parent);

            TypeAnalysis.Validate(statementList, resolver);

            return resolver;
        }
    }
}
