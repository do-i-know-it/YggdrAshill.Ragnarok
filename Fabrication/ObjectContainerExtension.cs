using System;
using YggdrAshill.Ragnarok.Fabrication;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class ObjectContainerExtension
    {
        public static void Register(this IObjectContainer container, Action<IObjectResolver> operation)
        {
            container.Registration.Register(new Operation(operation));
        }

        public static IConstructorDependencyInjection Register<T>(this IObjectContainer container, Lifetime lifetime)
            where T : notnull
        {
            var implementedType = typeof(T);

            if (!ValidateType.IsInstantiatable(implementedType))
            {
                throw new ArgumentException($"{implementedType} is not instantiatable.");
            }

            var statement = new ConstructorDependencyInjectionStatement(container.Compilation, implementedType, lifetime);

            container.Registration.Register(statement);

            return statement.Injection;
        }

        public static IConstructorDependencyInjection Register<TInterface, TImplementation>(this IObjectContainer container, Lifetime lifetime)
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

            statement.Assignment.As<T>();

            return statement.Assignment;
        }

        public static IInstanceDependencyInjection Register<T>(this IObjectContainer container, Func<T> instantiation, Lifetime lifetime, Ownership ownership = Ownership.External)
            where T : notnull
        {
            var statement
                = new InstanceDependencyInjectionStatement(container.Compilation, typeof(T), lifetime, ownership, new InstantiateInstance<T>(instantiation));

            container.Registration.Register(statement);

            return statement.Injection;
        }

        public static IInstanceDependencyInjection Register<TInterface, TImplementation>(this IObjectContainer container, Func<TImplementation> instantiation, Lifetime lifetime, Ownership ownership = Ownership.External)
            where TInterface : notnull
            where TImplementation : TInterface
        {
            var instanceInjection = container.Register(instantiation, lifetime, ownership);

            instanceInjection.As<TInterface>();

            return instanceInjection;
        }
    }
}
