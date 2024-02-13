using System;
using System.Runtime.CompilerServices;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class ObjectContainerExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IDependencyInjection Register<T>(this IObjectContainer container, Lifetime lifetime)
            where T : notnull
        {
            var implementedType = typeof(T);

            if (!TypeValidation.CanInstantiate(implementedType))
            {
                throw new ArgumentException($"{implementedType} is not instantiatable.");
            }

            var statement = new DependencyInjectionStatement(container, implementedType, lifetime);

            container.Registration.Register(statement);

            return statement.Source;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IDependencyInjection Register<TInterface, TImplementation>(this IObjectContainer container, Lifetime lifetime)
            where TInterface : notnull
            where TImplementation : TInterface
        {
            var injection = container.Register<TImplementation>(lifetime);

            injection.As<TInterface>();

            return injection;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ITypeAssignment RegisterInstance<T>(this IObjectContainer container, T instance)
            where T : notnull
        {
            var statement = new ReturnInstanceStatement(instance);

            container.Registration.Register(statement);

            var assignment = statement.Source;

            assignment.As<T>();

            return assignment;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IInstanceInjection RegisterInstance<T>(this IObjectContainer container, Func<T> instantiation, Lifetime lifetime = Lifetime.Global, Ownership ownership = Ownership.External)
            where T : notnull
        {
            var statement = new CreateInstanceStatement<T>(container, lifetime, ownership, instantiation);

            container.Registration.Register(statement);

            return statement.Source;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IInstanceInjection RegisterInstance<TInterface, TImplementation>(this IObjectContainer container, Func<TImplementation> instantiation, Lifetime lifetime = Lifetime.Global, Ownership ownership = Ownership.External)
            where TInterface : notnull
            where TImplementation : TInterface
        {
            var injection = container.RegisterInstance(instantiation, lifetime, ownership);

            injection.As<TInterface>();

            return injection;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ISubContainerResolution RegisterFromSubContainer<T>(this IObjectContainer container, IInstallation installation)
            where T : notnull
        {
            var statement = new ResolveFromSubContainerStatement(typeof(T), container, installation);

            container.Registration.Register(statement);

            return statement.Source;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ISubContainerResolution RegisterFromSubContainer<T>(this IObjectContainer container, Action<IObjectContainer> installation)
            where T : notnull
        {
            return container.RegisterFromSubContainer<T>(new Installation(installation));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ISubContainerResolution RegisterFromSubContainer<TInstance, TInstallation>(this IObjectContainer container)
            where TInstance : notnull
            where TInstallation : IInstallation
        {
            var installation = container.Resolver.Resolve<TInstallation>();

            return container.RegisterFromSubContainer<TInstance>(installation);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Install(this IObjectContainer container, params IInstallation[] installationList)
        {
            InstallationList.Install(container, installationList);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Install(this IObjectContainer container, Action<IObjectContainer> installation)
        {
            installation.Invoke(container);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Install<TInstallation>(this IObjectContainer container)
            where TInstallation : IInstallation
        {
            var installation = container.Resolver.Resolve<TInstallation>();

            installation.Install(container);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IObjectScope CreateSubScope(this IObjectContainer container, params IInstallation[] installationList)
        {
            var context = container.CreateContext();

            return context.CreateCurrentScope(installationList);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IObjectScope CreateSubScope(this IObjectContainer container, Action<IObjectContainer> installation)
        {
            var context = container.CreateContext();

            return context.CreateCurrentScope(installation);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IObjectScope CreateSubScope<TInstallation>(this IObjectContainer container)
            where TInstallation : IInstallation
        {
            var context = container.CreateContext();

            return context.CreateCurrentScope<TInstallation>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count(this IObjectContainer container, Func<IStatement, bool> condition)
        {
            return container.Registration.Count(new Condition(condition));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RegisterCallback(this IObjectContainer container, Action<IObjectResolver> execution)
        {
            container.Registration.Register(new Execution(execution));
        }
    }
}
