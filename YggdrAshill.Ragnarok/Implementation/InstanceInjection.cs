using YggdrAshill.Ragnarok.Construction;
using YggdrAshill.Ragnarok.Fabrication;
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    public sealed class InstanceInjection :
        IInjectIntoMethodExternally,
        IInjectIntoPropertiesExternally,
        IInjectIntoFieldsExternally,
        IInstanceInjection
    {
        private readonly ICompilation compilation;
        private readonly TypeAssignment typeAssignment;

        public InstanceInjection(ICompilation compilation, Type implementedType)
        {
            this.compilation = compilation;
            typeAssignment = new TypeAssignment(implementedType);
        }

        private List<IParameter>? fieldParameterList;
        private List<IParameter>? methodParameterList;
        private List<IParameter>? propertyParameterList;

        public Type ImplementedType => typeAssignment.ImplementedType;
        public IReadOnlyList<Type> AssignedTypeList => typeAssignment.AssignedTypeList;

        public IInjection? GetInjection()
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

            return compilation.GetFieldInjection(typeAssignment.ImplementedType, fieldParameterList);
        }
        private IInjection? GetPropertyInjection()
        {
            if (propertyParameterList == null)
            {
                return null;
            }

            return compilation.GetPropertyInjection(typeAssignment.ImplementedType, propertyParameterList);
        }
        private IInjection? GetMethodInjection()
        {
            if (methodParameterList == null)
            {
                return null;
            }

            return compilation.GetMethodInjection(typeAssignment.ImplementedType, methodParameterList);
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

        public IInjectIntoMethodExternally WithMethodInjected()
        {
            if (!ValidateType.IsInstantiatable(ImplementedType))
            {
                // TODO: throw original exception.
                throw new Exception($"{ImplementedType} is not instantiatable.");
            }

            methodParameterList = new List<IParameter>();

            return this;
        }
        IInjectIntoMethodExternally IInjectIntoMethodExternally.From(IParameter parameter)
        {
            // TODO: throw original exception.
            if (methodParameterList == null)
            {
                throw new InvalidOperationException();
            }

            if (!methodParameterList.Contains(parameter))
            {
                methodParameterList.Add(parameter);
            }

            return this;
        }

        public IInjectIntoPropertiesExternally WithPropertiesInjected()
        {
            if (!ValidateType.IsInstantiatable(ImplementedType))
            {
                // TODO: throw original exception.
                throw new Exception($"{ImplementedType} is not instantiatable.");
            }

            propertyParameterList = new List<IParameter>();

            return this;
        }
        IInjectIntoPropertiesExternally IInjectIntoPropertiesExternally.From(IParameter parameter)
        {
            // TODO: throw original exception.
            if (propertyParameterList == null)
            {
                throw new InvalidOperationException();
            }

            if (!propertyParameterList.Contains(parameter))
            {
                propertyParameterList.Add(parameter);
            }

            return this;
        }

        public IInjectIntoFieldsExternally WithFieldsInjected()
        {
            if (!ValidateType.IsInstantiatable(ImplementedType))
            {
                // TODO: throw original exception.
                throw new Exception($"{ImplementedType} is not instantiatable.");
            }

            fieldParameterList = new List<IParameter>();

            return this;
        }
        IInjectIntoFieldsExternally IInjectIntoFieldsExternally.From(IParameter parameter)
        {
            // TODO: throw original exception.
            if (fieldParameterList == null)
            {
                throw new InvalidOperationException();
            }

            if (!fieldParameterList.Contains(parameter))
            {
                fieldParameterList.Add(parameter);
            }

            return this;
        }
    }
}
