using System;
using System.Runtime.CompilerServices;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class ObjectScopeExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IObjectScope CreateChildScope(this IObjectScope scope, params IInstallation[] installationList)
        {
            var context = scope.CreateContext();

            return context.CreateCurrentScope(installationList);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IObjectScope CreateChildScope(this IObjectScope scope, Action<IObjectContainer> installation)
        {
            var context = scope.CreateContext();

            return context.CreateCurrentScope(installation);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IObjectScope CreateChildScope<TInstallation>(this IObjectScope scope)
            where TInstallation : IInstallation
        {
            var context = scope.CreateContext();

            return context.CreateCurrentScope<TInstallation>();
        }
    }
}
