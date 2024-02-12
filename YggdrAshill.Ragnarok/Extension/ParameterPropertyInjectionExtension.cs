using System;
using System.Runtime.CompilerServices;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class ParameterPropertyInjectionExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IParameterPropertyInjection WithProperty<T>(this IParameterPropertyInjection injection, T instance)
            where T : notnull
        {
            var parameter = new TypedParameterByInstance<T>(instance);

            return injection.WithProperty(parameter);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IParameterPropertyInjection WithProperty<T>(this IParameterPropertyInjection injection, Func<T> instantiation)
            where T : notnull
        {
            var parameter = new TypedParameterByMethod<T>(instantiation);

            return injection.WithProperty(parameter);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IParameterPropertyInjection WithProperty<T>(this IParameterPropertyInjection injection, string name, T instance)
            where T : notnull
        {
            var parameter = new NamedParameterByInstance<T>(name, instance);

            return injection.WithProperty(parameter);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IParameterPropertyInjection WithProperty<T>(this IParameterPropertyInjection injection, string name, Func<T> instantiation)
            where T : notnull
        {
            var parameter = new NamedParameterByMethod<T>(name, instantiation);

            return injection.WithProperty(parameter);
        }
    }
}
