using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Fabrication
{
    // TODO: add document comments.
    public sealed class InstanceInjectionStatement : IInstanceInjection, IStatement
    {
        private readonly ICompilation compilation;
        private readonly Func<IInstantiation> createInstantiation;
        private readonly TypeAssignmentStatement typeAssignment;

        public InstanceInjectionStatement(ICompilation compilation, Type type, Lifetime lifetime, Ownership ownership, Func<IInstantiation> createInstantiation)
        {
            this.compilation = compilation;
            this.createInstantiation = createInstantiation;
            typeAssignment = new TypeAssignmentStatement(type, lifetime, ownership, CreateInstantiation);
        }

        public InstanceInjectionStatement(ICompilation compilation, Type type, Lifetime lifetime, Ownership ownership, IInstantiation instantiation)
            : this(compilation, type, lifetime, ownership, () => instantiation)
        {

        }

        public Type ImplementedType => typeAssignment.ImplementedType;
        public IReadOnlyList<Type> AssignedTypeList => typeAssignment.AssignedTypeList;
        public Lifetime Lifetime => typeAssignment.Lifetime;
        public Ownership Ownership => typeAssignment.Ownership;
        public IInstantiation Instantiation => typeAssignment.Instantiation;

        private List<IParameter>? fieldParameterList;
        private List<IParameter>? methodParameterList;
        private List<IParameter>? propertyParameterList;

        private IInstantiation CreateInstantiation()
        {
            var instantiation = createInstantiation.Invoke();

            var candidate = GetInjection();

            return candidate == null ? instantiation : new InstantiateWithInjection(instantiation, candidate);
        }

        private IInjection? GetInjection()
        {
            var fieldInjection = GetFieldInjection();
            var propertyInjection = GetPropertyInjection();
            var methodInjection = GetMethodInjection();

            if (fieldInjection == null)
            {
                if (propertyInjection == null)
                {
                    if (methodInjection == null)
                    {
                        return null;
                    }

                    return methodInjection;
                }

                if (methodInjection == null)
                {
                    return propertyInjection;
                }

                return new InjectWithTwoInjection(propertyInjection, methodInjection);
            }

            if (propertyInjection == null)
            {
                if (methodInjection == null)
                {
                    return fieldInjection;
                }

                return new InjectWithTwoInjection(fieldInjection, methodInjection);
            }

            if (methodInjection == null)
            {
                return new InjectWithTwoInjection(fieldInjection, propertyInjection);
            }

            return new InjectWithThreeInjection(fieldInjection, propertyInjection, methodInjection);
        }
        private IInjection? GetFieldInjection()
        {
            if (fieldParameterList == null)
            {
                return null;
            }

            var infusion = compilation.GetFieldInfusion(typeAssignment.ImplementedType);

            return new InfuseToInject(infusion, fieldParameterList);
        }
        private IInjection? GetPropertyInjection()
        {
            if (propertyParameterList == null)
            {
                return null;
            }

            var infusion = compilation.GetPropertyInfusion(typeAssignment.ImplementedType);

            return new InfuseToInject(infusion, propertyParameterList);
        }
        private IInjection? GetMethodInjection()
        {
            if (methodParameterList == null)
            {
                return null;
            }

            var infusion = compilation.GetMethodInfusion(typeAssignment.ImplementedType);

            return new InfuseToInject(infusion, methodParameterList);
        }

        public void AsSelf()
        {
            typeAssignment.AsSelf();
        }
        public IAssignImplementedInterface As(Type implementedInterface)
        {
            return typeAssignment.As(implementedInterface);
        }
        public IAssignImplementedType AsImplementedInterfaces()
        {
            return typeAssignment.AsImplementedInterfaces();
        }

        public IMethodInjection WithMethodArgument(IParameter parameter)
        {
            if (methodParameterList == null)
            {
                methodParameterList = new List<IParameter>();
            }

            if (!methodParameterList.Contains(parameter))
            {
                methodParameterList.Add(parameter);
            }

            return this;
        }

        public IMethodInjection WithMethodInjection()
        {
            if (methodParameterList == null)
            {
                methodParameterList = new List<IParameter>();
            }

            return this;
        }

        public IPropertyInjection WithProperty(IParameter parameter)
        {
            if (propertyParameterList == null)
            {
                propertyParameterList = new List<IParameter>();
            }

            if (!propertyParameterList.Contains(parameter))
            {
                propertyParameterList.Add(parameter);
            }

            return this;
        }

        public IPropertyInjection WithPropertyInjection()
        {
            if (propertyParameterList == null)
            {
                propertyParameterList = new List<IParameter>();
            }

            return this;
        }

        public IFieldInjection WithField(IParameter parameter)
        {
            if (fieldParameterList == null)
            {
                fieldParameterList = new List<IParameter>();
            }

            if (!fieldParameterList.Contains(parameter))
            {
                fieldParameterList.Add(parameter);
            }

            return this;
        }

        public IFieldInjection WithFieldInjection()
        {
            if (fieldParameterList == null)
            {
                fieldParameterList = new List<IParameter>();
            }

            return this;
        }
    }
}
