﻿using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public sealed class InstanceInjectionSource : IInstanceInjection
    {
        private readonly Type type;
        private readonly ICompilation compilation;
        private readonly IRegistration registration;
        private readonly TypeAssignmentSource source;

        public InstanceInjectionSource(Type type, IObjectContainer container)
        {
            this.type = type;
            compilation = container.Compilation;
            registration = container.Registration;
            source = new TypeAssignmentSource(type);
        }

        private List<IParameter>? fieldParameterList;
        private List<IParameter>? propertyParameterList;
        private List<IParameter>? methodParameterList;

        public bool CanInjectIntoInstance(out IInjection injection)
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

            var request = fieldParameterList == null ?
                new DependencyInjectionRequest(type) :
                new DependencyInjectionRequest(type, fieldParameterList.ToArray());

            injection = compilation.CreateFieldInjection(request);

            return true;
        }

        private bool CanInjectIntoProperty(out IInjection injection)
        {
            injection = default!;

            if (propertyParameterList == null)
            {
                return false;
            }

            var request = propertyParameterList == null ?
                new DependencyInjectionRequest(type) :
                new DependencyInjectionRequest(type, propertyParameterList.ToArray());

            injection = compilation.CreatePropertyInjection(request);

            return true;
        }

        private bool CanInjectIntoMethod(out IInjection injection)
        {
            injection = default!;

            if (methodParameterList == null)
            {
                return false;
            }

            var request = methodParameterList == null ?
                new DependencyInjectionRequest(type) :
                new DependencyInjectionRequest(type, methodParameterList.ToArray());

            injection = compilation.CreateMethodInjection(request);

            return true;
        }

        public Type ImplementedType => source.ImplementedType;

        public IReadOnlyList<Type> AssignedTypeList => source.AssignedTypeList;

        public void AsOwnSelf()
        {
            source.AsOwnSelf();
        }

        public IInheritedTypeAssignment As(Type inheritedType)
        {
            return source.As(inheritedType);
        }

        public IOwnTypeAssignment AsImplementedInterfaces()
        {
            return source.AsImplementedInterfaces();
        }

        public IParameterMethodInjection WithMethod(IParameter parameter)
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

        public ITypeAssignment WithMethodInjection()
        {
            if (methodParameterList == null)
            {
                methodParameterList = new List<IParameter>();
            }

            return this;
        }

        public IParameterPropertyInjection WithProperty(IParameter parameter)
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

        public IMethodInjection WithPropertyInjection()
        {
            if (propertyParameterList == null)
            {
                propertyParameterList = new List<IParameter>();
            }

            return this;
        }

        public IParameterFieldInjection WithField(IParameter parameter)
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

        public IPropertyInjection WithFieldInjection()
        {
            if (fieldParameterList == null)
            {
                fieldParameterList = new List<IParameter>();
            }

            return this;
        }

        public IFieldInjection ResolvedImmediately()
        {
            var execution = new ExecuteToResolveImmediately(source);

            registration.Register(execution);

            return this;
        }
    }
}
