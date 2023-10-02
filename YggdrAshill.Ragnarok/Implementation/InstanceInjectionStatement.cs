using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public sealed class InstanceInjectionStatement : IInstanceInjection, IStatement
    {
        private readonly Func<IInstantiation> createInstantiation;
        private readonly TypeAssignmentStatement assignment;
        private readonly InstanceInjectionSource injection;

        public InstanceInjectionStatement(ICompilation compilation, Type type, Lifetime lifetime, Ownership ownership, Func<IInstantiation> createInstantiation)
        {
            this.createInstantiation = createInstantiation;
            assignment = new TypeAssignmentStatement(type, lifetime, ownership, CreateInstantiation);
            injection = new InstanceInjectionSource(type, compilation);
        }

        public InstanceInjectionStatement(ICompilation compilation, Type type, Lifetime lifetime, Ownership ownership, IInstantiation instantiation)
            : this(compilation, type, lifetime, ownership, () => instantiation)
        {

        }

        public Type ImplementedType => assignment.ImplementedType;
        public IReadOnlyList<Type> AssignedTypeList => assignment.AssignedTypeList;
        public Lifetime Lifetime => assignment.Lifetime;
        public Ownership Ownership => assignment.Ownership;
        public IInstantiation Instantiation => assignment.Instantiation;

        private IInstantiation CreateInstantiation()
        {
            var instantiation = createInstantiation.Invoke();

            return injection.CreateInstantiation(instantiation);
        }

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
            injection.AddMethodParameter(parameter);

            return this;
        }

        public IMethodInjection WithMethodInjection()
        {
            injection.CreateMethodParameterBuffer();

            return this;
        }

        public IPropertyInjection WithProperty(IParameter parameter)
        {
            injection.AddPropertyParameter(parameter);

            return this;
        }

        public IPropertyInjection WithPropertyInjection()
        {
            injection.CreatePropertyParameterBuffer();

            return this;
        }

        public IFieldInjection WithField(IParameter parameter)
        {
            injection.AddFieldParameter(parameter);

            return this;
        }

        public IFieldInjection WithFieldInjection()
        {
            injection.CreateFieldParameterBuffer();

            return this;
        }
    }
}
