using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class Engine : ICompilation
    {
        private readonly IRootResolver resolver;

        private readonly Func<Type, IActivation> createActivation;
        private readonly Func<Type, IInfusion> createFieldInfusion;
        private readonly Func<Type, IInfusion> createPropertyInfusion;
        private readonly Func<Type, IInfusion> createMethodInfusion;

        public Engine(IRootResolver resolver)
        {
            this.resolver = resolver;

            createActivation = CreateActivationInternally;
            createFieldInfusion = CreateFieldInfusionInternally;
            createPropertyInfusion = CreatePropertyInfusionInternally;
            createMethodInfusion = CreateMethodInfusionInternally;
        }

        public IObjectResolver Resolver => resolver;

        public IActivation CreateActivation(Type type)
        {
            return TypeAnalysis.GetActivation(type, createActivation);
        }
        private IActivation CreateActivationInternally(Type type)
        {
            if (CollectionDescription.CanResolve(type, out var arrayElementType))
            {
                return resolver.Solver.CreateCollectionActivation(arrayElementType);
            }

            var request = ServiceBundleDescription.CanResolve(type, out var bundleElementType)
                ? resolver.Selector.RequestServiceBundleInjection(bundleElementType)
                : resolver.Selector.RequestDependencyInjection(type);

            return resolver.Solver.CreateActivation(request);
        }

        public IInfusion GetFieldInfusion(Type type)
        {
            return TypeAnalysis.GetFieldInfusion(type, createFieldInfusion);
        }
        private IInfusion CreateFieldInfusionInternally(Type type)
        {
            var request = resolver.Selector.RequestFieldInjection(type);

            return request.FieldList.Length == 0 ? InfuseNothing.Instance : resolver.Solver.CreateFieldInfusion(request);
        }

        public IInfusion GetPropertyInfusion(Type type)
        {
            return TypeAnalysis.GetPropertyInfusion(type, createPropertyInfusion);
        }
        private IInfusion CreatePropertyInfusionInternally(Type type)
        {
            var request = resolver.Selector.RequestPropertyInjection(type);

            return request.PropertyList.Length == 0 ? InfuseNothing.Instance : resolver.Solver.CreatePropertyInfusion(request);
        }

        public IInfusion GetMethodInfusion(Type type)
        {
            return TypeAnalysis.GetMethodInfusion(type, createMethodInfusion);
        }
        private IInfusion CreateMethodInfusionInternally(Type type)
        {
            var request = resolver.Selector.RequestMethodInjection(type);

            return resolver.Solver.CreateMethodInfusion(request);
        }

        public DictionaryFactory CreateFactory(IEnumerable<IStatement> statementList)
        {
            return new DictionaryFactory(this, statementList);
        }
    }
}
