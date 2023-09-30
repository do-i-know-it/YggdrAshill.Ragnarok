using System;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class PropertyDependencyInjectionExtension
    {
        public static IPropertyDependencyInjection WithProperty<T>(this IPropertyDependencyInjection injection, string name, T instance)
            where T : notnull
        {
            var parameter = new Parameter<T>(name, instance);

            return injection.WithProperty(parameter);
        }

        public static IPropertyDependencyInjection WithProperty<T>(this IPropertyDependencyInjection injection, string name, Func<T> instantiation)
            where T : notnull
        {
            var parameter = new Parameter<T>(name, instantiation);

            return injection.WithProperty(parameter);
        }
    }
}
