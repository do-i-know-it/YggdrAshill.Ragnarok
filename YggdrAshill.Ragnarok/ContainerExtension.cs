using YggdrAshill.Ragnarok.Construction;
using YggdrAshill.Ragnarok.Fabrication;
using System;

namespace YggdrAshill.Ragnarok
{
    public static class ContainerExtension
    {
        public static IInjectIntoConstructor RegisterTemporal<T>(this IContainer container)
            where T : notnull
        {
            return container.Register<T>(Lifetime.Temporal);
        }
        public static IInjectIntoConstructor RegisterLocal<T>(this IContainer container)
            where T : notnull
        {
            return container.Register<T>(Lifetime.Local);
        }
        public static IInjectIntoConstructor RegisterGlobal<T>(this IContainer container)
            where T : notnull
        {
            return container.Register<T>(Lifetime.Global);
        }
        private static IInjectIntoConstructor Register<T>(this IContainer container, Lifetime lifetime)
            where T : notnull
        {
            var implementedType = typeof(T);

            // check implementedType is concrete class.

            var composition = new InjectIntoConstructorToCompose(container, implementedType, lifetime);

            container.Register(composition);

            return composition;
        }

        public static IInjectIntoConstructor RegisterTemporal<TInterface, TImplementation>(this IContainer container)
            where TInterface : notnull
            where TImplementation : TInterface
        {
            var injectIntoConstructor = container.RegisterTemporal<TImplementation>();

            injectIntoConstructor.As<TInterface>();

            return injectIntoConstructor;
        }
        public static IInjectIntoConstructor RegisterLocal<TInterface, TImplementation>(this IContainer container)
            where TInterface : notnull
            where TImplementation : TInterface
        {
            var injectIntoConstructor = container.RegisterLocal<TImplementation>();

            injectIntoConstructor.As<TInterface>();

            return injectIntoConstructor;
        }
        public static IInjectIntoConstructor RegisterGlobal<TInterface, TImplementation>(this IContainer container)
            where TInterface : notnull
            where TImplementation : TInterface
        {
            var injectIntoConstructor = container.RegisterGlobal<TImplementation>();

            injectIntoConstructor.As<TInterface>();

            return injectIntoConstructor;
        }

        public static IAssignAnyType RegisterTemporal<T>(this IContainer container, Func<T> instantiation, bool external = true)
            where T : notnull
        {
            return container.Register(instantiation, Lifetime.Temporal, external);
        }
        public static IAssignAnyType RegisterLocal<T>(this IContainer container, Func<T> instantiation, bool external = true)
            where T : notnull
        {
            return container.Register(instantiation, Lifetime.Local, external);
        }
        public static IAssignAnyType RegisterGlobal<T>(this IContainer container, Func<T> instantiation, bool external = true)
            where T : notnull
        {
            return container.Register(instantiation, Lifetime.Global, external);
        }
        private static IAssignAnyType Register<T>(this IContainer container, Func<T> onInstantiated, Lifetime lifetime, bool external)
            where T : notnull
        {
            var implementedType = typeof(T);

            var ownership = external ? Ownership.External : Ownership.Internal;

            var instantiation = new InstantiateWithFunction(() => onInstantiated.Invoke());

            return container.Register(implementedType, lifetime, ownership, instantiation);
        }

        public static IInjectIntoInstance RegisterInstance<T>(this IContainer container, T instance)
            where T : notnull
        {
            var implementedType = typeof(T);

            var instantiation = new ReturnInstanceDirectly(instance);

            return container.Register(implementedType, Lifetime.Global, Ownership.External, instantiation);
        }

        public static IInjectIntoInstance Register(this IContainer container, Type implementedType, Lifetime lifetime, Ownership ownership, IInstantiation instantiation)
        {
            var composition = new InjectIntoInstanceToCompose(container, implementedType, lifetime, ownership, instantiation);

            container.Register(composition);

            return composition;
        }
    }
}
