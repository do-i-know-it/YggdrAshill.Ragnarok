using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public sealed class DependencyInjectionStatement : IDependencyInjection, IStatement
    {
        private readonly TypeAssignmentStatement assignment;
        private readonly InstanceInjectionSource instanceInjection;
        private readonly DependencyInjectionSource dependencyInjection;

        public DependencyInjectionStatement(ICompilation compilation, Type implementedType, Lifetime lifetime)
        {
            assignment = new TypeAssignmentStatement(implementedType, lifetime, Ownership.Internal, CreateInstantiation);
            instanceInjection = new InstanceInjectionSource(implementedType, compilation);
            dependencyInjection = new DependencyInjectionSource(implementedType, compilation);
        }

        private IInstantiation CreateInstantiation()
        {
            var instantiation = dependencyInjection.CreateInstantiation();

            return instanceInjection.CreateInstantiation(instantiation);
        }

        public Type ImplementedType => assignment.ImplementedType;

        public IReadOnlyList<Type> AssignedTypeList => assignment.AssignedTypeList;

        public Lifetime Lifetime => assignment.Lifetime;

        public Ownership Ownership => assignment.Ownership;

        public IInstantiation Instantiation => assignment.Instantiation;

        public void AsOwnSelf()
        {
            assignment.AsOwnSelf();
        }
        public IInheritedTypeAssignment As(Type inheritedType)
        {
            return assignment.As(inheritedType);
        }
        public IOwnTypeAssignment AsImplementedInterfaces()
        {
            return assignment.AsImplementedInterfaces();
        }

        public IMethodInjection WithMethodArgument(IParameter parameter)
        {
            instanceInjection.AddMethodParameter(parameter);

            return this;
        }

        public IMethodInjection WithMethodInjection()
        {
            instanceInjection.CreateMethodParameterBuffer();

            return this;
        }

        public IPropertyInjection WithProperty(IParameter parameter)
        {
            instanceInjection.AddPropertyParameter(parameter);

            return this;
        }

        public IPropertyInjection WithPropertyInjection()
        {
            instanceInjection.CreatePropertyParameterBuffer();

            return this;
        }

        public IFieldInjection WithField(IParameter parameter)
        {
            instanceInjection.AddFieldParameter(parameter);

            return this;
        }

        public IFieldInjection WithFieldInjection()
        {
            instanceInjection.CreateFieldParameterBuffer();

            return this;
        }

        public IDependencyInjection WithArgument(IParameter parameter)
        {
            dependencyInjection.AddArgument(parameter);

            return this;
        }
    }
}
