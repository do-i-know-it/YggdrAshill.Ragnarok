using YggdrAshill.Ragnarok.Construction;
using YggdrAshill.Ragnarok.Hierarchization;
using YggdrAshill.Ragnarok.Motorization;
using YggdrAshill.Ragnarok.Materialization;
using YggdrAshill.Ragnarok.Reflection;
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    public sealed class DependencyInjectionContext :
        IContext
    {
        private readonly IContext context;

        public DependencyInjectionContext(ISelector selector, ISolver solver)
        {
            var engineBuilder = new EngineBuilder(new CodeBuilder(selector, solver));
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

        public void Register(IComposition composition)
        {
            context.Register(composition);
        }

        public void Register(Action<IResolver> callback)
        {
            context.Register(callback);
        }

        public IScope Build()
        {
            return context.Build();
        }

        public IInstantiation GetInstantiation(Type type, IReadOnlyList<IParameter> parameterList)
        {
            return context.GetInstantiation(type, parameterList);
        }

        public IInjection GetFieldInjection(Type type, IReadOnlyList<IParameter> parameterList)
        {
            return context.GetFieldInjection(type, parameterList);
        }

        public IInjection GetPropertyInjection(Type type, IReadOnlyList<IParameter> parameterList)
        {
            return context.GetPropertyInjection(type, parameterList);
        }

        public IInjection GetMethodInjection(Type type, IReadOnlyList<IParameter> parameterList)
        {
            return context.GetMethodInjection(type, parameterList);
        }
    }
}
