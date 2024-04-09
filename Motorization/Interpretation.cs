using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class Interpretation : IInterpretation
    {
        private readonly IDecision decision;
        private readonly IOperation operation;

        private readonly Func<Type, InstantiationRequest> createActivation;
        private readonly Func<Type, InjectionRequest> createFieldInfusion;
        private readonly Func<Type, InjectionRequest> createPropertyInfusion;
        private readonly Func<Type, InjectionRequest> createMethodInfusion;

        public Interpretation(IDecision decision, IOperation operation)
        {
            this.decision = decision;
            this.operation = operation;

            createActivation = CreateActivation;
            createFieldInfusion = CreateFieldInfusion;
            createPropertyInfusion = CreatePropertyInfusion;
            createMethodInfusion = CreateMethodInfusion;
        }

        public InstantiationRequest GetInstantiationRequest(Type type)
        {
            return TypeAnalysis.GetInstantiationRequest(type, createActivation);
        }
        private InstantiationRequest CreateActivation(Type type)
        {
            if (CollectionDescription.CanResolve(type, out var arrayElementType))
            {
                var collectionActivation = operation.CreateCollectionActivation(arrayElementType);
                return new InstantiationRequest(WithoutDependency.Instance, collectionActivation);
            }

            if (ServiceBundleDescription.CanResolve(type, out var bundleElementType))
            {
                var bundleRequest = decision.RequestServiceBundleInjection(bundleElementType);
                var bundleActivation = operation.CreateActivation(bundleRequest);
                return new InstantiationRequest(bundleRequest.Dependency, bundleActivation);
            }

            var request = decision.RequestDependencyInjection(type);
            var activation = operation.CreateActivation(request);
            return new InstantiationRequest(request.Dependency, activation);
        }

        public InjectionRequest GetFieldInjectionRequest(Type type)
        {
            return TypeAnalysis.GetFieldInjectionRequest(type, createFieldInfusion);
        }
        private InjectionRequest CreateFieldInfusion(Type type)
        {
            var request = decision.RequestFieldInjection(type);
            var infusion = request.FieldList.Length == 0 ? InfuseNothing.Instance : operation.CreateFieldInfusion(request);
            return new InjectionRequest(request.Dependency, infusion);
        }

        public InjectionRequest GetPropertyInjectionRequest(Type type)
        {
            return TypeAnalysis.GetPropertyInjectionRequest(type, createPropertyInfusion);
        }
        private InjectionRequest CreatePropertyInfusion(Type type)
        {
            var request = decision.RequestPropertyInjection(type);
            var infusion = request.PropertyList.Length == 0 ? InfuseNothing.Instance : operation.CreatePropertyInfusion(request);
            return new InjectionRequest(request.Dependency, infusion);
        }

        public InjectionRequest GetMethodInjectionRequest(Type type)
        {
            return TypeAnalysis.GetMethodInjectionRequest(type, createMethodInfusion);
        }
        private InjectionRequest CreateMethodInfusion(Type type)
        {
            var request = decision.RequestMethodInjection(type);
            var infusion = request.ParameterList.Length == 0 ? InfuseNothing.Instance : operation.CreateMethodInfusion(request);
            return new InjectionRequest(request.Dependency, infusion);
        }
    }
}
