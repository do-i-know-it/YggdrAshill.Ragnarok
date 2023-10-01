using System;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class ObjectScopeExtension
    {
        public static IObjectScope CreateScope(this IObjectScope scope, params IInstallation[] installationList)
        {
            return scope.CreateContext().Install(installationList).CreateScope();
        }

        public static IObjectScope CreateScope(this IObjectScope scope, Action<IObjectContainer> installation)
        {
            return scope.CreateContext().Install(installation).CreateScope();
        }
    }
}
