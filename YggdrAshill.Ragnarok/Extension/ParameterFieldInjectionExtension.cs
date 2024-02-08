using System;
using System.Runtime.CompilerServices;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class ParameterFieldInjectionExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IParameterFieldInjection WithField<T>(this IParameterFieldInjection injection, T instance)
            where T : notnull
        {
            var parameter = new Parameter<T>(instance);

            return injection.WithField(parameter);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IParameterFieldInjection WithField<T>(this IParameterFieldInjection injection, Func<T> instantiation)
            where T : notnull
        {
            var parameter = new Parameter<T>(instantiation);

            return injection.WithField(parameter);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IParameterFieldInjection WithField<T>(this IParameterFieldInjection injection, string name, T instance)
            where T : notnull
        {
            var parameter = new Parameter<T>(name, instance);

            return injection.WithField(parameter);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IParameterFieldInjection WithField<T>(this IParameterFieldInjection injection, string name, Func<T> instantiation)
            where T : notnull
        {
            var parameter = new Parameter<T>(name, instantiation);

            return injection.WithField(parameter);
        }
    }
}
