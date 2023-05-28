using System;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class ScopeExtension
    {
        public static IScope CreateChildScope(this IScope scope)
        {
            return scope.CreateContext().Build();
        }

        public static IScope CreateChildScope(this IScope scope, IInstallation installation)
        {
            var context = scope.CreateContext();

            installation.Install(context);

            return context.Build();
        }

        public static IScope CreateChildScope(this IScope scope, Action<IContainer> installation)
        {
            return scope.CreateChildScope(new Installation(installation));
        }
    }
}
