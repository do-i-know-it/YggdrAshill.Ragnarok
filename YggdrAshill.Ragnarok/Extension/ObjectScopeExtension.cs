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
            return scope.CreateContext().CreateCurrentScope(installationList);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IObjectScope CreateChildScope(this IObjectScope scope, Action<IObjectContainer> installation)
        {
            return scope.CreateContext().CreateCurrentScope(installation);
        }
    }
}
