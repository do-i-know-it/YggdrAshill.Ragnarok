using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Implementation of <see cref="IScopedResolverContext"/> using <see cref="IEngineContext"/>.
    /// </summary>
    public sealed class ScopedResolverContext :
        IScopedResolverContext
    {
        private readonly IEngineContext engineContext;
        private readonly IScopedResolver? parentScopedResolver;

        /// <summary>
        /// Constructor of <see cref="ScopedResolverContext"/>.
        /// </summary>
        /// <param name="engineContext">
        /// <see cref="IEngineContext"/> to instantiate <see cref="ScopedResolverContext"/>.
        /// </param>
        /// <param name="parentScopedResolver">
        /// <see cref="IScopedResolver"/> to instantiate <see cref="ScopedResolverContext"/>.
        /// </param>
        public ScopedResolverContext(IEngineContext engineContext, IScopedResolver? parentScopedResolver = null)
        {
            this.engineContext = engineContext;
            this.parentScopedResolver = parentScopedResolver;
        }

        /// <inheritdoc/>
        public IInstantiation GetInstantiation(Type type, IReadOnlyList<IParameter> parameterList)
        {
            var activation = engineContext.GetActivation(type);

            return new ActivateToInstantiate(activation, parameterList);
        }

        /// <inheritdoc/>
        public IInjection GetFieldInjection(Type type, IReadOnlyList<IParameter> parameterList)
        {
            var infusion = engineContext.GetFieldInfusion(type);

            return new InfuseToInject(infusion, parameterList);
        }

        /// <inheritdoc/>
        public IInjection GetPropertyInjection(Type type, IReadOnlyList<IParameter> parameterList)
        {
            var infusion = engineContext.GetPropertyInfusion(type);

            return new InfuseToInject(infusion, parameterList);
        }

        /// <inheritdoc/>
        public IInjection GetMethodInjection(Type type, IReadOnlyList<IParameter> parameterList)
        {
            var infusion = engineContext.GetMethodInfusion(type);

            return new InfuseToInject(infusion, parameterList);
        }

        /// <summary>
        /// Creates <see cref="IEngine"/> from <see cref="IDescription"/>s using <see cref="IEngineContext"/>,
        /// then creates <see cref="IScopedResolver"/>.
        /// </summary>
        /// <returns>
        /// <see cref="IScopedResolver"/> created.
        /// </returns>
        public IScopedResolver Build(IReadOnlyList<IDescription> descriptionList)
        {
            var engine = engineContext.Build(descriptionList);

            return new ScopedResolver(parentScopedResolver, engine);
        }
    }
}
