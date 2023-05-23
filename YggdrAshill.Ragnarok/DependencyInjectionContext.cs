using YggdrAshill.Ragnarok.Construction;
using YggdrAshill.Ragnarok.Hierarchization;
using YggdrAshill.Ragnarok.Motorization;
using YggdrAshill.Ragnarok.Materialization;
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Default implementation of <see cref="IContext"/>.
    /// </summary>
    public sealed class DependencyInjectionContext :
        IContext
    {
        private readonly IContext context;

        public DependencyInjectionContext(IContext context)
        {
            this.context = context;
        }

        public DependencyInjectionContext(IScopedResolverContext context) : this(new Context(context))
        {

        }

        public DependencyInjectionContext(IEngineBuilder builder) : this(new ScopedResolverContext(builder))
        {

        }

        public DependencyInjectionContext(IRegistryBuilder builder) : this(new EngineBuilder(builder))
        {

        }

        public DependencyInjectionContext(ISelector selector, ISolver solver) : this(new RegistryBuilder(selector, solver))
        {

        }

        public DependencyInjectionContext(ISolver solver) : this(DefaultSelector.Instance, solver)
        {

        }

        public DependencyInjectionContext() : this(DefaultSelector.Instance, ExpressionSolver.Instance)
        {

        }

        /// <inheritdoc/>
        public void Register(IComposition composition)
        {
            context.Register(composition);
        }

        /// <inheritdoc/>
        public void Register(Action<IResolver> callback)
        {
            context.Register(callback);
        }

        /// <inheritdoc/>
        public IScope Build()
        {
            return context.Build();
        }

        /// <inheritdoc/>
        public IInstantiation GetInstantiation(Type type, IReadOnlyList<IParameter> parameterList)
        {
            return context.GetInstantiation(type, parameterList);
        }

        /// <inheritdoc/>
        public IInjection GetFieldInjection(Type type, IReadOnlyList<IParameter> parameterList)
        {
            return context.GetFieldInjection(type, parameterList);
        }

        /// <inheritdoc/>
        public IInjection GetPropertyInjection(Type type, IReadOnlyList<IParameter> parameterList)
        {
            return context.GetPropertyInjection(type, parameterList);
        }

        /// <inheritdoc/>
        public IInjection GetMethodInjection(Type type, IReadOnlyList<IParameter> parameterList)
        {
            return context.GetMethodInjection(type, parameterList);
        }
    }
}
