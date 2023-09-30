using System;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class ConstructorDependencyInjectionExtension
    {
        public static IConstructorDependencyInjection WithArgument<T>(this IConstructorDependencyInjection injection, string name, T instance)
            where T : notnull
        {
            var parameter = new Parameter<T>(name, instance);

            return injection.WithArgument(parameter);
        }

        public static IConstructorDependencyInjection WithArgument<T>(this IConstructorDependencyInjection injection, string name, Func<T> instantiation)
            where T : notnull
        {
            var parameter = new Parameter<T>(name, instantiation);

            return injection.WithArgument(parameter);
        }
    }
}
