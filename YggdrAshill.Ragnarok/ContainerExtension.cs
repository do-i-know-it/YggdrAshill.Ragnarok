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

        public static IInjectIntoInstance RegisterInstance<T>(this IContainer container, T instance)
            where T : notnull
        {
            var composition = new InjectIntoInstanceToCompose(container, instance);

            container.Register(composition);

            return composition;
        }

        public static IAssignAnyType RegisterTemporal<T>(this IContainer container, Func<T> instantiation, bool external = true)
            where T : notnull
        {
            var ownership = external ? Ownership.External : Ownership.Internal;

            return container.Register(instantiation, Lifetime.Temporal, ownership);
        }
        public static IAssignAnyType RegisterLocal<T>(this IContainer container, Func<T> instantiation, bool external = true)
            where T : notnull
        {
            var ownership = external ? Ownership.External : Ownership.Internal;

            return container.Register(instantiation, Lifetime.Local, ownership);
        }
        public static IAssignAnyType RegisterGlobal<T>(this IContainer container, Func<T> instantiation, bool external = true)
            where T : notnull
        {
            var ownership = external ? Ownership.External : Ownership.Internal;

            return container.Register(instantiation, Lifetime.Global, ownership);
        }
        private static IAssignAnyType Register<T>(this IContainer container, Func<T> instantiation, Lifetime lifetime, Ownership ownership = Ownership.External)
            where T : notnull
        {
            var composition
                = new InjectIntoInstanceToCompose(container, typeof(T), lifetime, ownership, () => instantiation.Invoke());

            container.Register(composition);

            return composition;
        }
    }
}
