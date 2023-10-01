using System;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class FieldInjectionExtension
    {
        public static IFieldInjection WithField(this IFieldInjection injection, string name, object instance)
        {
            var parameter = new Parameter(name, instance);

            return injection.WithField(parameter);
        }

        public static IFieldInjection WithField(this IFieldInjection injection, string name, Func<object> instantiation)
        {
            var parameter = new Parameter(name, instantiation);

            return injection.WithField(parameter);
        }

        public static IFieldInjection WithField<T>(this IFieldInjection injection, T instance)
            where T : notnull
        {
            var parameter = new Parameter<T>(instance);

            return injection.WithField(parameter);
        }

        public static IFieldInjection WithField<T>(this IFieldInjection injection, Func<T> instantiation)
            where T : notnull
        {
            var parameter = new Parameter<T>(instantiation);

            return injection.WithField(parameter);
        }

        public static IFieldInjection WithField<T>(this IFieldInjection injection, string name, T instance)
            where T : notnull
        {
            var parameter = new Parameter<T>(name, instance);

            return injection.WithField(parameter);
        }

        public static IFieldInjection WithField<T>(this IFieldInjection injection, string name, Func<T> instantiation)
            where T : notnull
        {
            var parameter = new Parameter<T>(name, instantiation);

            return injection.WithField(parameter);
        }
    }
}
