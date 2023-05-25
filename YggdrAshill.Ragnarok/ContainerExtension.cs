using YggdrAshill.Ragnarok.Construction;
using YggdrAshill.Ragnarok.Fabrication;
using System;

namespace YggdrAshill.Ragnarok
{
    public static class ContainerExtension
    {
        public static IDependencyInjection RegisterTemporal<T>(this IContainer container)
            where T : notnull
        {
            return container.Register<T>(Lifetime.Temporal);
        }
        public static IDependencyInjection RegisterLocal<T>(this IContainer container)
            where T : notnull
        {
            return container.Register<T>(Lifetime.Local);
        }
        public static IDependencyInjection RegisterGlobal<T>(this IContainer container)
            where T : notnull
        {
            return container.Register<T>(Lifetime.Global);
        }
        private static IDependencyInjection Register<T>(this IContainer container, Lifetime lifetime)
            where T : notnull
        {
            var implementedType = typeof(T);

            if (!ValidateType.IsInstantiatable(implementedType))
            {
                // TODO: throw original exception.
                throw new Exception($"{implementedType} is not instantiatable.");
            }

            var statement = new DependencyInjectionStatement(container, implementedType);

            var composition = new Composition(lifetime, Ownership.Internal, statement);

            container.Register(composition);

            return statement;
        }

        public static IDependencyInjection RegisterTemporal<TInterface, TImplementation>(this IContainer container)
            where TInterface : notnull
            where TImplementation : TInterface
        {
            var dependencyInjection = container.RegisterTemporal<TImplementation>();

            dependencyInjection.As<TInterface>();

            return dependencyInjection;
        }
        public static IDependencyInjection RegisterLocal<TInterface, TImplementation>(this IContainer container)
            where TInterface : notnull
            where TImplementation : TInterface
        {
            var dependencyInjection = container.RegisterLocal<TImplementation>();

            dependencyInjection.As<TInterface>();

            return dependencyInjection;
        }
        public static IDependencyInjection RegisterGlobal<TInterface, TImplementation>(this IContainer container)
            where TInterface : notnull
            where TImplementation : TInterface
        {
            var dependencyInjection = container.RegisterGlobal<TImplementation>();

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

        public static IInstanceInjection RegisterTemporalInstance<T>(this IContainer container, Func<T> instantiation, bool external = true)
            where T : notnull
        {
            return container.RegisterInstance(Lifetime.Temporal, external, instantiation);
        }
        public static IInstanceInjection RegisterLocalInstance<T>(this IContainer container, Func<T> instantiation, bool external = true)
            where T : notnull
        {
            return container.RegisterInstance(Lifetime.Local, external, instantiation);
        }
        public static IInstanceInjection RegisterGlobalInstance<T>(this IContainer container, Func<T> instantiation, bool external = true)
            where T : notnull
        {
            return container.RegisterInstance(Lifetime.Global, external, instantiation);
        }
        private static IInstanceInjection RegisterInstance<T>(this IContainer container, Lifetime lifetime, bool external, Func<T> onInstantiated)
            where T : notnull
        {
            var ownership = external ? Ownership.External : Ownership.Internal;

            var instantiation = new InstantiateWithFunction(() => onInstantiated.Invoke());

            var statement = new InstanceInjectionStatement(container, typeof(T), instantiation);

            var composition = new Composition(lifetime, ownership, statement);

            container.Register(composition);

            return statement;
        }
    }
}
