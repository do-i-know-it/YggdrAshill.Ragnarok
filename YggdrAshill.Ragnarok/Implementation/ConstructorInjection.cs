using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Fabrication
{
    // TODO: add document comments.
    public sealed class ConstructorInjection : IConstructorInjection
    {
        private readonly ICompilation compilation;
        private readonly InstanceInjection injection;

        public ConstructorInjection(ICompilation compilation, Type implementedType)
        {
            this.compilation = compilation;
            injection = new InstanceInjection(compilation, implementedType);
        }

        private List<IParameter>? parameterList = new List<IParameter>();

        public Type ImplementedType => injection.ImplementedType;
        public IReadOnlyList<Type> AssignedTypeList => injection.AssignedTypeList;

        public IInstantiation GetInstantiation()
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

        public IMethodInjection WithMethodArgument(IParameter parameter)
        {
            return injection.WithMethodArgument(parameter);
        }

        public IMethodInjection WithMethodInjection()
        {
            return injection.WithMethodInjection();
        }

        public IPropertyInjection WithProperty(IParameter parameter)
        {
            return injection.WithProperty(parameter);
        }

        public IPropertyInjection WithPropertyInjection()
        {
            return injection.WithPropertyInjection();
        }

        public IFieldInjection WithField(IParameter parameter)
        {
            return injection.WithField(parameter);
        }

        public IFieldInjection WithFieldInjection()
        {
            return injection.WithFieldInjection();
        }

        public IConstructorInjection WithArgument(IParameter parameter)
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
