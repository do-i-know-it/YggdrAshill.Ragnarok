using System;
using System.Runtime.CompilerServices;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class ParameterMethodInjectionExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IParameterMethodInjection WithMethod<T>(this IParameterMethodInjection injection, T instance)
            where T : notnull
        {
            var parameter = new Parameter<T>(instance);

            return injection.WithMethod(parameter);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IParameterMethodInjection WithMethod<T>(this IParameterMethodInjection injection, Func<T> instantiation)
            where T : notnull
        {
            var parameter = new Parameter<T>(instantiation);

            return injection.WithMethod(parameter);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IParameterMethodInjection WithMethod<T>(this IParameterMethodInjection injection, string name, T instance)
            where T : notnull
        {
            var parameter = new Parameter<T>(name, instance);

            return injection.WithMethod(parameter);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IParameterMethodInjection WithMethod<T>(this IParameterMethodInjection injection, string name, Func<T> instantiation)
            where T : notnull
        {
            var parameter = new Parameter<T>(name, instantiation);

            return injection.WithMethod(parameter);
        }
    }
}
