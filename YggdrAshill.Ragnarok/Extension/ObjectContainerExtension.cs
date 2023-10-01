using YggdrAshill.Ragnarok.Fabrication;
using System;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class ObjectContainerExtension
    {
        public static IDependencyInjection Register<T>(this IObjectContainer container, Lifetime lifetime)
            where T : notnull
        {
            var implementedType = typeof(T);

            if (!TypeValidation.CanInstantiate(implementedType))
            {
                throw new ArgumentException($"{implementedType} is not instantiatable.");
            }

            var statement = new DependencyInjectionStatement(container.Compilation, implementedType, lifetime);

            container.Registration.Register(statement);

            return statement;
        }

        public static IDependencyInjection Register<TInterface, TImplementation>(this IObjectContainer container, Lifetime lifetime)
            where TInterface : notnull
            where TImplementation : TInterface
        {
            var injection = container.Register<TImplementation>(lifetime);

            injection.As<TInterface>();

            return injection;
        }

        public static ITypeAssignment RegisterInstance<T>(this IObjectContainer container, T instance)
            where T : notnull
        {
            var statement = new TypeAssignmentStatement(instance);

            container.Registration.Register(statement);

            statement.As<T>();

            return statement;
        }

        public static IInstanceInjection Register<T>(this IObjectContainer container, Func<T> instantiation, Lifetime lifetime, Ownership ownership = Ownership.External)
            where T : notnull
        {
            var statement
                = new InstanceInjectionStatement(container.Compilation, typeof(T), lifetime, ownership, new InstantiateInstance<T>(instantiation));

            container.Registration.Register(statement);

            return statement;
        }

        public static IInstanceInjection Register<TInterface, TImplementation>(this IObjectContainer container, Func<TImplementation> instantiation, Lifetime lifetime, Ownership ownership = Ownership.External)
            where TInterface : notnull
            where TImplementation : TInterface
        {
            var instanceInjection = container.Register(instantiation, lifetime, ownership);

            instanceInjection.As<TInterface>();

            return instanceInjection;
        }

        public static void Register(this IObjectContainer container, params IInstallation[] installationList)
        {
            foreach (var installation in installationList)
            {
                installation.Install(container);
            }
        }

        public static void Register(this IObjectContainer container, Action<IObjectContainer> installation)
        {
            container.Register(new Installation(installation));
        }

        public static void Register<TInstallation>(this IObjectContainer container)
            where TInstallation : IInstallation
        {
            var installation = container.Resolver.Resolve<TInstallation>();
            container.Register(installation);
        }

        public static void RegisterFromSubContainer<T>(this IObjectContainer container, params IInstallation[] installationList)
            where T : notnull
        {
            var statement = new SubContainerStatement(typeof(T), container, installationList);
            container.Registration.Register(statement);
        }

        public static void RegisterFromSubContainer<T>(this IObjectContainer container, Action<IObjectContainer> installation)
            where T : notnull
        {
            container.RegisterFromSubContainer<T>(new Installation(installation));
        }

        public static void RegisterFromSubContainer<TInstance, TInstallation>(this IObjectContainer container)
            where TInstance : notnull
            where TInstallation : IInstallation
        {
            var installation = container.Resolver.Resolve<TInstallation>();

            container.RegisterFromSubContainer<TInstance>(installation);
        }

        public static IObjectScope CreateScope(this IObjectContainer container, params IInstallation[] installationList)
        {
            return container.CreateContext().Install(installationList).CreateScope();
        }

        public static IObjectScope CreateScope(this IObjectContainer container, Action<IObjectContainer> installation)
        {
            return container.CreateScope(new Installation(installation));
        }

        public static IObjectScope CreateScope<TInstallation>(this IObjectContainer container)
            where TInstallation : IInstallation
        {
            var installation = container.Resolver.Resolve<TInstallation>();

            return container.CreateScope(installation);
        }

        public static bool IsRegistered(this IObjectContainer container,Func<IStatement, bool> condition)
        {
            return container.Registration.Count(new StatementSelection(condition)) > 0;
        }

        public static void Register(this IObjectContainer container, Action<IObjectResolver> operation)
        {
            container.Registration.Register(new Operation(operation));
        }
    }
}
