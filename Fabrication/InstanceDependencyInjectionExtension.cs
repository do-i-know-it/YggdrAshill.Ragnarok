using System;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class InstanceDependencyInjectionExtension
    {
        public static IInstanceDependencyInjection WithField<T>(this IInstanceDependencyInjection injection, string name, T instance)
            where T : notnull
        {
            var parameter = new Parameter<T>(name, instance);

            return injection.WithField(parameter);
        }

        public static IInstanceDependencyInjection WithField<T>(this IInstanceDependencyInjection injection, string name, Func<T> instantiation)
            where T : notnull
        {
            var parameter = new Parameter<T>(name, instantiation);

            return injection.WithField(parameter);
        }
    }
}
