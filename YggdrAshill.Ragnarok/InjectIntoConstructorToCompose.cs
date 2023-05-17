using YggdrAshill.Ragnarok.Construction;
using YggdrAshill.Ragnarok.Fabrication;
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    public sealed class InjectIntoConstructorToCompose :
        IInjectIntoConstructor,
        IComposition
    {
        private readonly Lazy<InjectIntoInstanceToCompose> injectIntoInstanceToCompose;

        public InjectIntoConstructorToCompose(ICompilation compilation, Type implementedType, Lifetime lifetime)
        {
            // TODO: check whether implementedType is concrete class.

            injectIntoInstanceToCompose = new Lazy<InjectIntoInstanceToCompose>(() =>
            {
                var instantiation = compilation.GetInstantiation(implementedType, parameterList.Value);

                return new InjectIntoInstanceToCompose(compilation, implementedType, lifetime, Ownership.Internal, instantiation);
            });
        }

        private readonly Lazy<List<IParameter>> parameterList = new Lazy<List<IParameter>>();

        public IDescription Compose()
        {
            return injectIntoInstanceToCompose.Value.Compose();
        }

        public IAfterAnyTypeAssigned As<T>()
            where T : notnull
        {
            return injectIntoInstanceToCompose.Value.As<T>();
        }

        public IAfterImplementedTypeAssigned AsSelf()
        {
            return injectIntoInstanceToCompose.Value.AsSelf();
        }

        public IAfterImplementedInterfacesAssigned AsImplementedInterfaces()
        {
            return injectIntoInstanceToCompose.Value.AsImplementedInterfaces();
        }

        public IInjectIntoField WithFieldInjection()
        {
            return injectIntoInstanceToCompose.Value.WithFieldInjection();
        }

        public IInjectIntoProperty WithPropertyInjection()
        {
            return injectIntoInstanceToCompose.Value.WithPropertyInjection();
        }

        public IInjectIntoMethod WithMethodInjection()
        {
            return injectIntoInstanceToCompose.Value.WithMethodInjection();
        }

        public IInjectIntoConstructor With<T>(string name, T instance)
            where T: notnull
        {
            return WithArgument<T>(name, instance);
        }

        public IInjectIntoConstructor WithArgument<T>(string name, T instance)
            where T : notnull
        {
            var parameter = new Parameter<T>(name, instance);

            parameterList.Value.Add(parameter);

            return this;
        }
    }
}
