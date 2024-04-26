using System;
using System.Collections.Generic;
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
        public static IInstanceInjection Register<TInput, TOutput>(this IObjectContainer container, Func<TInput, TOutput> onConverted, Ownership ownership = Ownership.External)
            where TInput : notnull
            where TOutput : notnull
        {
            var statement = new ConvertInstanceStatement<TInput, TOutput>(container, ownership, onConverted);
            container.Registration.Register(statement);
            return statement.Source;
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
        public static IInstanceInjection RegisterInstance<T>(this IObjectContainer container, Func<T> onCreated, Ownership ownership = Ownership.External)
            where T : notnull
        {
            var statement = new CreateInstanceStatement<T>(container, ownership, onCreated);
            container.Registration.Register(statement);
            return statement.Source;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ISubContainerResolution RegisterFromSubContainer<T>(this IObjectContainer container, IInstallation installation)
            where T : notnull
        {
            var statement = new ResolveFromSubContainerStatement(typeof(T), container);
            container.Registration.Register(statement);
            var source = statement.Source;
            source.With(installation);
            return source;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ISubContainerResolution RegisterFromSubContainer<T>(this IObjectContainer container, Action<IObjectContainer> onInstalled)
            where T : notnull
        {
            var installation = new Installation(onInstalled);
            return container.RegisterFromSubContainer<T>(installation);
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
        public static IFactoryResolution RegisterFactory<T>(this IObjectContainer container, IInstallation installation, Ownership ownership)
            where T : notnull
        {
            var statement = new ReturnFactoryStatement<T>(container, ownership);
            container.Registration.Register(statement);
            var source = statement.Source;
            source.With(installation);
            return source;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IFactoryResolution RegisterFactory<T>(this IObjectContainer container, Action<IObjectContainer> onInstalled, Ownership ownership)
            where T : notnull
        {
            var installation = new Installation(onInstalled);
            return container.RegisterFactory<T>(installation, ownership);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IFactoryResolution RegisterFactory<TInstance, TInstallation>(this IObjectContainer container, Ownership ownership)
            where TInstance : notnull
            where TInstallation : IInstallation
        {
            var installation = container.Resolver.Resolve<TInstallation>();
            return container.RegisterFactory<TInstance>(installation, ownership);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RegisterFactory<T>(this IObjectContainer container, Func<T> onCreated)
            where T : notnull
        {
            var factory = new Factory<T>(onCreated);
            container.RegisterInstance<IFactory<T>>(factory);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IFactoryResolution RegisterFactory<TInput, TOutput>(this IObjectContainer container, IInstallation installation, Ownership ownership)
            where TInput : notnull
            where TOutput : notnull
        {
            var statement = new ReturnFactoryStatement<TInput, TOutput>(container, ownership);
            container.Registration.Register(statement);
            var source = statement.Source;
            source.With(installation);
            return source;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IFactoryResolution RegisterFactory<TInput, TOutput>(this IObjectContainer container, Action<IObjectContainer> onInstalled, Ownership ownership)
            where TInput : notnull
            where TOutput : notnull
        {
            var installation = new Installation(onInstalled);
            return container.RegisterFactory<TInput, TOutput>(installation, ownership);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IFactoryResolution RegisterFactory<TInput, TOutput, TInstallation>(this IObjectContainer container, Ownership ownership)
            where TInput : notnull
            where TOutput : notnull
            where TInstallation : IInstallation
        {
            var installation = container.Resolver.Resolve<TInstallation>();
            return container.RegisterFactory<TInput, TOutput>(installation, ownership);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RegisterFactory<TInput, TOutput>(this IObjectContainer container, Func<TInput, TOutput> onCreated)
            where TInput : notnull
            where TOutput : notnull
        {
            var factory = new Factory<TInput, TOutput>(onCreated);
            container.RegisterInstance<IFactory<TInput, TOutput>>(factory);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Install(this IObjectContainer container, IReadOnlyList<IInstallation> installationList)
        {
            if (installationList.Count == 0)
            {
                return;
            }

            foreach (var installation in installationList)
            {
                installation.Install(container);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Install(this IObjectContainer container, params IInstallation[] installationList)
        {
            if (installationList.Length == 0)
            {
                return;
            }

            foreach (var installation in installationList)
            {
                installation.Install(container);
            }
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
            return container.Registration.Count(new StatementCondition(condition));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RegisterCallback(this IObjectContainer container, Action<IObjectResolver> onExecuted)
        {
            var execution = new Execution(onExecuted);
            container.Registration.Register(execution);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RegisterCallback<T>(this IObjectContainer container, IInvocation<T> invocation)
            where T : notnull
        {
            var execution = new ExecuteToInvoke<T>(invocation);
            container.Registration.Register(execution);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RegisterCallback<T>(this IObjectContainer container, Action<T> onInvoked)
            where T : notnull
        {
            var invocation = new Invocation<T>(onInvoked);
            container.RegisterCallback(invocation);
        }
    }
}
