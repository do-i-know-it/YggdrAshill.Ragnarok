using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class Interpretation : IInterpretation
    {
        private readonly IDecision decision;
        private readonly IOperation operation;

        private readonly Func<Type, IActivation> createActivation;
        private readonly Func<Type, IInfusion> createFieldInfusion;
        private readonly Func<Type, IInfusion> createPropertyInfusion;
        private readonly Func<Type, IInfusion> createMethodInfusion;

        public Interpretation(IDecision decision, IOperation operation)
        {
            this.decision = decision;
            this.operation = operation;

            createActivation = CreateActivation;
            createFieldInfusion = CreateFieldInfusion;
            createPropertyInfusion = CreatePropertyInfusion;
            createMethodInfusion = CreateMethodInfusion;
        }

        public IActivation ActivationOf(Type type)
        {
            return TypeAnalysis.GetActivation(type, createActivation);
        }
        private IActivation CreateActivation(Type type)
        {
            if (CollectionDescription.CanResolve(type, out var arrayElementType))
            {
                return operation.CreateCollectionActivation(new CollectionInjectionRequest(arrayElementType));
            }

            if (ServiceBundleDescription.CanResolve(type, out var bundleElementType))
            {
                var bundleRequest = decision.RequestServiceBundleInjection(bundleElementType);

                return operation.CreateActivation(bundleRequest);
            }

            var request = decision.RequestDependencyInjection(type);

            return operation.CreateActivation(request);
        }

        public IInfusion FieldInfusionOf(Type type)
        {
            return TypeAnalysis.GetFieldInfusion(type, createFieldInfusion);
        }
        private IInfusion CreateFieldInfusion(Type type)
        {
            var request = decision.RequestFieldInjection(type);

            return request.FieldList.Length == 0 ? InfuseNothing.Instance : operation.CreateFieldInfusion(request);
        }

        public IInfusion PropertyInfusionOf(Type type)
        {
            return TypeAnalysis.GetPropertyInfusion(type, createPropertyInfusion);
        }
        private IInfusion CreatePropertyInfusion(Type type)
        {
            var request = decision.RequestPropertyInjection(type);

            return request.PropertyList.Length == 0 ? InfuseNothing.Instance : operation.CreatePropertyInfusion(request);
        }

        public IInfusion MethodInfusionOf(Type type)
        {
            return TypeAnalysis.GetMethodInfusion(type, createMethodInfusion);
        }
        private IInfusion CreateMethodInfusion(Type type)
        {
            var request = decision.RequestMethodInjection(type);

            return request.ParameterList.Length == 0 ? InfuseNothing.Instance : operation.CreateMethodInfusion(request);
        }
    }
}
