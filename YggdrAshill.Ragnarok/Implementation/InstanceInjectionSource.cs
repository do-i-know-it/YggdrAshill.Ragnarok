﻿using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public sealed class InstanceInjectionSource
    {
        private readonly Type type;
        private readonly ICompilation compilation;

        public InstanceInjectionSource(Type type, ICompilation compilation)
        {
            this.type = type;
            this.compilation = compilation;
        }

        private List<IParameter>? fieldParameterList;
        private List<IParameter>? propertyParameterList;
        private List<IParameter>? methodParameterList;

        public void CreateFieldParameterBuffer()
        {
            if (fieldParameterList == null)
            {
                fieldParameterList = new List<IParameter>();
            }
        }

        public void AddFieldParameter(IParameter parameter)
        {
            if (fieldParameterList == null)
            {
                fieldParameterList = new List<IParameter>();
            }

            if (!fieldParameterList.Contains(parameter))
            {
                fieldParameterList.Add(parameter);
            }
        }

        public void CreatePropertyParameterBuffer()
        {
            if (propertyParameterList == null)
            {
                propertyParameterList = new List<IParameter>();
            }
        }

        public void AddPropertyParameter(IParameter parameter)
        {
            if (propertyParameterList == null)
            {
                propertyParameterList = new List<IParameter>();
            }

            if (!propertyParameterList.Contains(parameter))
            {
                propertyParameterList.Add(parameter);
            }
        }

        public void CreateMethodParameterBuffer()
        {
            if (methodParameterList == null)
            {
                methodParameterList = new List<IParameter>();
            }
        }

        public void AddMethodParameter(IParameter parameter)
        {
            if (methodParameterList == null)
            {
                methodParameterList = new List<IParameter>();
            }

            if (!methodParameterList.Contains(parameter))
            {
                methodParameterList.Add(parameter);
            }
        }

        public IInstantiation CreateInstantiation(IInstantiation instantiation)
        {
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

            var infusion = compilation.GetFieldInfusion(type);

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

            var infusion = compilation.GetPropertyInfusion(type);

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

            var infusion = compilation.GetMethodInfusion(type);

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
    }
}
