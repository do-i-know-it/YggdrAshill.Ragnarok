using YggdrAshill.Ragnarok.Construction;
using YggdrAshill.Ragnarok.Fabrication;
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class InjectIntoConstructorToCompose :
        IInjectIntoConstructor,
        IComposition
    {
        private readonly ICompilation compilation;
        private readonly AssignedTypeCollection collection;
        private readonly Lifetime lifetime;

        private readonly Lazy<InjectIntoInstanceToCompose> cache;

        public InjectIntoConstructorToCompose(ICompilation compilation, Type implementedType, Lifetime lifetime)
        {
            // TODO: check whether implementedType is concrete class.

            this.compilation = compilation;
            this.lifetime = lifetime;
            collection = new AssignedTypeCollection(implementedType);

            cache = new Lazy<InjectIntoInstanceToCompose>(CreateInjectIntoInstanceToCompose);
        }

        private readonly Lazy<List<IParameter>> parameterList = new Lazy<List<IParameter>>();

        private InjectIntoInstanceToCompose CreateInjectIntoInstanceToCompose()
        {
            var instantiation = compilation.GetInstantiation(collection.ImplementedType, parameterList.Value);

            return new InjectIntoInstanceToCompose(compilation, collection, lifetime, Ownership.Internal, instantiation);
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
            return cache.Value.WithFieldInjection();
        }

        public IInjectIntoProperty WithPropertyInjection()
        {
            return cache.Value.WithPropertyInjection();
        }

        public IInjectIntoMethod WithMethodInjection()
        {
            return cache.Value.WithMethodInjection();
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
