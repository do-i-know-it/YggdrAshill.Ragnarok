using System;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class ConstructorInjectionExtension
    {
        public static IConstructorInjection WithArgument<T>(this IConstructorInjection injection, string name, T instance)
            where T : notnull
        {
            var parameter = new Parameter<T>(name, instance);

            return injection.WithArgument(parameter);
        }

        public static IConstructorInjection WithArgument<T>(this IConstructorInjection injection, string name, Func<T> instantiation)
            where T : notnull
        {
            var parameter = new Parameter<T>(name, instantiation);

            return injection.WithArgument(parameter);
        }
    }
}
