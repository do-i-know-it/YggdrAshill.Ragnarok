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
            var parameter = new TypedParameter<T>(instance);

            return injection.WithMethod(parameter);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IParameterMethodInjection WithMethod<T>(this IParameterMethodInjection injection, Func<T> creation)
            where T : notnull
        {
            var parameter = new TypedParameter<T>(creation);

            return injection.WithMethod(parameter);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IParameterMethodInjection WithMethod<T>(this IParameterMethodInjection injection, string name, T instance)
            where T : notnull
        {
            var parameter = new NamedParameter<T>(name, instance);

            return injection.WithMethod(parameter);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IParameterMethodInjection WithMethod<T>(this IParameterMethodInjection injection, string name, Func<T> creation)
            where T : notnull
        {
            var parameter = new NamedParameter<T>(name, creation);

            return injection.WithMethod(parameter);
        }
    }
}
