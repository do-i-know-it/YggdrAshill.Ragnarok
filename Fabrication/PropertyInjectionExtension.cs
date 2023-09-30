using System;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class PropertyInjectionExtension
    {
        public static IPropertyInjection WithProperty<T>(this IPropertyInjection injection, string name, T instance)
            where T : notnull
        {
            var parameter = new Parameter<T>(name, instance);

            return injection.WithProperty(parameter);
        }

        public static IPropertyInjection WithProperty<T>(this IPropertyInjection injection, string name, Func<T> instantiation)
            where T : notnull
        {
            var parameter = new Parameter<T>(name, instantiation);

            return injection.WithProperty(parameter);
        }
    }
}
