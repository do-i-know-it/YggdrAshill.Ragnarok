using YggdrAshill.Ragnarok.Construction;
using YggdrAshill.Ragnarok.Hierarchization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace YggdrAshill.Ragnarok.Motorization
{
    /// <summary>
    /// Implementation of <see cref="IScopedResolverContext"/> using <see cref="IEngineBuilder"/>.
    /// </summary>
    public sealed class ScopedResolverContext :
        IScopedResolverContext
    {
        private readonly IEngineBuilder engineBuilder;
        private readonly IScopedResolver? parentScopedResolver;

        /// <summary>
        /// Constructor of <see cref="ScopedResolverContext"/>.
        /// </summary>
        /// <param name="engineBuilder"></param>
        /// <param name="parentScopedResolver"></param>
        public ScopedResolverContext(IEngineBuilder engineBuilder, IScopedResolver? parentScopedResolver = null)
        {
            this.engineBuilder = engineBuilder;
            this.parentScopedResolver = parentScopedResolver;
        }

        private readonly List<IComposition> compositionList = new List<IComposition>();

        public IInstantiation GetInstantiation(Type type, IReadOnlyList<IParameter> parameterList)
        {
            return engineBuilder.GetInstantiation(type, parameterList);
        }

        public IInjection GetFieldInjection(Type type, IReadOnlyList<IParameter> parameterList)
        {
            return engineBuilder.GetFieldInjection(type, parameterList);
        }

        public IInjection GetPropertyInjection(Type type, IReadOnlyList<IParameter> parameterList)
        {
            return engineBuilder.GetPropertyInjection(type, parameterList);
        }

        public IInjection GetMethodInjection(Type type, IReadOnlyList<IParameter> parameterList)
        {
            return engineBuilder.GetMethodInjection(type, parameterList);
        }

        /// <inheritdoc/>
        public void Register(IComposition composition)
        {
            if (compositionList.Contains(composition))
            {
                return;
            }

            compositionList.Add(composition);
        }

        /// <summary>
        /// Collects <see cref="IDescription"/>s to create <see cref="IEngine"/> from <see cref="IEngineBuilder"/>,
        /// then creates <see cref="IScopedResolver"/>.
        /// </summary>
        /// <returns>
        /// <see cref="IScopedResolver"/> created.
        /// </returns>
        public IScopedResolver Build()
        {
            var descriptionList = compositionList
                .Select(composition => composition.Compose())
                .Append(ResolverDescription.Instance);

            var engine = engineBuilder.Build(descriptionList);

            return new ScopedResolver(parentScopedResolver, engine, engineBuilder);
        }
    }
}