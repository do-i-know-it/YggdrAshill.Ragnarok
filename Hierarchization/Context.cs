using System;
using System.Collections.Generic;
using System.Linq;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Implementation of <see cref="IContext"/> using <see cref="IScopedResolver"/> and <see cref="IScopedResolverContext"/>.
    /// </summary>
    public sealed class Context :
        IContext
    {
        private readonly IScopedResolverContext scopedResolverContext;

        /// <summary>
        /// Constructor of <see cref="Context"/>.
        /// </summary>
        /// <param name="scopedResolverContext">
        /// <see cref="IScopedResolverContext"/> to instantiate <see cref="Context"/>.
        /// </param>
        public Context(IScopedResolverContext scopedResolverContext)
        {
            this.scopedResolverContext = scopedResolverContext;
        }

        private readonly List<IComposition> compositionList = new List<IComposition>();
        private readonly List<Action<IResolver>> callbacks = new List<Action<IResolver>>();

        /// <inheritdoc/>
        public IInstantiation GetInstantiation(Type type, IReadOnlyList<IParameter> parameterList)
        {
            return scopedResolverContext.GetInstantiation(type, parameterList);
        }

        /// <inheritdoc/>
        public IInjection GetFieldInjection(Type type, IReadOnlyList<IParameter> parameterList)
        {
            return scopedResolverContext.GetFieldInjection(type, parameterList);
        }

        /// <inheritdoc/>
        public IInjection GetPropertyInjection(Type type, IReadOnlyList<IParameter> parameterList)
        {
            return scopedResolverContext.GetPropertyInjection(type, parameterList);
        }

        /// <inheritdoc/>
        public IInjection GetMethodInjection(Type type, IReadOnlyList<IParameter> parameterList)
        {
            return scopedResolverContext.GetMethodInjection(type, parameterList);
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
        /// Adds <see cref="Action{T}"/> to execute event to use <see cref="IScopedResolverContext"/>.
        /// </summary>
        /// <param name="callback">
        /// <see cref="Action{T}"/> to receive <see cref="IResolver"/>.
        /// </param>
        public void Register(Action<IResolver> callback)
        {
            if (callbacks.Contains(callback))
            {
                return;
            }

            callbacks.Add(callback);
        }

        /// <summary>
        /// Creates <see cref="IScopedResolver"/> from <see cref="IScopedResolverContext"/>,
        /// then executes callbacks with <see cref="IScopedResolver"/>,
        /// and creates <see cref="IScope"/>.
        /// </summary>
        /// <returns>
        /// <see cref="IScope"/> created.
        /// </returns>
        public IScope Build()
        {
            var descriptionList = compositionList
                .Select(composition => composition.Compose())
                .Append(ResolverDescription.Instance);

            var resolver = scopedResolverContext.Build(descriptionList);

            foreach (var callback in callbacks)
            {
                callback.Invoke(resolver);
            }

            return new Scope(resolver);
        }
    }
}
