using System;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class MethodInjectionExtension
    {
        public static IMethodInjection WithMethodArgument<T>(this IMethodInjection injection, string name, T instance)
            where T : notnull
        {
            var parameter = new Parameter<T>(name, instance);

            return injection.WithMethodArgument(parameter);
        }

        public static IMethodInjection WithMethodArgument<T>(this IMethodInjection injection, string name, Func<T> instantiation)
            where T : notnull
        {
            var parameter = new Parameter<T>(name, instantiation);

            return injection.WithMethodArgument(parameter);
        }
    }
}
