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
            var parameter = new TypedParameter<T>(instance);

            return injection.WithField(parameter);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IParameterFieldInjection WithField<T>(this IParameterFieldInjection injection, Func<T> creation)
            where T : notnull
        {
            var parameter = new TypedParameter<T>(creation);

            return injection.WithField(parameter);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IParameterFieldInjection WithField<T>(this IParameterFieldInjection injection, string name, T instance)
            where T : notnull
        {
            var parameter = new NamedParameter<T>(name, instance);

            return injection.WithField(parameter);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IParameterFieldInjection WithField<T>(this IParameterFieldInjection injection, string name, Func<T> creation)
            where T : notnull
        {
            var parameter = new NamedParameter<T>(name, creation);

            return injection.WithField(parameter);
        }
    }
}
