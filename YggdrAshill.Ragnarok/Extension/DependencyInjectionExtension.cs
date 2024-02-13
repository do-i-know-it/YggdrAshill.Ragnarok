using System;
using System.Runtime.CompilerServices;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class DependencyInjectionExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IDependencyInjection WithArgument<T>(this IDependencyInjection injection, T instance)
            where T : notnull
        {
            var parameter = new TypedParameter<T>(instance);

            return injection.WithArgument(parameter);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IDependencyInjection WithArgument<T>(this IDependencyInjection injection, Func<T> creation)
            where T : notnull
        {
            var parameter = new TypedParameter<T>(creation);

            return injection.WithArgument(parameter);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IDependencyInjection WithArgument<T>(this IDependencyInjection injection, string name, T instance)
            where T : notnull
        {
            var parameter = new NamedParameter<T>(name, instance);

            return injection.WithArgument(parameter);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IDependencyInjection WithArgument<T>(this IDependencyInjection injection, string name, Func<T> creation)
            where T : notnull
        {
            var parameter = new NamedParameter<T>(name, creation);

            return injection.WithArgument(parameter);
        }
    }
}
