using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Fabrication
{
    // TODO: add document comments.
    public sealed class ConstructorDependencyInjection : IConstructorDependencyInjection
    {
        private readonly ICompilationV2 compilation;
        private readonly InstanceDependencyInjection injection;

        public ConstructorDependencyInjection(ICompilationV2 compilation, Type implementedType)
        {
            this.compilation = compilation;
            injection = new InstanceDependencyInjection(compilation, implementedType);
        }

        private List<IParameter>? parameterList = new List<IParameter>();

        public Type ImplementedType => injection.ImplementedType;
        public IReadOnlyList<Type> AssignedTypeList => injection.AssignedTypeList;

        public IInstantiationV2 GetInstantiation()
        {
            var activation = compilation.GetActivation(injection.ImplementedType);

            var instantiation = parameterList == null ?
                new ActivateToInstantiate(activation, Array.Empty<IParameter>()) :
                new ActivateToInstantiate(activation, parameterList);

            var candidate = injection.GetInjection();

            if (candidate == null)
            {
                return instantiation;
            }

            return new InstantiateWithInjection(instantiation, candidate);
        }

        public void AsSelf()
        {
            injection.AsSelf();
        }
        public IAssignImplementedInterface As(Type implementedInterface)
        {
            return injection.As(implementedInterface);
        }
        public IAssignImplementedType AsImplementedInterfaces()
        {
            return injection.AsImplementedInterfaces();
        }

        public IMethodDependencyInjection WithMethodArgument(IParameter parameter)
        {
            return injection.WithMethodArgument(parameter);
        }

        public IPropertyDependencyInjection WithProperty(IParameter parameter)
        {
            return injection.WithProperty(parameter);
        }

        public IInstanceDependencyInjection WithField(IParameter parameter)
        {
            return injection.WithField(parameter);
        }

        public IConstructorDependencyInjection WithArgument(IParameter parameter)
        {
            if (parameterList == null)
            {
                parameterList = new List<IParameter>();
            }

            if (!parameterList.Contains(parameter))
            {
                parameterList.Add(parameter);
            }

            return this;
        }
    }
}
