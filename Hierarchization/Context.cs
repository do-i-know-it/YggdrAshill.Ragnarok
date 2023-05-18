using YggdrAshill.Ragnarok.Construction;
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Hierarchization
{
    /// <summary>
    /// Implementation of <see cref="IContext"/> using <see cref="IScopedResolverContext"/>.
    /// </summary>
    public sealed class Context :
        IContext
    {
        private readonly IScopedResolverContext scopedResolverContext;

        /// <summary>
        /// Constructor of <see cref="Context"/>.
        /// </summary>
        /// <param name="scopedResolverContext"></param>
        public Context(IScopedResolverContext scopedResolverContext)
        {
            this.scopedResolverContext = scopedResolverContext;
        }

        private readonly List<Action<IResolver>> callbacks = new List<Action<IResolver>>();

        public IInstantiation GetInstantiation(Type type, IReadOnlyList<IParameter> parameterList)
        {
            return scopedResolverContext.GetInstantiation(type, parameterList);
        }

        public IInjection GetFieldInjection(Type type, IReadOnlyList<IParameter> parameterList)
        {
            return scopedResolverContext.GetFieldInjection(type, parameterList);
        }

        public IInjection GetPropertyInjection(Type type, IReadOnlyList<IParameter> parameterList)
        {
            return scopedResolverContext.GetPropertyInjection(type, parameterList);
        }

        public IInjection GetMethodInjection(Type type, IReadOnlyList<IParameter> parameterList)
        {
            return scopedResolverContext.GetMethodInjection(type, parameterList);
        }

        /// <summary>
        /// Adds <see cref="IComposition"/> to <see cref="IScopedResolverContext"/>.
        /// </summary>
        /// <param name="composition"></param>
        public void Register(IComposition composition)
        {
            scopedResolverContext.Register(composition);
        }

        /// <summary>
        /// Adds <see cref="IComposition"/> to <see cref="IScopedResolverContext"/>.
        /// </summary>
        /// <param name="callback"></param>
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
            var resolver = scopedResolverContext.Build();

            foreach (var callback in callbacks)
            {
                callback.Invoke(resolver);
            }

            return new Scope(resolver);
        }
    }
}
