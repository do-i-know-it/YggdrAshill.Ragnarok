using YggdrAshill.Ragnarok.Construction;
using YggdrAshill.Ragnarok.Hierarchization;
using YggdrAshill.Ragnarok.Motorization;
using YggdrAshill.Ragnarok.Materialization;
using YggdrAshill.Ragnarok.Fabrication;
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    /// <summary>
    /// Default implementation of <see cref="IContext"/>.
    /// </summary>
    public sealed class DependencyContext :
        IDependencyContext
    {
        private static Ownership ExternalToOwnership(bool external)
            => external ? Ownership.External : Ownership.Internal;

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

        public IDependencyInjection RegisterTemporal<T>()
            where T : notnull
        {
            return context.Register<T>(Lifetime.Temporal);
        }
        public IDependencyInjection RegisterLocal<T>()
            where T : notnull
        {
            return context.Register<T>(Lifetime.Local);
        }
        public IDependencyInjection RegisterGlobal<T>()
            where T : notnull
        {
            return context.Register<T>(Lifetime.Global);
        }
        public IDependencyInjection RegisterTemporal<TInterface, TImplementation>()
            where TInterface : notnull
            where TImplementation : TInterface
        {
            return context.Register<TInterface, TImplementation>(Lifetime.Temporal);
        }
        public IDependencyInjection RegisterLocal<TInterface, TImplementation>()
            where TInterface : notnull
            where TImplementation : TInterface
        {
            return context.Register<TInterface, TImplementation>(Lifetime.Local);
        }
        public IDependencyInjection RegisterGlobal<TInterface, TImplementation>()
            where TInterface : notnull
            where TImplementation : TInterface
        {
            return context.Register<TInterface, TImplementation>(Lifetime.Global);
        }

        public IInstanceInjection RegisterTemporal<T>(Func<T> instantiation, bool external = true)
            where T : notnull
        {
            return context.Register(instantiation, Lifetime.Temporal, ExternalToOwnership(external));
        }
        public IInstanceInjection RegisterLocal<T>(Func<T> instantiation, bool external = true)
            where T : notnull
        {
            return context.Register(instantiation, Lifetime.Local, ExternalToOwnership(external));
        }
        public IInstanceInjection RegisterGlobal<T>(Func<T> instantiation, bool external = true)
            where T : notnull
        {
            return context.Register(instantiation, Lifetime.Global, ExternalToOwnership(external));
        }
        public IInstanceInjection RegisterTemporal<TInterface, TImplementation>(Func<TImplementation> instantiation, bool external = true)
            where TInterface : notnull
            where TImplementation : TInterface
        {
            return context.Register<TInterface, TImplementation>(instantiation, Lifetime.Temporal, ExternalToOwnership(external));
        }
        public IInstanceInjection RegisterLocal<TInterface, TImplementation>(Func<TImplementation> instantiation, bool external = true)
            where TInterface : notnull
            where TImplementation : TInterface
        {
            return context.Register<TInterface, TImplementation>(instantiation, Lifetime.Local, ExternalToOwnership(external));
        }
        public IInstanceInjection RegisterGlobal<TInterface, TImplementation>(Func<TImplementation> instantiation, bool external = true)
            where TInterface : notnull
            where TImplementation : TInterface
        {
            return context.Register<TInterface, TImplementation>(instantiation, Lifetime.Global, ExternalToOwnership(external));
        }
    }
}
