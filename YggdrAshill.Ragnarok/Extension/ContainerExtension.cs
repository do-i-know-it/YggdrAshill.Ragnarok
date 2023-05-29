using System;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class ContainerExtension
    {
        public static IDependencyInjection Register<T>(this IContainer container, Lifetime lifetime)
            where T : notnull
        {
            var implementedType = typeof(T);

            if (!ValidateType.IsInstantiatable(implementedType))
            {
                throw new RagnarokArgumentException(implementedType, $"{implementedType} is not instantiatable.");
            }

            var statement = new DependencyInjectionStatement(container, implementedType);

            var composition = new Composition(lifetime, Ownership.Internal, statement);

            container.Register(composition);

            return statement;
        }

        public static IDependencyInjection Register<TInterface, TImplementation>(this IContainer container, Lifetime lifetime)
            where TInterface : notnull
            where TImplementation : TInterface
        {
            var dependencyInjection = container.Register<TImplementation>(lifetime);

            dependencyInjection.As<TInterface>();

            return dependencyInjection;
        }

        public static ITypeAssignment RegisterInstance<T>(this IContainer container, T instance)
            where T : notnull
        {
            var instantiation = new ReturnInstanceDirectly(instance);

            var statement = new TypeAssignmentStatement(instance.GetType(), instantiation);

            var composition = new Composition(Lifetime.Global, Ownership.External, statement);

            container.Register(composition);

            statement.As<T>();

            return statement;
        }

        public static IInstanceInjection Register<T>(this IContainer container, Func<T> instantiation, Lifetime lifetime, Ownership ownership = Ownership.External)
            where T : notnull
        {
            return container.Register<T>(lifetime, ownership, new InstantiateWithFunction(() => instantiation.Invoke()));
        }
        private static IInstanceInjection Register<T>(this IContainer container, Lifetime lifetime, Ownership ownership, IInstantiation instantiation)
            where T : notnull
        {
            var statement = new InstanceInjectionStatement(container, typeof(T), instantiation);

            var composition = new Composition(lifetime, ownership, statement);

            container.Register(composition);

            return statement;
        }
        public static IInstanceInjection Register<TInterface, TImplementation>(this IContainer container, Func<TImplementation> instantiation, Lifetime lifetime, Ownership ownership = Ownership.External)
            where TInterface : notnull
            where TImplementation : TInterface
        {
            var instanceInjection = container.Register(instantiation, lifetime, ownership);

            instanceInjection.As<TInterface>();

            return instanceInjection;
        }
    }
}
