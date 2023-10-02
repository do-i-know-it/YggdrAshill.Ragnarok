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

            return statement.DependencyInjection;
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
            var statement = new ReturnInstanceStatement(instance);

            container.Registration.Register(statement);

            var assignment = statement.TypeAssignment;

            assignment.As<T>();

            return assignment;
        }

        public static IInstanceInjection RegisterInstance<T>(this IObjectContainer container, Func<T> instantiation, Lifetime lifetime = Lifetime.Global, Ownership ownership = Ownership.External)
            where T : notnull
        {
            var statement = new CreateInstanceStatement<T>(container.Compilation, lifetime, ownership, instantiation);

            container.Registration.Register(statement);

            return statement.InstanceInjection;
        }

        public static IInstanceInjection RegisterInstance<TInterface, TImplementation>(this IObjectContainer container, Func<TImplementation> instantiation, Lifetime lifetime = Lifetime.Global, Ownership ownership = Ownership.External)
            where TInterface : notnull
            where TImplementation : TInterface
        {
            var instanceInjection = container.RegisterInstance(instantiation, lifetime, ownership);

            instanceInjection.As<TInterface>();

            return instanceInjection;
        }

        public static ITypeAssignment RegisterFromSubContainer<T>(this IObjectContainer container, params IInstallation[] installationList)
            where T : notnull
        {
            var statement = new ResolveFromSubContainerStatement(typeof(T), container, installationList);

            container.Registration.Register(statement);

            return statement.TypeAssignment;
        }

        public static ITypeAssignment RegisterFromSubContainer<T>(this IObjectContainer container, Action<IObjectContainer> installation)
            where T : notnull
        {
            return container.RegisterFromSubContainer<T>(new Installation(installation));
        }

        public static ITypeAssignment RegisterFromSubContainer<TInstance, TInstallation>(this IObjectContainer container)
            where TInstance : notnull
            where TInstallation : IInstallation
        {
            var installation = container.Resolver.Resolve<TInstallation>();

            return container.RegisterFromSubContainer<TInstance>(installation);
        }

        public static void Install(this IObjectContainer container, params IInstallation[] installationList)
        {
            foreach (var installation in installationList)
            {
                installation.Install(container);
            }
        }

        public static void Install(this IObjectContainer container, Action<IObjectContainer> installation)
        {
            container.Install(new Installation(installation));
        }

        public static void Install<TInstallation>(this IObjectContainer container)
            where TInstallation : IInstallation
        {
            var installation = container.Resolver.Resolve<TInstallation>();

            container.Install(installation);
        }

        public static IObjectScope CreateSubScope(this IObjectContainer container, params IInstallation[] installationList)
        {
            return container.CreateContext().CreateCurrentScope(installationList);
        }

        public static IObjectScope CreateSubScope(this IObjectContainer container, Action<IObjectContainer> installation)
        {
            return container.CreateContext().CreateCurrentScope(installation);
        }

        public static IObjectScope CreateSubScope<TInstallation>(this IObjectContainer container)
            where TInstallation : IInstallation
        {
            var installation = container.Resolver.Resolve<TInstallation>();

            return container.CreateSubScope(installation);
        }

        public static int Count(this IObjectContainer container, Func<IStatement, bool> condition)
        {
            return container.Registration.Count(new StatementSelection(condition));
        }

        public static void Register(this IObjectContainer container, Action<IObjectResolver> operation)
        {
            container.Registration.Register(new Operation(operation));
        }
    }
}
