using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class Compilation : ICompilation
    {
        private readonly IInterpretation interpretation;

        public Compilation(IInterpretation interpretation)
        {
            this.interpretation = interpretation;
        }

        public IInstantiation CreateInstantiation(DependencyInjectionRequest request)
        {
            var instantiationRequest = interpretation.GetInstantiationRequest(request.ImplementedType);

            var dependency = instantiationRequest.Dependency;
            var activation = instantiationRequest.Activation;
            if (dependency.DependentTypeList.Count == 0)
            {
                return new InstantiateToActivate(activation);
            }

            var realization = dependency.CreateRealization(request.ParameterList);
            return new InstantiateToRealizeThenActivate(realization, activation);
        }

        public IInjection CreateFieldInjection(DependencyInjectionRequest request)
        {
            var injection = interpretation.GetFieldInjectionRequest(request.ImplementedType);
            return CreateInjection(injection, request.ParameterList);
        }

        public IInjection CreatePropertyInjection(DependencyInjectionRequest request)
        {
            var injectionRequest = interpretation.GetPropertyInjectionRequest(request.ImplementedType);
            return CreateInjection(injectionRequest, request.ParameterList);
        }

        public IInjection CreateMethodInjection(DependencyInjectionRequest request)
        {
            var injectionRequest = interpretation.GetMethodInjectionRequest(request.ImplementedType);
            return CreateInjection(injectionRequest, request.ParameterList);
        }

        private static IInjection CreateInjection(InjectionRequest request, IReadOnlyList<IParameter> parameterList)
        {
            var dependency = request.Dependency;
            var infusion = request.Infusion;
            if (dependency.DependentTypeList.Count == 0)
            {
                return InjectIntoNothing.Instance;
            }

            var realization = dependency.CreateRealization(parameterList);
            return new InjectToRealizeThenInfuse(realization, infusion);
        }
    }
}
