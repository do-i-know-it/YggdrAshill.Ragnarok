using System;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class ConstructorInjectionExtension
    {
        public static IDependencyInjection WithArgument(this IDependencyInjection injection, string name, object instance)
        {
            var parameter = new Parameter(name, instance);

            return injection.WithArgument(parameter);
        }

        public static IDependencyInjection WithArgument(this IDependencyInjection injection, string name, Func<object> instantiation)
        {
            var parameter = new Parameter(name, instantiation);

            return injection.WithArgument(parameter);
        }

        public static IDependencyInjection WithArgument<T>(this IDependencyInjection injection, T instance)
            where T : notnull
        {
            var parameter = new Parameter<T>(instance);

            return injection.WithArgument(parameter);
        }

        public static IDependencyInjection WithArgument<T>(this IDependencyInjection injection, Func<T> instantiation)
            where T : notnull
        {
            var parameter = new Parameter<T>(instantiation);

            return injection.WithArgument(parameter);
        }

        public static IDependencyInjection WithArgument<T>(this IDependencyInjection injection, string name, T instance)
            where T : notnull
        {
            var parameter = new Parameter<T>(name, instance);

            return injection.WithArgument(parameter);
        }

        public static IDependencyInjection WithArgument<T>(this IDependencyInjection injection, string name, Func<T> instantiation)
            where T : notnull
        {
            var parameter = new Parameter<T>(name, instantiation);

            return injection.WithArgument(parameter);
        }
    }
}
