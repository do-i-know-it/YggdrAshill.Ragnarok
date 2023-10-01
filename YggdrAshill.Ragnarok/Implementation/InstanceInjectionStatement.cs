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

            if (!CanInjectIntoInstance(out var injection))
            {
                return instantiation;
            }

            return new InstantiateAntInject(instantiation, injection);
        }

        private bool CanInjectIntoInstance(out IInjection injection)
        {
            if (CanInjectIntoField(out injection))
            {
                // found field injection.
                var fieldInjection = injection;

                if (CanInjectIntoProperty(out injection))
                {
                    // found property injection.
                    var propertyInjection = injection;

                    if (CanInjectIntoMethod(out injection))
                    {
                        // found method injection.
                        injection = new ThreeInjection(fieldInjection, propertyInjection, injection);
                    }
                    else
                    {
                        injection = new TwoInjection(fieldInjection, propertyInjection);
                    }
                }
                else if (CanInjectIntoMethod(out injection))
                {
                    // found method injection.
                    injection = new TwoInjection(fieldInjection, injection);
                }
                else
                {
                    injection = fieldInjection;
                }

                return true;
            }

            if (CanInjectIntoProperty(out injection))
            {
                // found property injection.
                var propertyInjection = injection;

                if (CanInjectIntoMethod(out injection))
                {
                    // found method injection.
                    injection = new TwoInjection(propertyInjection, injection);
                }
                else
                {
                    injection = propertyInjection;
                }

                return true;
            }

            return CanInjectIntoMethod(out injection);
        }

        private bool CanInjectIntoField(out IInjection injection)
        {
            injection = default!;

            if (fieldParameterList == null)
            {
                return false;
            }

            var infusion = compilation.GetFieldInfusion(typeAssignment.ImplementedType);

            if (fieldParameterList.Count == 0)
            {
                injection = new InfuseToInjectWithoutParameterList(infusion);
            }
            else
            {
                injection = new InfuseToInjectWithParameterList(infusion, fieldParameterList);
            }

            return true;
        }

        private bool CanInjectIntoProperty(out IInjection injection)
        {
            injection = default!;

            if (propertyParameterList == null)
            {
                return false;
            }

            var infusion = compilation.GetPropertyInfusion(typeAssignment.ImplementedType);

            if (propertyParameterList.Count == 0)
            {
                injection = new InfuseToInjectWithoutParameterList(infusion);
            }
            else
            {
                injection = new InfuseToInjectWithParameterList(infusion, propertyParameterList);
            }

            return true;
        }

        private bool CanInjectIntoMethod(out IInjection injection)
        {
            injection = default!;

            if (methodParameterList == null)
            {
                return false;
            }

            var infusion = compilation.GetMethodInfusion(typeAssignment.ImplementedType);

            if (methodParameterList.Count == 0)
            {
                injection = new InfuseToInjectWithoutParameterList(infusion);
            }
            else
            {
                injection = new InfuseToInjectWithParameterList(infusion, methodParameterList);
            }

            return true;
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
