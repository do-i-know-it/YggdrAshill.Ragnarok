using YggdrAshill.Ragnarok.Construction;
using YggdrAshill.Ragnarok.Fabrication;
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class InjectIntoInstanceToCompose :
        IInjectIntoInstance,
        IComposition
    {
        private readonly ICompilation compilation;
        private readonly Lifetime lifetime;
        private readonly Ownership ownership;
        private readonly IInstantiation instantiation;
        private readonly AssignedTypeCollection collection;

        private readonly Lazy<AssignAnyTypeToCompose> cache;

        public InjectIntoInstanceToCompose(ICompilation compilation, AssignedTypeCollection collection, Lifetime lifetime, Ownership ownership, IInstantiation instantiation)
        {
            this.compilation = compilation;
            this.collection = collection;
            this.lifetime = lifetime;
            this.ownership = ownership;
            this.instantiation = instantiation;

            cache = new Lazy<AssignAnyTypeToCompose>(CreateAssignAnyTypeToCompose);
        }

        public InjectIntoInstanceToCompose(ICompilation compilation, Type implementedType, Lifetime lifetime, Ownership ownership, IInstantiation instantiation)
            : this(compilation, new AssignedTypeCollection(implementedType), lifetime, ownership, instantiation)
        {

        }

        public InjectIntoInstanceToCompose(ICompilation compilation, Type implementedType, Lifetime lifetime, Ownership ownership, Func<object> onInstantiated)
            : this(compilation, new AssignedTypeCollection(implementedType), lifetime, ownership, new InstantiateWithFunction(onInstantiated))
        {

        }

        private Func<ICompilation, Type, IInjection>? getFieldInjection;
        private Func<ICompilation, Type, IInjection>? getPropertyInjection;
        private Func<ICompilation, Type, IInjection>? getMethodInjection;

        private AssignAnyTypeToCompose CreateAssignAnyTypeToCompose()
        {
            var createdInstantiation = CreateInstantiation();

            return new AssignAnyTypeToCompose(collection, lifetime, ownership, createdInstantiation);
        }

        private IInstantiation CreateInstantiation()
        {
            var injection = GetInjection();

            if (injection == null)
            {
                return instantiation;
            }

            return new InstantiateAndInjectIntoInstance(instantiation, injection);
        }

        private IInjection? GetInjection()
        {
            var fieldInjection = getFieldInjection?.Invoke(compilation, collection.ImplementedType);
            var propertyInjection = getPropertyInjection?.Invoke(compilation, collection.ImplementedType);
            var methodInjection = getMethodInjection?.Invoke(compilation, collection.ImplementedType);

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

        public IDescription Compose()
        {
            return cache.Value.Compose();
        }

        public IAfterAnyTypeAssigned As<T>()
            where T : notnull
        {
            collection.Add(typeof(T));

            return new AfterAnyTypeAssigned(collection);
        }

        public IAfterImplementedTypeAssigned AsSelf()
        {
            collection.AddSelf();

            return new AfterImplementedTypeAssigned(collection);
        }

        public IAfterImplementedInterfacesAssigned AsImplementedInterfaces()
        {
            collection.AddImplementedInterfaces();

            return new AfterImplementedInterfacesAssigned(collection);
        }

        public IInjectIntoField WithFieldInjection()
        {
            return new AfterFieldInjectionAdded(this);
        }

        public IInjectIntoProperty WithPropertyInjection()
        {
            return new AfterPropertyInjectionAdded(this);
        }

        public IInjectIntoMethod WithMethodInjection()
        {
            return new AfterMethodInjectionAdded(this);
        }

        private sealed class InstantiateAndInjectIntoInstance :
            IInstantiation
        {
            private readonly IInstantiation instantiation;
            private readonly IInjection injection;

            public InstantiateAndInjectIntoInstance(IInstantiation instantiation, IInjection injection)
            {
                this.instantiation = instantiation;
                this.injection = injection;
            }

            public object Instantiate(IResolver resolver)
            {
                var instance = instantiation.Instantiate(resolver);

                injection.Inject(resolver, instance);

                return instance;
            }
        }


        private sealed class InjectWithTwoInjection :
            IInjection
        {
            private readonly IInjection first;
            private readonly IInjection second;

            public InjectWithTwoInjection(IInjection first, IInjection second)
            {
                this.first = first;
                this.second = second;
            }

            public void Inject(IResolver resolver, object instance)
            {
                first.Inject(resolver, instance);
                second.Inject(resolver, instance);
            }
        }
        private sealed class InjectWithThreeInjection :
            IInjection
        {
            private readonly IInjection first;
            private readonly IInjection second;
            private readonly IInjection third;

            public InjectWithThreeInjection(IInjection first, IInjection second, IInjection third)
            {
                this.first = first;
                this.second = second;
                this.third = third;
            }

            public void Inject(IResolver resolver, object instance)
            {
                first.Inject(resolver, instance);
                second.Inject(resolver, instance);
                third.Inject(resolver, instance);
            }
        }

        private sealed class AfterFieldInjectionAdded :
            IInjectIntoField
        {
            private readonly InjectIntoInstanceToCompose injectIntoInstanceToCompose;

            public AfterFieldInjectionAdded(InjectIntoInstanceToCompose injectIntoInstanceToCompose)
            {
                this.injectIntoInstanceToCompose = injectIntoInstanceToCompose;

                injectIntoInstanceToCompose.getFieldInjection = GetFieldInjection;

            }

            private readonly Lazy<List<IParameter>> parameterList = new Lazy<List<IParameter>>();

            private IInjection GetFieldInjection(ICompilation compilation, Type implementedType)
            {
                return compilation.GetFieldInjection(implementedType, parameterList.Value);
            }

            public IAfterAnyTypeAssigned As<T>()
                where T : notnull
            {
                return injectIntoInstanceToCompose.As<T>();
            }

            public IAfterImplementedTypeAssigned AsSelf()
            {
                return injectIntoInstanceToCompose.AsSelf();
            }

            public IAfterImplementedInterfacesAssigned AsImplementedInterfaces()
            {
                return injectIntoInstanceToCompose.AsImplementedInterfaces();
            }

            public IInjectIntoField With<T>(string name, T instance)
                where T: notnull
            {
                var parameter = new Parameter<T>(name, instance);

                parameterList.Value.Add(parameter);

                return this;
            }

            public IInjectIntoProperty WithPropertyInjection()
            {
                return new AfterPropertyInjectionAdded(injectIntoInstanceToCompose);
            }

            public IInjectIntoMethod WithMethodInjection()
            {
                return new AfterMethodInjectionAdded(injectIntoInstanceToCompose);
            }
        }
        private sealed class AfterPropertyInjectionAdded :
            IInjectIntoProperty
        {
            private readonly InjectIntoInstanceToCompose injectIntoInstanceToCompose;

            public AfterPropertyInjectionAdded(InjectIntoInstanceToCompose injectIntoInstanceToCompose)
            {
                this.injectIntoInstanceToCompose = injectIntoInstanceToCompose;

                injectIntoInstanceToCompose.getPropertyInjection = GetPropertyInjection;
            }

            private readonly List<IParameter> parameterList = new List<IParameter>(0);

            private IInjection GetPropertyInjection(ICompilation compilation, Type implementedType)
            {
                return compilation.GetPropertyInjection(implementedType, parameterList);
            }

            public IAfterAnyTypeAssigned As<T>()
                where T : notnull
            {
                return injectIntoInstanceToCompose.As<T>();
            }

            public IAfterImplementedTypeAssigned AsSelf()
            {
                return injectIntoInstanceToCompose.AsSelf();
            }

            public IAfterImplementedInterfacesAssigned AsImplementedInterfaces()
            {
                return injectIntoInstanceToCompose.AsImplementedInterfaces();
            }

            public IInjectIntoProperty With<T>(string name, T instance)
                where T: notnull
            {
                var parameter = new Parameter<T>(name, instance);

                parameterList.Add(parameter);

                return this;
            }

            public IInjectIntoMethod WithMethodInjection()
            {
                return new AfterMethodInjectionAdded(injectIntoInstanceToCompose);
            }
        }
        private sealed class AfterMethodInjectionAdded :
            IInjectIntoMethod
        {
            private readonly InjectIntoInstanceToCompose injectIntoInstanceToCompose;

            public AfterMethodInjectionAdded(InjectIntoInstanceToCompose injectIntoInstanceToCompose)
            {
                this.injectIntoInstanceToCompose = injectIntoInstanceToCompose;

                injectIntoInstanceToCompose.getMethodInjection = GetMethodInjection;
            }

            private readonly List<IParameter> parameterList = new List<IParameter>(0);

            private IInjection GetMethodInjection(ICompilation compilation, Type implementedType)
            {
                return compilation.GetMethodInjection(implementedType, parameterList);
            }

            public IAfterAnyTypeAssigned As<T>()
                where T : notnull
            {
                return injectIntoInstanceToCompose.As<T>();
            }

            public IAfterImplementedTypeAssigned AsSelf()
            {
                return injectIntoInstanceToCompose.AsSelf();
            }

            public IAfterImplementedInterfacesAssigned AsImplementedInterfaces()
            {
                return injectIntoInstanceToCompose.AsImplementedInterfaces();
            }

            public IInjectIntoMethod With<T>(string name, T instance)
                where T: notnull
            {
                var parameter = new Parameter<T>(name, instance);

                parameterList.Add(parameter);

                return this;
            }
        }
    }
}
