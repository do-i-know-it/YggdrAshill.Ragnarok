using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class Interpretation : IInterpretation
    {
        private readonly IRootResolverV2 rootResolver;
        private readonly IDecision decision;
        private readonly IInstruction instruction;

        private readonly Func<Type, IActivationV2> createActivation;
        private readonly Func<Type, IInfusionV2> createFieldInfusion;
        private readonly Func<Type, IInfusionV2> createPropertyInfusion;
        private readonly Func<Type, IInfusionV2> createMethodInfusion;

        public Interpretation(IRootResolverV2 rootResolver)
        {
            this.rootResolver = rootResolver;
            decision = rootResolver.Decision;
            instruction = rootResolver.Instruction;

            createActivation = CreateActivation;
            createFieldInfusion = CreateFieldInfusion;
            createPropertyInfusion = CreatePropertyInfusion;
            createMethodInfusion = CreateMethodInfusion;
        }

        public IObjectResolver RootResolver => rootResolver;

        public IActivationV2 ActivationOf(Type type)
        {
            return TypeAnalysisV2.GetActivation(type, createActivation);
        }
        private IActivationV2 CreateActivation(Type type)
        {
            if (CollectionDescriptionV2.CanResolve(type, out var arrayElementType))
            {
                return instruction.CreateCollectionActivation(arrayElementType);
            }

            var request = ServiceBundleDescriptionV2.CanResolve(type, out var bundleElementType)
                ? decision.RequestServiceBundleInjection(bundleElementType)
                : decision.RequestDependencyInjection(type);

            return instruction.CreateActivation(request);
        }

        public IInfusionV2 FieldInfusionOf(Type type)
        {
            return TypeAnalysisV2.GetFieldInfusion(type, createFieldInfusion);
        }
        private IInfusionV2 CreateFieldInfusion(Type type)
        {
            var request = decision.RequestFieldInjection(type);

            return request.FieldList.Length == 0 ? InfuseNothing.Instance : instruction.CreateFieldInfusion(request);
        }

        public IInfusionV2 PropertyInfusionOf(Type type)
        {
            return TypeAnalysisV2.GetPropertyInfusion(type, createPropertyInfusion);
        }
        private IInfusionV2 CreatePropertyInfusion(Type type)
        {
            var request = decision.RequestPropertyInjection(type);

            return request.PropertyList.Length == 0 ? InfuseNothing.Instance : instruction.CreatePropertyInfusion(request);
        }

        public IInfusionV2 MethodInfusionOf(Type type)
        {
            return TypeAnalysisV2.GetMethodInfusion(type, createMethodInfusion);
        }
        private IInfusionV2 CreateMethodInfusion(Type type)
        {
            var request = decision.RequestMethodInjection(type);

            return request.ParameterList.Length == 0 ? InfuseNothing.Instance : instruction.CreateMethodInfusion(request);
        }

        public void Validate(IScopedResolver resolver, IEnumerable<IStatement> statementList)
        {
            TypeAnalysisV2.Validate(statementList, resolver);
        }

        public DictionaryFactoryV2 CreateFactory(IEnumerable<IStatement> statementList)
        {
            return new DictionaryFactoryV2(this, statementList);
        }
    }
}
