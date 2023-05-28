using YggdrAshill.Ragnarok.Memorization;
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    /// <summary>
    /// Default implementation of <see cref="IContext"/>.
    /// </summary>
    public sealed class DependencyContext :
        IContext
    {
        private readonly IContext context;

        public DependencyContext(IContext context)
        {
            this.context = context;
        }

        public DependencyContext(IScopedResolverContext context) : this(new Context(context))
        {

        }

        public DependencyContext(IEngineBuilder builder) : this(new ScopedResolverContext(builder))
        {

        }

        public DependencyContext(IRegistryBuilder builder) : this(new EngineBuilder(builder))
        {

        }

        public DependencyContext(ISelector selector, ISolver solver) : this(new RegistryBuilder(selector, solver))
        {

        }

        public DependencyContext(ISolver solver) : this(AnnotationSelector.Instance, solver)
        {

        }

        public DependencyContext() : this(AnnotationSelector.Instance, ExpressionSolver.Instance)
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
