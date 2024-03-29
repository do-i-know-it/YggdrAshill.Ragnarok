﻿using System.Collections.Generic;

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
            var activation = interpretation.ActivationOf(request.ImplementedType);

            var dependency = activation.Dependency;

            if (dependency.DependentTypeList.Count == 0)
            {
                return new InstantiateToActivate(activation);
            }

            var realization = dependency.CreateRealization(request.ParameterList);

            return new InstantiateToRealizeThenActivate(realization, activation);
        }

        public IInjection CreateFieldInjection(DependencyInjectionRequest request)
        {
            var infusion = interpretation.FieldInfusionOf(request.ImplementedType);

            return CreateInjection(infusion, request.ParameterList);
        }

        public IInjection CreatePropertyInjection(DependencyInjectionRequest request)
        {
            var infusion = interpretation.PropertyInfusionOf(request.ImplementedType);

            return CreateInjection(infusion, request.ParameterList);
        }

        public IInjection CreateMethodInjection(DependencyInjectionRequest request)
        {
            var infusion = interpretation.MethodInfusionOf(request.ImplementedType);

            return CreateInjection(infusion, request.ParameterList);
        }

        private static IInjection CreateInjection(IInfusion infusion, IReadOnlyList<IParameter> parameterList)
        {
            var dependency = infusion.Dependency;

            if (dependency.DependentTypeList.Count == 0)
            {
                return InjectIntoNothing.Instance;
            }

            var realization = dependency.CreateRealization(parameterList);

            return new InjectToRealizeThenInfuse(realization, infusion);
        }
    }
}
