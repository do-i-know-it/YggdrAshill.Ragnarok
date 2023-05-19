using YggdrAshill.Ragnarok.Construction;
using YggdrAshill.Ragnarok.Hierarchization;
using YggdrAshill.Ragnarok.Motorization;
using YggdrAshill.Ragnarok.Materialization;
using YggdrAshill.Ragnarok.Reflection;
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

        public DependencyInjectionContext(ISelector selector, ISolver solver)
        {
            var registryBuilder = new RegistryBuilder(selector, solver);
            var engineBuilder = new EngineBuilder(registryBuilder);
            var scopedResolverContext = new ScopedResolverContext(engineBuilder);

            context = new Context(scopedResolverContext);
        }

        public DependencyInjectionContext(ISelector selector) : this(selector, ReflectionSolver.Instance)
        {

        }

        public DependencyInjectionContext(ISolver solver) : this(DefaultSelector.Instance, solver)
        {

        }

        public DependencyInjectionContext() : this(DefaultSelector.Instance, ReflectionSolver.Instance)
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
