using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Fabrication
{
    // TODO: add document comments.
    public sealed class InstanceDependencyInjection : IInstanceDependencyInjection
    {
        private readonly ICompilationV2 compilation;
        private readonly TypeAssignment typeAssignment;

        public InstanceDependencyInjection(ICompilationV2 compilation, Type implementedType)
        {
            this.compilation = compilation;
            typeAssignment = new TypeAssignment(implementedType);
        }

        private List<IParameter>? fieldParameterList;
        private List<IParameter>? methodParameterList;
        private List<IParameter>? propertyParameterList;

        public Type ImplementedType => typeAssignment.ImplementedType;
        public IReadOnlyList<Type> AssignedTypeList => typeAssignment.AssignedTypeList;

        public IInjectionV2? GetInjection()
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
        private IInjectionV2? GetFieldInjection()
        {
            if (fieldParameterList == null)
            {
                return null;
            }

            var infusion = compilation.GetFieldInfusion(typeAssignment.ImplementedType);

            return new InfuseToInject(infusion, fieldParameterList);
        }
        private IInjectionV2? GetPropertyInjection()
        {
            if (propertyParameterList == null)
            {
                return null;
            }

            var infusion = compilation.GetPropertyInfusion(typeAssignment.ImplementedType);

            return new InfuseToInject(infusion, propertyParameterList);
        }
        private IInjectionV2? GetMethodInjection()
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

        public IMethodDependencyInjection WithMethodArgument(IParameter parameter)
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
        public IPropertyDependencyInjection WithProperty(IParameter parameter)
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
        public IInstanceDependencyInjection WithField(IParameter parameter)
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
    }
}
