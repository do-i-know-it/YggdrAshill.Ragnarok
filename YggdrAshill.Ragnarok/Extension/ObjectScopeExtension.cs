using System;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class ObjectScopeExtension
    {
        public static IObjectScope CreateChildScope(this IObjectScope scope, params IInstallation[] installationList)
        {
            return scope.CreateContext().CreateCurrentScope(installationList);
        }

        public static IObjectScope CreateChildScope(this IObjectScope scope, Action<IObjectContainer> installation)
        {
            return scope.CreateContext().CreateCurrentScope(installation);
        }
    }
}
