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
            var parameter = new TypedParameter<T>(instance);

            return injection.WithProperty(parameter);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IParameterPropertyInjection WithProperty<T>(this IParameterPropertyInjection injection, Func<T> creation)
            where T : notnull
        {
            var parameter = new TypedParameter<T>(creation);

            return injection.WithProperty(parameter);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IParameterPropertyInjection WithProperty<T>(this IParameterPropertyInjection injection, string name, T instance)
            where T : notnull
        {
            var parameter = new NamedParameter<T>(name, instance);

            return injection.WithProperty(parameter);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IParameterPropertyInjection WithProperty<T>(this IParameterPropertyInjection injection, string name, Func<T> creation)
            where T : notnull
        {
            var parameter = new NamedParameter<T>(name, creation);

            return injection.WithProperty(parameter);
        }
    }
}
