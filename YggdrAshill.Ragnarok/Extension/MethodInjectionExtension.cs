using System;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class MethodInjectionExtension
    {
        public static IMethodInjection WithMethod(this IMethodInjection injection, string name, object instance)
        {
            var parameter = new Parameter(name, instance);

            return injection.WithMethod(parameter);
        }

        public static IMethodInjection WithMethod(this IMethodInjection injection, string name, Func<object> instantiation)
        {
            var parameter = new Parameter(name, instantiation);

            return injection.WithMethod(parameter);
        }

        public static IMethodInjection WithMethod<T>(this IMethodInjection injection, T instance)
            where T : notnull
        {
            var parameter = new Parameter<T>(instance);

            return injection.WithMethod(parameter);
        }

        public static IMethodInjection WithMethod<T>(this IMethodInjection injection, Func<T> instantiation)
            where T : notnull
        {
            var parameter = new Parameter<T>(instantiation);

            return injection.WithMethod(parameter);
        }

        public static IMethodInjection WithMethod<T>(this IMethodInjection injection, string name, T instance)
            where T : notnull
        {
            var parameter = new Parameter<T>(name, instance);

            return injection.WithMethod(parameter);
        }

        public static IMethodInjection WithMethod<T>(this IMethodInjection injection, string name, Func<T> instantiation)
            where T : notnull
        {
            var parameter = new Parameter<T>(name, instantiation);

            return injection.WithMethod(parameter);
        }
    }
}
