using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public sealed class DependencyInjectionStatement : IDependencyInjection, IStatement
    {
        private readonly ICompilation compilation;
        private readonly InstanceInjectionStatement injection;

        public DependencyInjectionStatement(ICompilation compilation, Type implementedType, Lifetime lifetime)
        {
            this.compilation = compilation;
            injection = new InstanceInjectionStatement(compilation, implementedType, lifetime, Ownership.Internal, CreateInstantiation);
        }

        private List<IParameter>? parameterList;

        private IInstantiation CreateInstantiation()
        {
            var activation = compilation.CreateActivation(ImplementedType);

            return parameterList == null
                ? new ActivateToInstantiateWithoutParameterList(activation)
                : new ActivateToInstantiateWithParameterList(activation, parameterList);
        }

        public Type ImplementedType => injection.ImplementedType;
        public IReadOnlyList<Type> AssignedTypeList => injection.AssignedTypeList;
        public Lifetime Lifetime => injection.Lifetime;
        public Ownership Ownership => injection.Ownership;
        public IInstantiation Instantiation => injection.Instantiation;

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

        public IDependencyInjection WithArgument(IParameter parameter)
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
